Declare @name nvarchar(max),@parentproject nvarchar(max),@parentprojectid int
Declare @cursorProject CURSOR

/* Define the cursor */
set @cursorProject = CURSOR FOR
SELECT name,parentprogram FROM project_staging

/* Open the cursor */
OPEN @cursorProject

/* Fetch the first project */
FETCH NEXT FROM @cursorProject INTO @name, @parentproject

/* Loop thru all projects (staging) - should match DB */
WHILE @@FETCH_STATUS = 0
BEGIN

/* Get the programid for the parent program */
SELECT @parentprojectid = programid FROM eca_dev.eca_dev.dbo.program WHERE name = @parentproject

/* update the parentprogramid on project */
UPDATE eca_dev.eca_dev.dbo.project SET parentprogram_programid = @parentprojectid WHERE name = @name

/* Fetch the next project */
FETCH NEXT FROM @cursorProject INTO @name, @parentproject
END

/* Cleanup */
CLOSE @cursorProject
DEALLOCATE @cursorProject
GO


UPDATE  eca_dev.eca_dev.dbo.project b
SET     b.parentprogram_programid = a.programid
FROM    eca_dev.eca_dev.dbo.program a
        INNER JOIN project_staging b
            ON a.Name = b.parenprojectname


