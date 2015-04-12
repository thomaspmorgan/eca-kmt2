/* created Project Status */

USE [ECA_Dev]
GO
SET IDENTITY_INSERT [dbo].[ProjectStatus] ON 

GO
INSERT [dbo].[ProjectStatus] ([ProjectStatusId], [Status], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) 
VALUES (1, N'Active', 0, CAST(N'2014-02-12T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2014-02-12T00:00:00.0000000-05:00' AS DateTimeOffset))
 (2, N'Pending', 0, CAST(N'2014-02-12T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2014-02-12T00:00:00.0000000-05:00' AS DateTimeOffset))
 (4, N'Completed', 0, CAST(N'2014-02-12T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2014-02-21T00:00:00.0000000-05:00' AS DateTimeOffset))
 (5, N'Draft', 0, CAST(N'2014-02-12T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2014-02-12T00:00:00.0000000-05:00' AS DateTimeOffset))
 (6, N'Canceled', 0, CAST(N'2015-02-12T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-02-12T00:00:00.0000000-05:00' AS DateTimeOffset))
 (7, N'Other', 0, CAST(N'2015-02-12T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-02-12T00:00:00.0000000-05:00' AS DateTimeOffset))
GO
SET IDENTITY_INSERT [dbo].[ProjectStatus] OFF
GO

/* For CE */
INSERT INTO [dbo].[ProjectStatus] ([ProjectStatusId], [Status], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) 
VALUES (N'Active - Use Actuals', 0, CAST(N'2014-04-10T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2014-04-10T00:00:00.0000000-05:00' AS DateTimeOffset))
(N'Project Postponed', 0, CAST(N'2014-04-10T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2014-04-10T00:00:00.0000000-05:00' AS DateTimeOffset))
(N'Proposed', 0, CAST(N'2014-04-10T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2014-04-10T00:00:00.0000000-05:00' AS DateTimeOffset))
(N'Rejected', 0, CAST(N'2014-04-10T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2014-04-10T00:00:00.0000000-05:00' AS DateTimeOffset))
 
GO


