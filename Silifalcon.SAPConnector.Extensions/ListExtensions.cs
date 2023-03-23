// Silifalcon.SAPConnector.Extensions.ListExtensions
using System.Collections.Generic;

public static class ListExtensions
{
	public static T First<T>(this List<T> list) where T : class, new()
	{
		return (list.Count == 0) ? null : First(list);
	}
}
