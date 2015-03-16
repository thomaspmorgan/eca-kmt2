USE VisitingScholar
GO

/* NOTE: This will only be run once to fix an oversight - the script to create participants has been updated to include OrgID */

DECLARE @personid int
DECLARE @organizationid int
DECLARE @cursorParticipant CURSOR

/* Define the cursor */
SET @cursorParticipant = CURSOR FOR
SELECT p.PersonId,
       o.organizationid
  FROM VisitingScholarData vs
  JOIN eca_dev.eca_dev.dbo.person p ON ((vs.[First Name] IS NULL OR (p.firstname = vs.[First Name])) AND
    (vs.[Last Name] IS NULL OR (p.lastname = vs.[Last Name])) AND
    (vs.[Prefix] IS NULL OR (p.nameprefix = vs.[Prefix])) AND
    (vs.[Suffix] IS NULL OR (p.namesuffix = vs.[Suffix])) AND
    (vs.[Second Last Name] IS NULL OR (p.secondlastname = vs.[Second Last Name])) AND
    (vs.[Middle Name] IS NULL OR (p.middlename = vs.[Middle Name])))
  LEFT JOIN eca_dev.eca_dev.dbo.organization o ON (o.description = vs.[Home Institution Name])

/* Open the cursor */
OPEN @cursorParticipant

/* Fetch the first participant */
FETCH NEXT FROM @cursorParticipant INTO @PersonId, @organizationid

/* Loop thru all participants - should match DB */
WHILE @@FETCH_STATUS = 0
BEGIN

  /* update the organizationid */
  UPDATE eca_dev.eca_dev.dbo.participant 
     SET organizationid = @organizationid
   WHERE personid = @personid

  /* Fetch the next participant */
  FETCH NEXT FROM @cursorParticipant INTO @PersonId, @organizationid

END

/* Cleanup */
CLOSE @cursorParticipant
DEALLOCATE @cursorParticipant
GO