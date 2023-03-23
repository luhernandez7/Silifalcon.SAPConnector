// Silifalcon.SAPConnector.Data.Default.DefaultSalesEmployeesProvider
using System;
using System.Collections.Generic;
using System.Data;
using Silifalcon.SAPConnector.Data;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Data.Default
{
	public class DefaultSalesEmployeesProvider : ISalesEmployeesProvider<SalesEmployee>
	{
		public virtual List<SalesEmployee> Load(DBConnection connection, SalesEmployeesFilter args)
		{
			if (args == null)
			{
				throw new ArgumentException("The parameter args can not be null");
			}
			List<SalesEmployee> list = new List<SalesEmployee>();
			if (!string.IsNullOrEmpty(args.Memo))
			{
				return LoadByMemo(connection, args);
			}
			return LoadByCode(connection, args);
		}

		private List<SalesEmployee> LoadByCode(DBConnection connection, SalesEmployeesFilter args)
		{
			string text = "";
			if (args.OnlyActives)
			{
				text = "[OSLP].[Active] = 'Y'";
			}
			else if (args.OnlyDesactives)
			{
				text = "[OSLP].[Active] = 'N'";
			}
			if (args.SlpCode != 0)
			{
				if (text.Length > 0)
				{
					text += " AND ";
				}
				text = text + "[OSLP].[SlpCode] = " + args.SlpCode;
			}
			if (text.Length > 0)
			{
				text = "WHERE " + text;
			}
			connection.SQLCommand.CommandText = "SELECT [OSLP].[SlpCode], [OSLP].[SlpName], \r\n\t\t\t\t\t\t[OSLP].[Memo], [OSLP].[Active] \r\n\t\t\t\t\tFROM [OSLP] " + text;
			return connection.CreateDataTable().ToList<SalesEmployee>();
		}

		private List<SalesEmployee> LoadByMemo(DBConnection connection, SalesEmployeesFilter args)
		{
			string text = "";
			if (args.OnlyActives)
			{
				text = "[OSLP].[Active] = 'Y'";
			}
			else if (args.OnlyDesactives)
			{
				text = "[OSLP].[Active] = 'N'";
			}
			connection.SQLCommand.CommandText = "SELECT [OSLP].[SlpCode], \r\n\t\t\t\t\t\t[OSLP].[SlpName], \r\n\t\t\t\t\t\t[OSLP].[Memo], \r\n\t\t\t\t\t\t[OSLP].[Active] \r\n\t\t\t\tFROM [OSLP] \r\n\t\t\t\tWHERE [OSLP].[Memo] = @MEMO AND " + text;
			connection.SQLCommand.Parameters.Add("@MEMO", SqlDbType.VarChar, 50).Value = args.Memo;
			return connection.CreateDataTable().ToList<SalesEmployee>();
		}
	}
}