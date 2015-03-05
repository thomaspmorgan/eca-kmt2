/* Insert all parent programs programs */
INSERT INTO ECA_Dev.ECA_Dev.dbo.program
(name,
 description,
 startdate,
 enddate,
 history_createdby,
 history_createdon,
 history_revisedby,
 history_revisedon,
 parentprogram_programid,
 owner_organizationid)
SELECT name,
       description,
       '2015-01-01',
       '2016-01-01',
       0,
       '2015-01-01',
       0,
       '2015-01-01',
       null,
       1
  FROM Program_Staging
 WHERE parentprogram IS NULL
GO

/* Insert all child programs */
INSERT INTO ECA_Dev.ECA_Dev.dbo.program
(name,
 description,
 startdate,
 enddate,
 history_createdby,
 history_createdon,
 history_revisedby,
 history_revisedon,
 parentprogram_programid,
 owner_organizationid)
SELECT ps.name,
       ps.description,
       '2015-01-01',
       '2016-01-01',
       0,
       '2015-01-01',
       0,
       '2015-01-01',
       pr.programid,
       1
  FROM Program_Staging ps
  LEFT OUTER JOIN ECA_Dev.ECA_Dev.dbo.Program pr
    ON (pr.name = ps.parentprogram)
 WHERE parentprogram IS NOT NULL
GO

