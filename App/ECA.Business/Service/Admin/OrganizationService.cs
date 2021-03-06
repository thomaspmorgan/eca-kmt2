﻿using ECA.Business.Queries.Admin;
using System.Data.Entity;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Core.Exceptions;
using ECA.Business.Validation;
using ECA.Business.Service.Lookup;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Service implementation for organizations
    /// </summary>
    public class OrganizationService : EcaService, IOrganizationService
    {

        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Action<int, Organization> throwIfOrganizationByIdNull;
        private readonly Action<int, OrganizationType> throwIfOrganizationTypeByIdNull;
        private IBusinessValidator<OrganizationValidationEntity, OrganizationValidationEntity> organizationValidator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Context to query</param>
        /// <param name="organizationValidator">The organization validator.</param>
        /// <param name="saveActions">The save actions.</param>
        public OrganizationService(EcaContext context, IBusinessValidator<OrganizationValidationEntity, OrganizationValidationEntity> organizationValidator, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(organizationValidator != null, "The validator must not be null.");
            this.organizationValidator = organizationValidator;
            throwIfOrganizationByIdNull = (orgId, org) =>
            {
                if (org == null)
                {
                    throw new ModelNotFoundException(String.Format("The organization with id [{0}] was not found.", orgId));
                }
            };
            throwIfOrganizationTypeByIdNull = (orgTypeId, orgType) =>
            {
                if (orgType == null)
                {
                    throw new ModelNotFoundException(String.Format("The organization type with id [{0}] was not found.", orgTypeId));
                }
            };
        }

        #region Get
        /// <summary>
        /// Returns list of organizations asyncronously
        /// </summary>
        /// <param name="queryOperator">The query operator to apply</param>
        /// <returns>List of organizations</returns>
        public Task<PagedQueryResults<SimpleOrganizationDTO>> GetOrganizationsAsync(QueryableOperator<SimpleOrganizationDTO> queryOperator)
        {
            var organizations = OrganizationQueries.CreateGetSimpleOrganizationsDTOQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved organizations with query operator [{0}].", queryOperator);
            return organizations;
        }

        /// <summary>
        /// Returns list of organizations asyncronously
        /// </summary>
        /// <param name="queryOperator">The query operator to apply</param>
        /// <returns>List of organizations</returns>
        public Task<PagedQueryResults<SimpleOrganizationDTO>> GetOrganizationsWOFundingSourceAsync(QueryableOperator<SimpleOrganizationDTO> queryOperator)
        {
            var organizations = OrganizationQueries.CreateGetSimpleOrganizationsDTOQueryWOFundingSource(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved organizations with query operator [{0}].", queryOperator);
            return organizations;
        }

        /// <summary>
        /// Returns organization hierarchy by role id
        /// </summary>
        /// <param name="organizationRoleId">The organization role id</param>
        /// <param name="queryOperator">The query operator to apply</param>
        /// <returns>The organizations hierarchy</returns>
        public async Task<PagedQueryResults<OrganizationHierarchyDTO>> GetOrganizationsHierarchyByRoleIdAsync(int organizationRoleId, QueryableOperator<OrganizationHierarchyDTO> queryOperator)
        {
            var results = await CreateGetOrganizationsByRoleIdSqlQuery(organizationRoleId).ToArrayAsync();
            var pagedResults = GetPagedQueryResults(results, queryOperator);
            this.logger.Trace("Retrieved offices with query operator [{0}].", queryOperator);
            return pagedResults;
        }

        private DbRawSqlQuery<OrganizationHierarchyDTO> CreateGetOrganizationsByRoleIdSqlQuery(int organizationRoleId)
        {
            return this.Context.Database.SqlQuery<OrganizationHierarchyDTO>("GetOrganizationsByRoleId @organizationRoleId", new SqlParameter("organizationRoleId", organizationRoleId));
        }

        private PagedQueryResults<T> GetPagedQueryResults<T>(IEnumerable<T> enumerable, QueryableOperator<T> queryOperator) where T : class
        {
            var queryable = enumerable.AsQueryable<T>();
            queryable = queryable.Apply(queryOperator);
            return queryable.ToPagedQueryResults<T>(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns list of organizations 
        /// </summary>
        /// <param name="queryOperator">The query operator to apply</param>
        /// <returns>List of organizations</returns>
        public PagedQueryResults<SimpleOrganizationDTO> GetOrganizations(QueryableOperator<SimpleOrganizationDTO> queryOperator)
        {
            var organizations = OrganizationQueries.CreateGetSimpleOrganizationsDTOQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved organizations with query operator [{0}].", queryOperator);
            return organizations;
        }

        /// <summary>
        /// Gets the organization with the given id.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <returns>The organization.</returns>
        public OrganizationDTO GetOrganizationById(int organizationId)
        {
            var dto = OrganizationQueries.CreateGetOrganizationDTOByOrganizationIdQuery(this.Context, organizationId).FirstOrDefault();
            this.logger.Trace("Retreived organization by id [{0}].", organizationId);
            return dto;
        }

        /// <summary>
        /// Gets the organization with the given id.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <returns>The organization.</returns>
        public async Task<OrganizationDTO> GetOrganizationByIdAsync(int organizationId)
        {
            var dto = await OrganizationQueries.CreateGetOrganizationDTOByOrganizationIdQuery(this.Context, organizationId).FirstOrDefaultAsync();
            this.logger.Trace("Retreived organization by id [{0}].", organizationId);
            return dto;
        }

        /// <summary>
        /// Get the organization roles
        /// </summary>
        /// <param name="queryOperator">The query operator to apply</param>
        /// <returns>List of organization roles</returns>
        public async Task<PagedQueryResults<SimpleLookupDTO>> GetOrganizationRolesAsync(QueryableOperator<SimpleLookupDTO> queryOperator)
        {
            var organizationRoles = await OrganizationQueries.CreateGetOrganizationRolesQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved organization roles with query operator [{0}].", queryOperator);
            return organizationRoles;
        }

        #endregion

        #region Create
        /// <summary>
        /// Create an organization
        /// </summary>
        /// <param name="newOrganization">The new organization to create</param>
        /// <returns>The created organization</returns>
        public async Task<Organization> CreateAsync(NewOrganization newOrganization)
        {
            var organizationType = this.Context.OrganizationTypes.Find(newOrganization.OrganizationType);
            var organization = DoCreate(newOrganization, organizationType);
            return await Task.FromResult<Organization>(organization);
        }

        /// <summary>
        /// Creates a participant organization
        /// </summary>
        /// <param name="participantOrganization">The participant organization to create</param>
        /// <returns>The organization created</returns>
        public async Task<Organization> CreateParticipantOrganizationAsync(ParticipantOrganization participantOrganization)
        {
            var project = this.Context.Projects.Find(participantOrganization.ProjectId);
            var organizationType = this.Context.OrganizationTypes.Find(participantOrganization.OrganizationType);
            var organization = DoCreate(participantOrganization, organizationType);
            var participant = CreateParticipant(organization, participantOrganization.ParticipantTypeId, project);
            return await Task.FromResult<Organization>(organization);
        }

        private Organization DoCreate(NewOrganization newOrganization, OrganizationType organizationType)
        {
            organizationValidator.ValidateCreate(GetOrganizationValidationEntity(newOrganization));
            var organization = new Organization
            {
                Name = newOrganization.Name,
                Description = newOrganization.Description,
                OrganizationType = organizationType,
                OrganizationTypeId = newOrganization.OrganizationType,
                Website = newOrganization.Website,
                Status = "Active"
            };

            SetOrganizationRoles(newOrganization.OrganizationRoles.ToList(), organization);
            SetPointOfContacts(newOrganization.PointsOfContact.ToList(), organization);

            newOrganization.Audit.SetHistory(organization);
            this.Context.Organizations.Add(organization);
            return organization;

        }

        private Participant CreateParticipant(Organization organization, int participantTypeId, Project project)
        {
            var participant = new Participant
            {
                OrganizationId = organization.OrganizationId,
                ParticipantTypeId = participantTypeId
            };

            participant.Project = project;
            this.Context.Participants.Add(participant);
            this.logger.Trace("Creating new participant {0}.", organization);
            return participant;
        }

        private OrganizationValidationEntity GetOrganizationValidationEntity(NewOrganization newOrganization)
        {
            return new OrganizationValidationEntity(name: newOrganization.Name, organizationTypeId: newOrganization.OrganizationType);
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates an organization.
        /// </summary>
        /// <param name="organization">The updated organization.</param>
        public void Update(EcaOrganization organization)
        {
            var organizationToUpdate = CreateGetOrganizationByIdQuery(organization.OrganizationId).FirstOrDefault();
            Organization parentOrg = null;
            if (organization.ParentOrganizationId.HasValue)
            {
                parentOrg = this.Context.Organizations.Find(organization.ParentOrganizationId.Value);
            }
            var organizationType = this.Context.OrganizationTypes.Find(organization.OrganizationTypeId);
            DoUpdate(organization, organizationToUpdate, parentOrg, organizationType);
        }

        /// <summary>
        /// Updates an organization.
        /// </summary>
        /// <param name="organization">The updated organization.</param>
        public async Task UpdateAsync(EcaOrganization organization)
        {
            var organizationToUpdate = await CreateGetOrganizationByIdQuery(organization.OrganizationId).FirstOrDefaultAsync();
            Organization parentOrg = null;
            if (organization.ParentOrganizationId.HasValue)
            {
                parentOrg = await this.Context.Organizations.FindAsync(organization.ParentOrganizationId.Value);
            }
            var organizationType = await this.Context.OrganizationTypes.FindAsync(organization.OrganizationTypeId);
            DoUpdate(organization, organizationToUpdate, parentOrg, organizationType);
        }

        private void DoUpdate(EcaOrganization organization, Organization organizationToUpdate, Organization parentOrganization, OrganizationType organizationType)
        {
            throwIfOrganizationByIdNull(organization.OrganizationId, organizationToUpdate);
            if (organization.ParentOrganizationId.HasValue)
            {
                throwIfOrganizationByIdNull(organization.ParentOrganizationId.Value, parentOrganization);
            }

            throwIfOrganizationTypeByIdNull(organization.OrganizationTypeId, organizationType);
            organizationValidator.ValidateUpdate(GetOrganizationValidationEntity(organization, organizationToUpdate, parentOrganization));
            organization.Update.SetHistory(organizationToUpdate);
            SetPointOfContacts(organization.ContactIds.ToList(), organizationToUpdate);
            SetOrganizationRoles(organization.OrganizationRoleIds.ToList(), organizationToUpdate);
            organizationToUpdate.Name = organization.Name;
            organizationToUpdate.Description = organization.Description;
            if (parentOrganization != null)
            {
                organizationToUpdate.ParentOrganization = parentOrganization;
                organizationToUpdate.ParentOrganizationId = parentOrganization.OrganizationId;
            }
            else
            {
                organizationToUpdate.ParentOrganization = null;
                organizationToUpdate.ParentOrganizationId = null;
            }
            organizationToUpdate.OrganizationTypeId = organization.OrganizationTypeId;
            organizationToUpdate.Website = organization.Website;
        }

        /// <summary>
        /// Sets the organization roles
        /// </summary>
        /// <param name="organizationRoleIds">Organization roles to set</param>
        /// <param name="organization">The organization entity</param>
        public void SetOrganizationRoles(List<int> organizationRoleIds, Organization organization)
        {
            Contract.Requires(organizationRoleIds != null, "The list of organization role ids must not be null.");
            Contract.Requires(organization != null, "The organization must not be null.");
            var roles = Context.OrganizationRoles.Where(x => organizationRoleIds.Contains(x.OrganizationRoleId)).ToList();
            organization.OrganizationRoles = roles;
        }

        private IQueryable<Organization> CreateGetOrganizationByIdQuery(int id)
        {
            return Context.Organizations
                .Include(x => x.Contacts)
                .Include(x => x.OrganizationRoles)
                .Include(x => x.ParentOrganization)
                .Where(x => x.OrganizationId == id);
        }

        private OrganizationValidationEntity GetOrganizationValidationEntity(EcaOrganization organization, Organization organizationToUpdate, Organization parentOrganization)
        {
            return new OrganizationValidationEntity(name: organization.Name, organizationTypeId: organization.OrganizationTypeId);
        }
        #endregion
    }
}
