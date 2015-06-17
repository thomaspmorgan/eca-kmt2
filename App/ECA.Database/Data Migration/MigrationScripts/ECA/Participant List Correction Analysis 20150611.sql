/****** Script for SelectTopNRows command from SSMS  ******/
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
and participantid in (833)
order by participantid,projectid

select * from participant where personid in (10)
select * from person where personid in (10)
