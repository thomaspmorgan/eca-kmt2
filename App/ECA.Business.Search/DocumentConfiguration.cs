using ECA.Core.DynamicLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    #region Interface
    /// <summary>
    /// An IDocumentConfiguration is used to detail how an object can be transformed into a Document
    /// for searching using an IIndexService.
    /// </summary>
    [ContractClass(typeof(DocumentConfigurationContract))]
    public interface IDocumentConfiguration
    {
        /// <summary>
        /// Returns the id of the object instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The Id of the object.</returns>
        object GetId(object instance);

        /// <summary>
        /// Returns the document key type for the given instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The document key type.</returns>
        DocumentKeyType GetDocumentKeyType(object instance);

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
        /// Returns the status of the instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The status.</returns>
        string GetStatus(object instance);

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
        /// Returns the regions of the given instance.
        /// </summary>
        /// <param name="instance">The given instance.</param>
        /// <returns>The regions.</returns>
        IEnumerable<string> GetRegions(object instance);

        /// <summary>
        /// Returns the countries of the given instance.
        /// </summary>
        /// <param name="instance">The given instance.</param>
        /// <returns>The countries.</returns>
        IEnumerable<string> GetCountries(object instance);

        /// <summary>
        /// Returns the locations of the given instance.
        /// </summary>
        /// <param name="instance">The given instance.</param>
        /// <returns>The locations.</returns>
        IEnumerable<string> GetLocations(object instance);

        /// <summary>
        /// Returns the websites of the given instance.
        /// </summary>
        /// <param name="instance">The given instance.</param>
        /// <returns>The websites.</returns>
        IEnumerable<string> GetWebsites(object instance);

        /// <summary>
        /// Returns the addresses of the given instance.
        /// </summary>
        /// <param name="instance">The given instances.</param>
        /// <returns>The addresses.</returns>
        IEnumerable<string> GetAddresses(object instance);
        
        /// <summary>
        /// Returns the phone numbers of the given instance.
        /// </summary>
        /// <param name="instance">The given instances.</param>
        /// <returns>The phone numbers.</returns>
        IEnumerable<string> GetPhoneNumbers(object instance);        

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

        /// <summary>
        /// Returns the start date from the given instance.
        /// </summary>
        /// <param name="instance">The given instance.</param>
        /// <returns>The start date.</returns>
        DateTimeOffset? GetStartDate(object instance);

        /// <summary>
        /// Returns the end date from the given instance.
        /// </summary>
        /// <param name="instance">The given instance.</param>
        /// <returns>The end date.</returns>
        DateTimeOffset? GetEndDate(object instance);
    }
    #endregion

    #region Contract
    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IDocumentConfiguration))]
    public abstract class DocumentConfigurationContract : IDocumentConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public IEnumerable<string> GetAddresses(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public IEnumerable<string> GetCountries(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public string GetDescription(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public DocumentKeyType GetDocumentKeyType(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            return DocumentKeyType.Int;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Guid GetDocumentTypeId()
        {
            Contract.Ensures(Contract.Result<Guid>() != Guid.Empty, "The document type id must not be the empty guid.");
            return Guid.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetDocumentTypeName()
        {
            Contract.Ensures(!String.IsNullOrWhiteSpace(Contract.Result<string>()), "The document type name must have a value.");
            Contract.Ensures(Contract.Result<string>().Length <= IndexService.MAX_DOCUMENT_TYPE_NAME_LENGTH, "The document type name must not be more than the max length.");
            Contract.Ensures(Regex.IsMatch(Contract.Result<string>(), @"^[a-zA-Z]+$"), "The document name may only have characters in it.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public DateTimeOffset? GetEndDate(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public IEnumerable<string> GetFoci(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public IEnumerable<string> GetGoals(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public object GetId(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            Contract.Ensures(Contract.Result<object>() != null, "The id of the instance must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public IEnumerable<string> GetLocations(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public string GetName(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public IEnumerable<string> GetObjectives(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public string GetOfficeSymbol(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public IEnumerable<string> GetPointsOfContact(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public IEnumerable<string> GetRegions(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public DateTimeOffset? GetStartDate(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public string GetStatus(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public IEnumerable<string> GetThemes(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public IEnumerable<string> GetWebsites(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsConfigurationForType(Type type)
        {
            Contract.Requires(type != null, "The given type must not be null.");
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public IEnumerable<string> GetPhoneNumbers(object instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            return null;
        }
    }
    #endregion

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
        /// Gets the status delegate to return the object status.
        /// </summary>
        public Func<TEntity, string> StatusDelegate { get; private set; }

        /// <summary>
        /// Gets the office symbol delegate to return the object office symbol.
        /// </summary>
        public Func<TEntity, string> OfficeSymbolDelegate { get; private set; }        

        /// <summary>
        /// Gets the website delegate to return the object websites.
        /// </summary>
        public Func<TEntity, IEnumerable<string>> WebsitesDelegate { get; private set; }

        /// <summary>
        /// Gets the country delegate to return the object countries.
        /// </summary>
        public Func<TEntity, IEnumerable<string>> CountriesDelegate { get; private set; }

        /// <summary>
        /// Gets the region delegate to return the object regions.
        /// </summary>
        public Func<TEntity, IEnumerable<string>> RegionsDelegate { get; private set; }

        /// <summary>
        /// Gets the location delegate to return the object locations.
        /// </summary>
        public Func<TEntity, IEnumerable<string>> LocationsDelegate { get; private set; }

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
        /// Gets the addresses degate to return the object addresses.
        /// </summary>
        public Func<TEntity, IEnumerable<string>> AddressesDelegate { get; private set; }        

        /// <summary>
        /// Gets the phone numbers delegate to return the object phone numbers.
        /// </summary>
        public Func<TEntity, IEnumerable<string>> PhoneNumbersDelegate { get; private set; }

        /// <summary>
        /// Gets the start delegate to return the object start date.
        /// </summary>
        public Func<TEntity, DateTimeOffset?> StartDateDelegate { get; private set; }

        /// <summary>
        /// Gets the end delegate to return the object start date.
        /// </summary>
        public Func<TEntity, DateTimeOffset?> EndDateDelegate { get; private set; }

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
        /// <param name="nameSelector">The expression to select the name.</param>
        public void HasName(Expression<Func<TEntity, string>> nameSelector)
        {
            Contract.Requires(nameSelector != null, "The nameSelector must not be null.");
            this.NameDelegate = nameSelector.Compile();
        }

        /// <summary>
        /// Enables a description to be documented using the given expression.
        /// </summary>
        /// <param name="descriptionSelector">The expression to select the description.</param>
        public void HasDescription(Expression<Func<TEntity, string>> descriptionSelector)
        {
            Contract.Requires(descriptionSelector != null, "The descriptionSelector must not be null.");
            this.DescriptionDelegate = descriptionSelector.Compile();
        }

        /// <summary>
        /// Enables a status to be documented using the given expression.
        /// </summary>
        /// <param name="statusSelector">The expression to select the status.</param>
        public void HasStatus(Expression<Func<TEntity, string>> statusSelector)
        {
            Contract.Requires(statusSelector != null, "The statusSelector must not be null.");
            this.StatusDelegate = statusSelector.Compile();
        }

        /// <summary>
        /// Enables an office symbol to be documented using the given expression.
        /// </summary>
        /// <param name="officeSymbolSelector">The expression to select the office symbol.</param>
        public void HasOfficeSymbol(Expression<Func<TEntity, string>> officeSymbolSelector)
        {
            Contract.Requires(officeSymbolSelector != null, "The officeSymbolSelector must not be null.");
            this.OfficeSymbolDelegate = officeSymbolSelector.Compile();
        }


        /// <summary>
        /// Enables locations to be documented using the given expression.
        /// </summary>
        /// <param name="locationsSelector">The expression to select location names.</param>
        public void HasLocations(Expression<Func<TEntity, IEnumerable<string>>> locationsSelector)
        {
            Contract.Requires(locationsSelector != null, "The locationsSelector must not be null.");
            this.LocationsDelegate = locationsSelector.Compile();
        }

        /// <summary>
        /// Enables regions to be documented using the given expression.
        /// </summary>
        /// <param name="regionsSelector">The expression to select region names.</param>
        public void HasRegions(Expression<Func<TEntity, IEnumerable<string>>> regionsSelector)
        {
            Contract.Requires(regionsSelector != null, "The regionsSelector must not be null.");
            this.RegionsDelegate = regionsSelector.Compile();
        }

        /// <summary>
        /// Enables countries to be documented using the given expression.
        /// </summary>
        /// <param name="countriesSelector">The expression to select country names.</param>
        public void HasCountries(Expression<Func<TEntity, IEnumerable<string>>> countriesSelector)
        {
            Contract.Requires(countriesSelector != null, "The countriesSelector must not be null.");
            this.CountriesDelegate = countriesSelector.Compile();
        }

        /// <summary>
        /// Enables websites to be documented using the given expression.
        /// </summary>
        /// <param name="websitesSelector">The expression to select websites.</param>
        public void HasWebsites(Expression<Func<TEntity, IEnumerable<string>>> websitesSelector)
        {
            Contract.Requires(websitesSelector != null, "The websitesSelector must not be null.");
            this.WebsitesDelegate = websitesSelector.Compile();
        }

        /// <summary>
        /// Enables themes to be documented using the given expression.
        /// </summary>
        /// <param name="themesSelector">The expression to select theme names.</param>
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
        /// Enables phone numbers to be documented using the given expression.
        /// </summary>
        /// <param name="phonenumbersSelector">The expression to select phone numbers.</param>
        public void HasPhoneNumbers(Expression<Func<TEntity, IEnumerable<string>>> phonenumbersSelector)
        {
            Contract.Requires(phonenumbersSelector != null, "The phonenumbersSelector must not be null.");
            this.PhoneNumbersDelegate = phonenumbersSelector.Compile();
        }

        /// <summary>
        /// Enables addresses to be documented using the given expression.
        /// </summary>
        /// <param name="addressesSelector">The expression to select addresses.</param>
        public void HasAddresses(Expression<Func<TEntity, IEnumerable<string>>> addressesSelector)
        {
            Contract.Requires(addressesSelector != null, "The addressesSelector must not be null.");
            this.AddressesDelegate = addressesSelector.Compile();
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
        /// Enables start date to be documented using the given expression.
        /// </summary>
        /// <param name="startDateSelector">The expression to select a start date.</param>
        public void HasStartDate(Expression<Func<TEntity, DateTimeOffset?>> startDateSelector)
        {
            Contract.Requires(startDateSelector != null, "The startDateSelector must not be null.");
            this.StartDateDelegate = startDateSelector.Compile();
        }

        /// <summary>
        /// Enables end date to be documented using the given expression.
        /// </summary>
        /// <param name="endDateSelector">The expression to select an end date.</param>
        public void HasEndDate(Expression<Func<TEntity, DateTimeOffset?>> endDateSelector)
        {
            Contract.Requires(endDateSelector != null, "The endDateSelector must not be null.");
            this.EndDateDelegate = endDateSelector.Compile();
        }

        /// <summary>
        /// Configures the class document type id and document type name.
        /// </summary>
        /// <param name="documentTypeId">The document type id.</param>
        /// <param name="documentTypeName">The document type name.</param>
        public void IsDocumentType(Guid documentTypeId, string documentTypeName)
        {
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

        /// <summary>
        /// Returns the description of the object.
        /// </summary>
        /// <param name="instance">The instance of the object.</param>
        /// <returns>The object description.</returns>
        public string GetStatus(object instance)
        {
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if (StatusDelegate != null)
            {
                return StatusDelegate((TEntity)instance);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the regions of the object.
        /// </summary>
        /// <param name="instance">The instance of the object.</param>
        /// <returns>The object regions.</returns>
        public IEnumerable<string> GetRegions(object instance)
        {
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if (RegionsDelegate != null)
            {
                return RegionsDelegate((TEntity)instance);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the countries of the object.
        /// </summary>
        /// <param name="instance">The instance of the object.</param>
        /// <returns>The object countries.</returns>
        public IEnumerable<string> GetCountries(object instance)
        {
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if (CountriesDelegate != null)
            {
                return CountriesDelegate((TEntity)instance);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the locations of the object.
        /// </summary>
        /// <param name="instance">The instance of the object.</param>
        /// <returns>The object locations.</returns>
        public IEnumerable<string> GetLocations(object instance)
        {
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if (LocationsDelegate != null)
            {
                return LocationsDelegate((TEntity)instance);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the websites of the object.
        /// </summary>
        /// <param name="instance">The instance of the object.</param>
        /// <returns>The object websites.</returns>
        public IEnumerable<string> GetWebsites(object instance)
        {
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if (WebsitesDelegate != null)
            {
                return WebsitesDelegate((TEntity)instance);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the document key type.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The document key type.</returns>
        public DocumentKeyType GetDocumentKeyType(object instance)
        {
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if(IdDelegate != null)
            {
                return DocumentKey.GetDocumentKeyType(IdDelegate((TEntity)instance));
            }
            else
            {
                throw new NotSupportedException("The document key type can not be determined because the Key has not been configured on this type.");
            }
        }

        /// <summary>
        /// Returns the start date of the given object.
        /// </summary>
        /// <param name="instance">The instance of the object.</param>
        /// <returns>The object start date.</returns>
        public DateTimeOffset? GetStartDate(object instance)
        {
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if (StartDateDelegate != null)
            {
                return StartDateDelegate((TEntity)instance);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the end date of the given object.
        /// </summary>
        /// <param name="instance">The instance of the object.</param>
        /// <returns>The object end date.</returns>
        public DateTimeOffset? GetEndDate(object instance)
        {
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if (EndDateDelegate != null)
            {
                return EndDateDelegate((TEntity)instance);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the addresses of the given object.
        /// </summary>
        /// <param name="instance">The instance of the object.</param>
        /// <returns>The object addresses.</returns>
        public IEnumerable<string> GetAddresses(object instance)
        {
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if (AddressesDelegate != null)
            {
                return AddressesDelegate((TEntity)instance);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the phone number of the given object.
        /// </summary>
        /// <param name="instance">The instance of the object.</param>
        /// <returns>The object phone numbers.</returns>
        public IEnumerable<string> GetPhoneNumbers(object instance)
        {
            Contract.Requires(instance.GetType() == typeof(TEntity), "The instance must be a TEntity.");
            if (PhoneNumbersDelegate != null)
            {
                return PhoneNumbersDelegate((TEntity)instance);
            }
            else
            {
                return null;
            }
        }
    }
}
