using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Data
{
    public partial class CamModel
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The key for retrieving the context in an IValidatatableObject instance.
        /// </summary>
        public const string VALIDATABLE_CONTEXT_KEY = "Context";

        public CamModel(string connectionStringOrKey)
            : base(connectionStringOrKey)
        {
            this.RandomNumber = Guid.NewGuid();
            logger.Info("Created cam model with id {0}.", this.RandomNumber);
        }

        public Guid RandomNumber { get; private set; }

        /// <summary>
        /// The ValidateEntity method override that addes this context to instance to the validation items.
        /// </summary>
        /// <param name="entityEntry">The entity entry to validate.</param>
        /// <param name="items">The items that will contain the DbContext.</param>
        /// <returns>The validation results.</returns>
        protected override System.Data.Entity.Validation.DbEntityValidationResult ValidateEntity(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            items.Add(VALIDATABLE_CONTEXT_KEY, this);
            return base.ValidateEntity(entityEntry, items);
        }
    }
}
