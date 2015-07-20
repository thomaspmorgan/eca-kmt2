select * 
from participant
where sevisid in (
select sevisid 
from participant
group by sevisid
having count(*) > 1)
order by sevisid

select c.name,c.object_id,t.name,t.object_id 
from sys.columns c
left join sys.tables t ON (t.object_id = c.object_id)
where c.name = 'Personid'


/* **************************** */
select * from eca_dev.eca_dev.dbo.maritalstatus
select * from ce_person where LAST_NAME = 'Norman' and FIRST_NAME = 'Camila' and MIDDLE_NAME = 'Q.'
select * from ivlp.dbo.ivlp_person where LAST_NAME = 'Adkins' and FIRST_NAME = 'Sarah' and MIDDLE_NAME = 'M.'
select * from VisitingScholar.dbo.VisitingScholarData where [Last Name] = 'Adkins' and [First Name] = 'Sarah' and [Middle Name] = 'M.'
select * from afcp.dbo.participants where [Name Last] = 'Adkins' and [Name First] = 'Sarah' 
select * from eca_dev.eca_dev.dbo.person where lastname = 'Norman' and firstname = 'Camila' and middlename = 'Q.'
select * from CE_Participant where person_id in ('6DDFDFAEAB2D43ABAE507B894D651FB0')
select * from ce_participant where participant_id in ('0032CC6AD8354E8C97E4FE9BA65A309B',
'F6F1B3D994B4416FB4EBF188EFD0197A')
select * from CE_Participant_Sevis where PARTICIPANT_ID in ('0032CC6AD8354E8C97E4FE9BA65A309B',
'F6F1B3D994B4416FB4EBF188EFD0197A')
select * from ce_component where component_id in ('3B3C63D93C084F959FD1512818275F35','91E185861DB245D29B931AB3CB29C109')
select * from ce_project where project_id in ('3F606A7C83C740F3AA1173F5426F430B',
'58D2EC91F84E4477A1C981CE4BAD0567')


select * from participant where participantid = 6561
select * from eca_dev.eca_dev.dbo.participant where personid in (8714)
select * from eca_dev.eca_dev.dbo.person where personid in (8714)
select * from eca_dev.eca_dev.dbo.location where locationid in (2231,3770)
select * from eca_dev.eca_dev.dbo.location where locationid in (94,150)
select * from eca_dev.eca_dev.dbo.location where locationname = 'West Bank'
select * from eca_dev.eca_dev.dbo.project where name = 'UT: Empowering Women and Girls Through Sports'


select * from project where projectid = 1653
select * from program where programid = 1053

