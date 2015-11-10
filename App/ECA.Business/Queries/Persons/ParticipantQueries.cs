﻿using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Persons
{
    /// <summary>
    /// The ParticipantQueries are used to query a DbContext for Participant information.
    /// </summary>
    public static class ParticipantQueries
    {
        /// <summary>
        /// Returns a query that selects SimpleParticipantDTO's from the given context's participant persons.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query.</returns>
        public static IQueryable<SimpleParticipantDTO> CreateGetPersonParticipantsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from participant in context.Participants
                        let person = participant.Person
                        let participantPerson = participant.ParticipantPerson
                        let location = person == null ? null : person.Addresses.OrderByDescending(x => x.IsPrimary).FirstOrDefault()
                        let participantType = participant.ParticipantType
                        where participant.PersonId.HasValue
                        select new SimpleParticipantDTO
                        {
                            Name = person.FullName,
                            City = location == null
                                ? null : location.Location == null
                                ? null : location.Location.City == null
                                ? null : location.Location.City.LocationName,
                            Country = location == null
                                ? null : location.Location == null
                                ? null : location.Location.Country == null
                                ? null : location.Location.Country.LocationName,
                            OrganizationId = default(int?),
                            ParticipantId = participant.ParticipantId,
                            ParticipantType = participantType.Name,
                            ParticipantTypeId = participantType.ParticipantTypeId,
                            PersonId = person.PersonId,
                            ProjectId = participant.ProjectId,
                            RevisedOn = participant.History.RevisedOn,
                            ParticipantStatus = participant.Status == null ? null : participant.Status.Status,
                            SevisStatus = participantPerson == null ? "None" : participantPerson.ParticipantPersonSevisCommStatuses.Count == 0 ? "None" : participantPerson.ParticipantPersonSevisCommStatuses.OrderByDescending(p => p.AddedOn).FirstOrDefault().SevisCommStatus.SevisCommStatusName
                        };
            return query;
        }

        /// <summary>
        /// Returns a query that selects SimpleParticipantDTOs from the given context's participant organizations.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query.</returns>
        public static IQueryable<SimpleParticipantDTO> CreateGetOrganizationParticipantsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from participant in context.Participants
                        let org = participant.Organization
                        let location = org == null ? null : org.Addresses.OrderByDescending(x => x.IsPrimary).FirstOrDefault()
                        let participantType = participant.ParticipantType
                        where participant.PersonId == null
                        select new SimpleParticipantDTO
                        {
                            Name = org.Name,
                            City = location == null
                                ? null : location.Location == null
                                ? null : location.Location.City == null
                                ? null : location.Location.City.LocationName,
                            Country = location == null
                                ? null : location.Location == null
                                ? null : location.Location.Country == null
                                ? null : location.Location.Country == null
                                ? null : location.Location.Country.LocationName,
                            OrganizationId = org.OrganizationId,
                            ParticipantId = participant.ParticipantId,
                            ParticipantType = participantType.Name,
                            ParticipantTypeId = participantType.ParticipantTypeId,
                            PersonId = default(int?),
                            ProjectId = participant.ProjectId,
                            RevisedOn = participant.History.RevisedOn,
                            ParticipantStatus = participant.Status == null ? null : participant.Status.Status,
                            SevisStatus = "N/A"
                        };

            return query;
        }

        private static IQueryable<SimpleParticipantDTO> CreateGetUnionedSimpleParticipantsDTOQuery(EcaContext context)
        {
            return CreateGetOrganizationParticipantsQuery(context).Union(CreateGetPersonParticipantsQuery(context));
        }

        /// <summary>
        /// Creates a query to return all participants in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The filtered and sorted query to retrieve participants.</returns>
        public static IQueryable<SimpleParticipantDTO> CreateGetSimpleParticipantsDTOQuery(EcaContext context, QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetUnionedSimpleParticipantsDTOQuery(context);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Creates a query to return all participants for the project with the given id in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>The filtered and sorted query to retrieve participants.</returns>
        public static IQueryable<SimpleParticipantDTO> CreateGetSimpleParticipantsDTOByProjectIdQuery(EcaContext context, int projectId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetUnionedSimpleParticipantsDTOQuery(context).Where(x => x.ProjectId == projectId);
            return query;
        }

        /// <summary>
        /// Creates a query to return all participants for the project with the given id in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>The filtered and sorted query to retrieve participants.</returns>
        public static IQueryable<SimpleParticipantDTO> CreateGetSimpleParticipantsDTOByProjectIdQuery(EcaContext context, int projectId, QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetSimpleParticipantsDTOByProjectIdQuery(context, projectId);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns the participant by participant id 
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participant</returns>
        public static IQueryable<ParticipantDTO> CreateGetParticipantDTOByIdQuery(EcaContext context, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from participant in context.Participants
                        let person = participant.Person
                        let participantType = participant.ParticipantType
                        where participant.PersonId != null &&
                        participant.ParticipantId == participantId
                        select new ParticipantDTO
                        {
                            Name = person.FullName,
                            OrganizationId = default(int?),
                            ParticipantId = participant.ParticipantId,
                            ParticipantType = participantType.Name,
                            ParticipantTypeId = participantType.ParticipantTypeId,
                            PersonId = person.PersonId,
                            ProjectId = participant.ProjectId,
                            Status = participant.Status.Status,
                            StatusDate = participant.StatusDate,
                            RevisedOn = participant.History.RevisedOn
                        };
            return query;
        }
    }
}
