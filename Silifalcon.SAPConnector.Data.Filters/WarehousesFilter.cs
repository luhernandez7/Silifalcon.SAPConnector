// Silifalcon.SAPConnector.Data.Filters.WarehousesFilter
using System;

public class WarehousesFilter
{
	public DateTime UpdateDateFrom { get; set; } = default(DateTime);


	public bool OnlyActive { get; set; }

	public bool OnlyDesactive { get; set; }
}
