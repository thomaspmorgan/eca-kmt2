/* created AddressType */
USE [ECA_Dev]
GO

/* Populate initial lookup values */
INSERT [dbo].[AddressType] 
	([AddressTypeId], [AddressTypeName], [History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn]) 
VALUES (N'Home', 0, CAST(N'2015-03-20T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-03-20T00:00:00.0000000-05:00' AS DateTimeOffset)),
       (N'Host', 0, CAST(N'2015-03-20T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-03-20T00:00:00.0000000-05:00' AS DateTimeOffset)),
       (N'Home Institution', 0, CAST(N'2015-03-20T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-03-20T00:00:00.0000000-05:00' AS DateTimeOffset)),
       (N'Host Institution', 0, CAST(N'2015-03-20T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-03-20T00:00:00.0000000-05:00' AS DateTimeOffset))


GO




SELECT * FROM organization WHERE name = 'Office of Academic Exchange Programs' AND organizationtypeid < 4

SELECT * FROM program WHERE owner_organizationid = 1414

SELECT * FROM project WHERE programid = 54 ORDER BY projectid

SELECT * FROM participantproject WHERE projectid BETWEEN 1375 AND 1473 ORDER BY participantid

SELECT * FROM participant WHERE participantid BETWEEN 1345 AND 1443 ORDER BY personid

SELECT * FROM person WHERE personid BETWEEN 522 AND 620






