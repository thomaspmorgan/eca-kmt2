using ECA.Business.Queries.Models.Persons;
using ECA.Business.Sevis.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using FluentValidation.Results;

namespace ECA.Business.Validation.Sevis
{
    public class NewExchangeVisitor :
        IFormPrintable,
        IUserIdentifiable,
        IRequestIdentifiable,
        IBiographical
    {
        

        #region IFormPrintable
        public bool PrintForm { get; set; }
        #endregion

        #region  IRequestIdentifiable
        public string RequestId { get; set; }

        #endregion

        #region IUserIdentifiable
        public string UserId { get; set; }
        #endregion

        #region IBiographical
        public BiographicalDTO Biography { get; set; }
        #endregion

        #region Dependents
        public IEnumerable<BiographicalDTO> Dependents { get; set; }
        #endregion

        public SEVISEVBatchTypeExchangeVisitor GetSEVISBatchTypeExchangeVisitor()
        {
            return new SEVISEVBatchTypeExchangeVisitor
            {
                
            };
        }
    }
}
