CREATE TABLE [dbo].[ParticipantType] (
    [ParticipantTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (150) NOT NULL,
    CONSTRAINT [PK_dbo.ParticipantType] PRIMARY KEY CLUSTERED ([ParticipantTypeId] ASC)
);

