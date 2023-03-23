// Silifalcon.SAPConnector.Data.Model.Currency
using System.Collections.Generic;

public class Currency
{
	public string CurrCode { get; set; }

	public string CurrName { get; set; }

	public decimal Rate { get; set; }

	public bool Default { get; set; }

	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		Currency currency = (Currency)obj;
		return EqualityComparer<string>.Default.Equals(CurrCode, currency.CurrCode) && EqualityComparer<string>.Default.Equals(CurrName, currency.CurrName) && EqualityComparer<decimal>.Default.Equals(Rate, currency.Rate);
	}

	public override int GetHashCode()
	{
		int num = 1955217041;
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CurrCode);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CurrName);
		num = num * -1521134295 + Rate.GetHashCode();
		return num * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(Default);
	}
}
