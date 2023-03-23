// Silifalcon.SAPConnector.Data.Model.Batch
using System;

public class Batch
{
	public string ItemCode { get; set; }

	public string BatchNumber { get; set; }

	public string WhsCode { get; set; }

	public DateTime ReceptionDate { get; set; }

	public DateTime DocDueDate { get; set; }

	public DateTime ManufacturingDate { get; set; }

	public int Quantity { get; set; }
}
