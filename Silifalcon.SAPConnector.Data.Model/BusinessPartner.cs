// Silifalcon.SAPConnector.Data.Model.BusinessPartner
using System.Collections.Generic;
using System.Linq;

public class BusinessPartner
{
	public string CardCode { get; set; }

	public string CardName { get; set; }

	public string ShipToDef { get; set; }

	public string BillToDef { get; set; }

	public string PymCode { get; set; }

	public int PaymentTermCode { get; set; }

	public List<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();


	public List<PaymentTerm> PaymentTerms { get; set; } = new List<PaymentTerm>();


	public List<Address> Address { get; set; } = new List<Address>();


	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		BusinessPartner businessPartner = (BusinessPartner)obj;
		return EqualityComparer<string>.Default.Equals(CardCode, businessPartner.CardCode) && EqualityComparer<string>.Default.Equals(CardName, businessPartner.CardName) && EqualityComparer<string>.Default.Equals(ShipToDef, businessPartner.ShipToDef) && EqualityComparer<string>.Default.Equals(BillToDef, businessPartner.BillToDef) && EqualityComparer<string>.Default.Equals(PymCode, businessPartner.PymCode) && EqualityComparer<int>.Default.Equals(PaymentTermCode, businessPartner.PaymentTermCode) && ((PaymentMethods == null && businessPartner.PaymentMethods == null) || PaymentMethods.SequenceEqual(businessPartner.PaymentMethods)) && ((PaymentTerms == null && businessPartner.PaymentTerms == null) || PaymentTerms.SequenceEqual(businessPartner.PaymentTerms)) && ((Address == null && businessPartner.Address == null) || Address.SequenceEqual(businessPartner.Address));
	}

	public override int GetHashCode()
	{
		int num = 2004550205;
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CardCode);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CardName);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ShipToDef);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BillToDef);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PymCode);
		num = num * -1521134295 + PaymentTermCode.GetHashCode();
		num = num * -1521134295 + EqualityComparer<List<PaymentMethod>>.Default.GetHashCode(PaymentMethods);
		num = num * -1521134295 + EqualityComparer<List<PaymentTerm>>.Default.GetHashCode(PaymentTerms);
		//return num * -1521134295 + EqualityComparer<List<Silifalcon.SAPConnector.Data.Model.Address>>.Default.GetHashCode(Address);
		return num * -1521134295 + EqualityComparer<List<Address>>.Default.GetHashCode(Address);
	}
}
