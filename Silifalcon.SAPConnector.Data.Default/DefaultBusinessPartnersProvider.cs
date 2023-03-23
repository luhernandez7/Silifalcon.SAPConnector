// Silifalcon.SAPConnector.Data.Default.DefaultBusinessPartnersProvider
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Silifalcon.SAPConnector.Data;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Data.Default
{
	public class DefaultBusinessPartnersProvider : IBusinessPartnersProvider<BusinessPartner>
	{
		public virtual List<BusinessPartner> Load(DBConnection connection, BusinessPartnersFilter args)
		{
			if (args == null)
			{
				throw new ArgumentException("The args parameter can not be null");
			}
			List<BusinessPartner> list = LoadBusinessPartner(connection, args);
			if (args.WithPaymentTerms)
			{
				AddPaymentTerms(connection, args, list);
			}
			if (args.WithPaymentMethods)
			{
				AddPaymentMethods(connection, args, list);
			}
			if (args.WithAddres)
			{
				AddAddress(connection, args, list);
			}
			return list;
		}

		private void AddAddress(DBConnection connection, BusinessPartnersFilter args, List<BusinessPartner> businessPartners)
		{
			string arg = "SELECT [OCRD].[CardCode]\r\n                FROM [OCRD]\r\n                WHERE " + GetMainFilter(args);
			connection.SQLCommand.CommandText = $"SELECT [CRD1].[Address] [Code], [CRD1].[CardCode], [CRD1].[TaxCode], [CRD1].[AdresType]\r\n\t\t\t\tFROM [CRD1]\r\n\t\t\t\tWHERE [CRD1].[CardCode]\tIN ( {arg} )";
			AddMainParameters(connection, args);
			List<Address> address = connection.CreateDataTable().ToList<Address>();
			businessPartners.ForEach(delegate (BusinessPartner x)
			{
				x.Address = address.Where((Address y) => y.CardCode == x.CardCode).ToList();
			});
		}

		private string GetMainFilter(BusinessPartnersFilter args)
		{
			string text = "";
			if (!string.IsNullOrEmpty(args.CardType))
			{
				text += "[OCRD].[CardType] = @CARD_TYPE";
			}
			if (!string.IsNullOrEmpty(args.CardCode))
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += " AND";
				}
				text += " [OCRD].[CardCode] = @CARD_CODE";
			}
			_ = args.UpdateDateFrom;
			if (true)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += " AND";
				}
				text += " COALESCE([OCRD].[UpdateDate], [OCRD].[CreateDate]) >= CAST(@FROM_DATE AS DATE)";
			}
			return text;
		}

		private void AddMainParameters(DBConnection connection, BusinessPartnersFilter args)
		{
			if (!string.IsNullOrEmpty(args.CardType))
			{
				connection.SQLCommand.Parameters.Add("@CARD_TYPE", SqlDbType.VarChar, 50).Value = args.CardType;
			}
			if (!string.IsNullOrEmpty(args.CardCode))
			{
				connection.SQLCommand.Parameters.Add("@CARD_CODE", SqlDbType.VarChar, 50).Value = args.CardCode;
			}
			_ = args.UpdateDateFrom;
			if (true)
			{
				connection.SQLCommand.Parameters.Add("@FROM_DATE", SqlDbType.DateTime).Value = args.UpdateDateFrom;
			}
		}

		private List<BusinessPartner> LoadBusinessPartner(DBConnection connection, BusinessPartnersFilter args)
		{
			connection.SQLCommand.CommandText = "SELECT [OCRD].[CardCode],\r\n\t\t                [OCRD].[CardName],\r\n\t\t\t\t\t\t[OCRD].[PymCode],\r\n\t\t\t\t\t\t[OCRD].[ShipToDef], \r\n\t\t\t\t\t\t[OCRD].[BillToDef]\r\n                FROM [OCRD]\r\n                WHERE " + GetMainFilter(args);
			AddMainParameters(connection, args);
			return connection.CreateDataTable().ToList<BusinessPartner>();
		}

		private void AddPaymentTerms(DBConnection connection, BusinessPartnersFilter args, List<BusinessPartner> businessPartners)
		{
			connection.SQLCommand.CommandText = "SELECT [OCTG].[GroupNum], [OCTG].[PymntGroup]\r\n\t\t\t\t\t\tFROM [OCTG]";
			List<PaymentTerm> paymentTerms = connection.CreateDataTable().ToList<PaymentTerm>();
			connection.SQLCommand.CommandText = "SELECT [OCRD].[CardCode], [OCRD].[GroupNum] \r\n\t\t\t\t\tFROM [OCRD]\r\n\t\t\t\t\tWHERE [OCRD].[CardCode] IN (\r\n\t\t\t\t\t\tSELECT [OCRD].[CardCode] \r\n\t\t\t\t\t\tFROM [OCRD] \r\n\t\t\t\t\t\tWHERE " + GetMainFilter(args) + " )";
			AddMainParameters(connection, args);
			DataTable dt = connection.CreateDataTable();
			businessPartners.ForEach(delegate (BusinessPartner x)
			{
				x.PaymentTerms = paymentTerms;
				DataRow dataRow = dt.AsEnumerable().SingleOrDefault((DataRow d) => d.Field<string>("CardCode").Equals(x.CardCode));
				if (dataRow != null)
				{
					x.PaymentTermCode = int.Parse(dataRow[1].ToString());
				}
			});
		}

		private void AddPaymentMethods(DBConnection connection, BusinessPartnersFilter args, List<BusinessPartner> businessPartners)
		{
			connection.SQLCommand.CommandText = "SELECT [OPYM].[PayMethCod], \r\n\t\t\t\t\t\t\t[OPYM].[Descript], \r\n\t\t\t\t\t\t\t[OPYM].[Active] \r\n\t\t\t\t\tFROM [OPYM]\r\n\t\t\t\t\tWHERE [OPYM].[Type] = 'I'";
			List<PaymentMethod> paymentMethods = connection.CreateDataTable().ToList<PaymentMethod>();
			StringBuilder codes = new StringBuilder();
			businessPartners.ForEach(delegate (BusinessPartner x)
			{
				if (codes.Length > 0)
				{
					codes.Append(",");
				}
				codes.Append("'" + x.CardCode.Replace("'", "''") + "'");
			});
			connection.SQLCommand.CommandText = "SELECT [CRD2].[CardCode], [CRD2].[PymCode]\r\n\t\t\t\t\tFROM [CRD2]\r\n\t\t\t\t\twhere [CRD2].[CardCode] IN ( \r\n\t\t\t\t\t   SELECT[OCRD].[CardCode]\r\n\t\t\t\t\t   FROM[OCRD]\r\n\t\t\t\t\t   WHERE " + GetMainFilter(args) + " )";
			AddMainParameters(connection, args);
			DataTable dt = connection.CreateDataTable();
			businessPartners.ForEach(delegate (BusinessPartner x)
			{
				List<PaymentMethod> payments = new List<PaymentMethod>();
				paymentMethods.ForEach(delegate (PaymentMethod pm)
				{
					DataRow[] source = dt.Select("CardCode  = '" + x.CardCode + "' and PymCode = '" + pm.PayMethCod + "'");
					payments.Add(new PaymentMethod
					{
						PayMethCod = pm.PayMethCod,
						Descript = pm.Descript,
						Active = pm.Active,
						Include = (source.Count() > 0)
					});
				});
				x.PaymentMethods = payments;
			});
		}
	}
}

