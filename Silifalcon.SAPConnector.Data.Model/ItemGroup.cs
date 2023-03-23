// Silifalcon.SAPConnector.Data.Model.ItemGroup
using System.Collections.Generic;

public class ItemGroup
{
	public string ItmsGrpCod { get; set; }

	public string ItmsGrpNam { get; set; }

	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		ItemGroup itemGroup = (ItemGroup)obj;
		return EqualityComparer<string>.Default.Equals(ItmsGrpCod, itemGroup.ItmsGrpCod) && EqualityComparer<string>.Default.Equals(ItmsGrpNam, itemGroup.ItmsGrpNam);
	}

	public override int GetHashCode()
	{
		int num = 210674128;
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ItmsGrpCod);
		return num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ItmsGrpNam);
	}
}
