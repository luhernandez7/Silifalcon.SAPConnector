// Silifalcon.SAPConnector.Data.ITaxGroupProvider<T>
using System.Collections.Generic;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Data
{
    public interface ITaxGroupProvider<T> where T : TaxGroup, new()
    {
        List<T> Load(DBConnection connection, TaxGroupFilter args);
    }
}

