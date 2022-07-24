using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

public static class IQueryableExtensions
{
	public static IQueryable<T> OrderBy<T, TKey>(this IQueryable<T> query, Expression<Func<T, TKey>> keySelector, SortOrder sortOrder, ref bool firstSort)
	{
		if (keySelector == null || sortOrder == SortOrder.Unspecified)
			return query;

		IOrderedQueryable<T> res = query as IOrderedQueryable<T>;
		if (sortOrder == SortOrder.Descending)
		{
			if (firstSort) 
			{
				res = res.OrderByDescending(keySelector);
			}
			else 
			{
				res = res.ThenByDescending(keySelector);
			}
		}
		else
		{
			if (firstSort)
			{
				res = res.OrderBy(keySelector);
			}
			else
			{
				res = res.ThenBy(keySelector);
			}
		}
		firstSort = false;
		return res;
	}
}

