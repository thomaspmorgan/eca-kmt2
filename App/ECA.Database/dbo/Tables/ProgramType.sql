CREATE TABLE [dbo].[ProgramType] (
    [ProgramTypeId]     INT                IDENTITY (1, 1) NOT NULL,
    [ProgramTypeName]   NVARCHAR (50)      NOT NULL,
    [History_CreatedBy] INT                NOT NULL,
    [History_CreatedOn] DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy] INT                NOT NULL,
    [History_RevisedOn] DATETIMEOFFSET (7) NOT NULL,
    [Program_ProgramId] INT                NULL,
    CONSTRAINT [PK_dbo.ProgramType] PRIMARY KEY CLUSTERED ([ProgramTypeId] ASC),
    CONSTRAINT [FK_dbo.ProgramType_dbo.Program_Program_ProgramId] FOREIGN KEY ([Program_ProgramId]) REFERENCES [dbo].[Program] ([ProgramId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Program_ProgramId]
    ON [dbo].[ProgramType]([Program_ProgramId] ASC);

