/* Update actual participant counts in KMT database */

update project set usparticipantsest = 0 where usparticipantsest IS NULL
update project set nonusparticipantsest = 0 where nonusparticipantsest IS NULL
update project set usparticipantsactual = 0 where usparticipantsactual IS NULL
update project set nonusparticipantsactual = 0 where nonusparticipantsactual IS NULL

/* US Participants */
SELECT 'UPDATE dbo.project SET usparticipantsactual = ' + CONVERT(nvarchar,count(p1.participantid)) + ' WHERE projectid = ' + CONVERT(nvarchar,p.projectid)
,p.projectid,count(p1.participantid)
from project p
join participant p1 on (p1.projectid = p.projectid)
join person p2 on (p2.personid = p1.personid)
join citizencountry c on (c.personid = p2.personid)
join location l on (l.locationid = c.LocationId)
where l.locationid = 193
group by p.projectid
order by p.projectid

/* Non-US Participants */
SELECT 'UPDATE dbo.project SET nonusparticipantsactual = ' + CONVERT(nvarchar,count(p1.participantid)) + ' WHERE projectid = ' + CONVERT(nvarchar,p.projectid)
,p.projectid,count(p1.participantid)
from project p
join participant p1 on (p1.projectid = p.projectid)
join person p2 on (p2.personid = p1.personid)
join citizencountry c on (c.personid = p2.personid)
join location l on (l.locationid = c.LocationId)
where l.locationid <> 193
group by p.projectid
order by p.projectid