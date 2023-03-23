using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;
using log4net;
using Silifalcon.SAPConnector;

namespace Silifalcon.SAPConnector.DBMS
{
	public class DBConnection : IDisposable
	{
		private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private SqlConnection SQLConnection { get; set; }

		public SqlCommand SQLCommand { get; set; }

		public DBConnection()
		{
			if (SAPSettings.Current == null)
			{
				SAPException ex = new SAPException(-1, "SAPSettings.Current not defined");
				logger.Error(ex.Message, ex);
				Debug.WriteLine(ex.Message);
				throw ex;
			}
			Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Server=").Append(SAPSettings.Current.Server).Append(";");
			stringBuilder.Append("Database=").Append(SAPSettings.Current.CompanyDB).Append(";");
			if (SAPSettings.Current.UseTrusted)
			{
				stringBuilder.Append("Trusted_Connection=True");
			}
			else
			{
				stringBuilder.Append("User Id=").Append(SAPSettings.Current.DbUserName).Append(";");
				stringBuilder.Append("Password=").Append(SAPSettings.Current.DbPassword).Append(";");
			}
			SQLConnection = new SqlConnection
			{
				ConnectionString = stringBuilder.ToString()
			};
			SQLCommand = new SqlCommand
			{
				Connection = SQLConnection
			};
			SQLConnection.Open();
		}

		public DataTable CreateDataTable()
		{
			DataSet dataSet = new DataSet();
			SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
			{
				SelectCommand = SQLCommand
			};
			sqlDataAdapter.Fill(dataSet);
			SQLCommand = new SqlCommand
			{
				Connection = SQLCommand.Connection
			};
			return dataSet.Tables[0];
		}

		public void Dispose()
		{
			Dispose(dispose: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool dispose)
		{
			if (SQLConnection.State != 0)
			{
				SQLConnection.Close();
			}
		}
	}

}
