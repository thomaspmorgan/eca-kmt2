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

    

    //public class DocumentConfiguration<T> where T : class
    //{


    //    public void HasKey(Expression<Func<T, string>> idSelector)
    //    {

    //    }
    //}

    //public static class DocumentBaseExtensions
    //{
    //    public static DocumentBase<T> Index<T>(this DocumentBase<T> source, Expression<Func<T, string>> propertySelector) where T : class
    //    {
    //        var propertyName = PropertyHelper.GetPropertyName<T>(propertySelector);
    //        source.AddField(propertyName);
    //        return source;
    //    }
    //}
}
