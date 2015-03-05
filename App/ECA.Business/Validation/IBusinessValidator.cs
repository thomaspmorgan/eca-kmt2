using System;
using System.Linq;
using System.Collections.Generic;
using ECA.Business.Exceptions;
namespace ECA.Business.Validation
{
    public interface IBusinessValidator<TCreate, TUpdate> 
        where TCreate : class
        where TUpdate : class
    {
        IEnumerable<BusinessValidationResult> ValidateCreate(TCreate validationEntity);

        IEnumerable<BusinessValidationResult> ValidateUpdate(TUpdate validationEntity);
    }

    public abstract class BusinessValidator<TCreate, TUpdate> : IBusinessValidator<TCreate, TUpdate>
        where TCreate : class
        where TUpdate : class
    {
        private bool throwExceptionOnValidation;

        public BusinessValidator(bool throwExceptionOnValidation = true)
        {
            this.throwExceptionOnValidation = throwExceptionOnValidation;
        }

        public IEnumerable<BusinessValidationResult> ValidateCreate(TCreate validationEntity)
        {
            if (throwExceptionOnValidation)
            {
                var results = DoValidateCreate(validationEntity).ToList();
                DoThrowException(results);
                return results;
            }
            else
            {
                return DoValidateCreate(validationEntity);
            }
        }

        public IEnumerable<BusinessValidationResult> ValidateUpdate(TUpdate validationEntity)
        {
            if (throwExceptionOnValidation)
            {
                var results = DoValidateUpdate(validationEntity).ToList();
                DoThrowException(results);
                return results;
            }
            else
            {
                return DoValidateUpdate(validationEntity);
            }
            
        }

        private void DoThrowException(List<BusinessValidationResult> validationResults)
        {
            if (validationResults.Count > 0)
            {
                throw new ValidationException("There was an error validating the changes.", validationResults);
            }
        }

        public abstract IEnumerable<BusinessValidationResult> DoValidateCreate(TCreate validationEntity);

        public abstract IEnumerable<BusinessValidationResult> DoValidateUpdate(TUpdate validationEntity);
    }
}
