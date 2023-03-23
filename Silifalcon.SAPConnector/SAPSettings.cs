using SAPbobsCOM;

namespace Silifalcon.SAPConnector
{
	public class SAPSettings
	{
		public string UserName { get; set; }

		public string Password { get; set; }

		public string Server { get; set; }

		public string SLDServer { get; set; }

		public string LicenseServer { get; set; }

		public string CompanyDB { get; set; }

		public string DbUserName { get; set; }

		public string DbPassword { get; set; }

		public bool UseTrusted { get; set; }

		public string DBServer { get; set; }

		public BoDataServerTypes DbServerType { get; set; }

		public static SAPSettings Current { get; set; }
	}

}
