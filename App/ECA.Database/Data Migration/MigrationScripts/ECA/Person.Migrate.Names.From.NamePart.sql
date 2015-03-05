USE ECA_Dev
GO

DECLARE @LastName    nvarchar(50)
DECLARE @FirstName   nvarchar(50)
DECLARE @Prefix      nvarchar(50)
DECLARE @Suffix      nvarchar(50)
DECLARE @PersonId    int
DECLARE @cursorNamePart CURSOR

/* Define the cursor */
set @cursorNamePart = CURSOR FOR
SELECT np.personid,
       np.lastname,
       np.firstname,
       np.prefix,
       np.suffix
  FROM CombinedNameVw np

/* Open the cursor */
OPEN @cursorNamePart

/* Fetch the first project */
FETCH NEXT FROM @cursorNamePart INTO @PersonId, @LastName, @FirstName, @Prefix, @Suffix

/* Loop thru all projects (staging) - should match DB */
WHILE @@FETCH_STATUS = 0
BEGIN

  /* update the startdate on project */
  UPDATE eca_dev.eca_dev.dbo.person 
     SET Lastname = @LastName,
         FirstName = @FirstName,
         PrefixName = @Prefix,
         SuffixName = @Suffix 
   WHERE personid = @personid

  /* Fetch the next project */
  FETCH NEXT FROM @cursorNamePart INTO @PersonId, @LastName, @FirstName, @Prefix, @Suffix

END

/* Cleanup */
CLOSE @cursorNamePart
DEALLOCATE @cursorNamePart
GO