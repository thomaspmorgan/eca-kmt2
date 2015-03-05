/* created PhoneNumberType */

USE [ECA_Dev]
GO
SET IDENTITY_INSERT [dbo].[PhoneNumberType] ON 

GO
INSERT [dbo].[PhoneNumberType] ([PhoneNumberTypeId], [PhoneNumberTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (1, N'Home', 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset))
GO
INSERT [dbo].[PhoneNumberType] ([PhoneNumberTypeId], [PhoneNumberTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (2, N'Work', 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset))
GO
INSERT [dbo].[PhoneNumberType] ([PhoneNumberTypeId], [PhoneNumberTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) VALUES (3, N'Cell', 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-02-22T00:00:00.0000000-05:00' AS DateTimeOffset))
GO



SET IDENTITY_INSERT [dbo].[PhoneNumberType] OFF
GO
