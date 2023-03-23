// Silifalcon.SAPConnector.Data.Model.ItemPrice
using System.Collections.Generic;

public class ItemPrice
{
	public string ItemCode { get; set; }

	public int PriceList { get; set; }

	public double MinimumPrice { get; set; }

	public double MaximumPrice { get; set; }

	public string WhsCode { get; set; }

	public string Currency { get; set; }

	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		ItemPrice itemPrice = (ItemPrice)obj;
		return EqualityComparer<string>.Default.Equals(ItemCode, itemPrice.ItemCode) && EqualityComparer<int>.Default.Equals(PriceList, itemPrice.PriceList) && EqualityComparer<double>.Default.Equals(MaximumPrice, itemPrice.MaximumPrice) && EqualityComparer<double>.Default.Equals(MinimumPrice, itemPrice.MinimumPrice) && EqualityComparer<string>.Default.Equals(Currency, itemPrice.Currency);
	}

	public override int GetHashCode()
	{
		int num = -769926439;
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ItemCode);
		num = num * -1521134295 + PriceList.GetHashCode();
		num = num * -1521134295 + MaximumPrice.GetHashCode();
		num = num * -1521134295 + MinimumPrice.GetHashCode();
		return num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Currency);
	}
}
