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

        string GetName(object instance);

        string GetDescription(object instance);

        string GetOfficeSymbol(object instance);

        IEnumerable<string> GetThemes(object instance);

        IEnumerable<string> GetGoals(object instance);

        IEnumerable<string> GetFoci(object instance);

        IEnumerable<string> GetObjectives(object instance);

        IEnumerable<string> GetPointsOfContact(object instance);

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
            
        }

        public Func<TEntity, TEntityKey> IdDelegate { get; private set; }

        public Func<TEntity, string> NameDelegate { get; private set; }

        public Func<TEntity, string> DescriptionDelegate { get; private set; }

        public Func<TEntity, string> OfficeSymbolDelegate { get; private set; }

        public Func<TEntity, IEnumerable<string>> ThemesDelegate { get; private set; }

        public Func<TEntity, IEnumerable<string>> ObjectivesDelegate { get; private set; }

        public Func<TEntity, IEnumerable<string>> FociDelegate { get; private set; }

        public Func<TEntity, IEnumerable<string>> GoalsDelegate { get; private set; }

        public Func<TEntity, IEnumerable<string>> PointsOfContactDelegate { get; private set; }


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

        public void HasName(Expression<Func<TEntity, string>> nameSelector)
        {
            Contract.Requires(nameSelector != null, "The nameSelector must not be null.");
            this.NameDelegate = nameSelector.Compile();
        }

        public void HasDescription(Expression<Func<TEntity, string>> descriptionSelector)
        {
            Contract.Requires(descriptionSelector != null, "The descriptionSelector must not be null.");
            this.DescriptionDelegate = descriptionSelector.Compile();
        }

        public void HasOfficeSymbol(Expression<Func<TEntity, string>> officeSymbolSelector)
        {
            Contract.Requires(officeSymbolSelector != null, "The officeSymbolSelector must not be null.");
            this.OfficeSymbolDelegate = officeSymbolSelector.Compile();
        }

        public void HasThemes(Expression<Func<TEntity, IEnumerable<string>>> themesSelector)
        {
            Contract.Requires(themesSelector != null, "The descriptionSelector must not be null.");
            this.ThemesDelegate = themesSelector.Compile();
        }

        public void HasObjectives(Expression<Func<TEntity, IEnumerable<string>>> objectivesSelector)
        {
            Contract.Requires(objectivesSelector != null, "The objectivesSelector must not be null.");
            this.ObjectivesDelegate = objectivesSelector.Compile();
        }

        public void HasGoals(Expression<Func<TEntity, IEnumerable<string>>> goalsSelector)
        {
            Contract.Requires(goalsSelector != null, "The goalsSelector must not be null.");
            this.GoalsDelegate = goalsSelector.Compile();
        }

        public void HasFoci(Expression<Func<TEntity, IEnumerable<string>>> fociSelector)
        {
            Contract.Requires(fociSelector != null, "The fociSelector must not be null.");
            this.FociDelegate = fociSelector.Compile();
        }

        public void HasPointsOfContact(Expression<Func<TEntity, IEnumerable<string>>> pocSelector)
        {
            Contract.Requires(pocSelector != null, "The pocSelector must not be null.");
            this.PointsOfContactDelegate = pocSelector.Compile();
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

        public string GetName(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if (NameDelegate != null)
            {
                return NameDelegate((TEntity)instance);
            }
            else
            {
                return null;
            }
        }

        public string GetOfficeSymbol(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if (OfficeSymbolDelegate != null)
            {
                return OfficeSymbolDelegate((TEntity)instance);
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

        public IEnumerable<string> GetThemes(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if (ThemesDelegate != null)
            {
                return ThemesDelegate((TEntity)instance);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<string> GetGoals(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if (GoalsDelegate != null)
            {
                return GoalsDelegate((TEntity)instance);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<string> GetFoci(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if (FociDelegate != null)
            {
                return FociDelegate((TEntity)instance);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<string> GetObjectives(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if(ObjectivesDelegate != null)
            {
                return ObjectivesDelegate((TEntity)instance);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<string> GetPointsOfContact(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if (PointsOfContactDelegate != null)
            {
                return PointsOfContactDelegate((TEntity)instance);
            }
            else
            {
                return null;
            }
        }

        
    }
}
