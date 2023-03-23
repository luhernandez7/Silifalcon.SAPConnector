// Silifalcon.SAPConnector.Data.IItemPricesProvider<T>
using System.Collections.Generic;
using Silifalcon.SAPConnector.DBMS;

namespace Silifalcon.SAPConnector.Data
{
    public interface IItemPricesProvider<T> where T : ItemPrice, new()
    {
        List<T> Load(DBConnection connection, ItemPricesFilter args);
    }

}
