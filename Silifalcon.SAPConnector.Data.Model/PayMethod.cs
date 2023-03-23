// Silifalcon.SAPConnector.Data.Model.PayMethod
using System.Collections.Generic;

public class PayMethod
{
	public string Code { get; set; }

	public string Name { get; set; }

	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		PayMethod payMethod = (PayMethod)obj;
		return EqualityComparer<string>.Default.Equals(Code, payMethod.Code) && EqualityComparer<string>.Default.Equals(Name, payMethod.Name);
	}

	public override int GetHashCode()
	{
		int num = -168117446;
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Code);
		return num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
	}
}
