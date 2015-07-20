/* This script populates the ParticipantStatusId and StatusDate in the Participant table */

/* Update Alumnus - Completed projects */
UPDATE participant 
SET participantstatusid  =
(
SELECT participantstatusid 
FROM participantstatus 
WHERE status = 'Alumnus'),
statusdate = 
(
SELECT enddate 
FROM project pp
WHERE pp.projectid = participant.projectid
)
WHERE projectid IN (select projectid from project where projectstatusid = 4)
GO

/* Update Active - Active projects */
UPDATE participant 
SET participantstatusid  =
(
SELECT participantstatusid 
FROM participantstatus 
WHERE status = 'Active')--,
--statusdate = 
--(
--SELECT enddate 
--FROM project pp
--WHERE pp.projectid = participant.projectid
--)
WHERE projectid IN (select projectid from project where projectstatusid = 1)
Go