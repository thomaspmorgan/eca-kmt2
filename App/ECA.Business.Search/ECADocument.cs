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
    [SerializePropertyNamesAsCamelCase]
    public class ECADocument
    {
        public const string ID_KEY = "id";
        public const string NAME_KEY = "NAME";
        public const string DESCRIPTION_KEY = "desc";
        public const string THEMES_KEY = "themes";
        public const string GOALS_KEY = "goals";
        public const string FOCI_KEY = "foci";
        public const string OBJECTIVES_KEY = "objectives";
        public const string POINTS_OF_CONTACT_KEY = "pointsOfContact";
        

        public string Description { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> PointsOfContact { get; set; }

        public IEnumerable<string> Themes { get; set; }

        public IEnumerable<string> Goals { get; set; }

        public IEnumerable<string> Foci { get; set; }

        public IEnumerable<string> Objectives { get; set; }

        public void SetKey(string key)
        {
            if(key != null)
            {
                SetKey(new DocumentKey(key));
            }
            else
            {
                this.Id = null;
            }
        }

        public void SetKey(DocumentKey key)
        {
            if(key != null)
            {
                this.Id = key.ToString();
            }
            else
            {
                this.Id = null;
            }
        }

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

        public DocumentType GetDocumentType()
        {
            var key = GetKey();
            if(key == null)
            {
                return null;
            }
            else
            {
                return key.DocumentType;
            }
        }
    }

    public class ECADocument<T> : ECADocument where T : class
    {
        public ECADocument(IDocumentConfiguration configuration, T instance)
        {
            Contract.Requires(configuration != null, "The configuration must not be null.");
            Contract.Requires(instance != null, "The instance must not be null.");
            Contract.Requires(configuration.IsConfigurationForType(typeof(T)), "The configuration must match the T instance.");
            Contract.Requires(configuration.GetDocumentType() != null, "The document type must be known.");
            Contract.Requires(configuration.GetId(instance) != null, "The id must not be null.");
            var documentType = configuration.GetDocumentType();
            var key = new DocumentKey(documentType, configuration.GetId(instance));
            SetKey(key);
            this.Name = configuration.GetName(instance);
            this.Description = configuration.GetDescription(instance);
            this.Foci = configuration.GetFoci(instance);
            this.Goals = configuration.GetGoals(instance);
            this.Objectives = configuration.GetObjectives(instance);
            this.Themes = configuration.GetThemes(instance);
        }
    }
}
