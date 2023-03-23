// Silifalcon.SAPConnector.Data.Model.ExchangeRate
using System;
using System.Collections.Generic;

public class ExchangeRate
{
	public string Currency { get; set; }

	public decimal Rate { get; set; }

	public DateTime RateDate { get; set; }

	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		ExchangeRate exchangeRate = (ExchangeRate)obj;
		return EqualityComparer<string>.Default.Equals(Currency, exchangeRate.Currency) && EqualityComparer<decimal>.Default.Equals(Rate, exchangeRate.Rate) && EqualityComparer<DateTime>.Default.Equals(RateDate, exchangeRate.RateDate);
	}

	public override int GetHashCode()
	{
		int num = -664654568;
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Currency);
		num = num * -1521134295 + Rate.GetHashCode();
		return num * -1521134295 + RateDate.GetHashCode();
	}
}
