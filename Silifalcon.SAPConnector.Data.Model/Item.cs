// Silifalcon.SAPConnector.Data.Model.Item
using System.Collections.Generic;

public class Item
{
	public string ItemCode { get; set; }

	public string ItemName { get; set; }

	public int FirmCode { get; set; }

	public Manufacturer ItemManufacturer { get; set; }

	public string ItmsGrpCod { get; set; }

	public ItemGroup ItemGroup { get; set; }

	public string ManBtchNum { get; set; }

	public string WhsCode { get; set; }

	public string FromWhsCod { get; set; }

	public string VATLiable { get; set; }

	public string IndirctTax { get; set; }

	public string IndirectTaxCode { get; set; }

	public string InvntItem { get; set; }

	public string SellItem { get; set; }

	public string PrchseItem { get; set; }

	public string frozenFor { get; set; }

	public bool IsInventoryItem => "Y".Equals(InvntItem);

	public bool IsSellItem => "Y".Equals(SellItem);

	public bool IsPurchaseItem => "Y".Equals(PrchseItem);

	public bool IsActive => "Y".Equals(frozenFor);

	public bool IsManageByBatches => "Y".Equals(ManBtchNum);

	public bool ApplyTax => "Y".Equals(VATLiable);

	public bool ApplyIndirectTax => "Y".Equals(IndirctTax);

	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		Item item = (Item)obj;
		return EqualityComparer<string>.Default.Equals(ItemCode, item.ItemCode) && EqualityComparer<string>.Default.Equals(ItemName, item.ItemName) && EqualityComparer<int>.Default.Equals(FirmCode, item.FirmCode) && EqualityComparer<Manufacturer>.Default.Equals(ItemManufacturer, item.ItemManufacturer) && EqualityComparer<string>.Default.Equals(ItmsGrpCod, item.ItmsGrpCod) && EqualityComparer<ItemGroup>.Default.Equals(ItemGroup, item.ItemGroup) && EqualityComparer<string>.Default.Equals(ManBtchNum, item.ManBtchNum) && EqualityComparer<string>.Default.Equals(WhsCode, item.WhsCode) && EqualityComparer<string>.Default.Equals(FromWhsCod, item.FromWhsCod) && EqualityComparer<string>.Default.Equals(IndirctTax, item.IndirctTax) && EqualityComparer<string>.Default.Equals(InvntItem, item.InvntItem) && EqualityComparer<string>.Default.Equals(SellItem, item.SellItem) && EqualityComparer<string>.Default.Equals(PrchseItem, item.PrchseItem) && EqualityComparer<string>.Default.Equals(frozenFor, item.frozenFor) && EqualityComparer<string>.Default.Equals(IndirectTaxCode, item.IndirectTaxCode);
	}

	public override int GetHashCode()
	{
		int num = 952290342;
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ItemCode);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ItemName);
		num = num * -1521134295 + FirmCode.GetHashCode();
		num = num * -1521134295 + EqualityComparer<Manufacturer>.Default.GetHashCode(ItemManufacturer);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ItmsGrpCod);
		num = num * -1521134295 + EqualityComparer<ItemGroup>.Default.GetHashCode(ItemGroup);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ManBtchNum);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(WhsCode);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FromWhsCod);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(VATLiable);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(IndirctTax);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(IndirectTaxCode);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InvntItem);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SellItem);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PrchseItem);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(frozenFor);
		num = num * -1521134295 + IsInventoryItem.GetHashCode();
		num = num * -1521134295 + IsSellItem.GetHashCode();
		num = num * -1521134295 + IsPurchaseItem.GetHashCode();
		num = num * -1521134295 + IsActive.GetHashCode();
		num = num * -1521134295 + IsManageByBatches.GetHashCode();
		num = num * -1521134295 + ApplyTax.GetHashCode();
		return num * -1521134295 + ApplyIndirectTax.GetHashCode();
	}
}
