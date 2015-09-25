using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    public interface IDocumentable
    {
        DocumentType GetDocumentType();

        List<string> GetDocumentFields();

        string GetValue(string field);

        object GetId();

        string GetTitle();

        string GetSubtitle();

        string GetDescription();
    }

    //public abstract class DocumentBase<T> : IDocumentable where T : class
    //{

    //    public DocumentBase()
    //    {
    //        this.FieldLambdas = new List<Expression<Func<T, string>>>();
    //    }

    //    public IEnumerable<Expression<Func<T, string>>> FieldLambdas { get; set; }

    //    public abstract string GetDescription();

    //    public List<string> GetDocumentFields()
    //    {
    //        throw new NotSupportedException();
    //    }

    //    public abstract DocumentType GetDocumentType();

    //    public abstract string GetId();

    //    public abstract string GetSubtitle();

    //    public abstract string GetTitle();

    //    public string GetValue(string field)
    //    {
    //        throw new NotSupportedException();
    //    }
    //}
}
