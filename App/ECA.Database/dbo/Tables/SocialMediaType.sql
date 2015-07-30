CREATE TABLE [dbo].[SocialMediaType] (
    [SocialMediaTypeId]   INT                IDENTITY (1, 1) NOT NULL,
    [SocialMediaTypeName] NVARCHAR (20)      NOT NULL,
	[Url] NVARCHAR (255)      NOT NULL DEFAULT '',
    [History_CreatedBy]   INT                NOT NULL,
    [History_CreatedOn]   DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]   INT                NOT NULL,
    [History_RevisedOn]   DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.SocialMediaType] PRIMARY KEY CLUSTERED ([SocialMediaTypeId] ASC)
);

