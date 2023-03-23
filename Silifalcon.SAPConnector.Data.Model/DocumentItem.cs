// Silifalcon.SAPConnector.Data.Model.DocumentItem
using System.Collections.Generic;

public class DocumentItem
{
	public int DocEntry { get; set; }

	public int LineNum { get; set; }

	public char LineStatus { get; set; }

	public bool IsClosed => 'C' == LineStatus;

	public string ItemCode { get; set; }

	public string Dscription { get; set; }

	public double Quantity { get; set; }

	public double DelivrdQty { get; set; }

	public double OpenCreQty { get; set; }

	public string FromWhsCod { get; set; }

	public string WhsCode { get; set; }

	public Warehouse Source { get; set; }

	public Warehouse Target { get; set; }

	public double Price { get; set; }

	public double DiscPrcnt { get; set; }

	public double TaxPercent
	{
		get
		{
			return VatPrcnt;
		}
		set
		{
			VatPrcnt = value;
		}
	}

	public string TaxCode { get; set; }

	public double Rate { get; set; }

	public string Currency { get; set; }

	public double VatPrcnt { get; set; }

	public TaxGroup TaxGroup { get; set; }

	public List<ConsumedBatch> ConsumedBatches { get; set; }

	public void LoadWarehouses(bool sales)
	{
		if (!string.IsNullOrEmpty(FromWhsCod))
		{
			Source = new Warehouse
			{
				WhsCode = FromWhsCod
			};
		}
		if (sales)
		{
			if (!string.IsNullOrEmpty(WhsCode))
			{
				Source = new Warehouse
				{
					WhsCode = WhsCode
				};
			}
		}
		else if (!string.IsNullOrEmpty(WhsCode))
		{
			Target = new Warehouse
			{
				WhsCode = WhsCode
			};
		}
	}

	public void LoadTaxGroup()
	{
		if (!string.IsNullOrEmpty(TaxCode))
		{
			TaxGroup = new TaxGroup
			{
				Code = TaxCode
			};
		}
	}

	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		DocumentItem documentItem = (DocumentItem)obj;
		return EqualityComparer<int>.Default.Equals(DocEntry, documentItem.DocEntry) && EqualityComparer<int>.Default.Equals(LineNum, documentItem.LineNum) && EqualityComparer<char>.Default.Equals(LineStatus, documentItem.LineStatus) && EqualityComparer<string>.Default.Equals(ItemCode, documentItem.ItemCode) && EqualityComparer<string>.Default.Equals(Dscription, documentItem.Dscription) && EqualityComparer<double>.Default.Equals(Quantity, documentItem.Quantity) && EqualityComparer<double>.Default.Equals(DelivrdQty, documentItem.DelivrdQty) && EqualityComparer<double>.Default.Equals(OpenCreQty, documentItem.OpenCreQty) && EqualityComparer<string>.Default.Equals(FromWhsCod, documentItem.FromWhsCod) && EqualityComparer<string>.Default.Equals(WhsCode, documentItem.WhsCode) && EqualityComparer<TaxGroup>.Default.Equals(TaxGroup, documentItem.TaxGroup) && EqualityComparer<Warehouse>.Default.Equals(Source, documentItem.Source) && EqualityComparer<Warehouse>.Default.Equals(Target, documentItem.Target) && EqualityComparer<double>.Default.Equals(Rate, documentItem.Rate) && EqualityComparer<string>.Default.Equals(Currency, documentItem.Currency) && EqualityComparer<double>.Default.Equals(Price, documentItem.Price) && EqualityComparer<double>.Default.Equals(DiscPrcnt, documentItem.DiscPrcnt) && EqualityComparer<double>.Default.Equals(TaxPercent, documentItem.TaxPercent);
	}

	public override int GetHashCode()
	{
		int num = -821127457;
		num = num * -1521134295 + DocEntry.GetHashCode();
		num = num * -1521134295 + LineNum.GetHashCode();
		num = num * -1521134295 + LineStatus.GetHashCode();
		num = num * -1521134295 + IsClosed.GetHashCode();
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ItemCode);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Dscription);
		num = num * -1521134295 + Quantity.GetHashCode();
		num = num * -1521134295 + DelivrdQty.GetHashCode();
		num = num * -1521134295 + OpenCreQty.GetHashCode();
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FromWhsCod);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(WhsCode);
		num = num * -1521134295 + EqualityComparer<Warehouse>.Default.GetHashCode(Source);
		num = num * -1521134295 + EqualityComparer<Warehouse>.Default.GetHashCode(Target);
		num = num * -1521134295 + Price.GetHashCode();
		num = num * -1521134295 + DiscPrcnt.GetHashCode();
		num = num * -1521134295 + TaxPercent.GetHashCode();
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TaxCode);
		num = num * -1521134295 + Rate.GetHashCode();
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Currency);
		num = num * -1521134295 + VatPrcnt.GetHashCode();
		num = num * -1521134295 + EqualityComparer<TaxGroup>.Default.GetHashCode(TaxGroup);
		return num * -1521134295 + EqualityComparer<List<ConsumedBatch>>.Default.GetHashCode(ConsumedBatches);
	}
}
