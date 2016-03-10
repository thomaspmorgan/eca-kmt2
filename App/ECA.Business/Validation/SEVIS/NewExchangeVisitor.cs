using ECA.Business.Sevis.Model;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis
{
    //
    public class NewExchangeVisitor :
        ExchangeVisitorBase//,
        //IFormPrintable,
        //IUserIdentifiable,
        //IRequestIdentifiable
    {


        //#region IFormPrintable
        //public bool PrintForm { get; set; }
        //#endregion

        //#region  IRequestIdentifiable
        //public string RequestId { get; set; }

        //#endregion

        //#region IUserIdentifiable
        //public string UserId { get; set; }
        //#endregion


        public override ValidationResult Validate(IValidator validator)
        {
            throw new NotImplementedException();
        }


        public SEVISEVBatchTypeExchangeVisitor GetSEVISBatchTypeExchangeVisitor()
        {
            return new SEVISEVBatchTypeExchangeVisitor
            {

            };
        }
    }
}
