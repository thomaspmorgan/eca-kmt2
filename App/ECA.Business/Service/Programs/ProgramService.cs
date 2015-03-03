using System.Linq;
using System.Data.Entity;
using ECA.Business.Models.Programs;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Queries.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Data;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using ECA.Core.Service;
using System.Collections.Generic;

namespace ECA.Business.Service.Programs
{
    /// <summary>
    /// A ProgramService is capable of performing crud operations on a program using entity framework.
    /// </summary>
    public class ProgramService : DbContextService<EcaContext>, IProgramService
    {
        /// <summary>
        /// Creates a new ProgramService with the given context to operator against.
        /// </summary>
        /// <param name="context">The context to operate on.</param>
        public ProgramService(EcaContext context) : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get

        /// <summary>
        /// Returns a paged, filtered, and sorted list of programs in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted list of program in the system.</returns>
        public PagedQueryResults<SimpleProgramDTO> GetPrograms(QueryableOperator<SimpleProgramDTO> queryOperator)
        {
            return ProgramQueries.CreateGetSimpleProgramDTOsQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns a paged, filtered, and sorted list of programs in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted list of program in the system.</returns>
        public Task<PagedQueryResults<SimpleProgramDTO>> GetProgramsAsync(QueryableOperator<SimpleProgramDTO> queryOperator)
        {
            return ProgramQueries.CreateGetSimpleProgramDTOsQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns the program with the given id, or null if it does not exist.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <returns>The program, or null if it doesn't exist.</returns>
        public ProgramDTO GetProgramById(int programId)
        {
            return ProgramQueries.CreateGetPublishedProgramByIdQuery(this.Context, programId).FirstOrDefault();
        }

        /// <summary>
        /// Returns the program with the given id, or null if it does not exist.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <returns>The program, or null if it doesn't exist.</returns>
        public Task<ProgramDTO> GetProgramByIdAsync(int programId)
        {
            return ProgramQueries.CreateGetPublishedProgramByIdQuery(this.Context, programId).FirstOrDefaultAsync();
        }

        #endregion

        #region Create

        /// <summary>
        /// Creates a new program in the ECA system with a status of draft.
        /// </summary>
        /// <param name="draftProgram">The draft program.</param>
        /// <returns>The saved program.</returns>
        public Program Create(DraftProgram draftProgram)
        {
            return DoCreate(draftProgram);
        }

        private Program DoCreate(DraftProgram draftProgram)
        {
            var program = new Program
            {
                Description = draftProgram.Description,
                EndDate = draftProgram.EndDate,
                Focus = draftProgram.Focus,
                Name = draftProgram.Name,
                ProgramType = null,
                ProgramStatusId = draftProgram.ProgramStatusId,
                StartDate = draftProgram.StartDate,
                OwnerId = draftProgram.OwnerOrganizationId,
                ParentProgramId = draftProgram.ParentProgramId,
                Website = draftProgram.Website
            };
            draftProgram.NewHistory.SetHistory(program);
            SetGoals(draftProgram.GoalIds, program);
            SetPointOfContacts(draftProgram.PointOfContactIds, program);
            SetThemes(draftProgram.ThemeIds, program);
            return program;
        }
        #endregion

        /// <summary>
        /// Updates the goals on the given program to the goals with the given ids.
        /// </summary>
        /// <param name="goalIds">The goal ids.</param>
        /// <param name="programEntity">The program.</param>
        public void SetGoals(List<int> goalIds, Program programEntity)
        {
            Contract.Requires(goalIds != null, "The goal ids must not be null.");
            Contract.Requires(programEntity != null, "The program entity must not be null.");
            programEntity.Goals.Clear();
            goalIds.ForEach(x =>
            {
                var goal = new Goal { GoalId = x };
                this.Context.Goals.Attach(goal);
                programEntity.Goals.Add(goal);
                goal.Programs.Add(programEntity);
            });
        }

        /// <summary>
        /// Updates the points of contacts on the given program to the pocs with the given ids.
        /// </summary>
        /// <param name="pointOfContactIds">The points of contacts by id.</param>
        /// <param name="programEntity">The program to update.</param>
        public void SetPointOfContacts(List<int> pointOfContactIds, Program programEntity)
        {
            Contract.Requires(pointOfContactIds != null, "The list of poc ids must not be null.");
            Contract.Requires(programEntity != null, "The program entity must not be null.");
            programEntity.Contacts.Clear();
            pointOfContactIds.ForEach(x =>
            {
                var contact = new Contact { ContactId = x };
                this.Context.Contacts.Attach(contact);
                programEntity.Contacts.Add(contact);
                contact.Programs.Add(programEntity);
            });
        }

        /// <summary>
        /// Updates the themes on the given program to the themes with the given ids.
        /// </summary>
        /// <param name="themeIds">The themes by id.</param>
        /// <param name="programEntity">The program to update.</param>
        public void SetThemes(List<int> themeIds, Program programEntity)
        {
            Contract.Requires(themeIds != null, "The theme ids must not be null.");
            Contract.Requires(programEntity != null, "The program entity must not be null.");
            programEntity.Themes.Clear();
            themeIds.ForEach(x =>
            {
                var theme = new Theme { ThemeId = x };
                this.Context.Themes.Attach(theme);
                programEntity.Themes.Add(theme);
                theme.Programs.Add(programEntity);
            });
        }


    }
}
