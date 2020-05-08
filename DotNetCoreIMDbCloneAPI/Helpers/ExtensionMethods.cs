using DotNetCoreIMDbCloneAPI.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCoreIMDbCloneAPI.Helpers
{
    public static class ExtensionMethods
    {
        public static string Create(string value, string salt)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                                password: value,
                                salt: Encoding.UTF8.GetBytes(salt),
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 10000,
                                numBytesRequested: 256 / 8);

            return Convert.ToBase64String(valueBytes);
        }

        public static bool Validate(string value, string salt, string hash)
            => Create(value, salt) == hash;
        
        /*
        public static IEnumerable<User> WithoutPasswords(this IEnumerable<User> users)
        {
            return users.Select(x => x.WithoutPassword());
        }
        
        public static User WithoutPassword(this User user)
        {
            user.Password = null;
            return user;
        }
        */

        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string sortExpression) where TEntity : class
        {
            if (string.IsNullOrEmpty(sortExpression))
                return source; // nothing to sort on

            var entityType = typeof(TEntity);
            string ascSortMethodName = "OrderBy";
            string descSortMethodName = "OrderByDescending";
            string[] sortExpressionParts = sortExpression.Split(' ');
            string sortProperty = sortExpressionParts[0];
            string sortMethod = ascSortMethodName;

            if (sortExpressionParts.Length > 1 && sortExpressionParts[1] == "DESC")
                sortMethod = descSortMethodName;

            var property = entityType.GetProperty(sortProperty);
            var parameter = Expression.Parameter(entityType, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);

            MethodCallExpression resultExp = Expression.Call(
                                                typeof(Queryable),
                                                sortMethod,
                                                new Type[] { entityType, property.PropertyType },
                                                source.Expression,
                                                Expression.Quote(orderByExp));

            return source.Provider.CreateQuery<TEntity>(resultExp);
        }

        public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            var enumerator = query.Provider.Execute<IEnumerable<TEntity>>(query.Expression).GetEnumerator();
            var relationalCommandCache = enumerator.Private("_relationalCommandCache");
            var selectExpression = relationalCommandCache.Private<SelectExpression>("_selectExpression");
            var factory = relationalCommandCache.Private<IQuerySqlGeneratorFactory>("_querySqlGeneratorFactory");

            var sqlGenerator = factory.Create();
            var command = sqlGenerator.GetCommand(selectExpression);

            string sql = command.CommandText;
            return sql;
        }

        private static object Private(this object obj, string privateField) => obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
        private static T Private<T>(this object obj, string privateField) => (T)obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
    }
}
