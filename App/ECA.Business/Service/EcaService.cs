using System.Data.Entity;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ECA.Business.Queries.Admin;
using NLog;

namespace ECA.Business.Service
{
    public class EcaService : DbContextService<EcaContext>
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public EcaService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Contact Extistence Validation

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
            Contract.Requires(categoryIds != null, "The theme ids must not be null.");
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
            logger.Trace("Checked all themes with ids {0} exist.", String.Join(", ", categoryIds));
            return response;
        }

        /// <summary>
        /// A method to verify all themes with the given ids exist in the system.
        /// </summary>
        /// <param name="themeIds">The themes by Id to validate existence.</param>
        /// <returns>True, if all themes exist, otherwise false.</returns>
        public bool CheckAllCategoriesExist(IEnumerable<int> categoryIds)
        {
            Contract.Requires(categoryIds != null, "The theme ids must not be null.");
            bool response = false;
            if (categoryIds.Count() == 0)
            {
                response = true;
            }
            else
            {
                var distinctIds = categoryIds.Distinct().ToList();
                response = CreateGetThemesByIdsQuery(categoryIds).Count() == distinctIds.Count();
            }
            logger.Trace("Checked all themes exist.");
            return response;
        }
        #endregion

        #region Objectives Existence Validation
        private IQueryable<Objective> CreateGetObjectivesByIdsQuery(IEnumerable<int> objectiveIds)
        {
            Contract.Requires(objectiveIds != null, "The theme ids must not be null.");
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
            Contract.Requires(objectiveIds != null, "The theme ids must not be null.");
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
            logger.Trace("Checked all objectives exist.");
            return response;
        }
        #endregion

        /// <summary>
        /// Returns the focus with the given id.
        /// </summary>
        /// <param name="focusId">The focus id.</param>
        /// <returns>The focus.</returns>
        protected Focus GetFocusById(int focusId)
        {
            var focus = this.Context.Foci.Find(focusId);
            logger.Trace("Loaded focus by id {0}.", focusId);
            return focus;
        }

        /// <summary>
        /// Returns the focus with the given id.
        /// </summary>
        /// <param name="focusId">The focus id.</param>
        /// <returns>The focus.</returns>
        protected async Task<Focus> GetFocusByIdAsync(int focusId)
        {
            var focus = await this.Context.Foci.FindAsync(focusId);
            logger.Trace("Loaded focus by id {0}.", focusId);
            return focus;
        }

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
                if (Context.GetEntityState(x) == EntityState.Detached)
                {
                    Context.Goals.Attach(x);
                }
                goalable.Goals.Add(x);
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
                if (Context.GetEntityState(x) == EntityState.Detached)
                {
                    Context.Contacts.Attach(x);
                }
                contactable.Contacts.Add(x);
            });
            contactsToRemove.ForEach(x =>
            {
                contactable.Contacts.Remove(x);
            });
        }

        /// <summary>
        /// Updates the regions on the given program to the regions with the given ids.
        /// </summary>
        /// <param name="regionIds">The regions by id.</param>
        /// <param name="programEntity">The program to update.</param>
        public void SetRegions(List<int> regionIds, ILocationable locationable)
        {
            Contract.Requires(regionIds != null, "The region ids must not be null.");
            Contract.Requires(locationable != null, "The contactable entity must not be null.");
            var regionsToRemove = locationable.Locations.Where(x => !regionIds.Contains(x.LocationId)).ToList();
            var regionsToAdd = new List<Location>();
            regionIds.Where(x => !locationable.Locations.Select(c => c.LocationId).ToList().Contains(x)).ToList()
                .Select(x => new Location { LocationId = x }).ToList()
                .ForEach(x => regionsToAdd.Add(x));

            regionsToAdd.ForEach(x =>
            {
                if (Context.GetEntityState(x) == EntityState.Detached)
                {
                    Context.Locations.Attach(x);
                }
                locationable.Locations.Add(x);
            });
            regionsToRemove.ForEach(x =>
            {
                locationable.Locations.Remove(x);
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
                if (Context.GetEntityState(x) == EntityState.Detached)
                {
                    Context.Themes.Attach(x);
                }
                themeable.Themes.Add(x);
            });
            themesToRemove.ForEach(x =>
            {
                themeable.Themes.Remove(x);
            });
        }

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
                if (Context.GetEntityState(x) == EntityState.Detached)
                {
                    Context.Categories.Attach(x);
                }
                categorizable.Categories.Add(x);
            });
            categoriesToRemove.ForEach(x =>
            {
                categorizable.Categories.Remove(x);
            });
        }

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
                if (Context.GetEntityState(x) == EntityState.Detached)
                {
                    Context.Objectives.Attach(x);
                }
                objectivable.Objectives.Add(x);
            });
            objectivesToRemove.ForEach(x =>
            {
                objectivable.Objectives.Remove(x);
            });
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
    }
}
