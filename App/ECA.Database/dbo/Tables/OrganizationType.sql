CREATE TABLE [dbo].[OrganizationType] (
    [OrganizationTypeId]   INT                IDENTITY (1, 1) NOT NULL,
    [OrganizationTypeName] NVARCHAR (MAX)     NOT NULL,
    [History_CreatedBy]    INT                NOT NULL,
    [History_CreatedOn]    DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]    INT                NOT NULL,
    [History_RevisedOn]    DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.OrganizationType] PRIMARY KEY CLUSTERED ([OrganizationTypeId] ASC)
);

