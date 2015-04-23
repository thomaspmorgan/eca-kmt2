/* created MaritalStatus */

USE [ECA_Dev]
GO
SET IDENTITY_INSERT [dbo].[MaritalStatus] ON 

GO
INSERT [dbo].[MaritalStatus] ([MaritalStatusId], [Status], [Description], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) 
	VALUES 
		(1, N'M', 'Married', 0, CAST(N'2015-03-27T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-03-27T00:00:00.0000000-05:00' AS DateTimeOffset)),
 	  	(2, N'U', 'Unmarried', 0, CAST(N'2015-03-27T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-03-27T00:00:00.0000000-05:00' AS DateTimeOffset)),
	  	(3, N'D', 'Divorced', 0, CAST(N'2015-03-27T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-03-27T00:00:00.0000000-05:00' AS DateTimeOffset)),
		(4, N'S', 'Single', 0, CAST(N'2015-03-27T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-03-27T00:00:00.0000000-05:00' AS DateTimeOffset)),
 	  	(5, N'P', 'Separated', 0, CAST(N'2015-04-11T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-04-11T00:00:00.0000000-05:00' AS DateTimeOffset)),
	  	(6, N'W', 'Widowed', 0, CAST(N'2015-04-11T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-04-11T00:00:00.0000000-05:00' AS DateTimeOffset)),
		(7, N'N', 'Not Disclosed', 0, CAST(N'2015-04-11T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-04-11T00:00:00.0000000-05:00' AS DateTimeOffset))

GO


SET IDENTITY_INSERT [dbo].[MaritalStatus] OFF
GO


