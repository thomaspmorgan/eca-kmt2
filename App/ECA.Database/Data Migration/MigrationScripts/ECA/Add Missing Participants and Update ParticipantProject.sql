/* These are all corrections and updates to correct the missing participants */
select * from participantproject
where participantid in (
select participantid from participant
where participantid in (
SELECT [ParticipantId]
      --,count([ProjectId])
  FROM [dbo].[ParticipantProject]
  group by participantid
  having count(projectid) > 1)
  and personid is not null)
--and participantid in (833)
order by participantid,projectid




select * from ParticipantProject where participantid = 8
select * from ParticipantProject where participantid = 8 and projectid = 628
select * from ParticipantProject where projectid = 628
--select * from participant where participantid in (1125,9275)


USE [ECA_Dev]
GO

INSERT INTO dbo.participant
      ([OrganizationId],[PersonId],[ParticipantTypeId],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy]
      ,[History_RevisedOn],[SevisId],[ContactAgreement],[ParticipantStatusId],[StatusDate])
SELECT [OrganizationId],[PersonId],[ParticipantTypeId],[History_CreatedBy],[History_CreatedOn],[History_RevisedBy]
      ,[History_RevisedOn],[SevisId],[ContactAgreement],[ParticipantStatusId],[StatusDate]
  FROM [dbo].[Participant]
 WHERE [ParticipantId] = 1
GO

/* Now update the ParticipantProject table */
UPDATE dbo.ParticipantProject
SET ParticipantId = @@IDENTITY
WHERE ParticipantId = 1 AND ProjectId = 1

GO
