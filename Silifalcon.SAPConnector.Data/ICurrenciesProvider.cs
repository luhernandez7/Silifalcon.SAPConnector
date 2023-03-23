// Silifalcon.SAPConnector.Data.ICurrenciesProvider<T>
using System.Collections.Generic;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Data
{
    public interface ICurrenciesProvider<T> where T : Currency, new()
    {
        List<T> Load(DBConnection connection, CurrenciesFilter args);
    }

}
