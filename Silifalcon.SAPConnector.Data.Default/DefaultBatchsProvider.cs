using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Silifalcon.SAPConnector.Data;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Data.Default
{
	public class DefaultBatchsProvider : IBatchProvider<Batch>
	{
		public List<Batch> Load(DBConnection connection, BatchFilter args)
		{
			if (args == null)
			{
				throw new ArgumentException("The args parameter can not be null");
			}
			_ = args.UpdateDateFrom;
			if (true)
			{
				return LoadBatchsByUpdateDate(connection, args);
			}
			return LoadBatchs(connection, args);
		}

		private List<Batch> LoadBatchsByUpdateDate(DBConnection connection, BatchFilter args)
		{
			connection.SQLCommand.CommandText = "SELECT DISTINCT T0.ItemCode, T0.DistNumber AS BatchNumber, T1.WhsCode,\r\n                    T0.[InDate] AS [ReceptionDate],\r\n                    T0.[ExpDate] AS [DocDueDate],\r\n                    T0.[MnfDate] AS [ManufacturingDate],\r\n                    COALESCE(T1.Quantity, 0) Quantity\r\n\t\t\t\t\tFROM OBTN AS T0 \r\n\t\t\t\t\tLEFT JOIN OBTQ AS T1 ON T0.ItemCode = T1.ItemCode AND T0.SysNumber = T1.SysNumber \r\n\t\t\t\t\tJOIN IBT1 ON IBT1.ItemCode = T0.ItemCode AND IBT1.BatchNum = T0.DistNumber AND IBT1.WhsCode = T1.WhsCode\r\n\t\t\t\t\tJOIN (\r\n\t\t\t\t\t\tSELECT OIGE.DocEntry, OIGE.ObjType \r\n\t\t\t\t\t\tFROM OIGE\r\n\t\t\t\t\t\tGROUP BY OIGE.ObjType, OIGE.DocEntry, OIGE.UpdateDate\r\n\t\t\t\t\t\t\tHAVING DATEADD(HOUR, (MAX([OIGE].[UpdateTS]) / 10000) % 100,\r\n\t\t\t\t\t\t\t\tDATEADD(MINUTE, (MAX([OIGE].[UpdateTS]) / 100) % 100,\r\n\t\t\t\t\t\t\t\tDATEADD(SECOND, MAX([OIGE].[UpdateTS]) % 100, [OIGE].[UpdateDate]))) >= @FROM_DATE\r\n\t\t\t\t\t\tUNION\r\n\t\t\t\t\t\tSELECT OIGN.DocEntry, OIGN.ObjType \r\n\t\t\t\t\t\tFROM OIGN\r\n\t\t\t\t\t\tGROUP BY OIGN.ObjType, OIGN.DocEntry, OIGN.UpdateDate\r\n\t\t\t\t\t\t\tHAVING DATEADD(HOUR, (MAX([OIGN].[UpdateTS]) / 10000) % 100,\r\n\t\t\t\t\t\t\t\tDATEADD(MINUTE, (MAX([OIGN].[UpdateTS]) / 100) % 100,\r\n\t\t\t\t\t\t\t\tDATEADD(SECOND, MAX([OIGN].[UpdateTS]) % 100, [OIGN].[UpdateDate]))) >= @FROM_DATE\r\n\t\t\t\t\t\tUNION\r\n\t\t\t\t\t\tSELECT OWTR.DocEntry, OWTR.ObjType \r\n\t\t\t\t\t\tFROM OWTR\r\n\t\t\t\t\t\tGROUP BY OWTR.ObjType, OWTR.DocEntry, OWTR.UpdateDate\r\n\t\t\t\t\t\t\tHAVING DATEADD(HOUR, (MAX([OWTR].[UpdateTS]) / 10000) % 100,\r\n\t\t\t\t\t\t\t\tDATEADD(MINUTE, (MAX([OWTR].[UpdateTS]) / 100) % 100,\r\n\t\t\t\t\t\t\t\tDATEADD(SECOND, MAX([OWTR].[UpdateTS]) % 100, [OWTR].[UpdateDate]))) >= @FROM_DATE\r\n\t\t\t\t\t\tUNION\r\n\t\t\t\t\t\tSELECT ODLN.DocEntry, ODLN.ObjType \r\n\t\t\t\t\t\tFROM ODLN\r\n\t\t\t\t\t\tGROUP BY ODLN.ObjType, ODLN.DocEntry, ODLN.UpdateDate\r\n\t\t\t\t\t\t\tHAVING DATEADD(HOUR, (MAX([ODLN].[UpdateTS]) / 10000) % 100,\r\n\t\t\t\t\t\t\t\tDATEADD(MINUTE, (MAX([ODLN].[UpdateTS]) / 100) % 100,\r\n\t\t\t\t\t\t\t\tDATEADD(SECOND, MAX([ODLN].[UpdateTS]) % 100, [ODLN].[UpdateDate]))) >= @FROM_DATE\r\n\t\t\t\t\t\tUNION\r\n\t\t\t\t\t\tSELECT OINV.DocEntry, OINV.ObjType \r\n\t\t\t\t\t\tFROM OINV\r\n\t\t\t\t\t\tGROUP BY OINV.ObjType, OINV.DocEntry, OINV.UpdateDate\r\n\t\t\t\t\t\t\tHAVING DATEADD(HOUR, (MAX([OINV].[UpdateTS]) / 10000) % 100,\r\n\t\t\t\t\t\t\t\tDATEADD(MINUTE, (MAX([OINV].[UpdateTS]) / 100) % 100,\r\n\t\t\t\t\t\t\t\tDATEADD(SECOND, MAX([OINV].[UpdateTS]) % 100, [OINV].[UpdateDate]))) >= @FROM_DATE\r\n\t\t\t\t\t\tUNION\r\n\t\t\t\t\t\tSELECT OPDN.DocEntry, OPDN.ObjType \r\n\t\t\t\t\t\tFROM OPDN\r\n\t\t\t\t\t\tGROUP BY OPDN.ObjType, OPDN.DocEntry, OPDN.UpdateDate\r\n\t\t\t\t\t\t\tHAVING DATEADD(HOUR, (MAX([OPDN].[UpdateTS]) / 10000) % 100,\r\n\t\t\t\t\t\t\t\tDATEADD(MINUTE, (MAX([OPDN].[UpdateTS]) / 100) % 100,\r\n\t\t\t\t\t\t\t\tDATEADD(SECOND, MAX([OPDN].[UpdateTS]) % 100, [OPDN].[UpdateDate]))) >= @FROM_DATE\r\n\t\t\t\t\t\tUNION\r\n\t\t\t\t\t\tSELECT ORDN.DocEntry, ORDN.ObjType \r\n\t\t\t\t\t\tFROM ORDN\r\n\t\t\t\t\t\tGROUP BY ORDN.ObjType, ORDN.DocEntry, ORDN.UpdateDate\r\n\t\t\t\t\t\t\tHAVING DATEADD(HOUR, (MAX([ORDN].[UpdateTS]) / 10000) % 100,\r\n\t\t\t\t\t\t\t\tDATEADD(MINUTE, (MAX([ORDN].[UpdateTS]) / 100) % 100,\r\n\t\t\t\t\t\t\t\tDATEADD(SECOND, MAX([ORDN].[UpdateTS]) % 100, [ORDN].[UpdateDate]))) >= @FROM_DATE\r\n\t\t\t\t\t) DOC ON DOC.ObjType = IBT1.BaseType AND DOC.DocEntry = IBT1.BaseEntry";
			connection.SQLCommand.Parameters.Add("@FROM_DATE", SqlDbType.DateTime).Value = args.UpdateDateFrom;
			return connection.CreateDataTable().ToList<Batch>();
		}

		private List<Batch> LoadBatchs(DBConnection connection, BatchFilter args)
		{
			string arg = "";
			if (!string.IsNullOrEmpty(args.ItemCode))
			{
				arg = " AND [OBTN].[ItemCode] = @ITEM_CODE";
				connection.SQLCommand.Parameters.Add("@ITEM_CODE", SqlDbType.VarChar, 50).Value = args.ItemCode;
			}
			if (!string.IsNullOrEmpty(args.WhsCode))
			{
				arg = " AND [OBTQ].[WhsCode] = @WHS_CODE";
				connection.SQLCommand.Parameters.Add("@WHS_CODE", SqlDbType.VarChar, 50).Value = args.WhsCode;
			}
			if (!string.IsNullOrEmpty(args.BatchNumber))
			{
				arg = " AND [OBTN].[DistNumber] = @BATCH";
				connection.SQLCommand.Parameters.Add("@BATCH", SqlDbType.VarChar, 50).Value = args.BatchNumber;
			}
			connection.SQLCommand.CommandText = $"SELECT  [OBTN].[ItemCode], [OBTN].[DistNumber] AS BatchNumber, \r\n                    [OBTN].[InDate] AS [ReceptionDate],\r\n                    [OBTN].[ExpDate] AS [DocDueDate],\r\n                    [OBTN].[MnfDate] AS [ManufacturingDate],\r\n\t\t\t\t\t[OBTQ].[WhsCode], COALESCE([OBTQ].[Quantity], 0) Quantity, \r\n\t\t\t\t\t[OBTN].[CreateDate]\r\n\t\t\t\tFROM [OBTN]\r\n\t\t\t\tLEFT OUTER JOIN [OBTQ] ON [OBTN].[ItemCode] = [OBTQ].[ItemCode] AND [OBTN].[SysNumber] = [OBTQ].[SysNumber] \r\n\t\t\t\tINNER JOIN [OBTW] ON [OBTN].[ItemCode] = [OBTW].[ItemCode] AND [OBTN].[SysNumber] = [OBTW].[SysNumber] AND [OBTQ].[WhsCode] = [OBTW].[WhsCode]\r\n\t\t\t\tWHERE [OBTQ].[Quantity] > 0 {arg}\r\n\t\t\t\tORDER BY [OBTN].[ItemCode], [OBTN].[CreateDate], [OBTQ].[WhsCode], [OBTN].[DistNumber]";
			return connection.CreateDataTable().ToList<Batch>();
		}

		public List<Batch> Load(DBConnection connection, List<string> batches, string whsCode)
		{
			string[] array = batches.Select((string x, int i) => "@BATCH" + i).ToArray();
			connection.SQLCommand.CommandText = string.Format("SELECT T0.ItemCode, T0.DistNumber AS BatchNumber, T1.WhsCode,\r\n                    T0.[InDate] AS [ReceptionDate],\r\n                    T0.[ExpDate] AS [DocDueDate],\r\n                    T0.[MnfDate] AS [ManufacturingDate],\r\n                COALESCE(T1.Quantity, 0) Quantity\r\n\t\t\t\tFROM OBTN AS T0 \r\n\t\t\t\tLEFT JOIN OBTQ AS T1 ON T0.ItemCode = T1.ItemCode AND T0.SysNumber = T1.SysNumber \r\n\t\t\t\tINNER JOIN OBTW AS T2 ON T0.ItemCode = T2.ItemCode AND T0.SysNumber = T2.SysNumber AND T1.WhsCode = T2.WhsCode\r\n\t\t\t\tWHERE T0.DistNumber in ({0}) AND t1.WhsCode = @WHS_CODE\r\n\t\t\t\tORDER BY T0.ItemCode, T0.CreateDate, T1.WhsCode, T0.DistNumber", string.Join(",", array));
			connection.SQLCommand.Parameters.Add("@WHS_CODE", SqlDbType.VarChar, 50).Value = whsCode;
			for (int j = 0; j < array.Length; j++)
			{
				connection.SQLCommand.Parameters.Add(array[j], SqlDbType.VarChar, 50).Value = batches[j];
			}
			return connection.CreateDataTable().ToList<Batch>();
		}
	}

}
