using Silifalcon.SAPConnector.Data;

namespace Silifalcon.SAPConnector.Data
{
	public abstract class DataProvider<TCurrencies, TBusinessPartners, TWarehouses, TManufacturers, TTaxGroups, TItems, TBatchs, TItemPrices, TSales, TSaleEmployees, TPurchases, TTransfers, TDeliveries, TGoodReceipts> where TCurrencies : Currency, new() where TBusinessPartners : BusinessPartner, new() where TWarehouses : Warehouse, new() where TManufacturers : Manufacturer, new() where TTaxGroups : TaxGroup, new() where TItems : Item, new() where TBatchs : Batch, new() where TItemPrices : ItemPrice, new() where TSales : Document, new() where TSaleEmployees : SalesEmployee, new() where TPurchases : Document, new() where TTransfers : Document, new() where TDeliveries : Document, new() where TGoodReceipts : Document, new()
	{
		public abstract ICurrenciesProvider<TCurrencies> Currencies { get; }

		public abstract IBusinessPartnersProvider<TBusinessPartners> BusinessPartners { get; }

		public abstract IWarehousesProvider<TWarehouses> Warehouses { get; }

		public abstract IManufacturersProvider<TManufacturers> Manufacturers { get; }

		public abstract ITaxGroupProvider<TTaxGroups> TaxGroups { get; }

		public abstract IItemsProvider<TItems> Items { get; }

		public abstract IBatchProvider<TBatchs> Batchs { get; }

		public abstract IItemPricesProvider<TItemPrices> ItemPrices { get; }

		public abstract ISalesProvider<TSales> Sales { get; }

		public abstract ISalesEmployeesProvider<TSaleEmployees> SaleEmployees { get; }

		public abstract IPurchasesProvider<TPurchases> Purchases { get; }

		public abstract ITransferProvider<TTransfers> Transfers { get; }

		public abstract IDeliveriesProvider<TDeliveries> Deliveries { get; }

		public abstract IGoodReceiptsProvider<TGoodReceipts> GoodReceipts { get; }
	}

}
