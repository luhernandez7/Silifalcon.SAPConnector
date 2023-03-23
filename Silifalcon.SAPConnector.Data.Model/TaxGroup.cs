// Silifalcon.SAPConnector.Data.Model.TaxGroup
using System.Collections.Generic;
public class TaxGroup
{
	public string Code { get; set; }

	public string Name { get; set; }

	public double Rate { get; set; }

	public string Lock { get; set; }

	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		TaxGroup taxGroup = (TaxGroup)obj;
		return EqualityComparer<string>.Default.Equals(Code, taxGroup.Code) && EqualityComparer<string>.Default.Equals(Name, taxGroup.Name) && EqualityComparer<double>.Default.Equals(Rate, taxGroup.Rate) && EqualityComparer<string>.Default.Equals(Lock, taxGroup.Lock);
	}

	public override int GetHashCode()
	{
		int num = 367446491;
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Code);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
		num = num * -1521134295 + Rate.GetHashCode();
		return num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Lock);
	}
}
