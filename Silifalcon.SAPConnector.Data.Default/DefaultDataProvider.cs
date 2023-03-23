// Silifalcon.SAPConnector.Data.Default.DefaultDataProvider
using Silifalcon.SAPConnector.Data;
using Silifalcon.SAPConnector.Data.Default;

namespace Silifalcon.SAPConnector.Data.Default
{
	public class DefaultDataProvider : DataProvider<Currency, BusinessPartner, Warehouse, Manufacturer, TaxGroup, Item, Batch, ItemPrice, Document, SalesEmployee, Document, Document, Document, Document>
	{
		private DefaultCurrenciesProvider currenciesProvider;

		private DefaultBusinessPartnersProvider businessPartnersProvider;

		private DefaultWarehousesProvider warehousesProvider;

		private DefaultManufacturersProvider manufacturersProvider;

		private DefaultTaxGroupProvider taxGroupsProvider;

		private DefaultItemsProvider itemsProvider;

		private DefaultBatchsProvider lotsProvider;

		private DefaultItemPricesProvider itemPricesProvider;

		private DefaultSalesProvider salesProvider;

		private DefaultSalesEmployeesProvider salesEmployeesProvider;

		private DefaultPurchasesProvider purchasesProvider;

		private DefaultTransferProvider transferRequestProvider;

		private DefaultDeliveriesProvider deliveriesProvider;

		private DefaultGoodReceiptsProvider goodReceiptsProvider;

		public static DefaultDataProvider Current { get; set; } = new DefaultDataProvider();


		public override ICurrenciesProvider<Currency> Currencies => currenciesProvider ?? (currenciesProvider = new DefaultCurrenciesProvider());

		public override IBusinessPartnersProvider<BusinessPartner> BusinessPartners => businessPartnersProvider ?? (businessPartnersProvider = new DefaultBusinessPartnersProvider());

		public override IWarehousesProvider<Warehouse> Warehouses => warehousesProvider ?? (warehousesProvider = new DefaultWarehousesProvider());

		public override IManufacturersProvider<Manufacturer> Manufacturers => manufacturersProvider ?? (manufacturersProvider = new DefaultManufacturersProvider());

		public override ITaxGroupProvider<TaxGroup> TaxGroups => taxGroupsProvider ?? (taxGroupsProvider = new DefaultTaxGroupProvider());

		public override IItemsProvider<Item> Items => itemsProvider ?? (itemsProvider = new DefaultItemsProvider());

		public override IBatchProvider<Batch> Batchs => lotsProvider ?? (lotsProvider = new DefaultBatchsProvider());

		public override IItemPricesProvider<ItemPrice> ItemPrices => itemPricesProvider ?? (itemPricesProvider = new DefaultItemPricesProvider());

		public override ISalesProvider<Document> Sales => salesProvider ?? (salesProvider = new DefaultSalesProvider());

		public override ISalesEmployeesProvider<SalesEmployee> SaleEmployees => salesEmployeesProvider ?? (salesEmployeesProvider = new DefaultSalesEmployeesProvider());

		public override IPurchasesProvider<Document> Purchases => purchasesProvider ?? (purchasesProvider = new DefaultPurchasesProvider());

		public override ITransferProvider<Document> Transfers => transferRequestProvider ?? (transferRequestProvider = new DefaultTransferProvider());

		public override IDeliveriesProvider<Document> Deliveries => deliveriesProvider ?? (deliveriesProvider = new DefaultDeliveriesProvider());

		public override IGoodReceiptsProvider<Document> GoodReceipts => goodReceiptsProvider ?? (goodReceiptsProvider = new DefaultGoodReceiptsProvider());
	}
}