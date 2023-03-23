// Silifalcon.SAPConnector.Data.Default.DefaultPurchasesProvider
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SAPbobsCOM;
using Silifalcon.SAPConnector;
using Silifalcon.SAPConnector.Data;
using Silifalcon.SAPConnector.DBMS;


namespace Silifalcon.SAPConnector.Data.Default
{
	public class DefaultPurchasesProvider : IPurchasesProvider<Document>
	{
		public virtual List<Document> Load(DBConnection connection, DocumentsFilter filter)
		{
			if (filter == null)
			{
				throw new ArgumentException("The filter can not be null");
			}
			return LoadByUpdatedDate(connection, filter.UpdateDateFrom);
		}

		public virtual Document Get(DBConnection connection, int docEntry)
		{
			connection.SQLCommand.CommandText = "SELECT\r\n                    [OPOR].[DocEntry],\r\n                    @KIND AS [DocumentKind],\r\n                    [OPOR].[DocNum],\r\n                    [OPOR].[DocStatus],\r\n                    DATEADD(HOUR, ([OPOR].[DocTime] / 10000) % 100,\r\n                    DATEADD(MINUTE, ([OPOR].[DocTime] / 100) % 100,\r\n                    DATEADD(SECOND, [OPOR].[DocTime] % 100, [OPOR].[DocDate]))) AS [DocDate],\r\n                    [OPOR].[DocDueDate],\r\n                    [OPOR].[CardCode],\r\n                    [OPOR].[CardName],\r\n\t\t\t\t\t[OCRD].[LicTradNum],\r\n                    [OPOR].[Comments],\r\n\t\t\t\t\t[OPOR].[Address], \r\n\t\t\t\t\t[OPOR].[Address2],\r\n\t\t\t\t\t[OSLP].[SlpName],\r\n\t\t\t\t\t[OCTG].[PymntGroup]\r\n                FROM [OPOR]\r\n\t\t\t\tJOIN [OCRD] ON [OCRD].[CardCode] = [OPOR].[CardCode]\r\n\t\t\t\tJOIN [OCTG] ON [OCTG].[GroupNum] = [OPOR].[GroupNum]\r\n\t\t\t\tLEFT JOIN [OSLP] ON [OSLP].[SlpCode] = [OPOR].[SlpCode]\r\n                WHERE [OPOR].[DocEntry] = @DOC_ENTRY";
			connection.SQLCommand.Parameters.Add("@DOC_ENTRY", SqlDbType.Int).Value = docEntry;
			connection.SQLCommand.Parameters.Add("@KIND", SqlDbType.Int).Value = 2;
			List<Document> list = connection.CreateDataTable().ToList<Document>();
			connection.SQLCommand.CommandText = "SELECT\r\n\t                [POR1].[DocEntry],\r\n\t                [POR1].[LineNum],\r\n\t                [POR1].[LineStatus],\r\n\t                [POR1].[ItemCode],\r\n\t                [POR1].[Dscription],\r\n\t                [POR1].[Quantity],\r\n                    [POR1].[OpenCreQty],\r\n                    [POR1].[Price],\r\n\t\t\t\t\t[POR1].[DiscPrcnt],\r\n\t                [POR1].[WhsCode],\r\n\t\t\t\t\t[RDR1].[TaxCode],\r\n\t\t\t\t\t[RDR1].[VatPrcnt]\r\n                FROM [POR1]\r\n                JOIN [OPOR] ON [POR1].[DocEntry] = [OPOR].[DocEntry]\r\n                WHERE [OPOR].[DocEntry] = @DOC_ENTRY\r\n                ORDER BY [POR1].[DocEntry], [POR1].[LineNum]";
			connection.SQLCommand.Parameters.Add("@DOC_ENTRY", SqlDbType.Int).Value = docEntry;
			List<DocumentItem> list2 = connection.CreateDataTable().ToList<DocumentItem>();
			foreach (DocumentItem item in list2)
			{
				item.LoadWarehouses(sales: false);
				foreach (Document item2 in list)
				{
					if (item.DocEntry == item2.DocEntry)
					{
						item2.Items.Add(item);
					}
				}
			}
			return list.Select(delegate (Document x)
			{
				if (x.Items != null && x.Items.Count > 0 && x.Items[0] != null)
				{
					x.Warehouse = x.Items[0].Target;
				}
				return x;
			}).FirstOrDefault();
		}

		public virtual List<Document> LoadByUpdatedDate(DBConnection connection, DateTime from)
		{
			connection.SQLCommand.CommandText = "SELECT\r\n\t                [OPOR].[DocEntry],\r\n                    @KIND AS [DocumentKind],\r\n                    [OPOR].[DocNum],\r\n                    [OPOR].[DocStatus],\r\n                    DATEADD(HOUR, ([OPOR].[DocTime] / 10000) % 100,\r\n                    DATEADD(MINUTE, ([OPOR].[DocTime] / 100) % 100,\r\n                    DATEADD(SECOND, [OPOR].[DocTime] % 100, [OPOR].[DocDate]))) AS [DocDate],\r\n                    [OPOR].[DocDueDate],\r\n                    [OPOR].[CardCode],\r\n                    [OPOR].[CardName],\r\n\t\t\t\t\t[OCRD].[LicTradNum],\r\n                    [OPOR].[Comments],\r\n\t\t\t\t\t[OPOR].[Address], \r\n\t\t\t\t\t[OPOR].[Address2],\r\n\t\t\t\t\t[OSLP].[SlpName],\r\n\t\t\t\t\t[OCTG].[PymntGroup]\r\n                FROM [OPOR]\r\n\t\t\t\tJOIN [OCRD] ON [OCRD].[CardCode] = [OPOR].[CardCode]\r\n\t\t\t\tJOIN [OCTG] ON [OCTG].[GroupNum] = [OPOR].[GroupNum]\r\n\t\t\t\tLEFT JOIN [OSLP] ON [OSLP].[SlpCode] = [OPOR].[SlpCode]\r\n                LEFT JOIN (\r\n\t                SELECT [PDN1].[BaseEntry], \r\n\t\t                DATEADD(HOUR, (MAX([OPDN].[UpdateTS]) / 10000) % 100,\r\n                        DATEADD(MINUTE, (MAX([OPDN].[UpdateTS]) / 100) % 100,\r\n                        DATEADD(SECOND, MAX([OPDN].[UpdateTS]) % 100, MAX([OPDN].[UpdateDate])))) AS [DocDate]\r\n\t                FROM [OPDN]\r\n\t                JOIN [PDN1] ON [PDN1].[DocEntry] = [OPDN].[DocEntry]\r\n\t                WHERE [PDN1].[BaseType] = 22\r\n\t                GROUP BY [PDN1].[BaseEntry]\r\n                ) [DEL] ON [DEL].[BaseEntry] = [OPOR].[DocEntry]\r\n                WHERE CASE WHEN\r\n\t\t                [DEL].[DocDate] IS NOT NULL AND [DEL].[DocDate] > \r\n\t\t                DATEADD(HOUR, ([OPOR].[UpdateTS] / 10000) % 100,\r\n                        DATEADD(MINUTE, ([OPOR].[UpdateTS] / 100) % 100,\r\n                        DATEADD(SECOND, [OPOR].[UpdateTS] % 100, [OPOR].[UpdateDate])))\r\n\t                THEN [DEL].[DocDate] ELSE\r\n\r\n\t\t                DATEADD(HOUR, ([OPOR].[UpdateTS] / 10000) % 100,\r\n                        DATEADD(MINUTE, ([OPOR].[UpdateTS] / 100) % 100,\r\n                        DATEADD(SECOND, [OPOR].[UpdateTS] % 100, [OPOR].[UpdateDate])))\r\n\t                END >= @FROM_DATE";
			connection.SQLCommand.Parameters.Add("@FROM_DATE", SqlDbType.DateTime).Value = from;
			connection.SQLCommand.Parameters.Add("@KIND", SqlDbType.Int).Value = 2;
			List<Document> list = connection.CreateDataTable().ToList<Document>();
			connection.SQLCommand.CommandText = "SELECT\r\n\t                [POR1].[DocEntry],\r\n\t                [POR1].[LineNum],\r\n\t                [POR1].[LineStatus],\r\n\t                [POR1].[ItemCode],\r\n\t                [POR1].[Dscription],\r\n\t                [POR1].[Quantity],\r\n                    [POR1].[OpenCreQty],\r\n                    [POR1].[Price],\r\n\t                [POR1].[DiscPrcnt],\r\n\t                [POR1].[WhsCode]\r\n                FROM [POR1]\r\n                JOIN [OPOR] ON [POR1].[DocEntry] = [OPOR].[DocEntry]\r\n                LEFT JOIN (\r\n\t                SELECT [PDN1].[BaseEntry], \r\n\t\t                DATEADD(HOUR, (MAX([OPDN].[UpdateTS]) / 10000) % 100,\r\n                        DATEADD(MINUTE, (MAX([OPDN].[UpdateTS]) / 100) % 100,\r\n                        DATEADD(SECOND, MAX([OPDN].[UpdateTS]) % 100, MAX([OPDN].[UpdateDate])))) AS [DocDate]\r\n\t                FROM [OPDN]\r\n\t                JOIN [PDN1] ON [PDN1].[DocEntry] = [OPDN].[DocEntry]\r\n\t                WHERE [PDN1].[BaseType] = 22\r\n\t                GROUP BY [PDN1].[BaseEntry]\r\n                ) [DEL] ON [DEL].[BaseEntry] = [OPOR].[DocEntry]\r\n                WHERE CASE WHEN\r\n\t\t                [DEL].[DocDate] IS NOT NULL AND [DEL].[DocDate] > \r\n\t\t                DATEADD(HOUR, ([OPOR].[UpdateTS] / 10000) % 100,\r\n                        DATEADD(MINUTE, ([OPOR].[UpdateTS] / 100) % 100,\r\n                        DATEADD(SECOND, [OPOR].[UpdateTS] % 100, [OPOR].[UpdateDate])))\r\n\t                THEN [DEL].[DocDate] ELSE\r\n\r\n\t\t                DATEADD(HOUR, ([OPOR].[UpdateTS] / 10000) % 100,\r\n                        DATEADD(MINUTE, ([OPOR].[UpdateTS] / 100) % 100,\r\n                        DATEADD(SECOND, [OPOR].[UpdateTS] % 100, [OPOR].[UpdateDate])))\r\n\t                END >= @FROM_DATE\r\n                ORDER BY [POR1].[DocEntry], [POR1].[LineNum]";
			connection.SQLCommand.Parameters.Add("@FROM_DATE", SqlDbType.DateTime).Value = from;
			List<DocumentItem> list2 = connection.CreateDataTable().ToList<DocumentItem>();
			foreach (DocumentItem item in list2)
			{
				item.LoadWarehouses(sales: false);
				foreach (Document item2 in list)
				{
					if (item.DocEntry == item2.DocEntry)
					{
						item2.Items.Add(item);
					}
				}
			}
			list.ForEach(delegate (Document x)
			{
				if (x.Items != null && x.Items.Count > 0 && x.Items[0] != null)
				{
					x.Warehouse = x.Items[0].Target;
				}
			});
			return list;
		}

		public virtual void Save(DBConnection connection, SAPConnection sap, Document document)
		{
			if (connection == null)
			{
				throw new ArgumentException("The param connection can not be null");
			}
			if (sap == null)
			{
				throw new ArgumentNullException("The param sap can not be null");
			}
			if (document == null)
			{
				throw new ArgumentNullException("The param document can not be null");
			}
			if (document.DocumentKind != 2)
			{
				throw new ArgumentException("The param document kind is not a type purchase order");
			}
			if (document.IsClosed)
			{
				throw new ArgumentException("The document status is closed");
			}
			if (document.DocEntry > 0)
			{
				UpdateDocument(connection, sap, document);
			}
			else
			{
				CreateDocument(connection, sap, document);
			}
		}

		private void UpdateDocument(DBConnection connection, SAPConnection sap, Document document)
		{
			Documents documents = (dynamic)sap.Company.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
			if (!documents.GetByKey(document.DocEntry))
			{
				throw new ArgumentException("The doc entry related to purchase order document not exist");
			}
			documents.DocDate = document.DocDate;
			if (document.DocDueDate == DateTime.MinValue)
			{
				documents.DocDueDate = document.DocDate;
			}
			else
			{
				documents.DocDueDate = document.DocDueDate;
			}
			documents.CardCode = document.CardCode;
			documents.Comments = document.Comments;
			int num = 0;
			foreach (DocumentItem item in document.Items)
			{
				if (num++ > 0)
				{
					documents.Lines.Add();
				}
				documents.Lines.SetCurrentLine(item.LineNum);
				if (item.IsClosed)
				{
					documents.Lines.LineStatus = BoStatus.bost_Close;
				}
				documents.Lines.ItemCode = item.ItemCode;
				documents.Lines.WarehouseCode = item.WhsCode;
				documents.Lines.Quantity = item.Quantity;
				documents.Lines.UnitPrice = item.Price;
				if (item.DiscPrcnt > 0.0)
				{
					documents.Lines.DiscountPercent = item.DiscPrcnt;
				}
			}
			sap.CheckResponse(documents.Update());
		}

		private void CreateDocument(DBConnection connection, SAPConnection sap, Document document)
		{
			Documents documents = (dynamic)sap.Company.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
			documents.DocDate = document.DocDate;
			if (document.DocDueDate == DateTime.MinValue)
			{
				documents.DocDueDate = document.DocDate;
			}
			else
			{
				documents.DocDueDate = document.DocDueDate;
			}
			documents.CardCode = document.CardCode;
			documents.Comments = document.Comments;
			int lineNum = 0;
			foreach (DocumentItem item in document.Items)
			{
				item.LineNum = lineNum;
				if (lineNum++ > 0)
				{
					documents.Lines.Add();
				}
				documents.Lines.ItemCode = item.ItemCode;
				documents.Lines.WarehouseCode = item.WhsCode;
				documents.Lines.Quantity = item.Quantity;
				documents.Lines.UnitPrice = item.Price;
				if (item.DiscPrcnt > 0.0)
				{
					documents.Lines.DiscountPercent = item.DiscPrcnt;
				}
			}
			sap.CheckResponse(documents.Add());
			int num2 = (document.DocEntry = int.Parse(sap.Company.GetNewObjectKey()));
		}
	}
}