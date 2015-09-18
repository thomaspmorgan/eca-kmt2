﻿CREATE TABLE [dbo].[OrganizationRole] (
    [OrganizationRoleId]   INT                IDENTITY (1, 1) NOT NULL,
    [OrganizationRoleName] NVARCHAR (100)     NOT NULL,
    [History_CreatedBy]    INT                NOT NULL,
    [History_CreatedOn]    DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]    INT                NOT NULL,
    [History_RevisedOn]    DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.OrganizationRole] PRIMARY KEY CLUSTERED ([OrganizationRoleId] ASC)
);