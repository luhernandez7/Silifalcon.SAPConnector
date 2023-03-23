// Silifalcon.SAPConnector.Data.Filters.UsersFilter
using System;

public class UsersFilter
{
	public DateTime UpdateDateFrom { get; set; }

	public string UserCode { get; set; }

	public bool WithWarehouses { get; set; }
}
