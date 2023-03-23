// Silifalcon.SAPConnector.Data.Model.PaymentTerm
using System.Collections.Generic;

public class PaymentTerm
{
	public int GroupNum { get; set; }

	public string PymntGroup { get; set; }

	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		PaymentTerm paymentTerm = (PaymentTerm)obj;
		return EqualityComparer<int>.Default.Equals(GroupNum, paymentTerm.GroupNum) && EqualityComparer<string>.Default.Equals(PymntGroup, paymentTerm.PymntGroup);
	}

	public override int GetHashCode()
	{
		int num = 456498202;
		num = num * -1521134295 + GroupNum.GetHashCode();
		return num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PymntGroup);
	}
}
