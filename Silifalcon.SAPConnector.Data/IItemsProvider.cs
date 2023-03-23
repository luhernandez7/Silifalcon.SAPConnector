// Silifalcon.SAPConnector.Data.IItemsProvider<T>
using System.Collections.Generic;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Data
{
    public interface IItemsProvider<T> where T : Item, new()
    {
        List<T> Load(DBConnection connection, ItemsFilter args);
    }
}
