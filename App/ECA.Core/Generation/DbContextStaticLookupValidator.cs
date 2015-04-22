using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

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
        public const string VALUE_DOES_NOT_EXIST_IN_DATABASE_FORMAT = "The static lookup with value [{0}].[{1}] was not found in the database.";

        /// <summary>
        /// The format string for an error message detailing a static lookup that matches by id but whose value is different.
        /// </summary>
        public const string LOOKUP_VALUES_DO_NOT_MATCH_FORMAT = "The static lookup with value [{0}].[{1}] is different than the database value [{2}].";

        /// <summary>
        /// The format string for an error message detailing a lookup value in the database is not in the code.
        /// </summary>
        public const string LOOKUP_FROM_DATABASE_IS_NOT_KNOWN_FORMAT = "The database table [{0}] has a value [{1}] which is not in the [{2}] type.";

        private static readonly string COMPONENT_NAME = typeof(DbContextStaticLookupValidator).Name;

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
        public List<string> Validate<T>() where T : class, IStaticLookup
        {
            var stopWatch = Stopwatch.StartNew();

            var errors = new List<string>();
            var t = typeof(T);
            var instance = (IStaticLookup)Activator.CreateInstance(t);
            var config = instance.GetConfig();
            var staticProperties = t.GetProperties(BindingFlags.Static | BindingFlags.Public);
            var allStaticLookups = staticProperties.Select(x => x.GetValue(null) as StaticLookup).ToList();
            var connectionString = this.context.Database.Connection.ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (var lookup in allStaticLookups)
                {   
                    var getByIdSql = String.Format("SELECT {0} AS Id, {1} AS Value FROM {2} WHERE {0} = {3}", config.IdColumnName, config.ValueColumnName, config.TableName, lookup.Id);
                    using (var sqlCommand = new SqlCommand(getByIdSql, connection))
                    {
                        using (var reader = sqlCommand.ExecuteReader())
                        {
                            reader.Read();
                            if (reader.HasRows)
                            {
                                var value = (string)reader["Value"];
                                if (lookup.Value != value)
                                {
                                    errors.Add(String.Format(LOOKUP_VALUES_DO_NOT_MATCH_FORMAT, t.Name, lookup.Value, value));
                                }
                            }
                            else
                            {
                                errors.Add(String.Format(VALUE_DOES_NOT_EXIST_IN_DATABASE_FORMAT, t.Name, lookup.Value));
                            }
                        }
                    }
                }
                var getAllLookupsSql = String.Format("SELECT {0} AS Id, {1} AS Value FROM {2}", config.IdColumnName, config.ValueColumnName, config.TableName);
                using (var sqlCommand = new SqlCommand(getAllLookupsSql, connection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        reader.Read();
                        if (reader.HasRows)
                        {
                            var id = (int)reader["Id"];
                            var value = (string)reader["Value"];
                            var lookupById = allStaticLookups.Where(x => x.Id == id).FirstOrDefault();
                            var lookupByValue = allStaticLookups.Where(x => x.Value == value).FirstOrDefault();
                            if (lookupById == null)
                            {
                                errors.Add(String.Format(LOOKUP_FROM_DATABASE_IS_NOT_KNOWN_FORMAT, config.TableName, id, t.Name));
                            }
                            if (lookupByValue == null)
                            {
                                errors.Add(String.Format(LOOKUP_FROM_DATABASE_IS_NOT_KNOWN_FORMAT, config.TableName, value, t.Name));
                            }
                        }
                    }
                }
                stopWatch.Stop();
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
