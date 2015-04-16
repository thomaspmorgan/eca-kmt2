USE IVLP
GO

/* Insert Participant rows for IVLP */

INSERT INTO eca_dev.eca_dev.dbo.Participant
	(Organizationid,Personid,ParticipantTypeId,
	History_CreatedBy,History_CreatedOn,History_RevisedBy,History_RevisedOn,
	SevisId,ContactAgreement)

SELECT NULL,
       p2.personid,
       pt.participanttypeid,
       p2.history_createdby,p2.history_createdon,p2.history_revisedby,p2.history_revisedon,
       SUBSTRING(ps.sevis_dhs_id,1,11),
       0

--,p.participant_id,p.person_id,p.project_id,
--       p1.person_id,p1.LAST_NAME,p1.FIRST_NAME,p1.MIDDLE_NAME,
 --      p2.*,
--	   ps.*
  FROM IVLP_Participant p
  JOIN IVLP_Person p1 ON (p1.person_id = p.person_id)
  JOIN eca_dev.eca_dev.dbo.person p2 ON 
   (((p1.[First_Name] IS NULL AND p2.firstname IS NULL) OR (p2.firstname = p1.[First_Name])) AND
    ((p1.[Last_Name] IS NULL AND p2.lastname IS NULL) OR (p2.lastname = p1.[Last_Name])) AND
    ((p1.[PREFIX_CD] IS NULL AND p2.nameprefix IS NULL) OR (p2.nameprefix = p1.[PREFIX_CD])) AND
    ((p1.[SUFFIX_CD] IS NULL AND p2.namesuffix IS NULL) OR (p2.namesuffix = p1.[SUFFIX_CD])) AND
    ((p1.[Middle_Name] IS NULL AND p2.middlename IS NULL) OR (p2.middlename = p1.[Middle_Name])) AND
    ((p1.BIRTH_DATE IS NULL AND p2.dateofbirth IS NULL) OR (p2.dateofbirth = p1.BIRTH_DATE)))
  JOIN ivlp_participant_sevis ps ON (ps.participant_id = p.PARTICIPANT_ID)
  JOIN eca_dev.eca_dev.dbo.participanttype pt ON (pt.name = 'Individual')
  GROUP BY p2.personid,
	   pt.participanttypeid,ps.sevis_dhs_id,
	   p2.history_createdby,p2.history_createdon,p2.history_revisedby,p2.history_revisedon


GO



