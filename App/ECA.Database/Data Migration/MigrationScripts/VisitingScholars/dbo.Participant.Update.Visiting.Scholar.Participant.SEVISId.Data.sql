USE VisitingScholar
GO

/* NOTE: This will only be run once to fix an oversight - the script to create participants has been updated to include SevisId */

DECLARE @personid int
DECLARE @participantid int
DECLARE @SevisId nvarchar(255)
DECLARE @cursorParticipant CURSOR

/* Define the cursor */
SET @cursorParticipant = CURSOR FOR
SELECT p.PersonId,
       p1.participantid,
       vs.[SEVIS-ID]
  FROM VisitingScholarData vs
  JOIN eca_dev.eca_dev.dbo.person p ON ((vs.[First Name] IS NULL OR (p.firstname = vs.[First Name])) AND
    (vs.[Last Name] IS NULL OR (p.lastname = vs.[Last Name])) AND
    (vs.[Prefix] IS NULL OR (p.nameprefix = vs.[Prefix])) AND
    (vs.[Suffix] IS NULL OR (p.namesuffix = vs.[Suffix])) AND
    (vs.[Second Last Name] IS NULL OR (p.secondlastname = vs.[Second Last Name])) AND
    (vs.[Middle Name] IS NULL OR (p.middlename = vs.[Middle Name])))
  LEFT JOIN eca_dev.eca_dev.dbo.participant p1 ON (p1.personid = p.personid)
  WHERE vs.[SEVIS-ID] IS NOT NULL

/* Open the cursor */
OPEN @cursorParticipant

/* Fetch the first participant */
FETCH NEXT FROM @cursorParticipant INTO @PersonId, @participantid, @sevisid

/* Loop thru all participants - should match DB */
WHILE @@FETCH_STATUS = 0
BEGIN

  /* update the organizationid */
--  UPDATE eca_dev.eca_dev.dbo.participant 
--     SET sevisid = @sevisid
--   WHERE personid = @personid
--     AND participantid = @participantid

  /* Fetch the next participant */
  FETCH NEXT FROM @cursorParticipant INTO @PersonId, @participantid, @sevisid

END

/* Cleanup */
CLOSE @cursorParticipant
DEALLOCATE @cursorParticipant
GO