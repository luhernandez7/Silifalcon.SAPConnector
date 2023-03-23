using System.Collections.Generic;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Data
{
	public interface IBusinessPartnersProvider<T> where T : BusinessPartner, new()
	{
		List<T> Load(DBConnection connection, BusinessPartnersFilter args);
	}
}

