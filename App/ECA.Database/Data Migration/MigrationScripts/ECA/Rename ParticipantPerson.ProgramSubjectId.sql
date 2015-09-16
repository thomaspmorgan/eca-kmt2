/* Run script to change column ProgramSubjectId to ProgramCategoryId in ParticipantPerson Table */
/* MS20150915 */

/* Drop constraint on ProgramSubjectId */
ALTER TABLE dbo.ParticipantPerson
DROP CONSTRAINT FK_ParticipantPerson_ToProgramSubject ;

/* Drop index on ProgramSubjectId */
DROP INDEX IX_ParticipantPerson_ProgramSubjectCode 
    ON dbo.ParticipantPerson;

/* Rename Column to ProgramCategoryId */
EXEC sp_rename 'dbo.ParticipantPerson.ProgramSubjectId', 'ProgramCategoryId', 'COLUMN';

/* Add FK Constraint to sevis.programcategory table */
ALTER TABLE dbo.ParticipantPerson
ADD CONSTRAINT FK_ParticipantPerson_ToSevisProgramCategory FOREIGN KEY (ProgramCategoryId)
    REFERENCES sevis.ProgramCategory (ProgramCategoryId) ;

/* Add index for ProgramCategoryId */
CREATE NONCLUSTERED INDEX [IX_ParticipantPerson_SevisProgramCategoryCode] ON [dbo].[ParticipantPerson]
([ProgramCategoryId] ASC)
WITH (PAD_INDEX = OFF, 
      STATISTICS_NORECOMPUTE = OFF, 
      SORT_IN_TEMPDB = OFF, 
      DROP_EXISTING = OFF, 
      ONLINE = OFF, 
      ALLOW_ROW_LOCKS = ON, 
      ALLOW_PAGE_LOCKS = ON) 
ON [PRIMARY]


GO


