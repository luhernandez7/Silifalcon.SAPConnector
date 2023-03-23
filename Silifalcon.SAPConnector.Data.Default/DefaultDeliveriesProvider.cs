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
	public class DefaultDeliveriesProvider : IDeliveriesProvider<Document>
	{
		public virtual Document Get(DBConnection connection, int docEntry)
		{
			connection.SQLCommand.CommandText = "SELECT\r\n                    [ODLN].[DocEntry],\r\n                    [ODLN].[DocNum],\r\n                    [ODLN].[DocStatus],\r\n                    DATEADD(HOUR, ([ODLN].[DocTime] / 10000) % 100,\r\n                    DATEADD(MINUTE, ([ODLN].[DocTime] / 100) % 100,\r\n                    DATEADD(SECOND, [ODLN].[DocTime] % 100, [ODLN].[DocDate]))) AS [DocDate],\r\n                    [ODLN].[DocDueDate],\r\n                    [ODLN].[CardCode],\r\n                    [ODLN].[CardName]\r\n                FROM [ODLN]\r\n                WHERE [ODLN].[DocEntry] = @DOC_ENTRY";
			connection.SQLCommand.Parameters.Add("@DOC_ENTRY", SqlDbType.Int).Value = docEntry;
			List<Document> source = connection.CreateDataTable().ToList<Document>();
			return source.FirstOrDefault();
		}

		public virtual void Save(DBConnection connection, SAPConnection sap, int docEntrySource, Document document)
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
			if (document.DocEntry == 0)
			{
				CreateDocument(connection, sap, docEntrySource, document);
			}
		}

		private void CreateDocument(DBConnection connection, SAPConnection sap, int docEntrySource, Document document)
		{
			Documents order = (dynamic)sap.Company.GetBusinessObject(BoObjectTypes.oDeliveryNotes);
			order.DocDate = document.DocDate;
			if (document.DocDueDate == DateTime.MinValue)
			{
				order.DocDueDate = document.DocDate;
			}
			else
			{
				order.DocDueDate = document.DocDueDate;
			}
			order.CardCode = document.CardCode;
			order.Comments = document.Comments;
			int lineNum = 0;
			foreach (DocumentItem item in document.Items)
			{
				item.LineNum = lineNum;
				if (lineNum++ > 0)
				{
					order.Lines.Add();
				}
				order.Lines.ItemCode = item.ItemCode;
				order.Lines.Quantity = item.Quantity;
				order.Lines.BaseEntry = docEntrySource;
				order.Lines.BaseType = 17;
				order.Lines.BaseLine = item.LineNum;
				if (item.TaxGroup != null)
				{
					order.Lines.TaxCode = item.TaxGroup.Code;
				}
				if (item.ConsumedBatches == null)
				{
					continue;
				}
				int batches = 0;
				item.ConsumedBatches.ForEach(delegate (ConsumedBatch x)
				{
					if (batches > 0)
					{
						order.Lines.BatchNumbers.Add();
					}
					order.Lines.BatchNumbers.BatchNumber = x.Batch.BatchNumber;
					order.Lines.BatchNumbers.Quantity = x.Quantity;
					batches++;
				});
			}
			if (document.PayMethod != null)
			{
				order.PaymentMethod = document.PayMethod.Code;
			}
			sap.CheckResponse(order.Add());
			int num2 = (document.DocEntry = int.Parse(sap.Company.GetNewObjectKey()));
		}
	}
}