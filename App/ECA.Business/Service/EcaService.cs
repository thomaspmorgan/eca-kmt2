using ECA.Business.Queries.Models.Persons;
using ECA.Core.Service;
using ECA.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace ECA.Business.Service
{
    /// <summary>
    /// An EcaService is a service that provides common functionality for checking existence and setting common properties.
    /// </summary>
    public class EcaService : DbContextService<EcaContext>
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// An EcaService is a common service that can be extended by ECA services to advantage of common code.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="saveActions">The save actions.</param>
        public EcaService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Contact Existence Validation

        private IQueryable<Contact> CreateGetContactsByIdsQuery(IEnumerable<int> contactIds)
        {
            Contract.Requires(contactIds != null, "The contact ids must not be null.");
            return Context.Contacts.Where(x => contactIds.Distinct().Contains(x.ContactId)).Distinct();
        }

        /// <summary>
        /// A method to verify all contacts with the given ids exist in the system.
        /// </summary>
        /// <param name="contactIds">The contacts by Id to validate existence.</param>
        /// <returns>True, if all contacts exist, otherwise false.</returns>
        public async Task<bool> CheckAllContactsExistAsync(IEnumerable<int> contactIds)
        {
            Contract.Requires(contactIds != null, "The contact ids must not be null.");
            bool response = false;
            if (contactIds.Count() == 0)
            {
                response = true;
            }
            else
            {
                var distinctIds = contactIds.Distinct().ToList();
                response = await CreateGetContactsByIdsQuery(contactIds).CountAsync() == distinctIds.Count();
            }
            logger.Trace("Checked all contacts with ids {0} exist.", String.Join(", ", contactIds));
            return response;
        }

        /// <summary>
        /// A method to verify all contacts with the given ids exist in the system.
        /// </summary>
        /// <param name="contactIds">The contacts by Id to validate existence.</param>
        /// <returns>True, if all contacts exist, otherwise false.</returns>
        public bool CheckAllContactsExist(IEnumerable<int> contactIds)
        {
            Contract.Requires(contactIds != null, "The contact ids must not be null.");
            bool response = false;
            if (contactIds.Count() == 0)
            {
                response = true;
            }
            else
            {
                var distinctIds = contactIds.Distinct().ToList();
                response = CreateGetContactsByIdsQuery(contactIds).Count() == distinctIds.Count();
            }
            logger.Trace("Checked all contacts with ids {0} exist.", String.Join(", ", contactIds));
            return response;
        }
        #endregion

        #region Location Existence Validation

        /// <summary>
        /// A method to verify all locations with the given ids exist in the system.
        /// </summary>
        /// <param name="locationIds">The locations by Id to validate existence.</param>
        /// <returns>True, if all locations exist, otherwise false.</returns>
        public async Task<bool> CheckAllLocationsExistAsync(IEnumerable<int> locationIds)
        {
            Contract.Requires(locationIds != null, "The location ids must not be null.");
            bool response = false;
            if (locationIds.Count() == 0)
            {
                response = true;
            }
            else
            {
                var distinctIds = locationIds.Distinct().ToList();
                response = await CreateGetLocationsById(locationIds.ToList()).CountAsync() == distinctIds.Count();
            }
            logger.Trace("Checked all locations with ids {0} exist.", String.Join(", ", locationIds));
            return response;
        }

        /// <summary>
        /// A method to verify all locations with the given ids exist in the system.
        /// </summary>
        /// <param name="locationIds">The locations by Id to validate existence.</param>
        /// <returns>True, if all locations exist, otherwise false.</returns>
        public bool CheckAllLocationsExist(IEnumerable<int> locationIds)
        {
            Contract.Requires(locationIds != null, "The location ids must not be null.");
            bool response = false;
            if (locationIds.Count() == 0)
            {
                response = true;
            }
            else
            {
                var distinctIds = locationIds.Distinct().ToList();
                response = CreateGetLocationsById(locationIds.ToList()).Count() == distinctIds.Count();
            }
            logger.Trace("Checked all locations with ids {0} exist.", String.Join(", ", locationIds));
            return response;
        }
        #endregion

        #region Theme Existence Validation

        private IQueryable<Theme> CreateGetThemesByIdsQuery(IEnumerable<int> themeIds)
        {
            Contract.Requires(themeIds != null, "The theme ids must not be null.");
            return Context.Themes.Where(x => themeIds.Distinct().Contains(x.ThemeId)).Distinct();
        }

        /// <summary>
        /// A method to verify all themes with the given ids exist in the system.
        /// </summary>
        /// <param name="themeIds">The theme by Id to validate existence.</param>
        /// <returns>True, if all themes exist, otherwise false.</returns>
        public async Task<bool> CheckAllThemesExistAsync(IEnumerable<int> themeIds)
        {
            Contract.Requires(themeIds != null, "The theme ids must not be null.");
            bool response = false;
            if (themeIds.Count() == 0)
            {
                response = true;
            }
            else
            {
                var distinctIds = themeIds.Distinct().ToList();
                response = await CreateGetThemesByIdsQuery(themeIds).CountAsync() == distinctIds.Count();
            }
            logger.Trace("Checked all themes with ids {0} exist.", String.Join(", ", themeIds));
            return response;
        }

        /// <summary>
        /// A method to verify all themes with the given ids exist in the system.
        /// </summary>
        /// <param name="themeIds">The themes by Id to validate existence.</param>
        /// <returns>True, if all themes exist, otherwise false.</returns>
        public bool CheckAllThemesExist(IEnumerable<int> themeIds)
        {
            Contract.Requires(themeIds != null, "The theme ids must not be null.");
            bool response = false;
            if (themeIds.Count() == 0)
            {
                response = true;
            }
            else
            {
                var distinctIds = themeIds.Distinct().ToList();
                response = CreateGetThemesByIdsQuery(themeIds).Count() == distinctIds.Count();
            }
            logger.Trace("Checked all themes exist.");
            return response;
        }
        #endregion

        #region Goals Existence Validation

        private IQueryable<Goal> CreateGetGoalsByIdsQuery(IEnumerable<int> goalIds)
        {
            Contract.Requires(goalIds != null, "The theme ids must not be null.");
            return Context.Goals.Where(x => goalIds.Distinct().Contains(x.GoalId)).Distinct();
        }

        /// <summary>
        /// A method to verify all goals with the given ids exist in the system.
        /// </summary>
        /// <param name="goalIds">The goals by Id to validate existence.</param>
        /// <returns>True, if all themes exist, otherwise false.</returns>
        public async Task<bool> CheckAllGoalsExistAsync(IEnumerable<int> goalIds)
        {
            Contract.Requires(goalIds != null, "The goal ids must not be null.");
            bool response = false;
            if (goalIds.Count() == 0)
            {
                response = true;
            }
            else
            {
                var distinctIds = goalIds.Distinct().ToList();
                return await CreateGetGoalsByIdsQuery(goalIds).CountAsync() == distinctIds.Count();
            }
            logger.Trace("Checked all goals with ids {0} exist.", String.Join(", ", goalIds));
            return response;
        }

        /// <summary>
        /// A method to verify all goals with the given ids exist in the system.
        /// </summary>
        /// <param name="goalIds">The goals by Id to validate existence.</param>
        /// <returns>True, if all themes exist, otherwise false.</returns>
        public bool CheckAllGoalsExist(IEnumerable<int> goalIds)
        {
            Contract.Requires(goalIds != null, "The goal ids must not be null.");
            bool response = false;
            if (goalIds.Count() == 0)
            {
                response = true;
            }
            else
            {
                var distinctIds = goalIds.Distinct().ToList();
                response = CreateGetGoalsByIdsQuery(goalIds).Count() == distinctIds.Count();
            }
            logger.Trace("Checked all goals with ids {0} exist.", String.Join(", ", goalIds));
            return response;
        }
        #endregion

        #region Categories Existence Validation
        private IQueryable<Category> CreateGetCategoriesByIdsQuery(IEnumerable<int> categoryIds)
        {
            Contract.Requires(categoryIds != null, "The category ids must not be null.");
            return Context.Categories.Where(x => categoryIds.Distinct().Contains(x.CategoryId)).Distinct();
        }

        public async Task<bool> CheckAllCategoriesExistAsync(IEnumerable<int> categoryIds)
        {
            Contract.Requires(categoryIds != null, "The Category Ids must not be null.");
            bool response = false;
            if (categoryIds.Count() == 0)
            {
                response = true;
            }
            else
            {
                var distinctIds = categoryIds.Distinct().ToList();
                response = await CreateGetCategoriesByIdsQuery(categoryIds).CountAsync() == distinctIds.Count();
            }
            logger.Trace("Checked all categories with ids {0} exist.", String.Join(", ", categoryIds));
            return response;
        }

        /// <summary>
        /// A method to verify all themes with the given ids exist in the system.
        /// </summary>
        /// <param name="themeIds">The themes by Id to validate existence.</param>
        /// <returns>True, if all themes exist, otherwise false.</returns>
        public bool CheckAllCategoriesExist(IEnumerable<int> categoryIds)
        {
            Contract.Requires(categoryIds != null, "The category ids must not be null.");
            bool response = false;
            if (categoryIds.Count() == 0)
            {
                response = true;
            }
            else
            {
                var distinctIds = categoryIds.Distinct().ToList();
                response = CreateGetCategoriesByIdsQuery(categoryIds).Count() == distinctIds.Count();
            }
            logger.Trace("Checked all categories with ids {0} exist.", String.Join(", ", categoryIds));
            return response;
        }
        #endregion

        #region Objectives Existence Validation
        private IQueryable<Objective> CreateGetObjectivesByIdsQuery(IEnumerable<int> objectiveIds)
        {
            Contract.Requires(objectiveIds != null, "The objective ids must not be null.");
            return Context.Objectives.Where(x => objectiveIds.Distinct().Contains(x.ObjectiveId)).Distinct();
        }
        public async Task<bool> CheckAllObjectivesExistAsync(IEnumerable<int> objectiveIds)
        {
            Contract.Requires(objectiveIds != null, "The Objective Ids must not be null.");
            bool response = false;
            if (objectiveIds.Count() == 0)
            {
                response = true;
            }
            else
            {
                var distinctIds = objectiveIds.Distinct().ToList();
                response = await CreateGetObjectivesByIdsQuery(objectiveIds).CountAsync() == distinctIds.Count();
            }
            logger.Trace("Checked all ojectives with ids {0} exist.", String.Join(", ", objectiveIds));
            return response;
        }

        public bool CheckAllObjectivesExist(IEnumerable<int> objectiveIds)
        {
            Contract.Requires(objectiveIds != null, "The objectives ids must not be null.");
            bool response = false;
            if (objectiveIds.Count() == 0)
            {
                response = true;
            }
            else
            {
                var distinctIds = objectiveIds.Distinct().ToList();
                response = CreateGetObjectivesByIdsQuery(objectiveIds).Count() == objectiveIds.Count();
            }
            logger.Trace("Checked all ojectives with ids {0} exist.", String.Join(", ", objectiveIds));
            return response;
        }
        #endregion

        /// <summary>
        /// Updates the goals on the given program to the goals with the given ids.  Ensure the goals
        /// are already loaded via the context before calling this method.
        /// </summary>
        /// <param name="goalIds">The goal ids.</param>
        /// <param name="goalable">The program.</param>
        public void SetGoals(List<int> goalIds, IGoalable goalable)
        {
            Contract.Requires(goalIds != null, "The goal ids must not be null.");
            Contract.Requires(goalable != null, "The goalable entity must not be null.");
            var goalsToRemove = goalable.Goals.Where(x => !goalIds.Contains(x.GoalId)).ToList();
            var goalsToAdd = new List<Goal>();
            goalIds.Where(x => !goalable.Goals.Select(c => c.GoalId).ToList().Contains(x)).ToList()
                .Select(x => new Goal { GoalId = x }).ToList()
                .ForEach(x => goalsToAdd.Add(x));

            goalsToAdd.ForEach(x =>
            {
                var localEntity = Context.GetLocalEntity<Goal>(y => y.GoalId == x.GoalId);
                if (localEntity == null)
                {
                    if (Context.GetEntityState(x) == EntityState.Detached)
                    {
                        Context.Goals.Attach(x);
                    }
                    goalable.Goals.Add(x);
                }
                else
                {
                    goalable.Goals.Add(localEntity);
                }
            });
            goalsToRemove.ForEach(x =>
            {
                goalable.Goals.Remove(x);
            });
        }

        /// <summary>
        /// Updates the points of contacts on the given program to the pocs with the given ids.  Ensure the contacts
        /// are already loaded via the context before calling this method.
        /// </summary>
        /// <param name="pointOfContactIds">The points of contacts by id.</param>
        /// <param name="contactable">The entity to update.</param>
        public void SetPointOfContacts(List<int> pointOfContactIds, IContactable contactable)
        {
            Contract.Requires(pointOfContactIds != null, "The list of poc ids must not be null.");
            Contract.Requires(contactable != null, "The contactable entity must not be null.");
            var contactsToRemove = contactable.Contacts.Where(x => !pointOfContactIds.Contains(x.ContactId)).ToList();
            var contactsToAdd = new List<Contact>();
            pointOfContactIds.Where(x => !contactable.Contacts.Select(c => c.ContactId).ToList().Contains(x)).ToList()
                .Select(x => new Contact { ContactId = x }).ToList()
                .ForEach(x => contactsToAdd.Add(x));

            contactsToAdd.ForEach(x =>
            {
                var localEntity = Context.GetLocalEntity<Contact>(y => y.ContactId == x.ContactId);
                if (localEntity == null)
                {
                    if (Context.GetEntityState(x) == EntityState.Detached)
                    {
                        Context.Contacts.Attach(x);
                    }
                    contactable.Contacts.Add(x);
                }
                else
                {
                    contactable.Contacts.Add(localEntity);
                }

            });
            contactsToRemove.ForEach(x =>
            {
                contactable.Contacts.Remove(x);
            });
        }

        /// <summary>
        /// Updates the regions on the given IRegionable to the locations with the given ids.  Ensure the locations
        /// are already loaded via the context before calling this method.
        /// </summary>
        /// <param name="regionIds">The locations by id.</param>
        /// <param name="regionable">The entity to update.</param>
        public void SetRegions(List<int> regionIds, IRegionable regionable)
        {
            Contract.Requires(regionIds != null, "The theme ids must not be null.");
            Contract.Requires(regionable != null, "The themeable entity must not be null.");
            var regionsToRemove = regionable.Regions.Where(x => !regionIds.Contains(x.LocationId)).ToList();
            var regionsToAdd = new List<Location>();
            regionIds.Where(x => !regionable.Regions.Select(c => c.LocationId).ToList().Contains(x)).ToList()
                .Select(x => new Location { LocationId = x }).ToList()
                .ForEach(x => regionsToAdd.Add(x));

            regionsToAdd.ForEach(x =>
            {
                var localEntity = Context.GetLocalEntity<Location>(y => y.LocationId == x.LocationId);
                if (localEntity == null)
                {
                    if (Context.GetEntityState(x) == EntityState.Detached)
                    {
                        Context.Locations.Attach(x);
                    }
                    regionable.Regions.Add(x);
                }
                else
                {
                    regionable.Regions.Add(localEntity);
                }

            });
            regionsToRemove.ForEach(x =>
            {
                regionable.Regions.Remove(x);
            });
        }

        /// <summary>
        /// Updates the themes on the given program to the themes with the given ids.  Ensure the themes
        /// are already loaded via the context before calling this method.
        /// </summary>
        /// <param name="themeIds">The themes by id.</param>
        /// <param name="themeable">The program to update.</param>
        public void SetThemes(List<int> themeIds, IThemeable themeable)
        {
            Contract.Requires(themeIds != null, "The theme ids must not be null.");
            Contract.Requires(themeable != null, "The themeable entity must not be null.");
            var themesToRemove = themeable.Themes.Where(x => !themeIds.Contains(x.ThemeId)).ToList();
            var themesToAdd = new List<Theme>();
            themeIds.Where(x => !themeable.Themes.Select(c => c.ThemeId).ToList().Contains(x)).ToList()
                .Select(x => new Theme { ThemeId = x }).ToList()
                .ForEach(x => themesToAdd.Add(x));

            themesToAdd.ForEach(x =>
            {
                var localEntity = Context.GetLocalEntity<Theme>(y => y.ThemeId == x.ThemeId);
                if (localEntity == null)
                {
                    if (Context.GetEntityState(x) == EntityState.Detached)
                    {
                        Context.Themes.Attach(x);
                    }
                    themeable.Themes.Add(x);
                }
                else
                {
                    themeable.Themes.Add(localEntity);
                }
            });
            themesToRemove.ForEach(x =>
            {
                themeable.Themes.Remove(x);
            });
        }

        /// <summary>
        /// Updates the categories on the given ICategorizable to the themes with the given ids.  Ensure the categories
        /// are already loaded via the context before calling this method.
        /// </summary>
        /// <param name="categoryIds">The categories by id.</param>
        /// <param name="categorizable">The categorizable to update.</param>
        public void SetCategories(List<int> categoryIds, ICategorizable categorizable)
        {
            Contract.Requires(categoryIds != null, "The category ids must not be null.");
            Contract.Requires(categorizable != null, "The categorizable entity must not be null.");
            var categoriesToRemove = categorizable.Categories.Where(x => !categoryIds.Contains(x.CategoryId)).ToList();
            var categoriesToAdd = new List<Category>();
            categoryIds.Where(x => !categorizable.Categories.Select(c => c.CategoryId).ToList().Contains(x)).ToList()
                .Select(x => new Category { CategoryId = x }).ToList()
                .ForEach(x => categoriesToAdd.Add(x));

            categoriesToAdd.ForEach(x =>
            {
                var localEntity = Context.GetLocalEntity<Category>(y => y.CategoryId == x.CategoryId);
                if (localEntity == null)
                {
                    if (Context.GetEntityState(x) == EntityState.Detached)
                    {
                        Context.Categories.Attach(x);
                    }
                    categorizable.Categories.Add(x);
                }
                else
                {
                    categorizable.Categories.Add(localEntity);
                }
            });
            categoriesToRemove.ForEach(x =>
            {
                categorizable.Categories.Remove(x);
            });
        }

        /// <summary>
        /// Updates the objectives on the given IObjectivable to the objectives with the given ids.  Ensure the objectives
        /// are already loaded via the context before calling this method.
        /// </summary>
        /// <param name="objectiveIds">The objectives by id.</param>
        /// <param name="objectivable">The Objectivable to update.</param>
        public void SetObjectives(List<int> objectiveIds, IObjectivable objectivable)
        {
            Contract.Requires(objectiveIds != null, "The objective ids must not be null.");
            Contract.Requires(objectivable != null, "The Objectivable entity must not be null.");
            var objectivesToRemove = objectivable.Objectives.Where(x => !objectiveIds.Contains(x.ObjectiveId)).ToList();
            var objectivesToAdd = new List<Objective>();
            objectiveIds.Where(x => !objectivable.Objectives.Select(o => o.ObjectiveId).ToList().Contains(x)).ToList()
                .Select(x => new Objective { ObjectiveId = x }).ToList()
                .ForEach(x => objectivesToAdd.Add(x));

            objectivesToAdd.ForEach(x =>
            {
                var localEntity = Context.GetLocalEntity<Objective>(y => y.ObjectiveId == x.ObjectiveId);
                if (localEntity == null)
                {
                    if (Context.GetEntityState(x) == EntityState.Detached)
                    {
                        Context.Objectives.Attach(x);
                    }
                    objectivable.Objectives.Add(x);
                }
                else
                {
                    objectivable.Objectives.Add(localEntity);
                }
            });
            objectivesToRemove.ForEach(x =>
            {
                objectivable.Objectives.Remove(x);
            });
        }

        /// <summary>
        /// Sets participants on the given instance with the given participant ids.
        /// </summary>
        /// <typeparam name="T">The class type with an ICollection of locations property.</typeparam>
        /// <param name="participantIds">The locations by id.</param>
        /// <param name="participantPropertySelector">The lambda to select a property typed to a collection of participants.</param>
        /// <param name="instance">The instance that contains a collection of locations to be set.</param>
        public void SetParticipants<T>(List<int> participantIds, T instance, Expression<Func<T, ICollection<Participant>>> participantPropertySelector)
        {
            Contract.Requires(participantIds != null, "The participant ids must not be null.");
            Contract.Requires(participantPropertySelector != null, "The expression must not be null.");
            Contract.Requires(instance != null, "The instance must not be null.");

            var participants = GetPropertyValue<T, ICollection<Participant>>(instance, participantPropertySelector);

            var participantsToRemove = participants.Where(x => !participantIds.Contains(x.ParticipantId)).ToList();
            var participantsToAdd = new List<Participant>();
            participantIds.Where(x => !participants.Select(o => o.ParticipantId).ToList().Contains(x)).ToList()
                .Select(x => new Participant { ParticipantId = x }).ToList()
                .ForEach(x => participantsToAdd.Add(x));

            participantsToAdd.ForEach(x =>
            {
                var localEntity = Context.GetLocalEntity<Participant>(y => y.ParticipantId == x.ParticipantId);
                if (localEntity == null)
                {
                    if (Context.GetEntityState(x) == EntityState.Detached)
                    {
                        Context.Participants.Attach(x);
                    }
                    participants.Add(x);
                }
                else
                {
                    participants.Add(localEntity);
                }
            });
            participantsToRemove.ForEach(x =>
            {
                participants.Remove(x);
            });
            SetPropertyValue<T, ICollection<Participant>>(instance, participantPropertySelector, participants);
        }

        /// <summary>
        /// Sets locations on the given instance with the given location ids.
        /// </summary>
        /// <typeparam name="T">The class type with an ICollection of locations property.</typeparam>
        /// <param name="locationIds">The locations by id.</param>
        /// <param name="locationPropertySelector">The lambda to select a property typed to a collection of locations.</param>
        /// <param name="instance">The instance that contains a collection of locations to be set.</param>
        public void SetLocations<T>(List<int> locationIds, T instance, Expression<Func<T, ICollection<Location>>> locationPropertySelector)
        {
            Contract.Requires(locationIds != null, "The location ids must not be null.");
            Contract.Requires(locationPropertySelector != null, "The expression must not be null.");
            Contract.Requires(instance != null, "The instance must not be null.");

            var locations = GetPropertyValue<T, ICollection<Location>>(instance, locationPropertySelector);

            var locationsToRemove = locations.Where(x => !locationIds.Contains(x.LocationId)).ToList();
            var locationsToAdd = new List<Location>();
            locationIds.Where(x => !locations.Select(o => o.LocationId).ToList().Contains(x)).ToList()
                .Select(x => new Location { LocationId = x }).ToList()
                .ForEach(x => locationsToAdd.Add(x));

            locationsToAdd.ForEach(x =>
            {
                var localEntity = Context.GetLocalEntity<Location>(y => y.LocationId == x.LocationId);
                if (localEntity == null)
                {
                    if (Context.GetEntityState(x) == EntityState.Detached)
                    {
                        Context.Locations.Attach(x);
                    }
                    locations.Add(x);
                }
                else
                {
                    locations.Add(localEntity);
                }
            });
            locationsToRemove.ForEach(x =>
            {
                locations.Remove(x);
            });
            SetPropertyValue<T, ICollection<Location>>(instance, locationPropertySelector, locations);
        }

        private TProperty GetPropertyValue<TInstance, TProperty>(TInstance instance, Expression<Func<TInstance, TProperty>> propertySelector)
        {
            var memberSelectorExpression = propertySelector.Body as MemberExpression;
            Contract.Assert(memberSelectorExpression != null, "The member must not be null.");
            var property = memberSelectorExpression.Member as PropertyInfo;
            Contract.Assert(property != null, "The property must not be null.");
            return (TProperty)property.GetValue(instance);
        }

        private void SetPropertyValue<TInstance, TProperty>(TInstance instance, Expression<Func<TInstance, TProperty>> propertySelector, TProperty value)
        {
            var memberSelectorExpression = propertySelector.Body as MemberExpression;
            Contract.Assert(memberSelectorExpression != null, "The member must not be null.");
            var property = memberSelectorExpression.Member as PropertyInfo;
            Contract.Assert(property != null, "The property must not be null.");
            property.SetValue(instance, value);
        }

        /// <summary>
        /// Get a list of locations
        /// </summary>
        /// <param name="locationIds">Ids to lookup</param>
        /// <returns>A list of locations</returns>
        protected List<Location> GetLocationsById(List<int> locationIds)
        {
            var locations = CreateGetLocationsById(locationIds).ToList();
            logger.Trace("Retrieved locations by ids {0}.", String.Join(", ", locationIds));
            return locations;
        }

        /// <summary>
        /// Get a list of locations
        /// </summary>
        /// <param name="locationIds">Ids to lookup</param>
        /// <returns>A list of locations</returns>
        protected async Task<List<Location>> GetLocationsByIdAsync(List<int> locationIds)
        {
            var locations = await CreateGetLocationsById(locationIds).ToListAsync();
            logger.Trace("Retrieved locations by ids {0}.", String.Join(", ", locationIds));
            return locations;
        }
        
        /// <summary>
        /// Get a list of citizen countries
        /// </summary>
        /// <param name="locations">Locations to lookup</param>
        /// <returns>A list of citizen countries</returns>
        protected List<PersonDependentCitizenCountry> GetCitizenshipCountriesById(int dependentId, List<CitizenCountryDTO> locations)
        {
            var locationIds = locations.Select(x => x.LocationId);
            List<PersonDependentCitizenCountry> dependentCitizenCountries = new List<PersonDependentCitizenCountry>();

            locations.ForEach(x =>
            {
                dependentCitizenCountries.Add(new PersonDependentCitizenCountry { DependentId = dependentId, LocationId = x.LocationId, IsPrimary = x.IsPrimary });
            });

            logger.Trace("Retrieved locations by ids {0}.", String.Join(", ", locations.Select(x => x.LocationId)));
            return dependentCitizenCountries;
        }
        
        /// <summary>
        /// Creates query for looking up a list of locations
        /// </summary>
        /// <param name="locationIds">Ids to lookup</param>
        /// <returns>Queryable list of locations</returns>
        private IQueryable<Location> CreateGetLocationsById(List<int> locationIds)
        {
            var locations = Context.Locations.Where(x => locationIds.Contains(x.LocationId));
            logger.Trace("Retrieved locations by ids {0}.", String.Join(", ", locationIds));
            return locations;
        }
        
        /// <summary>
        /// Gets a location by id
        /// </summary>
        /// <param name="locationId">Location id to lookup</param>
        /// <returns>Location</returns>
        public Location GetLocationById(int locationId)
        {
            var location = CreateGetLocationById(locationId).FirstOrDefault();
            logger.Trace("Retrieved location by id {0}.", locationId);
            return location;
        }

        /// <summary>
        /// Gets a location by id
        /// </summary>
        /// <param name="locationId">Location id to lookup</param>
        /// <returns>Location</returns>
        protected async Task<Location> GetLocationByIdAsync(int locationId)
        {
            var location = await CreateGetLocationById(locationId).FirstOrDefaultAsync();
            logger.Trace("Retrieved location by id {0}.", locationId);
            return location;
        }

        /// <summary>
        /// Creates query to lookup a single location
        /// </summary>
        /// <param name="locationId">The location id to lookup</param>
        /// <returns>Location</returns>
        private IQueryable<Location> CreateGetLocationById(int locationId)
        {
            var location = Context.Locations.Where(x => x.LocationId == locationId);
            logger.Trace("Retrieved location by id {0}.", locationId);
            return location;
        }

        /// <summary>
        /// Gets a participant by person id
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>Participant</returns>
        public ParticipantPersonSevisDTO GetParticipantByPersonId(int personId)
        {
            var participant = CreateGetParticipantById(personId).FirstOrDefault();
            logger.Trace("Retrieved location by id {0}.", personId);
            return participant;
        }

        /// <summary>
        /// Gets a participant by person id
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>Participant</returns>
        public async Task<ParticipantPersonSevisDTO> GetParticipantByPersonIdAsync(int personId)
        {
            var participant = await CreateGetParticipantById(personId).FirstOrDefaultAsync();
            logger.Trace("Retrieved location by id {0}.", personId);
            return participant;
        }

        /// <summary>
        /// Creates query to lookup a single participant
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        private IQueryable<ParticipantPersonSevisDTO> CreateGetParticipantById(int personId)
        {
            var participant = (from p in Context.ParticipantPersons
                              let sevisStatus = p.ParticipantPersonSevisCommStatuses.OrderByDescending(x => x.AddedOn).Select(x => x.SevisCommStatusId).FirstOrDefault()
                              where p.Participant.PersonId == personId 
                              && p.Participant.ParticipantStatusId == ParticipantStatus.Active.Id
                               select new ParticipantPersonSevisDTO
                               {
                                   ParticipantId = p.ParticipantId,
                                   SevisStatusId = sevisStatus
                               });

            //Context.Participants.Where(x => x.PersonId == personId && x.ParticipantStatusId == ParticipantStatus.Active.Id).Include(x => x.ParticipantPerson).Include(x => x.ParticipantPerson.ParticipantPersonSevisCommStatuses.OrderByDescending(d => d.AddedOn).Select(s => s.SevisCommStatusId).FirstOrDefault());
            logger.Trace("Retrieved location by id {0}.", personId);
            return participant;
        }
        
        /// <summary>
        /// Returns a query to find locations that were not previously set on an entity and are currently inactive.  This is useful
        /// when an entity had locations set that have since become inactive and those inactive values should be not removed.
        /// </summary>
        /// <param name="entityLocations">The query to retrieve locations of the entity, both active and inactive.</param>
        /// <param name="allLocationIds">All locations by id that should be set on the entity.</param>
        /// <returns>The locations that were not previously set on the entity that have since become inactive.</returns>
        protected IQueryable<Location> CreateGetNewInactiveEntityLocationsQuery(IQueryable<Location> entityLocations, IEnumerable<int> allLocationIds)
        {
            var newInactiveLocationsQuery = from location in Context.Locations
                                            where allLocationIds.Contains(location.LocationId)
                                            && !location.IsActive
                                            && !entityLocations.Select(x => x.LocationId).Contains(location.LocationId)
                                            select location;
            return newInactiveLocationsQuery;
        }

        /// <summary>
        /// Returns a query to find locations for a project by id that were not previously set on the project and are inactive.
        /// </summary>
        /// <param name="projectId">The id of the project.</param>
        /// <param name="allLocationIds">The ids of the locations that the project will have set.</param>
        /// <returns>The query to retrieve locations that were not previously set on the project and are inactive.</returns>
        protected IQueryable<Location> GetNewInactiveProjectLocations(int projectId, IEnumerable<int> allLocationIds)
        {
            return CreateGetNewInactiveEntityLocationsQuery(
                Context.Projects.Where(x => x.ProjectId == projectId).SelectMany(x => x.Locations),
                allLocationIds);
        }

        /// <summary>
        /// Returns a query to find locations for a program by id that were not previously set on the program and are inactive.
        /// </summary>
        /// <param name="programId">The id of the program.</param>
        /// <param name="allLocationIds">The ids of the locations that the program will have set.</param>
        /// <returns>The query to retrieve locations that were not previously set on the program and are inactive.</returns>
        protected IQueryable<Location> GetNewInactiveProgramLocations(int programId, IEnumerable<int> allLocationIds)
        {
            var programLocations = (from program in Context.Programs
                                    let regions = program.Regions
                                    let locations = program.Locations
                                    where program.ProgramId == programId
                                    select regions.Union(locations)).SelectMany(x => x).Distinct();

            return CreateGetNewInactiveEntityLocationsQuery(programLocations, allLocationIds);
        }

        /// <summary>
        /// Returns a query to find theme ids
        /// </summary>
        /// <param name="currentThemes">Current themes of project or program</param>
        /// <returns>The query to retrieve allowed theme ids</returns>
        protected IQueryable<int> CreateGetAllowedThemeIdsQuery(IEnumerable<int> currentThemes)
        {
            var activeThemeIds = Context.Themes.Where(x => x.IsActive == true).Select(x => x.ThemeId);
            var allowedThemeIds = activeThemeIds.Union(currentThemes);
            return allowedThemeIds;
        }

        /// <summary>
        /// Returns a query to find goal ids
        /// </summary>
        /// <param name="currentGoals">Current goals of project or program</param>
        /// <returns>The query to retrieve allowed goal ids</returns>
        protected IQueryable<int> CreateGetAllowedGoalIdsQuery(IEnumerable<int> currentGoals)
        {
            var activeGoalIds = Context.Goals.Where(x => x.IsActive == true).Select(x => x.GoalId);
            var allowedGoalIds = activeGoalIds.Union(currentGoals);
            return allowedGoalIds;
        }

    }
}
