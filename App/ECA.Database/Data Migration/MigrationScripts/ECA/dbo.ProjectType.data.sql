/* created ProjectType */

USE [ECA_Dev]
GO
SET IDENTITY_INSERT [dbo].[ProjectType] ON 

GO
INSERT [dbo].[ProjectType] ([ProjectTypeId], [ProjectTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (1, N'ProjectType1', 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset))
GO
INSERT [dbo].[ProjectType] ([ProjectTypeId], [ProjectTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (2, N'ProjectType2', 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset))
GO




SET IDENTITY_INSERT [dbo].[ProjectType] OFF
GO
