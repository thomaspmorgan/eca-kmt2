using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Generation
{
    /// <summary>
    /// The DbContextStaticLookupValidator uses a DbContext to validate StaticLookups defined on a class exist in the database.
    /// </summary>
    public class DbContextStaticLookupValidator : IStaticGeneratorValidator, IDisposable
    {
        /// <summary>
        /// The format string for an error message detailing a value that does not exist in the database.
        /// </summary>
        public const string VALUE_DOES_NOT_EXIST_IN_DATABASE_FORMAT = "The static lookup with value [{0}] was not found in the database.";

        /// <summary>
        /// The format string for an error message detailing a static lookup that matches by id but whose value is different.
        /// </summary>
        public const string LOOKUP_VALUES_DO_NOT_MATCH_FORMAT = "The static lookup with value [{0}] is different than the database value [{1}].";

        private DbContext context;

        /// <summary>
        /// Creates a new validator.
        /// </summary>
        /// <param name="context">The context to validate against.</param>
        public DbContextStaticLookupValidator(DbContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            this.context = context;
        }

        /// <summary>
        /// Returns a collection fo error string detailing the static lookups configured for the class of type T.
        /// </summary>
        /// <typeparam name="T">The type of object to validate.</typeparam>
        /// <returns>The list of errors, or zero errors if there are no validation concerns.</returns>
        public List<string> Validate<T>() where T : class
        {
            var errors = new List<string>();
            var t = typeof(T);
            var staticProperties = t.GetProperties(BindingFlags.Static | BindingFlags.Public);
            var allStaticLookups = staticProperties.Select(x => x.GetValue(null) as StaticLookup).ToList();
            var idProperty = t.GetProperties().Where(x => x.Name.ToLower().Contains("id") && x.PropertyType == typeof(int)).SingleOrDefault();
            var nameProperty = t.GetProperties().Where(x => x.Name.ToLower().Contains("name") || t.Name.ToLower().Contains(x.Name.ToLower())).SingleOrDefault();
            var set = this.context.Set(t);
            foreach (var staticLookup in allStaticLookups)
            {
                var entity = set.Find(staticLookup.Id);
                if (entity == null)
                {
                    errors.Add(String.Format(VALUE_DOES_NOT_EXIST_IN_DATABASE_FORMAT, staticLookup.Value));
                }
                else
                {
                    var idValue = idProperty.GetValue(entity);
                    var nameValue = (string)nameProperty.GetValue(entity);
                    Debug.Assert(idValue != null, "The id must not be null.");
                    Debug.Assert(nameValue != null, "The nameValue must not be null.");
                    if (staticLookup.Value != nameValue)
                    {
                        errors.Add(String.Format(LOOKUP_VALUES_DO_NOT_MATCH_FORMAT, staticLookup.Value, nameValue));
                    }                    
                }
            }
            return errors;
        }


        #region IDispose

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.context.Dispose();
                this.context = null;
            }
        }

        #endregion
    }
}
