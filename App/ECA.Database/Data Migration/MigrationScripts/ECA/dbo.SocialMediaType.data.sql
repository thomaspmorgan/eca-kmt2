/* created SocialMediaType */

USE [ECA_Dev]
GO
SET IDENTITY_INSERT [dbo].[SocialMediaType] ON 

GO
INSERT [dbo].[SocialMediaType] ([SocialMediaTypeId], [SocialMediaTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (1, N'Facebook', 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset))
GO
INSERT [dbo].[SocialMediaType] ([SocialMediaTypeId], [SocialMediaTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (2, N'LinkedIn', 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset))
GO
INSERT [dbo].[SocialMediaType] ([SocialMediaTypeId], [SocialMediaTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (3, N'Twitter', 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset))
GO
INSERT [dbo].[SocialMediaType] ([SocialMediaTypeId], [SocialMediaTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (4, N'Weibo', 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset))
GO




SET IDENTITY_INSERT [dbo].[SocialMediaType] OFF
GO
