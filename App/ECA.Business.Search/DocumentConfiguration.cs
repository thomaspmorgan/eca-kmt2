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

        Dictionary<string, string> GetAdditionalFields(object instance);

        bool IsConfigurationForType(Type type);

        DocumentType GetDocumentType();

    }

    public interface IDocumentConfiguration<TEntity, TEntityKey> : IDocumentConfiguration
        where TEntity : class
        where TEntityKey : struct
    {
        Func<TEntity, TEntityKey> IdDelegate { get; }
    }

    public class DocumentConfiguration<TEntity, TEntityKey> : IDocumentConfiguration<TEntity, TEntityKey>
        where TEntity : class
        where TEntityKey : struct
    {

        private DocumentType documentType;

        public Func<TEntity, TEntityKey> IdDelegate { get; private set; }

        public Func<TEntity, string> TitleDelegate { get; private set; }

        public Func<TEntity, string> DescriptionDelegate { get; private set; }

        public Func<TEntity, string> SubtitleDelegate { get; private set; }

        public Dictionary<string, Func<TEntity, string>> AdditionalFieldsDelegates { get; private set; }

        public DocumentConfiguration()
        {
            this.AdditionalFieldsDelegates = new Dictionary<string, Func<TEntity, string>>();
        }

        public DocumentType GetDocumentType()
        {
            return this.documentType;
        }

        public object GetId(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            return IdDelegate((TEntity)instance);
        }

        public void HasKey(Expression<Func<TEntity, TEntityKey>> idSelector)
        {
            this.IdDelegate = idSelector.Compile();
        }

        public void HasTitle(Expression<Func<TEntity, string>> titleSelector)
        {
            this.TitleDelegate = titleSelector.Compile();
        }

        public void HasSubtitle(Expression<Func<TEntity, string>> subtitleSelector)
        {
            this.SubtitleDelegate = subtitleSelector.Compile();
        }

        public void HasDescription(Expression<Func<TEntity, string>> descriptionSelector)
        {
            this.DescriptionDelegate = descriptionSelector.Compile();
        }

        public void HasAdditionalField(Expression<Func<TEntity, string>> additionalFieldSelector)
        {
            var propertyName = PropertyHelper.GetPropertyName(additionalFieldSelector);
            var valueFn = additionalFieldSelector.Compile();
            AdditionalFieldsDelegates.Add(propertyName, additionalFieldSelector.Compile());
        }

        public void IsDocumentType(DocumentType type)
        {
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
                dictionary[key] = fn((TEntity)instance);
            }
            return dictionary;
        }
    }


    //public class DocumentConfiguration<TEntity, TEntityKey> 
    //    where TEntity : class
    //    where TEntityKey : struct
    //{
    //    public DocumentConfiguration()
    //    {
    //        this.FieldDelegates = new Dictionary<string, Func<TEntity, string>>();
    //    }

    //    public Func<TEntity, TEntityKey> IdDelegate { get; private set; }

    //    public Dictionary<string, Func<TEntity,string>> FieldDelegates { get; private set; }

    //    public void HasKey(Expression<Func<TEntity, TEntityKey>> idSelector)
    //    {
    //        this.IdDelegate = idSelector.Compile();
    //    }

    //    public void HasField(Expression<Func<TEntity, string>> fieldSelector)
    //    {
    //        var propertyName = PropertyHelper.GetPropertyName<TEntity>(fieldSelector);
    //        var propertyDelegate = fieldSelector.Compile();
    //        this.FieldDelegates.Add(propertyName, propertyDelegate);
    //    }
    //}
}
