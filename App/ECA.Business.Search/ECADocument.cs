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

    public class ECADocument : Document
    {
        public const string ID_KEY = "id";
        public const string TITLE_KEY = "title";
        public const string SUBTITLE_KEY = "subtitle";
        public const string DESCRIPTION_KEY = "desc";
        public const string DOCUMENT_TYPE_ID_KEY = "typeId";

        public DocumentKey Key { get; private set; }

        public DocumentType DocumentType { get; private set; }

        public List<string> AdditionalFields { get; private set; }


        protected void SetKey(DocumentKey key)
        {
            this.Key = key;
            this.Add(ID_KEY, key.ToString());
        }

        protected void SetTitle(string title)
        {
            this.Add(TITLE_KEY, title);
        }

        protected void SetDescription(string description)
        {
            this.Add(DESCRIPTION_KEY, description);
        }

        protected void SetSubtitle(string subtitle)
        {
            this.Add(SUBTITLE_KEY, subtitle);
        }

        protected void SetDocumentType(DocumentType documentType)
        {
            this.DocumentType = documentType;
            this.Add(DOCUMENT_TYPE_ID_KEY, documentType.Id.ToString());
        }

        protected void SetAdditionalFields(Dictionary<string, string> additionalFields)
        {
            this.AdditionalFields = additionalFields.Keys.ToList();
            foreach (var key in additionalFields.Keys)
            {
                this.Add(key, additionalFields[key]);
            }
        }
    }

    //public class SimpleDocument : ECADocument
    //{
    //    public SimpleDocument(IDocumentable documentable)
    //    {
    //        Contract.Requires(documentable != null, "The documentable object must not be null.");
    //        var key = new DocumentKey(documentable);
    //        SetKey(key);
    //        SetTitle(documentable.GetTitle());
    //        SetDescription(documentable.GetDescription());
    //        SetSubtitle(documentable.GetSubtitle());
    //        SetDocumentType(documentable.GetDocumentType());
    //        var dictionary = new Dictionary<string, string>();
    //        foreach (var field in documentable.GetDocumentFields())
    //        {
    //            dictionary[field] = documentable.GetValue(field);
    //        }
    //        SetAdditionalFields(dictionary);
    //    }
    //}

    public class ConfiguredDocument<T> : ECADocument where T : class
    {
        public ConfiguredDocument(IDocumentConfiguration configuration, T instance)
        {
            Contract.Requires(configuration.IsConfigurationForType(typeof(T)), "The configuration must match the T instance.");
            var documentType = configuration.GetDocumentType();
            var key = new DocumentKey(documentType, configuration.GetId(instance));
            SetDocumentType(documentType);
            SetKey(key);
            SetTitle(configuration.GetTitle(instance));
            SetDescription(configuration.GetDescription(instance));
            SetSubtitle(configuration.GetSubtitle(instance));
            SetAdditionalFields(configuration.GetAdditionalFields(instance));
        }
    }

    //public class ECADocument<TEntity, TEntityKey> : ECADocument
    //    where TEntity : class
    //    where TEntityKey : struct
    //{
    //    private TEntity instance;

    //    private Func<TEntity, TEntityKey> idDelegate;

    //    public ECADocument(TEntity instance, 
    //        DocumentType documentType,
    //        Expression<Func<TEntity, TEntityKey>> idSelector, 
    //        Expression<Func<TEntity, string>> titleSelector,
    //        Expression<Func<TEntity, string>> subtitleSelector,
    //        Expression<Func<TEntity, string>> descriptionSelector,
    //        params Expression<Func<TEntity, string>>[] fieldSelectors)
    //    {

    //        SetTitle(titleSelector.Compile()(instance));
    //        SetDescription(descriptionSelector.Compile()(instance));
    //        SetSubtitle(subtitleSelector.Compile()(instance));
    //        SetDocumentType(documentType);

    //        var id = idSelector.Compile()(instance);
    //        var key = new DocumentKey(documentType, id);
    //        SetKey(key);


    //    }
    //}

    //public class DocumentableBase<T, TKeyType> : Document

    //    where T : class
    //    where TKeyType : struct
    //{
    //    private T instance;
    //    private Func<T, TKeyType> idDelegate;
    //    private Dictionary<string, Func<T, string>> fieldDelegates;

    //    public DocumentableBase(T instance)
    //    {
    //        this.instance = instance;
    //        this.fieldDelegates = new Dictionary<string, Func<T, string>>();
    //    }

    //    public void HasKey(Expression<Func<T, TKeyType>> idSelector)
    //    {
    //        idDelegate = idSelector.Compile();
    //    }

    //    public void HasField(Expression<Func<T, string>> fieldSelector)
    //    {
    //        var propertyName = PropertyHelper.GetPropertyName<T>(fieldSelector);
    //        var propertyDelegate = fieldSelector.Compile();
    //        fieldDelegates.Add(propertyName, propertyDelegate);
    //    }

    //    public ECADocument ToDocument()
    //    {
    //        return new ECADocument
    //        {

    //        };

    //    }
    //}


    //public class ECADocument<T> : ECADocument where T : class, IDocumentable
    //{
    //    private IDocumentable documentable;
    //    private DocumentConfiguration<T> configuration;

    //    public ECADocument(T documentable, Action<DocumentConfiguration<T>> config)
    //    {
    //        Contract.Requires(documentable != null, "The documentable object must not be null.");
    //        Contract.Requires(configuration != null, "The configuration must not be null.");
    //        this.documentable = documentable;
    //        this.configuration = new DocumentConfiguration<T>();
    //        config(configuration);

    //    }

    //    public ECADocument(T documentable, DocumentConfiguration<T> configuration)
    //    {
    //        Contract.Requires(documentable != null, "The documentable object must not be null.");
    //        Contract.Requires(configuration != null, "The configuration must not be null.");
    //        this.documentable = documentable;
    //        this.configuration = configuration;
    //    }

    //    //public DocumentBase(IDocumentable document)
    //    //{
    //    //    this.fields = new List<string>();
    //    //}


    //    //public abstract string GetDescription();

    //    //public List<string> GetDocumentFields()
    //    //{
    //    //    return null;
    //    //}

    //    //public abstract DocumentType GetDocumentType();

    //    //public object GetId()
    //    //{
    //    //    if (idDelegate == null)
    //    //    {
    //    //        throw new NotSupportedException("The key has not been defined.");
    //    //    }
    //    //    return idDelegate(this);
    //    //}

    //    //public abstract string GetSubtitle();

    //    //public abstract string GetTitle();

    //    //public string GetValue(string field)
    //    //{
    //    //    var property = this.GetType().GetProperty(field);
    //    //    return property.GetValue(this).ToString();
    //    //}

    //    //public void HasKey(Expression<Func<T, string>> idSelector)
    //    //{
    //    //    var f = idSelector.Compile();
    //    //    this.idDelegate = f;
    //    //}
    //}
}
