using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using Silifalcon.Rest.Connection;
using Silifalcon.SAPConnector;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Connector
{
	public abstract class RestConnector<T, U>
	{
		private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public ISyncDownload<T, U> SyncDownload { get; set; }

		public ISyncUpload<T, U> SyncUpload { get; set; }

		public Fetched<U> Fetch(DBConnection connection, RestClient rest, DateTime lastSyncDate)
		{
			if (SyncDownload == null)
			{
				throw new Exception("'SyncDownload' property not defined");
			}
			logger.Debug("Fetching changes");
			List<U> list = SyncDownload.Pull(rest, lastSyncDate);
			if (list != null && list.Count > 0)
			{
				logger.Info("Fetched " + list.Count + " records");
				return new Fetched<U>
				{
					NewRecords = (SyncDownload.AllowNew ? list.FindAll((U x) => SyncDownload.IsNew(x)) : null),
					ExistingRecords = (SyncDownload.AllowUpdate ? list.FindAll((U x) => !SyncDownload.IsNew(x)) : null)
				};
			}
			return null;
		}

		public void Upload(DBConnection connection, RestClient rest, DateTime lastSyncDate, DateTime confirmDate)
		{
			if (SyncUpload == null)
			{
				throw new Exception("'SyncUpload' property not defined");
			}
			logger.Debug("Loading local changes");
			List<T> list = SyncUpload.LoadLocal(connection, lastSyncDate);
			if (list == null || list.Count <= 0)
			{
				return;
			}
			logger.Info("Found " + list.Count + " records since: " + lastSyncDate.ToString("dd/MM/yyyy HH:mm:ss"));
			for (int i = 0; i < list.Count; i += SyncUpload.PageSize)
			{
				int num = list.Count - i;
				if (num > SyncUpload.PageSize)
				{
					num = SyncUpload.PageSize;
				}
				SyncUpload.Push(rest, list.GetRange(i, num));
				logger.Info("Uploading " + 100f * (float)i / (float)list.Count + "%");
			}
			logger.Info("Uploading 100%");
			SyncUpload.Commit(rest, confirmDate);
		}

		public void Storing(SAPConnection sap, DBConnection connection, RestClient rest, Fetched<U> fetched, DateTime nowDate)
		{
			if (SyncDownload == null)
			{
				throw new Exception("'SyncDownload' property not defined");
			}
			if (fetched == null)
			{
				return;
			}
			logger.Debug("Storing data");
			bool flag = false;
			if (fetched.NewRecords != null && fetched.NewRecords.Count > 0)
			{
				foreach (U newRecord in fetched.NewRecords)
				{
					SyncDownload.Create(connection, sap, newRecord);
				}
				flag = true;
			}
			if (fetched.ExistingRecords != null && fetched.ExistingRecords.Count > 0)
			{
				foreach (U existingRecord in fetched.ExistingRecords)
				{
					SyncDownload.Update(connection, sap, existingRecord);
				}
				flag = true;
			}
			if (flag)
			{
				SyncDownload.CommitFetch(rest, nowDate);
			}
		}
	}
}

