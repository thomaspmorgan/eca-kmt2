/* Change Language Proficiency Native Language Indicator from char to bit */
/* Change the name to IsNativeLanguage */
/* MDS20150820 */

/* update the column - 'Y' -> 1, 'N' -> 0 */
UPDATE [dbo].[PersonLanguageProficiency]
SET [NativeLanguageInd] = CASE [NativeLanguageInd] WHEN 'Y' THEN 1 ELSE 0 END;


/* Change the name of the PrimaryLanguageInd column to IsPrimaryLanguage */
EXEC sp_rename 'dbo.PersonLanguageProficiency.NativeLanguageInd', 'IsNativeLanguage', 'COLUMN';                    


/* Drop DEFAULT constraint on the column */
DECLARE @DefaultObjectName NVARCHAR(100)
SELECT @DefaultObjectName = OBJECT_NAME([default_object_id]) FROM SYS.COLUMNS
WHERE [object_id] = OBJECT_ID('[dbo].[PersonLanguageProficiency]') AND [name] = 'IsNativeLanguage';
EXEC('ALTER TABLE [dbo].[PersonLanguageProficiency] DROP CONSTRAINT [' + @DefaultObjectName + ']');


/* Need to drop PK constraint */
DECLARE @PKObjectName NVARCHAR(100)
SELECT @PKObjectName = [name] FROM SYS.indexes
WHERE [object_id] = OBJECT_ID('[dbo].[PersonLanguageProficiency]') AND [is_primary_key] = 1;
EXEC('ALTER TABLE [dbo].[PersonLanguageProficiency] DROP CONSTRAINT [' + @PKObjectName + ']');


/* Alter the table to change datatype */
ALTER TABLE [dbo].[PersonLanguageProficiency] ALTER COLUMN [IsNativeLanguage] bit NOT NULL;


/* And then create a new default constraint:  */
EXEC('ALTER TABLE [dbo].[PersonLanguageProficiency] ADD CONSTRAINT [' + @ObjectName + '] DEFAULT 0 FOR [IsNativeLanguage]');


/* Reenable PK */
/****** Object:  Index [PK_dbo.PersonLanguageProficiency]    Script Date: 8/20/2015 11:40:17 AM ******/
EXEC('ALTER TABLE [dbo].[PersonLanguageProficiency] ADD CONSTRAINT [' + @PKObjectName + '] PRIMARY KEY CLUSTERED ' + 
'([LanguageId] ASC,[PersonId] ASC,[IsNativeLanguage] ASC) ' + 
'WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]');
GO

