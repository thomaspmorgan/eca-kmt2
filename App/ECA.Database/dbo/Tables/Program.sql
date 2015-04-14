CREATE TABLE [dbo].[Program] (
    [ProgramId]               INT                IDENTITY (1, 1) NOT NULL,
	[ProgramStatusId]	INT NOT NULL DEFAULT 1,
    [Name]                    NVARCHAR (255)     NOT NULL,
    [Description]             NVARCHAR (MAX)     NOT NULL,
	[FocusId]             INT     NOT NULL DEFAULT 0,
	[Website]             NVARCHAR (MAX)     NULL,
    [StartDate]               DATETIMEOFFSET (7) NOT NULL,
    [EndDate]                 DATETIMEOFFSET (7) NOT NULL,
    [History_CreatedBy]       INT                NOT NULL,
    [History_CreatedOn]       DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]       INT                NOT NULL,
    [History_RevisedOn]       DATETIMEOFFSET (7) NOT NULL,
    [ParentProgram_ProgramId] INT                NULL,
    [Owner_OrganizationId]    INT                NOT NULL,
    [RowVersion] TIMESTAMP NOT NULL, 
    CONSTRAINT [PK_dbo.Program] PRIMARY KEY CLUSTERED ([ProgramId] ASC),
    CONSTRAINT [FK_dbo.Program_dbo.Organization_Owner_OrganizationId] FOREIGN KEY ([Owner_OrganizationId]) REFERENCES [dbo].[Organization] ([OrganizationId]),
    CONSTRAINT [FK_dbo.Program_dbo.Program_ParentProgram_ProgramId] FOREIGN KEY ([ParentProgram_ProgramId]) REFERENCES [dbo].[Program] ([ProgramId]), 
    CONSTRAINT [FK_Program_ToProgramStatus] FOREIGN KEY ([ProgramStatusId]) REFERENCES [ProgramStatus]([ProgramStatusId]), 
    CONSTRAINT [FK_Program_ToFocus] FOREIGN KEY ([FocusId]) REFERENCES [Focus]([FocusId])
);


GO
CREATE NONCLUSTERED INDEX [IX_ParentProgram_ProgramId]
    ON [dbo].[Program]([ParentProgram_ProgramId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Owner_OrganizationId]
    ON [dbo].[Program]([Owner_OrganizationId] ASC);

