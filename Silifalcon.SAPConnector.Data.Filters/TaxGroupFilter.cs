// Silifalcon.SAPConnector.Data.Filters.TaxGroupFilter
public class TaxGroupFilter
{
	public string TaxGroupCode { get; set; }

	public bool OnlyActives { get; set; } = false;


	public bool OnlyDesactives { get; set; } = false;

}
