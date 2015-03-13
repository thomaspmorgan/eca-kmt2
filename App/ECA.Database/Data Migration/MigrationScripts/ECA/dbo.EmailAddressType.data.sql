/* created EmailAddressType */

USE [ECA_Dev]
GO

/* Populate initial lookup values */
INSERT [dbo].[EmailAddressType] 
	([EmailAddressTypeId], [EmailAddressTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) 
VALUES (N'Home', 0, CAST(N'2015-03-13T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-03-13T00:00:00.0000000-05:00' AS DateTimeOffset)),
	(N'Home Emergency', 0, CAST(N'2015-03-13T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-03-13T00:00:00.0000000-05:00' AS DateTimeOffset)),
	(N'Host', 0, CAST(N'2015-03-13T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-03-13T00:00:00.0000000-05:00' AS DateTimeOffset)),
	(N'Host Emergency', 0, CAST(N'2015-03-13T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-03-13T00:00:00.0000000-05:00' AS DateTimeOffset)),
	(N'Organization', 0, CAST(N'2015-03-13T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-03-13T00:00:00.0000000-05:00' AS DateTimeOffset)),
	(N'Personal', 0, CAST(N'2015-03-13T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-03-13T00:00:00.0000000-05:00' AS DateTimeOffset)),
	(N'Other', 0, CAST(N'2015-03-13T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-03-13T00:00:00.0000000-05:00' AS DateTimeOffset))
GO






