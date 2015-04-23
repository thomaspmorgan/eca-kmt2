USE VisitingScholar
GO

/* NOTE: This will only be run once to fix an oversight - the script to create Persons has been updated to include BirthLocation */

DECLARE @personid int
DECLARE @BirthCity nvarchar(255)
DECLARE @BirthLocationid int
DECLARE @cursorPerson CURSOR
DECLARE @CityLocationTypeID int = 0; 

/* get the correct ID for the region location type */ 
SELECT @CityLocationTypeID = locationtypeid 
  FROM ECA_Dev.Eca_dev.dbo.locationtype 
 WHERE locationtypename = 'City'

/* output to be sure */
SELECT @CityLocationTypeID AS 'City Location Type ID'

/* Define the cursor */
SET @cursorPerson = CURSOR FOR
SELECT vs.[Birth City],
       l.locationid, 
       p.PersonId
  FROM VisitingScholarData vs
  JOIN eca_dev.eca_dev.dbo.person p ON ((vs.[First Name] IS NULL OR (p.firstname = vs.[First Name])) AND
    (vs.[Last Name] IS NULL OR (p.lastname = vs.[Last Name])) AND
    (vs.[Prefix] IS NULL OR (p.nameprefix = vs.[Prefix])) AND
    (vs.[Suffix] IS NULL OR (p.namesuffix = vs.[Suffix])) AND
    (vs.[Second Last Name] IS NULL OR (p.secondlastname = vs.[Second Last Name])) AND
    (vs.[Middle Name] IS NULL OR (p.middlename = vs.[Middle Name])))
  JOIN eca_dev.eca_dev.dbo.location l ON (l.locationname = vs.[Birth City] AND l.locationtypeid = @CityLocationTypeID)


/* Open the cursor */
OPEN @cursorPerson

/* Fetch the first person */
FETCH NEXT FROM @cursorPerson INTO @BirthCity,@BirthLocationid,@PersonId

/* Loop thru all persons - should match DB */
WHILE @@FETCH_STATUS = 0
BEGIN

  /* update the marital status */
--  UPDATE eca_dev.eca_dev.dbo.person 
--     SET PlaceOfBirth_LocationId = @BirthLocationid
--   WHERE personid = @personid

  /* Fetch the next person */
  FETCH NEXT FROM @cursorPerson INTO @BirthCity,@BirthLocationid,@PersonId

END

/* Cleanup */
CLOSE @cursorPerson
DEALLOCATE @cursorPerson
GO