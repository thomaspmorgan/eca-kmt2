/* creates test Theme+Program cross-reference data for ThemeProgram Table  */

/*--Professional Fellows "On Demand" Programs */
INSERT INTO ProgramTheme
SELECT p.programid,t.themeid 
  FROM theme t
  JOIN program p
    ON 1=1
 WHERE t.themename in (
       'Civil Society',
       'Culture/Sports/American Society',
       'Democracy/Good Governance/Rule of Law',
       'Diversity',
       'Education',
       'Entrepreneurship/Job Creation',
       'Environment',
       'Women''s Empowerment',
       'Youth Engagement')
   AND p.name = 'Professional Fellows "On Demand" Program'
GO


/* Youth Leadership Programs (YLP) */
INSERT INTO ProgramTheme
SELECT p.programid,t.themeid 
  FROM theme t
  JOIN program p
    ON 1=1
 WHERE t.themename in (
       'Civil Society',
       'Culture/Sports/American Society',
       'Democracy/Good Governance/Rule of Law',
       'Education',
       'Science and Technology',
       'Youth Engagement')
   AND p.name = 'Youth Leadership Programs (YLP)'
GO


/* J-1 Visa Exchange Visitor Program: Secondary School Student */
INSERT INTO ProgramTheme
SELECT p.programid,t.themeid 
  FROM theme t
  JOIN program p
    ON 1=1
 WHERE t.themename in (
       'Culture/Sports/American Society',
       'Diversity',
       'Education',
       'Youth Engagement')
   AND p.name = 'J-1 Visa Exchange Visitor Program: Secondary School Student'
GO

/* Partners of the Americas */
INSERT INTO ProgramTheme
SELECT p.programid,t.themeid 
  FROM theme t
  JOIN program p
    ON 1=1
 WHERE t.themename in (
       'Culture/Sports/American Society',
       'Diversity',
       'Education',
       'Environment',
       'Human Rights')
   AND p.name = 'Partners of the Americas'
GO


/* Institute For Representative Government */
INSERT INTO ProgramTheme
SELECT p.programid,t.themeid 
  FROM theme t
  JOIN program p
    ON 1=1
 WHERE t.themename in (
       'Culture/Sports/American Society',
       'Democracy/Good Governance/Rule of Law')
   AND p.name = 'Institute For Representative Government'
GO