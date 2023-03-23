// Silifalcon.SAPConnector.Data.Model.CostCenter
using System;
using System.Collections.Generic;

public class CostCenter
{
	public string PrcCode { get; set; }

	public string PrcName { get; set; }

	public DateTime ValidFrom { get; set; }

	public DateTime ValidTo { get; set; }

	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		CostCenter costCenter = (CostCenter)obj;
		return EqualityComparer<string>.Default.Equals(PrcCode, costCenter.PrcCode) && EqualityComparer<string>.Default.Equals(PrcName, costCenter.PrcName) && EqualityComparer<DateTime>.Default.Equals(ValidFrom, costCenter.ValidFrom) && EqualityComparer<DateTime>.Default.Equals(ValidTo, costCenter.ValidTo);
	}

	public override int GetHashCode()
	{
		int num = -2021372647;
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PrcCode);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PrcName);
		num = num * -1521134295 + ValidFrom.GetHashCode();
		return num * -1521134295 + ValidTo.GetHashCode();
	}
}
