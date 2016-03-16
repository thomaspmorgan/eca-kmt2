CREATE TABLE [sevis].[StudentTermination] (
    [StudentTerminationId] INT                IDENTITY (1, 1) NOT NULL,
    [TerminationCode]      CHAR (2)           NOT NULL,
    [Description]          NVARCHAR (100)     NOT NULL,
    [F_1_Ind]              CHAR (1)           NOT NULL,
    [M_1_Ind]              CHAR (1)           NOT NULL,
    [History_CreatedBy]    INT                NOT NULL,
    [History_CreatedOn]    DATETIMEOFFSET (7) NOT NULL,
    [History_RevisedBy]    INT                NOT NULL,
    [History_RevisedOn]    DATETIMEOFFSET (7) NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_TerminationCode]
    ON [sevis].[StudentTermination]([TerminationCode] ASC);


GO
ALTER TABLE [sevis].[StudentTermination]
    ADD CONSTRAINT [PK_sevis.StudentTermination] PRIMARY KEY CLUSTERED ([StudentTerminationId] ASC);


