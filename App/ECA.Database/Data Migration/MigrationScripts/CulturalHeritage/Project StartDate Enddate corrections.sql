USE AFCP
GO

DECLARE @name   nvarchar(max)
DECLARE @projectid int
DECLARE @title   nvarchar(max)
DECLARE @summary   nvarchar(max)
DECLARE @startdate   nvarchar(max)
DECLARE @expiresdate   nvarchar(max)
DECLARE @AFCPId    int
DECLARE @cursorProject CURSOR

/* Define the cursor */
set @cursorProject = CURSOR FOR
SELECT a.id,
       a.title,
       p.projectid,
       a.expires,
       N'09-30-'+CONVERT(CHAR(4),y.programyear)+' 12:00:00 AM -05:00'
  FROM afcp a 
  JOIN years y 
    ON (y.id = a.YearID)
  JOIN eca_dev.dbo.project p
    ON (p.name = a.title AND p.description = a.summary)
 ORDER BY a.id

/* Open the cursor */
OPEN @cursorProject

/* Fetch the first project */
FETCH NEXT FROM @cursorProject INTO @AFCPId, @title, @projectid, @expiresdate , @startdate

/* Loop thru all projects (staging) - should match DB */
WHILE @@FETCH_STATUS = 0
BEGIN

  /* update the startdate on project */
  UPDATE eca_dev.dbo.project 
     SET startdate = @startdate,
         enddate = @expiresdate 
   WHERE projectid = @projectid

  /* Fetch the next project */
  FETCH NEXT FROM @cursorProject INTO @AFCPId, @title, @projectid, @expiresdate , @startdate

END

/* Cleanup */
CLOSE @cursorProject
DEALLOCATE @cursorProject
GO