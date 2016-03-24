/* Change Database */
USE ECA_KMT_DEV
GO

/* Show Program Hierarchy */
SELECT o.organizationid
,o.officesymbol OfficeSymbol
,o.name  OfficeName
,ISNULL(p.name,'No Programs') "Program Name"
,ISNULL(p2.name,'-') "Sub-Program Name"
,ISNULL(p3.name,'-') "Sub-Program Name"
,ISNULL(p4.name,'-') "Sub-Program Name"
FROM organization o
LEFT OUTER JOIN program p ON (p.owner_organizationid = o.organizationid AND p.parentprogram_programid IS NULL)
LEFT OUTER JOIN program p2 ON (p2.parentprogram_programid = p.programid)
LEFT OUTER JOIN program p3 ON (p3.parentprogram_programid = p2.programid)
LEFT OUTER JOIN program p4 ON (p4.parentprogram_programid = p3.programid)
WHERE o.officesymbol IS NOT NULL 
  AND o.organizationtypeid = 1 
  AND p.owner_organizationid IN (SELECT organizationid FROM organization WHERE officesymbol LIKE 'ECA/A/E%')
ORDER BY o.officesymbol,p.name,p2.name,p3.name,p4.name