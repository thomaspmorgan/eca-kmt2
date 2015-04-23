USE VisitingScholar
GO

/* NOTE: This will only be run once to fix an oversight - the script to create participants has been updated to include OrgID */

DECLARE @personid int
DECLARE @MaritalStatus NVARCHAR(max)
DECLARE @MaritalStatusid int
DECLARE @cursorPerson CURSOR

/* Define the cursor */
SET @cursorPerson = CURSOR FOR
SELECT vs.[Marital Status],
       m.maritalstatusid, 
       p.PersonId
  FROM VisitingScholarData vs
  JOIN eca_dev.eca_dev.dbo.person p ON ((vs.[First Name] IS NULL OR (p.firstname = vs.[First Name])) AND
    (vs.[Last Name] IS NULL OR (p.lastname = vs.[Last Name])) AND
    (vs.[Prefix] IS NULL OR (p.nameprefix = vs.[Prefix])) AND
    (vs.[Suffix] IS NULL OR (p.namesuffix = vs.[Suffix])) AND
    (vs.[Second Last Name] IS NULL OR (p.secondlastname = vs.[Second Last Name])) AND
    (vs.[Middle Name] IS NULL OR (p.middlename = vs.[Middle Name])))
  JOIN eca_dev.eca_dev.dbo.MaritalStatus m ON (m.status = vs.maritalstatus)

/* Open the cursor */
OPEN @cursorPerson

/* Fetch the first person */
FETCH NEXT FROM @cursorPerson INTO @MaritalStatus,@MaritalStatusId,@PersonId

/* Loop thru all persons - should match DB */
WHILE @@FETCH_STATUS = 0
BEGIN

  /* update the marital status */
--  UPDATE eca_dev.eca_dev.dbo.person 
--     SET maritalstatusid = @maritalstatusid
--   WHERE personid = @personid

  /* Fetch the next person */
  FETCH NEXT FROM @cursorPerson INTO @MaritalStatus,@MaritalStatusId,@PersonId

END

/* Cleanup */
CLOSE @cursorPerson
DEALLOCATE @cursorPerson
GO