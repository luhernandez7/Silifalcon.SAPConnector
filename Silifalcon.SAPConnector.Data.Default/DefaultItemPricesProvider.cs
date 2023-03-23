// Silifalcon.SAPConnector.Data.Default.DefaultItemPricesProvider
using System;
using System.Collections.Generic;
using System.Data;
using Silifalcon.SAPConnector.Data;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Data.Default
{
	public class DefaultItemPricesProvider : IItemPricesProvider<ItemPrice>
	{
		public virtual List<ItemPrice> Load(DBConnection connection, ItemPricesFilter args)
		{
			if (args == null)
			{
				throw new ArgumentException("The args parameter can not be null");
			}
			return LoadItemPrices(connection, args);
		}

		private List<ItemPrice> LoadItemPrices(DBConnection connection, ItemPricesFilter args)
		{
			string text = "";
			if (args.PriceList > 0)
			{
				text = text + "[ITM1].[PriceList] = " + args.PriceList;
			}
			if (!string.IsNullOrEmpty(args.ItemCode))
			{
				if (text.Length > 0)
				{
					text += " AND ";
				}
				text += "[ITM1].ItemCode = @ITEM_CODE";
			}
			if (text.Length > 0)
			{
				text = "WHERE " + text;
			}
			connection.SQLCommand.CommandText = "SELECT [ITM1].[ItemCode], \r\n\t\t\t\t\t    [ITM1].[PriceList], \r\n\t\t\t\t\t    [ITM1].[Price] AS [MinimumPrice], \r\n\t\t\t\t\t\t[ITM1].[Price] AS [MaximumPrice], \r\n\t\t\t\t\t    [ITM1].[Currency]\r\n\t\t\t\t    FROM [ITM1]" + text;
			if (!string.IsNullOrEmpty(args.ItemCode))
			{
				connection.SQLCommand.Parameters.Add("@ITEM_CODE", SqlDbType.VarChar, 50).Value = args.ItemCode;
			}
			return connection.CreateDataTable().ToList<ItemPrice>();
		}
	}
}