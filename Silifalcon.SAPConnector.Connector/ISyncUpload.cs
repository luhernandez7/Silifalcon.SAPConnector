using System;
using System.Collections.Generic;
using Silifalcon.Rest.Connection;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Connector
{
	public interface ISyncUpload<T, U>
	{
		int PageSize { get; set; }

		List<T> LoadLocal(DBConnection connection, DateTime lastSyncDate);

		void Push(RestClient rest, List<T> records);

		void Commit(RestClient rest, DateTime commitDate);
	}
}

