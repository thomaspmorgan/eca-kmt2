select * from participantperson where participantid in (
select participantid from participant where projectid in (
select projectid from project where name like '%Fulbright%'))

select * from address where organizationid = 2113
select * from location where locationid in (1899,1922)

update participantperson 
set hostinstitutionaddressid = (select top 1 addressid from address where organizationid = participantperson.hostinstitutionid)
where participantid in (
select participantid from participant where projectid in (
select projectid from project where name like '%Fulbright%'))
and hostinstitutionaddressid is null 

update participantperson 
set homeinstitutionaddressid = (select top 1 addressid from address where organizationid = participantperson.homeinstitutionid)
where participantid in (
select participantid from participant where projectid in (
select projectid from project where name like '%Fulbright%'))
and homeinstitutionaddressid is null