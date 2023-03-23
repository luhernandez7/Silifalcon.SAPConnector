// Silifalcon.SAPConnector.Data.IBatchProvider<T>
using System.Collections.Generic;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Data
{
	public interface IBatchProvider<T> where T : Batch, new()
	{
		List<T> Load(DBConnection connection, BatchFilter args);

		List<T> Load(DBConnection connection, List<string> batches, string whsCode);
	}

}
