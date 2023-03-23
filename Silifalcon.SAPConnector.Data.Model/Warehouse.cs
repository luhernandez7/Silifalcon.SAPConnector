// Silifalcon.SAPConnector.Data.Model.Warehouse
using System.Collections.Generic;

public class Warehouse
{
	public string WhsCode { get; set; }

	public string WhsName { get; set; }

	public string Inactive { get; set; }

	public bool IsActive()
	{
		return "Y".Equals(Inactive);
	}

	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		Warehouse warehouse = (Warehouse)obj;
		return EqualityComparer<string>.Default.Equals(WhsCode, warehouse.WhsCode) && EqualityComparer<string>.Default.Equals(WhsName, warehouse.WhsName) && EqualityComparer<string>.Default.Equals(Inactive, warehouse.Inactive);
	}

	public override int GetHashCode()
	{
		int num = -1730701478;
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(WhsCode);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(WhsName);
		return num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Inactive);
	}
}
