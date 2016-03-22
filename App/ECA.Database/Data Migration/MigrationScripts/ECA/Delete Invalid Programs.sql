/* ECA/A/L ... Done (Local,DEV,QA,PRE) */ 
/* ECA/A/E ... Done (Local,DEV,QA,PRE) */
/* ECA/A/S ... Done (Local,DEV,QA,PRE) */
/* ECA/EC ... Done (Local,DEV,QA,PRE)  */
/* ECA/PE/C/PF ... Done (DEV,QA,PRE)   */
/* ECA/PE/C/CU ... Done (DEV,QA,PRE)   */
/* ECA/PE/C/SU ... Done (DEV,QA,PRE)   */
/* ECA/PE/C/PY ... Done (DEV,QA,PRE)   */

/* Substitute office you are working with */
select * from organization where officesymbol LIKE 'ECA/PE/C/PF%'

/* Query Programs */
select * from program where owner_organizationid IN (select organizationid from organization where officesymbol LIKE 'ECA/PE/C/PF%') order by parentprogram_programid,name
select * from program where owner_organizationid IN (select organizationid from organization where officesymbol LIKE 'ECA/PE/C/PF%') and programstatusid = 6
select * from program where owner_organizationid IN (select organizationid from organization where officesymbol LIKE 'ECA/PE/C/PF%' AND officesymbol <> 'ECA/PE/C/PF')
select * from program where parentprogram_programid in (select programid from program where owner_organizationid IN (select organizationid from organization where officesymbol LIKE 'ECA/PE/C/PF%') and programstatusid = 6)
select * from program where parentprogram_programid in (select programid from program where parentprogram_programid in (select programid from program where owner_organizationid IN (select organizationid from organization where officesymbol LIKE 'ECA/PE/C/PF%') and programstatusid = 6))

/* This is SO wrong! */
select * from program where owner_organizationid in (select organizationid from organization where officesymbol LIKE 'ECA/PE/C/PF%') and parentprogram_programid = programid
update program set parentprogram_programid = NULL where owner_organizationid in (select organizationid from organization where officesymbol LIKE 'ECA/PE/C/PF%') and parentprogram_programid = programid

/* Query Projects affected by delete */
select * from project where programid in (select programid from program where owner_organizationid IN (select organizationid from organization where officesymbol LIKE 'ECA/PE/C/PF%'))
select * from project where programid in (select programid from program where owner_organizationid = (select organizationid from organization where officesymbol = 'ECA/PE/C/PF') and programstatusid = 6)
select * from project where programid in (select programid from program where owner_organizationid IN (select organizationid from organization where officesymbol LIKE 'ECA/PE/C/PF%') and programstatusid = 6)
select * from project where programid in (select programid from program where parentprogram_programid in (select programid from program where owner_organizationid = (select organizationid from organization where officesymbol = 'ECA/PE/C/PF') and programstatusid = 6))
select * from project where programid in (select programid from program where parentprogram_programid in (select programid from program where parentprogram_programid in (select programid from program where owner_organizationid = (select organizationid from organization where officesymbol = 'ECA/PE/C/PF') and programstatusid = 6)))

/* Re-Map Projects to other Programs */
select * from project where projectid IN (1669,1670)
update project set programid = 1105 where projectid IN (1669,1670)
select * from project where projectid in (1728)
update project set programid = 1086 where projectid IN (1728)
select * from project where projectid in (1729,1730)
update project set programid = 1102 where projectid IN (1729,1730)

/* Query Participants */
select * from participant where projectid in (select projectid from project where programid in (select programid from program where owner_organizationid IN (select organizationid from organization where officesymbol LIKE 'ECA/PE/C/PF%'))) order by projectid
select * from participant where projectid in (select projectid from project where programid in (select programid from program where owner_organizationid = (select organizationid from organization where officesymbol = 'ECA/PE/C/PF') and programstatusid = 6))
select * from participant where projectid in (select projectid from project where programid in (select programid from program where owner_organizationid IN (select organizationid from organization where officesymbol LIKE 'ECA/PE/C/PF%') and programstatusid = 6))

/* Delete Bookmarks */
Select * from bookmark where programid in (select programid from program where owner_organizationid IN (select organizationid from organization where officesymbol LIKE 'ECA/PE/C/PF%') and programstatusid = 6)
select * from bookmark where programid in (select programid from program where parentprogram_programid in (select programid from program where owner_organizationid = (select organizationid from organization where officesymbol LIKE 'ECA/PE/C/PF%') and programstatusid = 6))
select * from bookmark where programid in (select programid from program where parentprogram_programid in (select programid from program where parentprogram_programid in (select programid from program where owner_organizationid = (select organizationid from organization where officesymbol LIKE 'ECA/PE/C/PF%') and programstatusid = 6)))
delete from bookmark where programid in (select programid from program where owner_organizationid IN (select organizationid from organization where officesymbol LIKE 'ECA/PE/C/PF%') and programstatusid = 6)
delete from bookmark where programid in (select programid from program where parentprogram_programid in (select programid from program where owner_organizationid = (select organizationid from organization where officesymbol LIKE 'ECA/PE/C/PF%') and programstatusid = 6))
delete from bookmark where programid in (select programid from program where parentprogram_programid in (select programid from program where parentprogram_programid in (select programid from program where owner_organizationid = (select organizationid from organization where officesymbol LIKE 'ECA/PE/C/PF%') and programstatusid = 6)))

/* Delete Projects */
delete from project where programid in (select programid from program where parentprogram_programid in (select programid from program where parentprogram_programid in (select programid from program where owner_organizationid = (select organizationid from organization where officesymbol = 'ECA/PE/C/PF') and programstatusid = 6)))
delete from project where programid in (select programid from program where parentprogram_programid in (select programid from program where owner_organizationid = (select organizationid from organization where officesymbol = 'ECA/PE/C/PF') and programstatusid = 6))
delete from project where programid in (select programid from program where owner_organizationid = (select organizationid from organization where officesymbol = 'ECA/PE/C/PF') and programstatusid = 6)

/* Delete Programs */
delete from program where parentprogram_programid in (select programid from program where parentprogram_programid in (select programid from program where owner_organizationid = (select organizationid from organization where officesymbol = 'ECA/PE/C/PF') and programstatusid = 6))
delete from program where parentprogram_programid IN (select programid from program where owner_organizationid = (select organizationid from organization where officesymbol = 'ECA/PE/C/PF') and programstatusid = 6)
delete from program where owner_organizationid = (select organizationid from organization where officesymbol = 'ECA/PE/C/PF') and programstatusid = 6
delete from program where owner_organizationid IN (select organizationid from organization where officesymbol LIKE 'ECA/PE/C/PF%' AND officesymbol <> 'ECA/PE/C/PF')


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
WHERE o.officesymbol IS NOT NULL AND o.organizationtypeid = 1 AND p.owner_organizationid IN (SELECT organizationid FROM organization WHERE officesymbol LIKE 'ECA/PE/C/PF%')
ORDER BY o.officesymbol,p.name,p2.name,p3.name,p4.name

/* Show programs by Office */
SELECT o.organizationid
,o.officesymbol OfficeSymbol
,o.name  OfficeName
,p.name "Program Name"
,p.programid
,p.parentprogram_programid
,p2.name "Parent Program Name"
,p2.programid
,p2.parentprogram_programid
,pj.projectid
,pj.name
FROM organization o
LEFT OUTER JOIN program p ON (p.owner_organizationid = o.organizationid)
LEFT OUTER JOIN program p2 ON (p2.programid = p.parentprogram_programid)
LEFT OUTER JOIN project pj ON (pj.programid = p.programid)
WHERE o.officesymbol IS NOT NULL AND o.organizationtypeid = 1 AND p.owner_organizationid IN (SELECT organizationid FROM organization WHERE officesymbol LIKE 'ECA/PE/C/PF%')
ORDER BY o.officesymbol,p.parentprogram_programid,p.programid,pj.projectid