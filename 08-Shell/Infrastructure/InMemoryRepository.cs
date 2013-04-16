using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Domain.Core.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure
{
    /// <summary>
    /// An in memory representation of a data store. This could as easily be an sql, file or key value store.
    /// The point of the repository pattern is to hide the technical details of the specific storage technology 
    /// from the from the application service.
    /// </summary>
    public sealed class InMemoryRepository : IRepository, IDataQuery
    {
        readonly Dictionary<Type, object> repository = new Dictionary<Type, object>();

        public IQueryable<TPersistable> GetQueryable<TPersistable>() where TPersistable : class
        {
            return GetCollection<TPersistable>().AsQueryable();
        }

        public TPersistable Get<TPersistable>(object id) where TPersistable : class
        {
            PropertyInfo keyProperty = GetKeyProperty<TPersistable>();
            Expression<Func<TPersistable, bool>> lambda = BuildLambdaExpressionForKey<TPersistable>(id, keyProperty);
            IQueryable<TPersistable> collectionQuery = GetCollection<TPersistable>().AsQueryable();

            return collectionQuery.Single(lambda);
        }

        private static Expression<Func<TPersistable, bool>> BuildLambdaExpressionForKey<TPersistable>(object id, PropertyInfo keyProperty) where TPersistable : class
        {
            ParameterExpression parameter = Expression.Parameter(typeof (TPersistable), "x");
            Expression property = Expression.Property(parameter, keyProperty.Name);
            Expression target = Expression.Constant(id);
            Expression equalsMethod = Expression.Equal(property, target);
            return Expression.Lambda<Func<TPersistable, bool>>(equalsMethod, parameter);
        }

        private static PropertyInfo GetKeyProperty<TPersistable>() where TPersistable : class
        {
            return typeof (TPersistable)
                .GetProperties()
                .Single(propertyInfo => Attribute.IsDefined(propertyInfo, typeof (KeyAttribute)));
        }

        public TPersistable Add<TPersistable>(TPersistable item) where TPersistable : class
        {
            HashSet<TPersistable> collection = GetCollection<TPersistable>();
            collection.Add(item);
            return item;
        }

        public TPersistable Remove<TPersistable>(TPersistable item) where TPersistable : class
        {
            HashSet<TPersistable> collection = GetCollection<TPersistable>();

            collection.Remove(item);
            return item;
        }

        private HashSet<TPersistable> GetCollection<TPersistable>() where TPersistable : class
        {
            if (!repository.ContainsKey(typeof(TPersistable)))
            {
                var collection = new HashSet<TPersistable>();
                repository.Add(typeof(TPersistable), collection);
                return collection;
            }

            return repository[typeof(TPersistable)] as HashSet<TPersistable>;
        }
    }
}
