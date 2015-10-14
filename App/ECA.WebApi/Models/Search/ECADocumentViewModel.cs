using ECA.Business.Search;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Search
{
    /// <summary>
    /// An ECADocumentViewModel is a view model used to return an indexed document to a client of the web api.
    /// </summary>
    public class ECADocumentViewModel : ECADocument
    {
        /// <summary>
        /// Creates a new ECADocumentViewModel with the given ECADocument.
        /// </summary>
        /// <param name="ecaDocument">The eca document.</param>
        public ECADocumentViewModel(ECADocument ecaDocument)
        {
            Contract.Requires(ecaDocument != null, "The eca document must not be null.");
            this.Description = ecaDocument.Description;
            this.DocumentTypeId = ecaDocument.DocumentTypeId;
            this.DocumentTypeName = ecaDocument.DocumentTypeName;
            this.Foci = ecaDocument.Foci;
            this.Goals = ecaDocument.Goals;
            this.Id = ecaDocument.Id;
            this.Name = ecaDocument.Name;
            this.Objectives = ecaDocument.Objectives;
            this.OfficeSymbol = ecaDocument.OfficeSymbol;
            this.PointsOfContact = ecaDocument.PointsOfContact;
            this.Themes = ecaDocument.Themes;
            this.Websites = ecaDocument.Websites;
            this.Regions = ecaDocument.Regions;
            this.Countries = ecaDocument.Countries;
            this.Locations = ecaDocument.Locations;
            this.Status = ecaDocument.Status;
            this.StartDate = ecaDocument.StartDate;
            this.EndDate = ecaDocument.EndDate;
            this.Addresses = ecaDocument.Addresses;
            this.PhoneNumbers = ecaDocument.PhoneNumbers;
        }

        /// <summary>
        /// The document key.
        /// </summary>
        public DocumentKey Key
        {
            get
            {
                return this.GetKey();
            }
        }
    }
}