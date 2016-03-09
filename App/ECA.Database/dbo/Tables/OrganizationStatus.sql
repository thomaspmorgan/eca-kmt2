CREATE TABLE [dbo].[OrganizationStatus] (
    [OrganizationStatusId]           INT                IDENTITY (1, 1) NOT NULL,
    [Status]                        NVARCHAR(50)                NOT NULL,
    [History_CreatedBy]             INT                NOT NULL,
    [History_CreatedOn]             DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]             INT                NOT NULL,
    [History_RevisedOn]             DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.OrganizationStatus] PRIMARY KEY CLUSTERED ([OrganizationStatusId] ASC)
);


GO
