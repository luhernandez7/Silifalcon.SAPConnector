// Silifalcon.SAPConnector.Data.Model.SalesEmployee
using System.Collections.Generic;

public class SalesEmployee
{
	public int SlpCode { get; set; }

	public string SlpName { get; set; }

	public string Memo { get; set; }

	public string Active { get; set; }

	public bool IsActive => "Y".Equals(Active);

	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		SalesEmployee salesEmployee = (SalesEmployee)obj;
		return EqualityComparer<int>.Default.Equals(SlpCode, salesEmployee.SlpCode) && EqualityComparer<string>.Default.Equals(SlpName, salesEmployee.SlpName) && EqualityComparer<string>.Default.Equals(Memo, salesEmployee.Memo) && EqualityComparer<string>.Default.Equals(Active, salesEmployee.Active);
	}

	public override int GetHashCode()
	{
		int num = -1772649103;
		num = num * -1521134295 + SlpCode.GetHashCode();
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SlpName);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Memo);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Active);
		return num * -1521134295 + IsActive.GetHashCode();
	}
}
