// Silifalcon.SAPConnector.Data.Model.PayType
using System.Collections.Generic;

public class PayType
{
	public string Code { get; set; }

	public string Name { get; set; }

	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		PayType payType = (PayType)obj;
		return EqualityComparer<string>.Default.Equals(Code, payType.Code) && EqualityComparer<string>.Default.Equals(Name, payType.Name);
	}

	public override int GetHashCode()
	{
		int num = -168117446;
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Code);
		return num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
	}
}
