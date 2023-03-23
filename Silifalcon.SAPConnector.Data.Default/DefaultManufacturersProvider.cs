// Silifalcon.SAPConnector.Data.Default.DefaultManufacturersProvider
using System;
using System.Collections.Generic;
using System.Linq;
using Silifalcon.SAPConnector.Data;
using Silifalcon.SAPConnector.Data.Default;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Data.Default
{
	public class DefaultManufacturersProvider : IManufacturersProvider<Manufacturer>
	{
		private class FirmCodeItemGroup
		{
			public int FirmCode { get; set; }

			public string ItmsGrpCod { get; set; }
		}

		public virtual List<Manufacturer> Load(DBConnection connection, ManufacturersFilter args)
		{
			if (args == null)
			{
				throw new ArgumentException("The args parameter can not be null");
			}
			List<Manufacturer> list = LoadByUpdatedDate(connection, args);
			if (args.WithGroups)
			{
				AddGroups(connection, list, args);
			}
			return list;
		}

		public virtual List<Manufacturer> LoadByUpdatedDate(DBConnection connection, ManufacturersFilter args)
		{
			connection.SQLCommand.CommandText = "SELECT [OMRC].[FirmCode], [OMRC].[FirmName] FROM [OMRC]";
			return connection.CreateDataTable().ToList<Manufacturer>();
		}

		public virtual void AddGroups(DBConnection connection, List<Manufacturer> items, ManufacturersFilter args)
		{
			connection.SQLCommand.CommandText = "SELECT [OITB].[ItmsGrpCod], [OITB].[ItmsGrpNam] FROM [OITB]";
			List<ItemGroup> itemGroups = connection.CreateDataTable().ToList<ItemGroup>();
			connection.SQLCommand.CommandText = "SELECT DISTINCT [OITM].[FirmCode], [OITM].[ItmsGrpCod] FROM [OITM]";
			List<FirmCodeItemGroup> relations = connection.CreateDataTable().ToList<FirmCodeItemGroup>();
			items.ForEach(delegate (Manufacturer x)
			{
				x.ItemGroups = (from y in relations
								where y.FirmCode == x.FirmCode
								select itemGroups.FirstOrDefault((ItemGroup z) => z.ItmsGrpCod == y.ItmsGrpCod) into y
								where y != null
								select y).ToList();
			});
		}
	}
}
