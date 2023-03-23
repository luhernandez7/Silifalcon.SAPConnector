// Silifalcon.SAPConnector.Data.Default.DefaultItemsProvider
using System;
using System.Collections.Generic;
using System.Data;
using Silifalcon.SAPConnector.Data;
using Silifalcon.SAPConnector.DBMS;


namespace Silifalcon.SAPConnector.Data.Default
{
	public class DefaultItemsProvider : IItemsProvider<Item>
	{
		public virtual List<Item> Load(DBConnection connection, ItemsFilter args)
		{
			if (args == null)
			{
				throw new ArgumentException("The args parameter can not be null");
			}
			List<Item> list = new List<Item>();
			if (!string.IsNullOrEmpty(args.ItemCode))
			{
				list = LoadByCode(connection, args);
			}
			else
			{
				_ = args.UpdateDateFrom;
				if (true)
				{
					list = LoadByUpdatedDate(connection, args);
				}
			}
			if (args.WithItemGroup)
			{
				AddItemGroup(connection, list);
			}
			if (args.WithItemManufacturer)
			{
				AddManufacturer(connection, list);
			}
			return list;
		}

		public virtual List<Item> LoadByUpdatedDate(DBConnection connection, ItemsFilter args)
		{
			string text = "[OITM].[ItemCode], [OITM].[ItemName], [OITM].[VATLiable], \r\n\t\t\t\t\t\t\t\t[OITM].[IndirctTax], [OTCX].[LnTaxCode] IndirectTaxCode,\r\n\t\t\t\t\t\t\t\t[OITM].[InvntItem], [OITM].[SellItem], [OITM].[PrchseItem], [OITM].[frozenFor]";
			if (args.WithItemManufacturer)
			{
				text += ",[OITM].[FirmCode]";
			}
			if (args.WithItemGroup)
			{
				text += ",[OITM].[ItmsGrpCod]";
			}
			if (args.WithFlagForBatches)
			{
				text += ",[OITM].[ManBtchNum]";
			}
			connection.SQLCommand.CommandText = $"SELECT {text}\r\n                FROM [OITM]\r\n\t\t\t\tLEFT JOIN [OTCX] ON [OTCX].[StrVal1] = [OITM].[ItemCode] \r\n\t\t\t\t\tAND [OTCX].[DocType] = 0 AND [OTCX].[BusArea] = 0 AND [OTCX].[Cond1] = 9\r\n                WHERE SellItem = 'Y' \r\n\t\t\t\t\tAND COALESCE([OITM].[UpdateDate], [OITM].[CreateDate]) >= CAST(@FROM_DATE AS DATE)";
			connection.SQLCommand.Parameters.Add("@FROM_DATE", SqlDbType.DateTime).Value = args.UpdateDateFrom;
			return connection.CreateDataTable().ToList<Item>();
		}

		public virtual List<Item> LoadByCode(DBConnection connection, ItemsFilter args)
		{
			string text = "[OITM].[ItemCode], [OITM].[ItemName], [OITM].[VATLiable], \r\n\t\t\t\t\t\t\t\t[OITM].[IndirctTax], [OTCX].[LnTaxCode] IndirectTaxCode,\r\n\t\t\t\t\t\t\t\t[OITM].[InvntItem], [OITM].[SellItem], [OITM].[PrchseItem], [OITM].[frozenFor]";
			if (args.WithItemManufacturer)
			{
				text += ",[OITM].[FirmCode]";
			}
			if (args.WithItemGroup)
			{
				text += ",[OITM].[ItmsGrpCod]";
			}
			if (args.WithFlagForBatches)
			{
				text += ",[OITM].[ManBtchNum]";
			}
			connection.SQLCommand.CommandText = $"SELECT {text}\r\n                FROM [OITM]\r\n\t\t\t\tLEFT JOIN [OTCX] ON [OTCX].[StrVal1] = [OITM].[ItemCode] \r\n\t\t\t\t\tAND [OTCX].[DocType] = 0 AND [OTCX].[BusArea] = 0 AND [OTCX].[Cond1] = 9\r\n                WHERE SellItem = 'Y' \r\n\t\t\t\t\tAND [OITM].[ItemCode] = @CODE";
			connection.SQLCommand.Parameters.Add("@CODE", SqlDbType.VarChar, 50).Value = args.ItemCode;
			return connection.CreateDataTable().ToList<Item>();
		}

		private void AddItemGroup(DBConnection connection, List<Item> items)
		{
			connection.SQLCommand.CommandText = "SELECT [OITB].[ItmsGrpCod], \r\n\t\t\t\t\t\t\t[OITB].[ItmsGrpNam] \r\n\t\t\t\t\tFROM [OITB]";
			List<ItemGroup> itemGroups = connection.CreateDataTable().ToList<ItemGroup>();
			items.ForEach(delegate (Item i)
			{
				ItemGroup itemGroup = itemGroups.Find((ItemGroup g) => g.ItmsGrpCod.Equals(i.ItmsGrpCod));
				if (itemGroup != null)
				{
					i.ItemGroup = itemGroup;
				}
			});
		}

		private void AddManufacturer(DBConnection connection, List<Item> items)
		{
			connection.SQLCommand.CommandText = "SELECT [OMRC].[FirmCode], \r\n\t\t\t\t\t\t[OMRC].[FirmName] \r\n\t\t\t\t\tFROM [OMRC];";
			List<Manufacturer> itemGroups = connection.CreateDataTable().ToList<Manufacturer>();
			items.ForEach(delegate (Item i)
			{
				Manufacturer manufacturer = itemGroups.Find((Manufacturer g) => g.FirmCode == i.FirmCode);
				if (manufacturer != null)
				{
					i.ItemManufacturer = manufacturer;
				}
			});
		}
	}
}