CREATE TABLE [dbo].[Theme] (
    [ThemeId]           INT                IDENTITY (1, 1) NOT NULL,
    [ThemeName]         NVARCHAR (150)     NOT NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.Theme] PRIMARY KEY CLUSTERED ([ThemeId] ASC)
);

