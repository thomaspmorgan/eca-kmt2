/* Update IVLP Program Mappings per C. Remsen email */

/* 
Our office used to have a Multi-Regional Program Division but it was absorbed a couple of years ago by the IVLP Division 
(and our application was never updated to reflect this change). 
So all projects with the project number prefix E/VM are owned by the IVLP Division.  
*/

/*
All PL-402 (they should start with the prefix VCR or VCP) –with the exception of the 
entries with “Gold Star” in the project title – should map to Global Government-to-Government Partnership (G3P)
*/

/*
The projects with Gold Star in the project title should map to the Gold Star sub-program. 
We do not have this project-program type in our current application so they were tagged with PL-402.
*/


/* One update */
UPDATE ivlp_xref.dbo.local_ivlp_Program_Mapping_xref_interim
SET IVLP_MappedProgramName =
CASE
WHEN UPPER(PROJECT_NUMBER) LIKE '%E/VM%' OR UPPER(PROJECT_NUMBER) LIKE '%E/VP%' 
   THEN 'International Vistor Leadership Program (IVLP)'
WHEN UPPER(PROGRAM) = 'PL-402' AND (PROJECT_NUMBER LIKE '%VCR%' OR PROJECT_NUMBER LIKE '%VCP%') AND UPPER(PROJECT_TITLE) NOT LIKE '%GOLD STAR%'
   THEN 'IVLP Global Government-to-Government Partnership (G3P)'
WHEN UPPER(PROGRAM) = 'PL-402' AND (PROJECT_NUMBER LIKE '%VCR%' OR PROJECT_NUMBER LIKE '%VCP%') AND UPPER(PROJECT_TITLE) LIKE '%GOLD STAR%'
   THEN 'IVLP Gold Stars'
WHEN UPPER(PROGRAM) = UPPER('Voluntary Visitors') AND UPPER(Project_Number) LIKE '%VFA%'
   THEN 'IVLP On Demand - EAP, NEA, SCA, WHA'
WHEN UPPER(PROGRAM) = UPPER('Voluntary Visitors') AND UPPER(Project_Number) LIKE '%VFE%'
   THEN 'IVLP On Demand - Africa and Europe'
WHEN UPPER(PROGRAM) = UPPER('Voluntary Visitors') AND (UPPER(Project_Number) NOT LIKE '%VFE%' AND UPPER(Project_Number) NOT LIKE '%VFA%')
   THEN 'IVLP On Demand'
WHEN UPPER(PROGRAM) = 'IV Regional Visitors'
   THEN 'Regional Programs'
END




