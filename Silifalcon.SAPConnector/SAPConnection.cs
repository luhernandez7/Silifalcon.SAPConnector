using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using log4net;
using SAPbobsCOM;

namespace Silifalcon.SAPConnector
{
    public class SAPConnection : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Company Company { get; private set; }

        public SAPConnection()
        {
            GetValueConnect();
            log.Info("Connecting to SAP.");
            Company = new Company();
            Company.SLDServer = SAPSettings.Current.SLDServer;
            Company.LicenseServer = SAPSettings.Current.LicenseServer;
            Company.Server = SAPSettings.Current.Server;
            Company.CompanyDB = SAPSettings.Current.CompanyDB;
            Company.UserName = SAPSettings.Current.UserName;
            Company.Password = SAPSettings.Current.Password;
            Company.DbUserName = SAPSettings.Current.DbUserName;
            Company.DbPassword = SAPSettings.Current.DbPassword;
            Company.UseTrusted = SAPSettings.Current.UseTrusted;
            Company.DbServerType = SAPSettings.Current.DbServerType;
            int errCode = Company.Connect();
            CheckResponse(errCode);
            if (errCode != 0)
            {
                Company.GetLastError(out errCode, out var errMsg);
                SAPConnectionException ex2 = new SAPConnectionException(errCode, errMsg);
                log.Error(ex2.Message, ex2);
                Debug.WriteLine(ex2.Message);
                throw ex2;
            }
            log.Info("Successful connection with SAP.");
        }

        private void GetValueConnect()
        {
            if (SAPSettings.Current == null)
            {
                SAPSettings.Current = new SAPSettings
                {
                    SLDServer = ConfigurationManager.AppSettings["SLDServer"],
                    LicenseServer = ConfigurationManager.AppSettings["LicenseServer"],
                    Server = ConfigurationManager.AppSettings["Server"],
                    CompanyDB = ConfigurationManager.AppSettings["CompanyDB"],
                    UserName = ConfigurationManager.AppSettings["UserName"],
                    Password = ConfigurationManager.AppSettings["Password"],
                    DbUserName = ConfigurationManager.AppSettings["DbUserName"],
                    DbPassword = ConfigurationManager.AppSettings["DbPassword"],
                    UseTrusted = ConfigurationManager.AppSettings["UseTrusted"].Equals("True"),
                    DbServerType = ConfigurationManager.AppSettings["DbServerType"].Equals("MSSQL2019")
                    ? BoDataServerTypes.dst_MSSQL2019
                    : ConfigurationManager.AppSettings["DbServerType"].Equals("MSSQL2017")
                    ? BoDataServerTypes.dst_MSSQL2017
                    : ConfigurationManager.AppSettings["DbServerType"].Equals("MSSQL2016")
                    ? BoDataServerTypes.dst_MSSQL2016
                    : ConfigurationManager.AppSettings["DbServerType"].Equals("MSSQL2014")
                    ? BoDataServerTypes.dst_MSSQL2014
                    : ConfigurationManager.AppSettings["DbServerType"].Equals("MSSQL2012")
                    ? BoDataServerTypes.dst_MSSQL2012
                    : BoDataServerTypes.dst_HANADB
                };
            }
        }

        public void CheckResponse(int response)
        {
            if (response == 0)
            {
                log.Info("Success action");
                Debug.WriteLine("Success action");
                return;
            }
            Company.GetLastError(out response, out var errMsg);
            SAPException ex = new SAPException(response, errMsg);
            log.Error(ex.Message, ex);
            throw ex;
        }

        public void Dispose()
        {
            Dispose(x: false);
        }

        protected virtual void Dispose(bool x)
        {
            if (Company.Connected)
            {
                log.Info("Disconnecting");
                Debug.WriteLine("Disconnecting");
                Company.Disconnect();
            }
            GC.SuppressFinalize(this);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }

}
