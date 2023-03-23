using System;
using System.Collections.Generic;
using Silifalcon.Rest.Connection;
using Silifalcon.SAPConnector;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Connector
{
	public interface ISyncDownload<T, U>
	{
		bool AllowNew { get; }

		bool AllowUpdate { get; }

		bool IsNew(U record);

		List<U> Pull(RestClient rest, DateTime lastSyncDate);

		void CommitFetch(RestClient rest, DateTime commitDate);

		T Create(DBConnection connection, SAPConnection sap, U record);

		T Update(DBConnection connection, SAPConnection sap, U document);
	}

}
