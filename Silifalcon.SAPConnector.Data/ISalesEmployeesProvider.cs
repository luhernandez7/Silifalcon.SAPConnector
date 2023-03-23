// Silifalcon.SAPConnector.Data.ISalesEmployeesProvider<T>
using System.Collections.Generic;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Data
{
    public interface ISalesEmployeesProvider<T> where T : SalesEmployee, new()
    {
        List<T> Load(DBConnection connection, SalesEmployeesFilter args);
    }

}
