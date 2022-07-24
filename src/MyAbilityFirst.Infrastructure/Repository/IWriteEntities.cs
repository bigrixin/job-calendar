using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyAbilityFirst.Infrastructure
{
	public interface IWriteEntities : IReadEntities, IUnitOfWork
	{
		void Create<TEntity>(TEntity entity) where TEntity : class;
		void Update<TEntity>(TEntity entity) where TEntity : class;
		void Delete<TEntity>(TEntity entity) where TEntity : class;
		List<TEntity> SqlQuery<TEntity>(string sql, params object[] parameters) where TEntity : class;
		List<int> SqlQuery(string sql, params object[] parameters);
	}
}