// Silifalcon.SAPConnector.Data.Filters.ItemsFilter
using System;

public class ItemsFilter
{
	public DateTime UpdateDateFrom { get; set; }

	public string ItemCode { get; set; }

	public bool WithItemGroup { get; set; } = false;


	public bool WithItemManufacturer { get; set; } = false;


	public bool WithFlagForBatches { get; set; }
}
