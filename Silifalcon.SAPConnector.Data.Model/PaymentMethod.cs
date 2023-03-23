// Silifalcon.SAPConnector.Data.Model.PaymentMethod
using System.Collections.Generic;

public class PaymentMethod
{
	public string PayMethCod { get; set; }

	public string Descript { get; set; }

	public string Active { get; set; }

	public bool Include { get; set; }

	public bool IsActive()
	{
		return "Y".Equals(Active);
	}

	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		PaymentMethod paymentMethod = (PaymentMethod)obj;
		return EqualityComparer<string>.Default.Equals(PayMethCod, paymentMethod.PayMethCod) && EqualityComparer<string>.Default.Equals(Descript, paymentMethod.Descript) && EqualityComparer<string>.Default.Equals(Active, paymentMethod.Active) && EqualityComparer<bool>.Default.Equals(Include, paymentMethod.Include);
	}

	public override int GetHashCode()
	{
		int num = -270119730;
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PayMethCod);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Descript);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Active);
		return num * -1521134295 + Include.GetHashCode();
	}
}
