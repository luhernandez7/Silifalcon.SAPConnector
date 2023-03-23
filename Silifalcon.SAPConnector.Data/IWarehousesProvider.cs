// Silifalcon.SAPConnector.Data.IWarehousesProvider<T>
using System.Collections.Generic;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Data
{
	public interface IWarehousesProvider<T> where T : Warehouse, new()
	{
		List<T> Load(DBConnection connection, WarehousesFilter args);

		T Get(DBConnection connection, string code);
	}
}

