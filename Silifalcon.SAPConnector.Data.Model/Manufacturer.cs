// Silifalcon.SAPConnector.Data.Model.Manufacturer
using System.Collections.Generic;
using System.Linq;


public class Manufacturer
{
	public int FirmCode { get; set; }

	public string FirmName { get; set; }

	public List<ItemGroup> ItemGroups { get; set; }

	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		Manufacturer manufacturer = (Manufacturer)obj;
		return EqualityComparer<int>.Default.Equals(FirmCode, manufacturer.FirmCode) && EqualityComparer<string>.Default.Equals(FirmName, manufacturer.FirmName) && ((ItemGroups == null && manufacturer.ItemGroups == null) || ItemGroups.SequenceEqual(manufacturer.ItemGroups));
	}

	public override int GetHashCode()
	{
		int num = -427053776;
		num = num * -1521134295 + FirmCode.GetHashCode();
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirmName);
		return num * -1521134295 + EqualityComparer<List<ItemGroup>>.Default.GetHashCode(ItemGroups);
	}
}
