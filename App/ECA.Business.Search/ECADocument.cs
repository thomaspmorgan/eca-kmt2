using ECA.Core.DynamicLinq;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// An ECADocument represents a object that has been transformed into an indexable document
    /// for fast searching through a search engine, such as Azure Search.
    /// </summary>
    [SerializePropertyNamesAsCamelCase]
    public class ECADocument
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the document type name.
        /// </summary>
        public string DocumentTypeName { get; set; }

        /// <summary>
        /// Gets or sets the document type id.
        /// </summary>
        public string DocumentTypeId { get; set; }

        /// <summary>
        /// Gets or sets the office symbol.
        /// </summary>
        public string OfficeSymbol { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the points of contact.
        /// </summary>
        public IEnumerable<string> PointsOfContact { get; set; }

        /// <summary>
        /// Gets or sets the themes.
        /// </summary>
        public IEnumerable<string> Themes { get; set; }

        /// <summary>
        /// Gets or sets the goals.
        /// </summary>
        public IEnumerable<string> Goals { get; set; }

        /// <summary>
        /// Gets or sets the foci.
        /// </summary>
        public IEnumerable<string> Foci { get; set; }

        /// <summary>
        /// Gets or sets the objectives.
        /// </summary>
        public IEnumerable<string> Objectives { get; set; }

        /// <summary>
        /// Gets or sets the regions.
        /// </summary>
        public IEnumerable<string> Regions { get; set; }

        /// <summary>
        /// Gets or sets the countries.
        /// </summary>
        public IEnumerable<string> Countries { get; set; }

        /// <summary>
        /// Gets or sets the locations.
        /// </summary>
        public IEnumerable<string> Locations { get; set; }

        /// <summary>
        /// Gets or sets the websites.
        /// </summary>
        public IEnumerable<string> Websites { get; set; }

        /// <summary>
        /// Sets the Id and DocumentTypeId given the document key as a string.
        /// </summary>
        /// <param name="key">The document key.</param>
        public void SetKey(string key)
        {
            if(key != null)
            {
                SetKey(new DocumentKey(key));
            }
            else
            {
                DocumentKey documentKey = null;
                SetKey(documentKey);
            }
        }

        /// <summary>
        /// Sets the Id and DocumentTypeId of this document via the given document key.
        /// </summary>
        /// <param name="key">The document key.</param>
        public void SetKey(DocumentKey key)
        {
            if(key != null)
            {
                this.Id = key.ToString();
                this.DocumentTypeId = key.DocumentTypeId.ToString();
            }
            else
            {
                this.Id = null;
                this.DocumentTypeId = null;
            }
        }

        /// <summary>
        /// Returns the Id of this document as a Document Key.
        /// </summary>
        /// <returns>The DocumentKey or null if the Id is null.</returns>
        public DocumentKey GetKey()
        {
            if(this.Id == null)
            {
                return null;
            }
            else
            {
                return new DocumentKey(this.Id);
            }
        }        
    }

    /// <summary>
    /// An ECADocument that uses an IDocumentConfiguration and an instance of type T
    /// to initialize an ECADocument.
    /// </summary>
    /// <typeparam name="T">The object of type T that will become a document.</typeparam>
    public class ECADocument<T> : ECADocument where T : class
    {
        /// <summary>
        /// Creates and initializes a new ECADocument given an instance of type T and
        /// an associated IDocumentConfiguration that is registered for the type T.
        /// </summary>
        /// <param name="configuration">The IDocumentConfiguration that is registered for type T.</param>
        /// <param name="instance">The object instance of T.</param>
        public ECADocument(IDocumentConfiguration configuration, T instance)
        {
            Contract.Requires(configuration != null, "The configuration must not be null.");
            Contract.Requires(instance != null, "The instance must not be null.");
            Contract.Requires(configuration.IsConfigurationForType(typeof(T)), "The configuration must match the T instance.");
            Contract.Requires(configuration.GetDocumentTypeId() != Guid.Empty, "The document type id must be known and not the empty guid.");
            Contract.Requires(!String.IsNullOrWhiteSpace(configuration.GetDocumentTypeName()), "The document type name must be valid.");
            Contract.Requires(configuration.GetId(instance) != null, "The id must not be null.");
            Contract.Requires(configuration.GetDocumentTypeName() != null, "The document type name must not be null.");

            var documentTypeId = configuration.GetDocumentTypeId();
            var documentTypeName = configuration.GetDocumentTypeName();
            var key = new DocumentKey(documentTypeId, configuration.GetId(instance));
            SetKey(key);
            
            this.DocumentTypeName = documentTypeName;
            this.Name = configuration.GetName(instance);
            this.OfficeSymbol = configuration.GetOfficeSymbol(instance);
            this.Description = configuration.GetDescription(instance);
            this.Status = configuration.GetStatus(instance);
            this.Foci = configuration.GetFoci(instance);
            this.Goals = configuration.GetGoals(instance);
            this.Objectives = configuration.GetObjectives(instance);
            this.Themes = configuration.GetThemes(instance);
            this.PointsOfContact = configuration.GetPointsOfContact(instance);
            this.Websites = configuration.GetWebsites(instance);
            this.Regions = configuration.GetRegions(instance);
            this.Countries = configuration.GetCountries(instance);
            this.Locations = configuration.GetLocations(instance);
        }
    }
}
