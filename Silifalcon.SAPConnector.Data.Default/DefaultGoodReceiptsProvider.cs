// Silifalcon.SAPConnector.Data.Default.DefaultGoodReceiptsProvider
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
	public class DefaultGoodReceiptsProvider : IGoodReceiptsProvider<Document>
	{
		public virtual Document Get(DBConnection connection, int docEntry)
		{
			connection.SQLCommand.CommandText = "SELECT\r\n                    [OPDN].[DocEntry],\r\n                    [OPDN].[DocNum],\r\n                    [OPDN].[DocStatus],\r\n                    DATEADD(HOUR, ([OPDN].[DocTime] / 10000) % 100,\r\n                    DATEADD(MINUTE, ([OPDN].[DocTime] / 100) % 100,\r\n                    DATEADD(SECOND, [OPDN].[DocTime] % 100, [OPDN].[DocDate]))) AS [DocDate],\r\n                    [OPDN].[DocDueDate],\r\n                    [OPDN].[CardCode],\r\n                    [OPDN].[CardName]\r\n                FROM [OPDN]\r\n                WHERE [OPDN].[DocEntry] = @DOC_ENTRY";
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
			Documents documents = (dynamic)sap.Company.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes);
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
				documents.Lines.Quantity = item.Quantity;
				documents.Lines.BaseEntry = docEntrySource;
				documents.Lines.BaseType = 22;
				documents.Lines.BaseLine = item.LineNum;
			}
			sap.CheckResponse(documents.Add());
			int num2 = (document.DocEntry = int.Parse(sap.Company.GetNewObjectKey()));
		}
	}
}