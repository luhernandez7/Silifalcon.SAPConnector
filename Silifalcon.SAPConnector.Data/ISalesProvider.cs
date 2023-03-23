// Silifalcon.SAPConnector.Data.ISalesProvider<T>
using System.Collections.Generic;
using Silifalcon.SAPConnector;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Data
{
	public interface ISalesProvider<T> where T : Document, new()
	{
		List<T> Load(DBConnection connection, DocumentsFilter args);

		T Get(DBConnection connection, int docEntry);

		void Save(DBConnection connection, SAPConnection sap, T document);
	}

}
