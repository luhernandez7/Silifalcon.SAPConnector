// Silifalcon.SAPConnector.Data.Default.DefaultCurrenciesProvider
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Silifalcon.SAPConnector.Data;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Data.Default
{
	public class DefaultCurrenciesProvider : ICurrenciesProvider<Currency>
	{
		public virtual List<Currency> Load(DBConnection connection, CurrenciesFilter args)
		{
			if (args == null)
			{
				throw new ArgumentException("The args parameter can not be null");
			}
			List<Currency> list = LoadCurrenysByUpdateDate(connection, args);
			if (args.WithCurrentRate)
			{
				LoadCurrentRates(connection, list);
			}
			return list;
		}

		private List<Currency> LoadCurrenysByUpdateDate(DBConnection connection, CurrenciesFilter args)
		{
			connection.SQLCommand.CommandText = "SELECT [OADM].[MainCurncy] FROM [OADM]";
			DataTable dataTable = connection.CreateDataTable();
			string defaultCurrencie = "";
			if (dataTable.Rows.Count > 0)
			{
				defaultCurrencie = dataTable.Rows[0][0].ToString();
			}
			connection.SQLCommand.CommandText = "SELECT [OCRN].[CurrCode], [OCRN].[CurrName]\r\n\t\t\t\t\tFROM [OCRN]";
			List<Currency> list = connection.CreateDataTable().ToList<Currency>();
			if (!string.IsNullOrEmpty(defaultCurrencie))
			{
				list.ForEach(delegate (Currency x)
				{
					x.Default = defaultCurrencie.Equals(x.CurrCode);
				});
			}
			return list;
		}

		private void LoadCurrentRates(DBConnection connection, List<Currency> currencys)
		{
			connection.SQLCommand.CommandText = "SELECT [ORTT].[Currency], [ORTT].[Rate] \r\n\t\t\t\tFROM [ORTT]\r\n\t\t\t\tWHERE [ORTT].[RateDate] = CAST(GETDATE() AS DATE)";
			List<ExchangeRate> rates = connection.CreateDataTable().ToList<ExchangeRate>();
			currencys.ForEach(delegate (Currency x)
			{
				x.Rate = (from y in rates
						  where y.Currency == x.CurrCode
						  select y.Rate).FirstOrDefault();
			});
		}
	}
}

