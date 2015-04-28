CREATE TABLE [dbo].[Log]
(
	[Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Date]       DATETIME       NULL,
    [Level]      VARCHAR (15)  NULL,
    [Message]    VARCHAR (MAX) NULL,
    [Username]   VARCHAR (255) NULL,
	[Controller]   VARCHAR (255) NULL,
	[Action]   VARCHAR (255) NULL,
    [Exception]  VARCHAR (MAX) NULL,
    [Stacktrace] VARCHAR (MAX) NULL,
	[Callsite] VARCHAR (MAX) NULL,
	[ActionArguments] VARCHAR (MAX) NULL, 
    [RequestId] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [PK_Log] PRIMARY KEY ([Id]),
)
