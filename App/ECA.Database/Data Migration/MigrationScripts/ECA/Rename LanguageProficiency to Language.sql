/* Change Language Proficiency and Language tables for Sprint 18 */

/* Change the name of the LanguageProficiency table to Language */
EXEC sp_rename 'dbo.LanguageProficiency', 'Language';                    

GO

EXEC sp_rename 'dbo.LanguageProficiencyPerson', 'PersonLanguageProficiency'; 

GO   