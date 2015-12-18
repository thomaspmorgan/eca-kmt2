using System;
using FluentValidation.Attributes;
using ECA.Business.Validation.Model.CreateEV;

namespace ECA.Business.Validation.Model.Shared
{
    public class ExchangeVisitorUpdate
    {
        public ExchangeVisitorUpdate()
        {
            Biographical = new Biographical();


        }
        
        /// <summary>
        /// Request identifier
        /// </summary>
        public string requestID { get; set; }

        /// <summary>
        /// SEVIS user id
        /// </summary>
        public string userID { get; set; }

        /// <summary>
        /// Status code of student
        /// </summary>
        public string statusCode { get; set; }

        /// <summary>
        /// User defined field A
        /// </summary>
        public string UserDefinedA { get; set; }

        /// <summary>
        /// User defined field B
        /// </summary>
        public string UserDefinedB { get; set; }

        /// <summary>
        /// Biographical information
        /// </summary>
        public Biographical Biographical { get; set; }

        /// <summary>
        /// Print request indicator
        /// </summary>
        public bool printForm { get; set; }





    }
}
