// Silifalcon.SAPConnector.Data.Default.DefaultSalesProvider
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
	public class DefaultSalesProvider : ISalesProvider<Document>
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
			connection.SQLCommand.CommandText = "SELECT\r\n                    [ORDR].[DocEntry],\r\n                    @KIND AS [DocumentKind],\r\n                    [ORDR].[DocNum],\r\n                    [ORDR].[DocStatus],\r\n                    DATEADD(HOUR, ([ORDR].[DocTime] / 10000) % 100,\r\n                    DATEADD(MINUTE, ([ORDR].[DocTime] / 100) % 100,\r\n                    DATEADD(SECOND, [ORDR].[DocTime] % 100, [ORDR].[DocDate]))) AS [DocDate],\r\n                    [ORDR].[DocDueDate],\r\n                    [ORDR].[CardCode],\r\n                    [ORDR].[CardName],\r\n                    [ORDR].[GroupNum],\r\n                    [NNM1].[SeriesName] AS [Serie],\r\n                    [OCTG].[PymntGroup] AS [PymntGroup],\r\n                    [ORDR].[PeyMethod],\r\n                    [OPYM].[Descript] AS [PeyDescript],\r\n                    [ORDR].[Comments]\r\n                FROM [ORDR]\r\n                LEFT JOIN [NNM1] ON [NNM1].[Series] = [ORDR].[Series]\r\n                LEFT JOIN [OCTG] ON [OCTG].[GroupNum] = [ORDR].[GroupNum]\r\n                LEFT JOIN [OPYM] ON [OPYM].[PayMethCod] = [ORDR].[PeyMethod]\r\n                WHERE [ORDR].[DocEntry] = @DOC_ENTRY";
			connection.SQLCommand.Parameters.Add("@DOC_ENTRY", SqlDbType.Int).Value = docEntry;
			connection.SQLCommand.Parameters.Add("@KIND", SqlDbType.Int).Value = 3;
			List<Document> list = connection.CreateDataTable().ToList<Document>();
			connection.SQLCommand.CommandText = "SELECT\r\n\t                [RDR1].[DocEntry],\r\n\t                [RDR1].[LineNum],\r\n\t                [RDR1].[LineStatus],\r\n\t                [RDR1].[ItemCode],\r\n\t                [RDR1].[Dscription],\r\n\t                [RDR1].[Quantity],\r\n                    [RDR1].[DelivrdQty],\r\n                    [RDR1].[Price],\r\n\t\t\t\t\t[RDR1].[DiscPrcnt],\r\n\t                [RDR1].[WhsCode],\r\n\t\t\t\t\t[RDR1].[TaxCode],\r\n\t\t\t\t\t[RDR1].[VatPrcnt]\r\n                FROM [RDR1]\r\n                JOIN [ORDR] ON [RDR1].[DocEntry] = [ORDR].[DocEntry]\r\n                WHERE [ORDR].[DocEntry] = @DOC_ENTRY\r\n                ORDER BY [RDR1].[DocEntry], [RDR1].[LineNum]";
			connection.SQLCommand.Parameters.Add("@DOC_ENTRY", SqlDbType.Int).Value = docEntry;
			List<DocumentItem> list2 = connection.CreateDataTable().ToList<DocumentItem>();
			foreach (DocumentItem item in list2)
			{
				item.LoadWarehouses(sales: true);
				item.LoadTaxGroup();
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
				x.PayType = new PayType
				{
					Code = (x.GroupNum.ToString() ?? ""),
					Name = x.PymntGroup
				};
				x.PayMethod = new PayMethod
				{
					Code = x.PeyMethod,
					Name = x.PeyDescript
				};
				if (x.Items != null && x.Items.Count > 0 && x.Items[0] != null)
				{
					x.Warehouse = x.Items[0].Source;
				}
				return x;
			}).FirstOrDefault();
		}

		public virtual List<Document> LoadByUpdatedDate(DBConnection connection, DateTime from)
		{
			connection.SQLCommand.CommandText = "SELECT\r\n\t                [ORDR].[DocEntry],\r\n                    @KIND AS [DocumentKind],\r\n                    [ORDR].[DocNum],\r\n                    [ORDR].[DocStatus],\r\n                    DATEADD(HOUR, ([ORDR].[DocTime] / 10000) % 100,\r\n                    DATEADD(MINUTE, ([ORDR].[DocTime] / 100) % 100,\r\n                    DATEADD(SECOND, [ORDR].[DocTime] % 100, [ORDR].[DocDate]))) AS [DocDate],\r\n                    [ORDR].[DocDueDate],\r\n                    [ORDR].[CardCode],\r\n                    [ORDR].[CardName],\r\n                    [ORDR].[GroupNum],\r\n                    [NNM1].[SeriesName] AS [Serie],\r\n                    [OCTG].[PymntGroup] AS [PymntGroup],\r\n                    [ORDR].[PeyMethod],\r\n                    [OPYM].[Descript] AS [PeyDescript],\r\n                    [ORDR].[Comments]\r\n                FROM [ORDR]\r\n                LEFT JOIN [NNM1] ON [NNM1].[Series] = [ORDR].[Series]\r\n                LEFT JOIN [OCTG] ON [OCTG].[GroupNum] = [ORDR].[GroupNum]\r\n                LEFT JOIN [OPYM] ON [OPYM].[PayMethCod] = [ORDR].[PeyMethod]\r\n                LEFT JOIN (\r\n\t                SELECT [DLN1].[BaseEntry], \r\n\t\t                DATEADD(HOUR, (MAX([ODLN].[UpdateTS]) / 10000) % 100,\r\n                        DATEADD(MINUTE, (MAX([ODLN].[UpdateTS]) / 100) % 100,\r\n                        DATEADD(SECOND, MAX([ODLN].[UpdateTS]) % 100, MAX([ODLN].[UpdateDate])))) AS [DocDate]\r\n\t                FROM [ODLN]\r\n\t                JOIN [DLN1] ON [DLN1].[DocEntry] = [ODLN].[DocEntry]\r\n\t                WHERE [DLN1].[BaseType] = 17\r\n\t                GROUP BY [DLN1].[BaseEntry]\r\n                ) [DEL] ON [DEL].[BaseEntry] = [ORDR].[DocEntry]\r\n                WHERE CASE WHEN\r\n\t\t                [DEL].[DocDate] IS NOT NULL AND [DEL].[DocDate] > \r\n\t\t                DATEADD(HOUR, ([ORDR].[UpdateTS] / 10000) % 100,\r\n                        DATEADD(MINUTE, ([ORDR].[UpdateTS] / 100) % 100,\r\n                        DATEADD(SECOND, [ORDR].[UpdateTS] % 100, [ORDR].[UpdateDate])))\r\n\t                THEN [DEL].[DocDate] ELSE\r\n\r\n\t\t                DATEADD(HOUR, ([ORDR].[UpdateTS] / 10000) % 100,\r\n                        DATEADD(MINUTE, ([ORDR].[UpdateTS] / 100) % 100,\r\n                        DATEADD(SECOND, [ORDR].[UpdateTS] % 100, [ORDR].[UpdateDate])))\r\n\t                END >= @FROM_DATE";
			connection.SQLCommand.Parameters.Add("@FROM_DATE", SqlDbType.DateTime).Value = from;
			connection.SQLCommand.Parameters.Add("@KIND", SqlDbType.Int).Value = 3;
			List<Document> list = connection.CreateDataTable().ToList<Document>();
			connection.SQLCommand.CommandText = "SELECT\r\n\t                [RDR1].[DocEntry],\r\n\t                [RDR1].[LineNum],\r\n\t                [RDR1].[LineStatus],\r\n\t                [RDR1].[ItemCode],\r\n\t                [RDR1].[Dscription],\r\n\t                [RDR1].[Quantity],\r\n                    [RDR1].[DelivrdQty],\r\n                    [RDR1].[Price],\r\n\t                [RDR1].[DiscPrcnt],\r\n\t                [RDR1].[WhsCode],\r\n\t\t\t\t\t[RDR1].[TaxCode],\r\n\t\t\t\t\t[RDR1].[VatPrcnt]\r\n                FROM [RDR1]\r\n                JOIN [ORDR] ON [RDR1].[DocEntry] = [ORDR].[DocEntry]\r\n                LEFT JOIN (\r\n\t                SELECT [DLN1].[BaseEntry], \r\n\t\t                DATEADD(HOUR, (MAX([ODLN].[UpdateTS]) / 10000) % 100,\r\n                        DATEADD(MINUTE, (MAX([ODLN].[UpdateTS]) / 100) % 100,\r\n                        DATEADD(SECOND, MAX([ODLN].[UpdateTS]) % 100, MAX([ODLN].[UpdateDate])))) AS [DocDate]\r\n\t                FROM [ODLN]\r\n\t                JOIN [DLN1] ON [DLN1].[DocEntry] = [ODLN].[DocEntry]\r\n\t                WHERE [DLN1].[BaseType] = 17\r\n\t                GROUP BY [DLN1].[BaseEntry]\r\n                ) [DEL] ON [DEL].[BaseEntry] = [ORDR].[DocEntry]\r\n                WHERE CASE WHEN\r\n\t\t                [DEL].[DocDate] IS NOT NULL AND [DEL].[DocDate] > \r\n\t\t                DATEADD(HOUR, ([ORDR].[UpdateTS] / 10000) % 100,\r\n                        DATEADD(MINUTE, ([ORDR].[UpdateTS] / 100) % 100,\r\n                        DATEADD(SECOND, [ORDR].[UpdateTS] % 100, [ORDR].[UpdateDate])))\r\n\t                THEN [DEL].[DocDate] ELSE\r\n\r\n\t\t                DATEADD(HOUR, ([ORDR].[UpdateTS] / 10000) % 100,\r\n                        DATEADD(MINUTE, ([ORDR].[UpdateTS] / 100) % 100,\r\n                        DATEADD(SECOND, [ORDR].[UpdateTS] % 100, [ORDR].[UpdateDate])))\r\n\t                END >= @FROM_DATE\r\n                ORDER BY [RDR1].[DocEntry], [RDR1].[LineNum]";
			connection.SQLCommand.Parameters.Add("@FROM_DATE", SqlDbType.DateTime).Value = from;
			List<DocumentItem> list2 = connection.CreateDataTable().ToList<DocumentItem>();
			foreach (DocumentItem item in list2)
			{
				item.LoadWarehouses(sales: true);
				item.LoadTaxGroup();
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
				x.PayType = new PayType
				{
					Code = (x.GroupNum.ToString() ?? ""),
					Name = x.PymntGroup
				};
				x.PayMethod = new PayMethod
				{
					Code = x.PeyMethod,
					Name = x.PeyDescript
				};
				if (x.Items != null && x.Items.Count > 0 && x.Items[0] != null)
				{
					x.Warehouse = x.Items[0].Source;
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
			if (document.DocumentKind != 3)
			{
				throw new ArgumentException("The param document kind is not a type sales order");
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
			Documents documents = (dynamic)sap.Company.GetBusinessObject(BoObjectTypes.oOrders);
			if (!documents.GetByKey(document.DocEntry))
			{
				throw new ArgumentException("The doc entry related to sale order document not exist");
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
				documents.Lines.TaxCode = item.TaxGroup.Code;
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
			Documents documents = (dynamic)sap.Company.GetBusinessObject(BoObjectTypes.oOrders);
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
				if (item.TaxGroup != null)
				{
					documents.Lines.TaxCode = item.TaxGroup.Code;
				}
			}
			if (document.PayMethod != null)
			{
				documents.PaymentMethod = document.PayMethod.Code;
			}
			if (document.PayType != null)
			{
				documents.GroupNumber = int.Parse(document.PayType.Code);
			}
			sap.CheckResponse(documents.Add());
			int num2 = (document.DocEntry = int.Parse(sap.Company.GetNewObjectKey()));
		}
	}
}