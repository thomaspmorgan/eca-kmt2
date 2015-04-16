USE IVLP
GO

/* Insert Participant rows for IVLP */

INSERT INTO eca_dev.eca_dev.dbo.ParticipantProject
	(ParticipantId,ProjectId)

SELECT p4.participantid,
       p3.projectid

/*
select p.participant_id,p.project_id,ip.project_id,ip.ProjectNameNew,
p1.person_id,p1.PREFIX_CD,p1.FIRST_NAME,p1.MIDDLE_NAME,p1.LAST_NAME,p1.SUFFIX_CD,p1.BIRTH_DATE,
p2.personid,p2.nameprefix,p2.firstname,p2.middlename,p2.lastname,p2.namesuffix,p2.dateofbirth,
ps.sevis_dhs_id,
p3.projectid,p3.name,
p4.participantid,p4.personid
*/


FROM IVLP_Participant p
  LEFT JOIN ivlp_project ip ON (ip.project_id = p.project_id)
  LEFT JOIN IVLP_Person p1 ON (p1.person_id = p.person_id)
  LEFT JOIN ivlp_participant_sevis ps ON (ps.participant_id = p.PARTICIPANT_ID)
  LEFT JOIN eca_dev.eca_dev.dbo.person p2 ON (((p1.[First_Name] IS NULL AND p2.firstname IS NULL) OR (p2.firstname = p1.[First_Name])) AND
    ((p1.[Last_Name] IS NULL AND p2.lastname IS NULL) OR (p2.lastname = p1.[Last_Name])) AND
    ((p1.[PREFIX_CD] IS NULL AND p2.nameprefix IS NULL) OR (p2.nameprefix = p1.[PREFIX_CD])) AND
    ((p1.[SUFFIX_CD] IS NULL AND p2.namesuffix IS NULL) OR (p2.namesuffix = p1.[SUFFIX_CD])) AND
    ((p1.[Middle_Name] IS NULL AND p2.middlename IS NULL) OR (p2.middlename = p1.[Middle_Name])) AND
    ((p1.BIRTH_DATE IS NULL AND p2.dateofbirth IS NULL) OR (p2.dateofbirth = p1.BIRTH_DATE)))
  LEFT JOIN eca_dev.eca_dev.dbo.project p3 ON (p3.name = ip.ProjectNameNew)
  LEFT JOIN eca_dev.eca_dev.dbo.participant p4 ON (p4.personid = p2.personid 
     AND ((ps.sevis_dhs_id IS NULL AND p4.sevisid IS NULL) OR (p4.sevisid = SUBSTRING(ps.sevis_dhs_id,1,11))))

  --where p.project_id = 'B5AD81A6D8CE4D11863BE5309316FF3A' AND p.person_id = 'CF247EDC237DD224104A2C8E6148943E'
  order by p3.projectid

	   


GO



