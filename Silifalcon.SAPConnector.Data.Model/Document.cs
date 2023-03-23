// Silifalcon.SAPConnector.Data.Model.Document
using System;
using System.Collections.Generic;
using System.Linq;

public class Document
{
	public const int KIND_TRANSFER_REQUEST = 1;

	public const int KIND_PURCHASE_ORDER = 2;

	public const int KIND_SALES_ORDER = 3;

	public int DocEntry { get; set; }

	public int DocumentKind { get; set; }

	public string Serie { get; set; }

	public int DocNum { get; set; }

	public char DocStatus { get; set; }

	public bool IsClosed => 'C' == DocStatus;

	public DateTime DocDate { get; set; }

	public DateTime DocDueDate { get; set; }

	public string CardCode { get; set; }

	public string CardName { get; set; }

	public string LicTradNum { get; set; }

	public string Comments { get; set; }

	public string Address { get; set; }

	public string Address2 { get; set; }

	public Warehouse Warehouse { get; set; }

	public int GroupNum { get; set; }

	public string PymntGroup { get; set; }

	public string PeyMethod { get; set; }

	public string PeyDescript { get; set; }

	public PayType PayType { get; set; }

	public PayMethod PayMethod { get; set; }

	public double SysRate { get; set; }

	public string DocCur { get; set; }

	public string SlpName { get; set; }

	public List<DocumentItem> Items { get; set; } = new List<DocumentItem>();


	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		Document document = (Document)obj;
		return EqualityComparer<int>.Default.Equals(DocEntry, document.DocEntry) && EqualityComparer<int>.Default.Equals(DocumentKind, document.DocumentKind) && EqualityComparer<int>.Default.Equals(DocNum, document.DocNum) && EqualityComparer<char>.Default.Equals(DocStatus, document.DocStatus) && EqualityComparer<string>.Default.Equals(Serie, document.Serie) && EqualityComparer<DateTime>.Default.Equals(DocDate, document.DocDate) && EqualityComparer<DateTime>.Default.Equals(DocDueDate, document.DocDueDate) && EqualityComparer<Warehouse>.Default.Equals(Warehouse, document.Warehouse) && EqualityComparer<PayType>.Default.Equals(PayType, document.PayType) && EqualityComparer<PayMethod>.Default.Equals(PayMethod, document.PayMethod) && EqualityComparer<string>.Default.Equals(DocCur, document.DocCur) && EqualityComparer<double>.Default.Equals(SysRate, document.SysRate) && EqualityComparer<string>.Default.Equals(CardCode, document.CardCode) && EqualityComparer<string>.Default.Equals(CardName, document.CardName) && EqualityComparer<string>.Default.Equals(Comments, document.Comments) && ((Items == null && document.Items == null) || Items.SequenceEqual(document.Items));
	}

	public override int GetHashCode()
	{
		int num = -708620742;
		num = num * -1521134295 + DocEntry.GetHashCode();
		num = num * -1521134295 + DocumentKind.GetHashCode();
		num = num * -1521134295 + DocNum.GetHashCode();
		num = num * -1521134295 + DocStatus.GetHashCode();
		num = num * -1521134295 + IsClosed.GetHashCode();
		num = num * -1521134295 + DocDate.GetHashCode();
		num = num * -1521134295 + DocDueDate.GetHashCode();
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Serie);
		num = num * -1521134295 + EqualityComparer<Warehouse>.Default.GetHashCode(Warehouse);
		num = num * -1521134295 + EqualityComparer<PayType>.Default.GetHashCode(PayType);
		num = num * -1521134295 + EqualityComparer<PayMethod>.Default.GetHashCode(PayMethod);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CardCode);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CardName);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Comments);
		return num * -1521134295 + EqualityComparer<List<DocumentItem>>.Default.GetHashCode(Items);
	}
}
