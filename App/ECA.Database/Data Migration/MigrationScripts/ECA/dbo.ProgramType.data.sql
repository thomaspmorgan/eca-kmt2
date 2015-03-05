/* created ProgramType */

USE [ECA_Dev]
GO
SET IDENTITY_INSERT [dbo].[ProgramType] ON 

GO
INSERT [dbo].[ProgramType] ([ProgramTypeId], [ProgramTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (1, N'Program', 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset))
GO
INSERT [dbo].[ProgramType] ([ProgramTypeId], [ProgramTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (2, N'Office', 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset))
GO
INSERT [dbo].[ProgramType] ([ProgramTypeId], [ProgramTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (3, N'Branch', 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset))
GO



SET IDENTITY_INSERT [dbo].[ProgramType] OFF
GO
