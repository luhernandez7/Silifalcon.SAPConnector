// Silifalcon.SAPConnector.Data.Default.DefaultWarehousesProvider
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Silifalcon.SAPConnector.Data;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Data.Default
{
	public class DefaultWarehousesProvider : IWarehousesProvider<Warehouse>
	{
		public virtual List<Warehouse> Load(DBConnection connection, WarehousesFilter args)
		{
			if (args == null)
			{
				throw new ArgumentException("The args can not be null");
			}
			return LoadByUpdateDate(connection, args);
		}

		private List<Warehouse> LoadByUpdateDate(DBConnection connection, WarehousesFilter args)
		{
			string text = "";
			if (args.OnlyActive)
			{
				text = "AND [OWHS].[Inactive] = 'N'";
			}
			else if (args.OnlyDesactive)
			{
				text = "AND [OWHS].[Inactive] = 'Y'";
			}
			connection.SQLCommand.CommandText = "SELECT [OWHS].[WhsCode], \r\n\t\t\t\t\t[OWHS].[WhsName], \r\n\t\t\t\t\t[OWHS].[Inactive]\r\n\t\t\t\tFROM OWHS\r\n\t\t\t\tWHERE COALESCE([OWHS].[UpdateDate], [OWHS].[CreateDate]) >= CAST(@FROM_DATE AS DATE)\r\n\t\t\t\t\t" + text;
			connection.SQLCommand.Parameters.Add("@FROM_DATE", SqlDbType.DateTime).Value = args.UpdateDateFrom;
			return connection.CreateDataTable().ToList<Warehouse>();
		}

		public virtual Warehouse Get(DBConnection connection, string code)
		{
			string text = "";
			connection.SQLCommand.CommandText = "SELECT [OWHS].[WhsCode], \r\n\t\t\t\t\t[OWHS].[WhsName], \r\n\t\t\t\t\t[OWHS].[Inactive]\r\n\t\t\t\tFROM OWHS\r\n\t\t\t\tWHERE [OWHS].[WhsCode] = @USER_CODE\r\n\t\t\t\t\t" + text;
			connection.SQLCommand.Parameters.Add("@USER_CODE", SqlDbType.VarChar, 50).Value = code;
			return connection.CreateDataTable().ToList<Warehouse>().FirstOrDefault();
		}
	}
}