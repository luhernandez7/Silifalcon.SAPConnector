// Silifalcon.SAPConnector.Data.Model.Address
using System.Collections.Generic;

public class Address
{
	public string Code { get; set; }

	public string CardCode { get; set; }

	public string TaxCode { get; set; }

	public string AdresType { get; set; }

	public bool IsShipTo => "S".Equals(AdresType);

	public bool IsBillTo => "B".Equals(AdresType);

	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		Address address = (Address)obj;
		return EqualityComparer<string>.Default.Equals(Code, address.Code) && EqualityComparer<string>.Default.Equals(CardCode, address.CardCode) && EqualityComparer<string>.Default.Equals(TaxCode, address.TaxCode) && EqualityComparer<string>.Default.Equals(AdresType, address.AdresType);
	}

	public override int GetHashCode()
	{
		int num = -201856969;
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Code);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CardCode);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TaxCode);
		return num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AdresType);
	}
}
