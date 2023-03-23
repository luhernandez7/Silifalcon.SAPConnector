// Silifalcon.SAPConnector.Data.Model.User<T>
using System.Collections.Generic;
using System.Linq;

public class User<T> where T : Warehouse
{
	public int USERID { get; set; }

	public string USER_CODE { get; set; }

	public string U_NAME { get; set; }

	public string E_Mail { get; set; }

	public string PASSWORD { get; set; }

	public List<T> Warehouses { get; set; }

	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}
		User<T> user = (User<T>)obj;
		return EqualityComparer<int>.Default.Equals(USERID, user.USERID) && EqualityComparer<string>.Default.Equals(USER_CODE, user.USER_CODE) && EqualityComparer<string>.Default.Equals(U_NAME, user.U_NAME) && EqualityComparer<string>.Default.Equals(E_Mail, user.E_Mail) && EqualityComparer<string>.Default.Equals(PASSWORD, user.PASSWORD) && ((Warehouses == null && user.Warehouses == null) || Warehouses.SequenceEqual(user.Warehouses));
	}

	public override int GetHashCode()
	{
		int num = 1079719150;
		num = num * -1521134295 + USERID.GetHashCode();
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(USER_CODE);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(U_NAME);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(E_Mail);
		num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PASSWORD);
		return num * -1521134295 + EqualityComparer<List<T>>.Default.GetHashCode(Warehouses);
	}
}
