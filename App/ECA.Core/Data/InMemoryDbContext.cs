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
        /// <summary>
        /// This is very slow
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateInMemoryContext<T>() where T : DbContext
        {
            var instance = Activator.CreateInstance<T>();
            var dbSetProperties = typeof(T).GetProperties().Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>)).ToList();
            foreach (var dbSetProperty in dbSetProperties)
            {
                var setType = dbSetProperty.PropertyType.GetGenericArguments()[0];
                var newDbSetType = typeof(TestDbSet<>).MakeGenericType(setType);
                var newDbSet = Activator.CreateInstance(newDbSetType);
                dbSetProperty.SetValue(instance, newDbSet);
            }
            return instance;
        }
    }
}
