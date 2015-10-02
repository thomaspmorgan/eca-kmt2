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
    /// <summary>
    /// An IDocumentConfiguration is used to detail how an object can be transformed into a Document
    /// for searching using an IIndexService.
    /// </summary>
    public interface IDocumentConfiguration
    {
        /// <summary>
        /// Returns the id of the object instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The Id of the object.</returns>
        object GetId(object instance);

        /// <summary>
        /// Returns the name of the object instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The name of the object.</returns>
        string GetName(object instance);

        /// <summary>
        /// Returns the description of the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The description.</returns>
        string GetDescription(object instance);

        /// <summary>
        /// Returns the office symbol of the given instance.
        /// </summary>
        /// <param name="instance">The object instance.</param>
        /// <returns>The office symbol.</returns>
        string GetOfficeSymbol(object instance);

        /// <summary>
        /// Returns the themes of the given instance.
        /// </summary>
        /// <param name="instance">The given instance.</param>
        /// <returns>The themes.</returns>
        IEnumerable<string> GetThemes(object instance);

        /// <summary>
        /// Returns the goals of the given instance.
        /// </summary>
        /// <param name="instance">The given instance.</param>
        /// <returns>The goals.</returns>
        IEnumerable<string> GetGoals(object instance);

        /// <summary>
        /// Returns the foci of the given instance.
        /// </summary>
        /// <param name="instance">The given instance.</param>
        /// <returns>The foci.</returns>
        IEnumerable<string> GetFoci(object instance);

        /// <summary>
        /// Returns the objectives of the given instance.
        /// </summary>
        /// <param name="instance">The given instance.</param>
        /// <returns>The objectives.</returns>
        IEnumerable<string> GetObjectives(object instance);

        /// <summary>
        /// Returns the points of contact of the given instance.
        /// </summary>
        /// <param name="instance">The given instance.</param>
        /// <returns>The points of contact.</returns>
        IEnumerable<string> GetPointsOfContact(object instance);

        /// <summary>
        /// Returns true if this configuration is used for the given class type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>True if this configuration is used for the given type, otherwise, false.</returns>
        bool IsConfigurationForType(Type type);

        /// <summary>
        /// Returns the document type id.  This guid should be unique for all document types.
        /// </summary>
        /// <returns>The document type id.</returns>
        Guid GetDocumentTypeId();

        /// <summary>
        /// Returns a display name of the document type.
        /// </summary>
        /// <returns>A display name of the document type.</returns>
        string GetDocumentTypeName();
    }

    /// <summary>
    /// A DocumentConfiguration class is used to perform a strongly typed document configuration
    /// given an object type and the object type's id type.
    /// </summary>
    /// <typeparam name="TEntity">The type of object that will become a document.</typeparam>
    /// <typeparam name="TEntityKey">The type of the object's id.</typeparam>
    public class DocumentConfiguration<TEntity, TEntityKey> : IDocumentConfiguration
        where TEntity : class
        where TEntityKey : struct
    {
        /// <summary>
        /// Gets the document type name.
        /// </summary>
        public string DocumentTypeName { get; protected set; }

        /// <summary>
        /// Gets the document type id.
        /// </summary>
        public Guid DocumentTypeId { get; protected set; }

        /// <summary>
        /// Gets the id delegate to return the object id.
        /// </summary>
        public Func<TEntity, TEntityKey> IdDelegate { get; private set; }

        /// <summary>
        /// Gets the name delegate to return the object name.
        /// </summary>
        public Func<TEntity, string> NameDelegate { get; private set; }

        /// <summary>
        /// Gets the description delegate to return the object description.
        /// </summary>
        public Func<TEntity, string> DescriptionDelegate { get; private set; }

        /// <summary>
        /// Gets the office symbol delegate to return the object office symbol.
        /// </summary>
        public Func<TEntity, string> OfficeSymbolDelegate { get; private set; }

        /// <summary>
        /// Gets the theme delegate to return the object themes.
        /// </summary>
        public Func<TEntity, IEnumerable<string>> ThemesDelegate { get; private set; }

        /// <summary>
        /// Gets the objective delegate to return the object objectives.
        /// </summary>
        public Func<TEntity, IEnumerable<string>> ObjectivesDelegate { get; private set; }

        /// <summary>
        /// Gets the foci delegate to return the object foci.
        /// </summary>
        public Func<TEntity, IEnumerable<string>> FociDelegate { get; private set; }

        /// <summary>
        /// Gets the goal delegate to return the object goals.
        /// </summary>
        public Func<TEntity, IEnumerable<string>> GoalsDelegate { get; private set; }

        /// <summary>
        /// Gets the points of contact delegate to return the object points of contact.
        /// </summary>
        public Func<TEntity, IEnumerable<string>> PointsOfContactDelegate { get; private set; }
        
        /// <summary>
        /// Returns the id of the object.
        /// </summary>
        /// <param name="instance">The id of the object.</param>
        /// <returns>The object id.</returns>
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

        /// <summary>
        /// Enables an id to be documented using the given expression.
        /// </summary>
        /// <param name="idSelector">The expression to select the id.</param>
        public void HasKey(Expression<Func<TEntity, TEntityKey>> idSelector)
        {
            Contract.Requires(idSelector != null, "The idSelector must not be null.");
            this.IdDelegate = idSelector.Compile();
        }

        /// <summary>
        /// Enables a name to be documented using the given expression.
        /// </summary>
        /// <param name="objectivesSelector">The expression to select the name.</param>
        public void HasName(Expression<Func<TEntity, string>> nameSelector)
        {
            Contract.Requires(nameSelector != null, "The nameSelector must not be null.");
            this.NameDelegate = nameSelector.Compile();
        }

        /// <summary>
        /// Enables a description to be documented using the given expression.
        /// </summary>
        /// <param name="objectivesSelector">The expression to select the description.</param>
        public void HasDescription(Expression<Func<TEntity, string>> descriptionSelector)
        {
            Contract.Requires(descriptionSelector != null, "The descriptionSelector must not be null.");
            this.DescriptionDelegate = descriptionSelector.Compile();
        }

        /// <summary>
        /// Enables an office symbol to be documented using the given expression.
        /// </summary>
        /// <param name="objectivesSelector">The expression to select the office symbol.</param>
        public void HasOfficeSymbol(Expression<Func<TEntity, string>> officeSymbolSelector)
        {
            Contract.Requires(officeSymbolSelector != null, "The officeSymbolSelector must not be null.");
            this.OfficeSymbolDelegate = officeSymbolSelector.Compile();
        }

        /// <summary>
        /// Enables themes to be documented using the given expression.
        /// </summary>
        /// <param name="objectivesSelector">The expression to select theme names.</param>
        public void HasThemes(Expression<Func<TEntity, IEnumerable<string>>> themesSelector)
        {
            Contract.Requires(themesSelector != null, "The descriptionSelector must not be null.");
            this.ThemesDelegate = themesSelector.Compile();
        }

        /// <summary>
        /// Enables objectives to be documented using the given expression.
        /// </summary>
        /// <param name="objectivesSelector">The expression to select objective names.</param>
        public void HasObjectives(Expression<Func<TEntity, IEnumerable<string>>> objectivesSelector)
        {
            Contract.Requires(objectivesSelector != null, "The objectivesSelector must not be null.");
            this.ObjectivesDelegate = objectivesSelector.Compile();
        }

        /// <summary>
        /// Enables goals to be documented using the given expression.
        /// </summary>
        /// <param name="goalsSelector">The expression to retrieve goal names.</param>
        public void HasGoals(Expression<Func<TEntity, IEnumerable<string>>> goalsSelector)
        {
            Contract.Requires(goalsSelector != null, "The goalsSelector must not be null.");
            this.GoalsDelegate = goalsSelector.Compile();
        }

        /// <summary>
        /// Enables foci to be documented using the given expression.
        /// </summary>
        /// <param name="fociSelector">The expression to retrieve foci names.</param>
        public void HasFoci(Expression<Func<TEntity, IEnumerable<string>>> fociSelector)
        {
            Contract.Requires(fociSelector != null, "The fociSelector must not be null.");
            this.FociDelegate = fociSelector.Compile();
        }

        /// <summary>
        /// Enables points of contact to be documented using the given expression.
        /// </summary>
        /// <param name="pocSelector">The expression to select points of contact names.</param>
        public void HasPointsOfContact(Expression<Func<TEntity, IEnumerable<string>>> pocSelector)
        {
            Contract.Requires(pocSelector != null, "The pocSelector must not be null.");
            this.PointsOfContactDelegate = pocSelector.Compile();
        }

        /// <summary>
        /// Configures the class document type id and document type name.
        /// </summary>
        /// <param name="documentTypeId">The document type id.</param>
        /// <param name="documentTypeName">The document type name.</param>
        public void IsDocumentType(Guid documentTypeId, string documentTypeName)
        {
            Contract.Requires(documentTypeName != null, "The document type name must not be null.");
            Contract.Requires(documentTypeId != Guid.Empty, "The document type id must not be the empty guid.");
            this.DocumentTypeId = documentTypeId;
            this.DocumentTypeName = documentTypeName;
        }

        /// <summary>
        /// Returns true, if this configuration is used for the given type.
        /// </summary>
        /// <param name="type">The class type.</param>
        /// <returns>True, if this configuration is used for the given type, otherwise false.</returns>
        public bool IsConfigurationForType(Type type)
        {
            return typeof(TEntity) == type;
        }

        /// <summary>
        /// Returns the name of the object.
        /// </summary>
        /// <param name="instance">The instance of the object.</param>
        /// <returns>The object name.</returns>
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

        /// <summary>
        /// Returns the office symbol of the object.
        /// </summary>
        /// <param name="instance">The instance of the object.</param>
        /// <returns>The object office symbol.</returns>
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

        /// <summary>
        /// Returns the description of the object.
        /// </summary>
        /// <param name="instance">The instance of the object.</param>
        /// <returns>The object description.</returns>
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

        /// <summary>
        /// Returns the themes of the object.
        /// </summary>
        /// <param name="instance">The instance of the object.</param>
        /// <returns>The object themes.</returns>
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

        /// <summary>
        /// Returns the goals of the object.
        /// </summary>
        /// <param name="instance">The instance of the object.</param>
        /// <returns>The object goals.</returns>
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

        /// <summary>
        /// Returns the foci of the object.
        /// </summary>
        /// <param name="instance">The instance of the object.</param>
        /// <returns>The object foci.</returns>
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

        /// <summary>
        /// Returns the objectives of the object.
        /// </summary>
        /// <param name="instance">The instance of the object.</param>
        /// <returns>The object objectives.</returns>
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

        /// <summary>
        /// Returns the points of contact of the object.
        /// </summary>
        /// <param name="instance">The instance of the object.</param>
        /// <returns>The object points of contact.</returns>
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

        /// <summary>
        /// Returns the document type id.
        /// </summary>
        /// <returns>The document type id.</returns>
        public Guid GetDocumentTypeId()
        {
            return this.DocumentTypeId;
        }

        /// <summary>
        /// The document type name.
        /// </summary>
        /// <returns></returns>
        public string GetDocumentTypeName()
        {
            return this.DocumentTypeName;
        }
    }
}
