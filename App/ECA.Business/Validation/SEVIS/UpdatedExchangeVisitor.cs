using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Sevis.Bio;

namespace ECA.Business.Validation.Sevis
{
    public interface ISevisExchangeVisitorUpdatableComponent
    {
        /// <summary>
        /// Returns the item representing the updated exchange visitor object.  This object is one of the following class types,
        /// SEVISEVBatchTypeExchangeVisitorBiographical,
        /// SEVISEVBatchTypeExchangeVisitorDependent,
        /// SEVISEVBatchTypeExchangeVisitorFinancialInfo, among others, that are not used currently.
        /// </summary>
        /// <returns>The updated exchange visitor object.</returns>

        object GetSevisEvBatchTypeExchangeVisitorUpdateComponent();
    }

    public abstract class UpdatedExchangeVisitor<T> 
        : IRequestIdentifiable,
        IUserIdentifiable,
        ISevisIdentifable
        where T : ISevisExchangeVisitorUpdatableComponent
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

        #region ISevisIdentifiable
        public string SevisId { get; set; }
        #endregion

        public T Item { get; set; }



        /// <summary>
        /// Returns the updated sevis batch exchange visitor instance.
        /// </summary>
        /// <returns></returns>
        public SEVISEVBatchTypeExchangeVisitor1 SEVISEVBatchTypeExchangeVisitor1()
        {
            return new Business.Sevis.Model.SEVISEVBatchTypeExchangeVisitor1
            {
                Item = this.Item.GetSevisEvBatchTypeExchangeVisitorUpdateComponent()
            };
        }
    }

    //public class UpdatedBiographicalExchangeVisitor : UpdatedPerson
    //{
    //    public BiographicalDTO Biography { get; set; }

    //    public override object GetItem()
    //    {
    //        throw new NotImplementedException();
    //        //return new SEVISEVBatchTypeExchangeVisitorBiographical
    //        //{

    //        //};
    //    }
    //}

    //public class AddedDependentExchangeVisitor : UpdatedExchangeVisitor, IBiographical
    //{
    //    public BiographicalDTO Biography { get; set; }

    //    public override object GetItem()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class UpdatedDependentExchangeVisitor : UpdatedExchangeVisitor, IBiographical
    //{
    //    public BiographicalDTO Biography { get; set; }

    //    public override object GetItem()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
