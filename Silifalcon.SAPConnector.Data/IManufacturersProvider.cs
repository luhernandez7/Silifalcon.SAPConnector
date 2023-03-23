// Silifalcon.SAPConnector.Data.IManufacturersProvider<T>
using System.Collections.Generic;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Data
{
    public interface IManufacturersProvider<T> where T : Manufacturer, new()
    {
        List<T> Load(DBConnection connection, ManufacturersFilter args);
    }

}
