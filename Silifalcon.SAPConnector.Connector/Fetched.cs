using System.Collections.Generic;

namespace Silifalcon.SAPConnector.Connector
{
	public class Fetched<T>
	{
		public List<T> NewRecords { get; set; }

		public List<T> ExistingRecords { get; set; }

		public bool Empty => (NewRecords == null || NewRecords.Count == 0) && (ExistingRecords == null || ExistingRecords.Count == 0);
	}

}
