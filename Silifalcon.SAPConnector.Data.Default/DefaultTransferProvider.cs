// Silifalcon.SAPConnector.Data.Default.DefaultTransferProvider
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
	public class DefaultTransferProvider : ITransferProvider<Document>
	{
		public virtual List<Document> Load(DBConnection connection, DocumentsFilter filter)
		{
			if (filter == null)
			{
				throw new ArgumentException("The filter can not be null");
			}
			connection.SQLCommand.CommandText = "SELECT\r\n\t                [OWTR].[DocEntry],\r\n                    @KIND AS [DocumentKind],\r\n                    [OWTR].[DocNum],\r\n                    [OWTR].[DocStatus],\r\n                    DATEADD(HOUR, ([OWTR].[DocTime] / 10000) % 100,\r\n                    DATEADD(MINUTE, ([OWTR].[DocTime] / 100) % 100,\r\n                    DATEADD(SECOND, [OWTR].[DocTime] % 100, [OWTR].[DocDate]))) AS [DocDate],\r\n                    [OWTR].[DocDueDate],\r\n                    [OWTR].[Comments]\r\n                FROM [OWTR]\r\n                WHERE \r\n\t\t            DATEADD(HOUR, ([OWTR].[UpdateTS] / 10000) % 100,\r\n                    DATEADD(MINUTE, ([OWTR].[UpdateTS] / 100) % 100,\r\n                    DATEADD(SECOND, [OWTR].[UpdateTS] % 100, [OWTR].[UpdateDate])))\r\n\t                    >= @FROM_DATE";
			connection.SQLCommand.Parameters.Add("@FROM_DATE", SqlDbType.DateTime).Value = filter.UpdateDateFrom;
			connection.SQLCommand.Parameters.Add("@KIND", SqlDbType.Int).Value = 1;
			List<Document> list = connection.CreateDataTable().ToList<Document>();
			connection.SQLCommand.CommandText = "SELECT\r\n\t                [WTR1].[DocEntry],\r\n\t                [WTR1].[LineNum],\r\n\t                [WTR1].[LineStatus],\r\n\t                [WTR1].[ItemCode],\r\n\t                [WTR1].[Dscription],\r\n\t                [WTR1].[Quantity],\r\n\t                [WTR1].[WhsCode],\r\n\t                [WTR1].[FromWhsCod]\r\n                FROM [WTR1]\r\n                JOIN [OWTR] ON [WTR1].[DocEntry] = [OWTR].[DocEntry]\r\n                WHERE \r\n\t\t            DATEADD(HOUR, ([OWTR].[UpdateTS] / 10000) % 100,\r\n                    DATEADD(MINUTE, ([OWTR].[UpdateTS] / 100) % 100,\r\n                    DATEADD(SECOND, [OWTR].[UpdateTS] % 100, [OWTR].[UpdateDate])))\r\n\t                 >= @FROM_DATE\r\n                ORDER BY [WTR1].[DocEntry], [WTR1].[LineNum]";
			connection.SQLCommand.Parameters.Add("@FROM_DATE", SqlDbType.DateTime).Value = filter.UpdateDateFrom;
			List<DocumentItem> list2 = connection.CreateDataTable().ToList<DocumentItem>();
			foreach (DocumentItem item in list2)
			{
				item.LoadWarehouses(sales: true);
				foreach (Document item2 in list)
				{
					if (item.DocEntry == item2.DocEntry)
					{
						item2.Items.Add(item);
					}
				}
			}
			return list;
		}

		public virtual Document Get(DBConnection connection, int docEntry)
		{
			connection.SQLCommand.CommandText = "SELECT\r\n                    [OWTR].[DocEntry],\r\n                    @KIND AS [DocumentKind],\r\n                    [OWTR].[DocNum],\r\n                    [OWTR].[DocStatus],\r\n                    DATEADD(HOUR, ([OWTR].[DocTime] / 10000) % 100,\r\n                    DATEADD(MINUTE, ([OWTR].[DocTime] / 100) % 100,\r\n                    DATEADD(SECOND, [OWTR].[DocTime] % 100, [OWTR].[DocDate]))) AS [DocDate],\r\n                    [OWTR].[DocDueDate],\r\n                    [OWTR].[Comments]\r\n                FROM [OWTR]\r\n                WHERE [OWTR].[DocEntry] = @DOC_ENTRY";
			connection.SQLCommand.Parameters.Add("@DOC_ENTRY", SqlDbType.Int).Value = docEntry;
			connection.SQLCommand.Parameters.Add("@KIND", SqlDbType.Int).Value = 3;
			List<Document> list = connection.CreateDataTable().ToList<Document>();
			connection.SQLCommand.CommandText = "SELECT\r\n\t                [WTR1].[DocEntry],\r\n\t                [WTR1].[LineNum],\r\n\t                [WTR1].[LineStatus],\r\n\t                [WTR1].[ItemCode],\r\n\t                [WTR1].[Dscription],\r\n\t                [WTR1].[Quantity],\r\n\t                [WTR1].[WhsCode],\r\n\t                [WTR1].[FromWhsCod]\r\n                FROM [WTR1]\r\n                JOIN [OWTR] ON [WTR1].[DocEntry] = [OWTR].[DocEntry]\r\n                WHERE [OWTR].[DocEntry] = @DOC_ENTRY\r\n                ORDER BY [WTR1].[DocEntry], [WTR1].[LineNum]";
			connection.SQLCommand.Parameters.Add("@DOC_ENTRY", SqlDbType.Int).Value = docEntry;
			List<DocumentItem> list2 = connection.CreateDataTable().ToList<DocumentItem>();
			foreach (DocumentItem item in list2)
			{
				item.LoadWarehouses(sales: true);
				foreach (Document item2 in list)
				{
					if (item.DocEntry == item2.DocEntry)
					{
						item2.Items.Add(item);
					}
				}
			}
			return list.FirstOrDefault();
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
			if (document.DocumentKind != 1)
			{
				throw new ArgumentException("The param document kind is not a type tranfer request");
			}
			if (document.IsClosed)
			{
				throw new ArgumentException("The document status is closed");
			}
			CreateDocument(connection, sap, document);
		}

		private void CreateDocument(DBConnection connection, SAPConnection sap, Document document)
		{
			StockTransfer stockTransfer = (dynamic)sap.Company.GetBusinessObject(BoObjectTypes.oStockTransfer);
			stockTransfer.DocDate = document.DocDate;
			stockTransfer.Comments = document.Comments;
			if (document.Items != null && document.Items.Count > 0)
			{
				stockTransfer.FromWarehouse = document.Items[0].FromWhsCod;
				stockTransfer.ToWarehouse = document.Items[0].WhsCode;
			}
			int lineNum = 0;
			foreach (DocumentItem item in document.Items)
			{
				if (item.FromWhsCod.Equals(item.WhsCode))
				{
					throw new ArgumentException("The origin and detination warehouse can not be the same in the line " + item.LineNum);
				}
				item.LineNum = lineNum;
				if (lineNum++ > 0)
				{
					stockTransfer.Lines.Add();
				}
				stockTransfer.Lines.FromWarehouseCode = item.FromWhsCod;
				stockTransfer.Lines.WarehouseCode = item.WhsCode;
				stockTransfer.Lines.ItemCode = item.ItemCode;
				stockTransfer.Lines.Quantity = item.Quantity;
			}
			sap.CheckResponse(stockTransfer.Add());
			int num2 = (document.DocEntry = int.Parse(sap.Company.GetNewObjectKey()));
		}
	}
}