/* created Project Status */

USE [ECA_Dev]
GO
SET IDENTITY_INSERT [dbo].[ProjectStatus] ON 

GO
INSERT [dbo].[ProjectStatus] ([ProjectStatusId], [Status], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (1, N'Active', 0, CAST(N'2014-02-12T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2014-02-12T00:00:00.0000000-05:00' AS DateTimeOffset))
GO
INSERT [dbo].[ProjectStatus] ([ProjectStatusId], [Status], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (2, N'Pending', 0, CAST(N'2014-02-12T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2014-02-12T00:00:00.0000000-05:00' AS DateTimeOffset))
GO
INSERT [dbo].[ProjectStatus] ([ProjectStatusId], [Status], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (4, N'Completed', 0, CAST(N'2014-02-12T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2014-02-21T00:00:00.0000000-05:00' AS DateTimeOffset))
GO
INSERT [dbo].[ProjectStatus] ([ProjectStatusId], [Status], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (5, N'Draft', 0, CAST(N'2014-02-12T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2014-02-12T00:00:00.0000000-05:00' AS DateTimeOffset))
GO
INSERT [dbo].[ProjectStatus] ([ProjectStatusId], [Status], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (6, N'Canceled', 0, CAST(N'2015-02-12T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-02-12T00:00:00.0000000-05:00' AS DateTimeOffset))
GO
INSERT [dbo].[ProjectStatus] ([ProjectStatusId], [Status], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (7, N'Other', 0, CAST(N'2015-02-12T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-02-12T00:00:00.0000000-05:00' AS DateTimeOffset))
GO
SET IDENTITY_INSERT [dbo].[ProjectStatus] OFF
GO
