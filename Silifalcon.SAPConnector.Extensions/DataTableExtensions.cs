// Silifalcon.SAPConnector.Extensions.DataTableExtensions
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Threading;

public static class DataTableExtensions
{
	public static List<T> ToList<T>(this DataTable table) where T : class, new()
	{
		Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture;
		try
		{
			List<T> list = new List<T>();
			foreach (DataRow item in table.AsEnumerable())
			{
				T val = new T();
				PropertyInfo[] properties = val.GetType().GetProperties();
				foreach (PropertyInfo propertyInfo in properties)
				{
					if (item.Table.Columns.Contains(propertyInfo.Name))
					{
						try
						{
							PropertyInfo property = val.GetType().GetProperty(propertyInfo.Name);
							property.SetValue(val, Convert.ChangeType(item[propertyInfo.Name], property.PropertyType), null);
						}
						catch
						{
						}
					}
				}
				list.Add(val);
			}
			return list;
		}
		catch
		{
			return null;
		}
	}
}
