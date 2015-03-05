/* Build the list of project-specific goals *** This part is GOOD! */

INSERT
  INTO eca_dev.eca_dev.dbo.projectgoal
       (projectid,goalid)
SELECT p.projectid,g.goalid
  FROM afcp a
  JOIN afcprationales ar
    ON (a.id = ar.afcp_id)
  JOIN eca_dev.eca_dev.dbo.project p 
    ON (p.name = a.title)
  JOIN rationales r 
    ON (r.id = ar.rationales_id)
  JOIN ratcat rc
    ON (rc.id = r.RatCat_ID)
  JOIN eca_dev.eca_dev.dbo.goal g 
    ON (g.goalname = rc.ratcat)
 WHERE g.goalid IS NOT NULL
 GROUP BY p.ProjectId,g.goalid
 ORDER BY p.ProjectId,g.goalid



/* Build the list of project-specific goals *** NOT APPLICABLE AT THIS TIME!!!! */

INSERT
  INTO eca_dev.eca_dev.dbo.projectgoal
       (projectid,goalid)
SELECT p.projectid,g.goalid
  FROM afcprationales ar
  LEFT OUTER JOIN afcp a 
               ON (a.id = ar.afcp_id)
  LEFT OUTER JOIN eca_dev.eca_dev.dbo.project p 
               ON (p.name = a.title)
  LEFT OUTER JOIN rationales r 
               ON (r.id = ar.rationales_id)
  LEFT OUTER JOIN eca_dev.eca_dev.dbo.goal g 
               ON (g.goalname = r.rationale)
  WHERE g.goalid IS NOT NULL
  ORDER BY ar.afcp_id,ar.rationales_id


/*SELECT a.id,a.title, p.projectid,p.name,ar.afcp_id,rationales_id ,r.id,r.rationale,g.goalid,g.goalname*/