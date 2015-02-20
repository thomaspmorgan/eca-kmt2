using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Data
{
    public class InMemoryDbContext
    {
        public static T CreateInMemoryContext<T>() where T : DbContext
        {
            var instance = Activator.CreateInstance<T>();
            var dbSetProperties = typeof(T).GetProperties().Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>)).ToList();
            foreach (var dbSetProperty in dbSetProperties)
            {
                var underlyingType = dbSetProperty.PropertyType.GetGenericTypeDefinition();
                var genericType = dbSetProperty.PropertyType.MakeGenericType(underlyingType);
                dbSetProperty.SetValue(instance, genericType);
            }
            return instance;
        }
    }
}
