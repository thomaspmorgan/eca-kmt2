using System.Data.Entity;
using ECA.Core.Logging;
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

namespace ECA.Business.Service
{
    public class EcaService : DbContextService<EcaContext>
    {
        private static readonly string COMPONENT_NAME = typeof(EcaService).FullName;

        private readonly ILogger logger;

        public EcaService(EcaContext context, ILogger logger)
            : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(logger != null, "The logger must not be null.");
            this.logger = logger;
        }

        #region Contact Extistence Validation

        private IQueryable<Contact> CreateGetContactsByIdsQuery(IEnumerable<int> contactIds)
        {
            Contract.Requires(contactIds != null, "The contact ids must not be null.");
            return Context.Contacts.Where(x => contactIds.Distinct().Contains(x.ContactId)).Distinct();
        }

        public async Task<bool> CheckAllContactsExistAsync(IEnumerable<int> contactIds)
        {
            Contract.Requires(contactIds != null, "The contact ids must not be null.");
            var stopWatch = Stopwatch.StartNew();
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
            stopWatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
            return response;
        }

        public bool CheckAllContactsExist(IEnumerable<int> contactIds)
        {
            Contract.Requires(contactIds != null, "The contact ids must not be null.");
            var stopWatch = Stopwatch.StartNew();
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
            stopWatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
            return response;
        }
        #endregion

        #region Theme Existence Validation

        private IQueryable<Theme> CreateGetThemesByIdsQuery(IEnumerable<int> themeIds)
        {
            Contract.Requires(themeIds != null, "The theme ids must not be null.");
            return Context.Themes.Where(x => themeIds.Distinct().Contains(x.ThemeId)).Distinct();
        }

        public async Task<bool> CheckAllThemesExistAsync(IEnumerable<int> themeIds)
        {
            Contract.Requires(themeIds != null, "The theme ids must not be null.");
            var stopWatch = Stopwatch.StartNew();
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
            stopWatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
            return response;
        }

        public bool CheckAllThemesExist(IEnumerable<int> themeIds)
        {
            Contract.Requires(themeIds != null, "The theme ids must not be null.");
            var stopWatch = Stopwatch.StartNew();
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
            stopWatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
            return response;
        }
        #endregion

        #region Goals Existence Validation

        private IQueryable<Goal> CreateGetGoalsByIdsQuery(IEnumerable<int> goalIds)
        {
            Contract.Requires(goalIds != null, "The theme ids must not be null.");
            return Context.Goals.Where(x => goalIds.Distinct().Contains(x.GoalId)).Distinct();
        }

        public async Task<bool> CheckAllGoalsExistAsync(IEnumerable<int> goalIds)
        {
            Contract.Requires(goalIds != null, "The goal ids must not be null.");
            var stopWatch = Stopwatch.StartNew();
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
            stopWatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
            return response;
        }

        public bool CheckAllGoalsExist(IEnumerable<int> goalIds)
        {
            Contract.Requires(goalIds != null, "The goal ids must not be null.");
            var stopWatch = Stopwatch.StartNew();
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
            stopWatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
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
            var stopWatch = Stopwatch.StartNew();
            var focus = this.Context.Foci.Find(focusId);
            stopWatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
            return focus;
        }

        /// <summary>
        /// Returns the focus with the given id.
        /// </summary>
        /// <param name="focusId">The focus id.</param>
        /// <returns>The focus.</returns>
        protected async Task<Focus> GetFocusByIdAsync(int focusId)
        {
            var stopWatch = Stopwatch.StartNew();
            var focus = await this.Context.Foci.FindAsync(focusId);
            stopWatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
            return focus;
        }

        /// <summary>
        /// Updates the goals on the given program to the goals with the given ids.
        /// </summary>
        /// <param name="goalIds">The goal ids.</param>
        /// <param name="goalable">The program.</param>
        public void SetGoals(List<int> goalIds, IGoalable goalable)
        {
            Contract.Requires(goalIds != null, "The goal ids must not be null.");
            Contract.Requires(goalable != null, "The goalable entity must not be null.");
            goalable.Goals.Clear();
            goalIds.ForEach(x =>
            {
                var goal = new Goal { GoalId = x };
                this.Context.Goals.Attach(goal);
                goalable.Goals.Add(goal);
            });
        }

        /// <summary>
        /// Updates the points of contacts on the given program to the pocs with the given ids.
        /// </summary>
        /// <param name="pointOfContactIds">The points of contacts by id.</param>
        /// <param name="contactable">The program to update.</param>
        public void SetPointOfContacts(List<int> pointOfContactIds, IContactable contactable)
        {
            Contract.Requires(pointOfContactIds != null, "The list of poc ids must not be null.");
            Contract.Requires(contactable != null, "The contactable entity must not be null.");
            contactable.Contacts.Clear();
            pointOfContactIds.ForEach(x =>
            {
                var contact = new Contact { ContactId = x };
                this.Context.Contacts.Attach(contact);
                contactable.Contacts.Add(contact);
            });
        }

        /// <summary>
        /// Updates the themes on the given program to the themes with the given ids.
        /// </summary>
        /// <param name="themeIds">The themes by id.</param>
        /// <param name="themeable">The program to update.</param>
        public void SetThemes(List<int> themeIds, IThemeable themeable)
        {
            Contract.Requires(themeIds != null, "The theme ids must not be null.");
            Contract.Requires(themeable != null, "The themeable entity must not be null.");
            themeable.Themes.Clear();
            themeIds.ForEach(x =>
            {
                var theme = new Theme { ThemeId = x };
                this.Context.Themes.Attach(theme);
                themeable.Themes.Add(theme);
            });
        }
    }
}
