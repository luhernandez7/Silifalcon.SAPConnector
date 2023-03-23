// Silifalcon.SAPConnector.Data.Model.TransferItem
using System.Collections.Generic;

public class TransferItem
{
	public int LineNum { get; set; }

	public double Quantity { get; set; }

	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		TransferItem transferItem = (TransferItem)obj;
		return EqualityComparer<int>.Default.Equals(LineNum, transferItem.LineNum) && EqualityComparer<double>.Default.Equals(Quantity, transferItem.Quantity);
	}

	public override int GetHashCode()
	{
		int num = 1206811637;
		num = num * -1521134295 + LineNum.GetHashCode();
		return num * -1521134295 + Quantity.GetHashCode();
	}
}
