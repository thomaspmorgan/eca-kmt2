using ECA.Business.Sevis.Model;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECA.Business.Validation.Sevis
{

    public class UpdatedExchangeVisitor :
        ExchangeVisitorBase//,
        //IFormPrintable,
        //IUserIdentifiable,
        //IRequestIdentifiable,
        //ISevisIdentifable
    {

        //#region IFormPrintable
        //public bool PrintForm { get; set; }
        //#endregion

        //#region  IRequestIdentifiable
        //public string RequestId { get; set; }
        //#endregion

        //#region ISevisIdentifiable

        //public string SevisId { get; set; }
        //#endregion

        //#region IUserIdentifiable
        ///// <summary>
        ///// Gets or sets the user id.
        ///// </summary>
        //public string UserId { get; set; }
        //#endregion


        /// <summary>
        /// Returns all update sevis batch objects.  For example, if a participant has been sent to sevis and
        /// a name has changed, a dependent has been added, and another dependent has been updated.  This collection
        /// will contain all update sevis exchange visitor objects to perform those updates.
        /// </summary>
        /// <returns>All update sevis batch objects.</returns>
        public IEnumerable<SEVISEVBatchTypeExchangeVisitor1> GetSEVISEVBatchTypeExchangeVisitor1Collection()
        {
            return Enumerable.Empty<SEVISEVBatchTypeExchangeVisitor1>();
        }

        public override ValidationResult Validate(IValidator validator)
        {
            throw new NotImplementedException();
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ECA.Business.Queries.Models.Persons;
//using ECA.Business.Sevis.Model;
//using ECA.Business.Validation.Sevis.Bio;

//namespace ECA.Business.Validation.Sevis
//{
//    public interface ISevisExchangeVisitorUpdatableComponent
//    {
//        /// <summary>
//        /// Returns the item representing the updated exchange visitor object.  This object is one of the following class types,
//        /// SEVISEVBatchTypeExchangeVisitorBiographical,
//        /// SEVISEVBatchTypeExchangeVisitorDependent,
//        /// SEVISEVBatchTypeExchangeVisitorFinancialInfo, among others, that are not used currently.
//        /// </summary>
//        /// <returns>The updated exchange visitor object.</returns>

//        object GetSevisEvBatchTypeExchangeVisitorUpdateComponent();
//    }

//    public abstract class UpdatedExchangeVisitor<T> 
//        : IRequestIdentifiable,
//        IUserIdentifiable,
//        ISevisIdentifable
//        where T : ISevisExchangeVisitorUpdatableComponent
//    {
//        #region IFormPrintable
//        public bool PrintForm { get; set; }
//        #endregion

//        #region  IRequestIdentifiable
//        public string RequestId { get; set; }

//        #endregion

//        #region IUserIdentifiable
//        public string UserId { get; set; }
//        #endregion

//        #region ISevisIdentifiable
//        public string SevisId { get; set; }
//        #endregion

//        public T Item { get; set; }



//        /// <summary>
//        /// Returns the updated sevis batch exchange visitor instance.
//        /// </summary>
//        /// <returns></returns>
//        public SEVISEVBatchTypeExchangeVisitor1 SEVISEVBatchTypeExchangeVisitor1()
//        {
//            return new Business.Sevis.Model.SEVISEVBatchTypeExchangeVisitor1
//            {
//                Item = this.Item.GetSevisEvBatchTypeExchangeVisitorUpdateComponent()
//            };
//        }
//    }

//    //public class UpdatedBiographicalExchangeVisitor : UpdatedPerson
//    //{
//    //    public BiographicalDTO Biography { get; set; }

//    //    public override object GetItem()
//    //    {
//    //        throw new NotImplementedException();
//    //        //return new SEVISEVBatchTypeExchangeVisitorBiographical
//    //        //{

//    //        //};
//    //    }
//    //}

//    //public class AddedDependentExchangeVisitor : UpdatedExchangeVisitor, IBiographical
//    //{
//    //    public BiographicalDTO Biography { get; set; }

//    //    public override object GetItem()
//    //    {
//    //        throw new NotImplementedException();
//    //    }
//    //}

//    //public class UpdatedDependentExchangeVisitor : UpdatedExchangeVisitor, IBiographical
//    //{
//    //    public BiographicalDTO Biography { get; set; }

//    //    public override object GetItem()
//    //    {
//    //        throw new NotImplementedException();
//    //    }
//    //}
//}
