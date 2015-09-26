using ECA.Core.DynamicLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    public interface IDocumentConfiguration
    {
        object GetId(object instance);

        string GetTitle(object instance);

        string GetDescription(object instance);

        string GetSubtitle(object instance);

        List<string> GetAdditionalFieldNames();

        Dictionary<string, string> GetAdditionalFields(object instance);

        bool IsConfigurationForType(Type type);

        DocumentType GetDocumentType();
    }

    
    public class DocumentConfiguration<TEntity, TEntityKey> : IDocumentConfiguration
        where TEntity : class
        where TEntityKey : struct
    {

        private DocumentType documentType;

        public DocumentConfiguration()
        {
            this.AdditionalFieldsDelegates = new Dictionary<string, Func<TEntity, string>>();
        }

        public Func<TEntity, TEntityKey> IdDelegate { get; private set; }

        public Func<TEntity, string> TitleDelegate { get; private set; }

        public Func<TEntity, string> DescriptionDelegate { get; private set; }

        public Func<TEntity, string> SubtitleDelegate { get; private set; }

        public Dictionary<string, Func<TEntity, string>> AdditionalFieldsDelegates { get; private set; }


        public DocumentType GetDocumentType()
        {
            return this.documentType;
        }

        public object GetId(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if(IdDelegate == null)
            {
                throw new NotSupportedException("The id has not been configured.  Use the HasKey method.");
            }
            return IdDelegate((TEntity)instance);
        }

        public void HasKey(Expression<Func<TEntity, TEntityKey>> idSelector)
        {
            Contract.Requires(idSelector != null, "The idSelector must not be null.");
            this.IdDelegate = idSelector.Compile();
        }

        public void HasTitle(Expression<Func<TEntity, string>> titleSelector)
        {
            Contract.Requires(titleSelector != null, "The titleSelector must not be null.");
            this.TitleDelegate = titleSelector.Compile();
        }

        public void HasSubtitle(Expression<Func<TEntity, string>> subtitleSelector)
        {
            Contract.Requires(subtitleSelector != null, "The subtitleSelector must not be null.");
            this.SubtitleDelegate = subtitleSelector.Compile();
        }

        public void HasDescription(Expression<Func<TEntity, string>> descriptionSelector)
        {
            Contract.Requires(descriptionSelector != null, "The descriptionSelector must not be null.");
            this.DescriptionDelegate = descriptionSelector.Compile();
        }

        public void HasAdditionalField(Expression<Func<TEntity, string>> additionalFieldSelector)
        {
            Contract.Requires(additionalFieldSelector != null, "The additionalFieldSelector must not be null.");
            var propertyName = PropertyHelper.GetPropertyName(additionalFieldSelector);
            HasAdditionalField(propertyName, additionalFieldSelector);
        }

        public void HasAdditionalField(string name, Expression<Func<TEntity, string>> additionalFieldSelector)
        {
            Contract.Requires(additionalFieldSelector != null, "The additionalFieldSelector must not be null.");
            Contract.Requires(!String.IsNullOrWhiteSpace(name), "The name of field is invalid.");
            var valueFn = additionalFieldSelector.Compile();
            AdditionalFieldsDelegates.Add(name, additionalFieldSelector.Compile());
        }

        public void HasAdditionalField<TCollectionType>(Expression<Func<TEntity, IEnumerable<TCollectionType>>> additionalFieldSelector, Expression<Func<TEntity, string>> valueSelector)
        {
            Contract.Requires(additionalFieldSelector != null, "The additionalFieldSelector must not be null.");
            Contract.Requires(valueSelector != null, "The valueSelector must not be null.");
            var propertyName = PropertyHelper.GetPropertyName(additionalFieldSelector);
            HasAdditionalField<TCollectionType>(propertyName, valueSelector);
        }

        public void HasAdditionalField<TCollectionType>(string name, Expression<Func<TEntity, string>> valueSelector)
        {
            Contract.Requires(valueSelector != null, "The valueSelector must not be null.");
            Contract.Requires(!String.IsNullOrWhiteSpace(name), "The name of field is invalid.");
            var valueFn = valueSelector.Compile();
            AdditionalFieldsDelegates.Add(name, valueFn);
        }

        public void IsDocumentType(DocumentType type)
        {
            Contract.Requires(type != null, "The document type must not be null.");
            this.documentType = type;
        }

        public bool IsConfigurationForType(Type type)
        {
            return typeof(TEntity) == type;
        }

        public string GetTitle(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if (TitleDelegate != null)
            {
                return TitleDelegate((TEntity)instance);
            }
            else
            {
                return null;
            }
        }

        public string GetDescription(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if (DescriptionDelegate != null)
            {
                return DescriptionDelegate((TEntity)instance);
            }
            else
            {
                return null;
            }
        }

        public string GetSubtitle(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if (SubtitleDelegate != null)
            {
                return SubtitleDelegate((TEntity)instance);

            }
            else
            {
                return null;
            }
        }

        public Dictionary<string, string> GetAdditionalFields(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            var dictionary = new Dictionary<string, string>();
            foreach (var key in AdditionalFieldsDelegates.Keys)
            {
                var fn = AdditionalFieldsDelegates[key];
                var value = fn((TEntity)instance);
                if(value != null)
                {
                    dictionary[key] = value;
                }
            }
            return dictionary;
        }

        public List<string> GetAdditionalFieldNames()
        {
            return this.AdditionalFieldsDelegates.Keys.ToList();
        }
    }
}
