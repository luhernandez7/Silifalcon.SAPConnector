using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using log4net;
using SAPbobsCOM;

namespace Silifalcon.SAPConnector
{
	public class SAPConnection : IDisposable
	{
		private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public Company Company { get; private set; }

		public SAPConnection()
		{
			if (SAPSettings.Current == null)
			{
				SAPException ex = new SAPException(-1, "SAPSettings.Current not defined");
				logger.Error(ex.Message, ex);
				Debug.WriteLine(ex.Message);
				throw ex;
			}
			//Company obj = (Company)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("632F4591-AA62-4219-8FB6-22BCF5F60100")));
			Company = new Company();
			Company.SLDServer = SAPSettings.Current.SLDServer;
			Company.LicenseServer = SAPSettings.Current.LicenseServer;
			Company.Server = SAPSettings.Current.Server;
			Company.CompanyDB = SAPSettings.Current.CompanyDB;
			Company.UserName = SAPSettings.Current.UserName;
			Company.Password = SAPSettings.Current.Password;
			Company.DbUserName = SAPSettings.Current.DbUserName;
			Company.DbPassword = SAPSettings.Current.DbPassword;
			Company.UseTrusted = SAPSettings.Current.UseTrusted;
			Company.DbServerType = SAPSettings.Current.DbServerType;
			//Company = obj;
			logger.Info("Connecting");
			Debug.WriteLine("Connecting");
			int errCode = Company.Connect();
			if (errCode != 0)
			{
				Company.GetLastError(out errCode, out var errMsg);
				SAPConnectionException ex2 = new SAPConnectionException(errCode, errMsg);
				logger.Error(ex2.Message, ex2);
				Debug.WriteLine(ex2.Message);
				throw ex2;
			}
		}

		public void CheckResponse(int response)
		{
			if (response == 0)
			{
				logger.Info("Success action");
				Debug.WriteLine("Success action");
				return;
			}
			Company.GetLastError(out response, out var errMsg);
			SAPException ex = new SAPException(response, errMsg);
			logger.Error(ex.Message, ex);
			Debug.WriteLine(ex.Message);
			throw ex;
		}

		public void Dispose()
		{
			Dispose(x: false);
		}

		protected virtual void Dispose(bool x)
		{
			if (Company.Connected)
			{
				logger.Info("Disconnecting");
				Debug.WriteLine("Disconnecting");
				Company.Disconnect();
			}
			GC.SuppressFinalize(this);
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
		}
	}

}
