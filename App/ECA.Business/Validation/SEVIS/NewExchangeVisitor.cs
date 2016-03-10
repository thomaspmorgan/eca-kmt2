using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Sevis.Bio;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis
{
    //
    public class NewExchangeVisitor :
        ExchangeVisitorBase
    {
        
        public override ValidationResult Validate(IValidator validator)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the Sevis model to create a new exchange visitor.
        /// </summary>
        /// <returns>The sevis model to create a new exchange visitor.</returns>
        public SEVISEVBatchTypeExchangeVisitor GetSEVISBatchTypeExchangeVisitor()
        {
            Contract.Requires(this.Person != null, "The person must not be null.");
            Contract.Requires(this.Person.FullName != null, "The person full name must not be null.");
            Contract.Requires(this.CategoryCode != null, "The category code must not be null.");
            var instance = new SEVISEVBatchTypeExchangeVisitor();
            instance.Biographical = this.Person.GetEVPersonTypeBiographical();
            instance.CategoryCode = this.CategoryCode.GetEVCategoryCodeType();
            SetDependents(instance);
            

            
            return instance;
        }

        private void SetDependents(SEVISEVBatchTypeExchangeVisitor instance)
        {
            var dependentsList = this.Dependents ?? new List<AddedDependent>();
            var addedDependents = new List<EVPersonTypeDependent>();
            foreach (var dependent in dependentsList)
            {
                if (dependent.GetType() != typeof(AddedDependent))
                {
                    throw new NotSupportedException("The dependent must be an added dependent.");
                }
                var addedDependent = (AddedDependent)dependent;
                addedDependents.Add(addedDependent.GetEVPersonTypeDependent());
            }
            instance.CreateDependent = addedDependents.ToArray();
        }
    }
}

