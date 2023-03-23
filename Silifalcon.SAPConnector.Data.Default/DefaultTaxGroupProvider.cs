// Silifalcon.SAPConnector.Data.Default.DefaultTaxGroupProvider
using System;
using System.Collections.Generic;
using System.Data;
using Silifalcon.SAPConnector.Data;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Data.Default
{
	public class DefaultTaxGroupProvider : ITaxGroupProvider<TaxGroup>
	{
		public List<TaxGroup> Load(DBConnection connection, TaxGroupFilter args)
		{
			if (args == null)
			{
				throw new ArgumentException("The args parameter can not be null");
			}
			return LoadTaxGroups(connection, args);
		}

		private List<TaxGroup> LoadTaxGroups(DBConnection connection, TaxGroupFilter args)
		{
			string text = "";
			if (!string.IsNullOrEmpty(args.TaxGroupCode))
			{
				text = " [OSTC].[Code] = @TAX_CODE";
				connection.SQLCommand.Parameters.Add("@TAX_CODE", SqlDbType.VarChar, 50).Value = args.TaxGroupCode;
			}
			if (args.OnlyActives)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += " AND ";
				}
				text += "[OSTC].[Lock] = 'N'";
			}
			if (args.OnlyDesactives)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += " AND ";
				}
				text += "[OSTC].[Lock] = 'Y'";
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = " WHERE " + text;
			}
			connection.SQLCommand.CommandText = $"SELECT [OSTC].[Code], [OSTC].[Name], \r\n\t\t\t\t\t\t\t[OSTC].[Rate], [OSTC].[Lock] \r\n\t\t\t\t\tFROM [OSTC] {text}";
			return connection.CreateDataTable().ToList<TaxGroup>();
		}
	}
}