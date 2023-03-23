// Silifalcon.SAPConnector.Data.IGoodReceiptsProvider<T>
using Silifalcon.SAPConnector;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Data
{
	public interface IGoodReceiptsProvider<T> where T : Document, new()
	{
		void Save(DBConnection connection, SAPConnection sap, int docEntrySource, T document);

		T Get(DBConnection connection, int docEntry);
	}

}
