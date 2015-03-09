/* created ItineraryStatus */

USE [ECA_Dev]
GO
SET IDENTITY_INSERT [dbo].[ItineraryStatus] ON 

GO
INSERT [dbo].[ItineraryStatus] ([ItineraryStatusId], [ItineraryStatusName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (1, N'Planned', 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset))
GO
INSERT [dbo].[ItineraryStatus] ([ItineraryStatusId], [ItineraryStatusName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (2, N'InProgress', 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset))
GO
INSERT [dbo].[ItineraryStatus] ([ItineraryStatusId], [ItineraryStatusName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (3, N'Completed', 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset))
GO


SET IDENTITY_INSERT [dbo].[ItineraryStatus] OFF
GO