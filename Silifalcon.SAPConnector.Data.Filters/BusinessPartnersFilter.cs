// Silifalcon.SAPConnector.Data.Filters.BusinessPartnersFilter
using System;

public class BusinessPartnersFilter
{
	public DateTime UpdateDateFrom { get; set; }

	public string CardType { get; set; }

	public string CardCode { get; set; }

	public bool WithPaymentMethods { get; set; } = false;


	public bool WithPaymentTerms { get; set; } = false;


	public bool WithAddres { get; set; } = false;

}
