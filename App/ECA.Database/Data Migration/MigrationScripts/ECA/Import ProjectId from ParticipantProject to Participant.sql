/* Transfer ProjectId from ParticipantProject to Participant table */

UPDATE participant 
SET projectid = 
(
SELECT projectid 
FROM participantproject pp
WHERE pp.participantid = participant.participantid
)