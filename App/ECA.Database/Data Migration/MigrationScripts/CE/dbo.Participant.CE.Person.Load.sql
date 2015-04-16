USE CE
GO

/* Insert Participant rows for CE */

INSERT INTO eca_dev_local_copy.dbo.Participant
	(Organizationid,Personid,ParticipantTypeId,
	History_CreatedBy,History_CreatedOn,History_RevisedBy,History_RevisedOn,
	SevisId,ContactAgreement)

/*
SELECT  p.participant_id,p.component_id,p.person_id,p.role,
       p1.person_id,p1.gender_cd,p1.marital_status,p1.prefix_cd,p1.first_name,p1.middle_name,p1.last_name,p1.suffix_cd,p1.birth_date,p1.birth_city,p1.birth_country,
	  g.genderid,g.gendername,
	  l.locationid,l.locationname,l.locationtypeid,
	   p2.personid,p2.firstname,p2.lastname,p2.nameprefix,p2.namesuffix,p2.middlename,p2.dateofbirth,p2.PlaceOfBirth_LocationId,p2.genderid,p2.MaritalStatusId,
	   	   pt.participanttypeid,pt.name,
		         ps.sevis_dhs_id,0
*/


SELECT NULL,p2.personid,pt.ParticipantTypeId,
  	0, CAST(N'2015-04-14T00:00:00.0000000-00:00' AS DateTimeOffset), 0, CAST(N'2015-04-14T00:00:00.0000000-00:00' AS DateTimeOffset),
  	ps.sevis_dhs_id,0
  FROM CE_Participant p
  JOIN CE_Person p1 ON (p1.person_id = p.person_id)
  LEFT JOIN ECA_Dev_Local_Copy.dbo.gender g 
  ON (g.gendername = 	CASE 
			WHEN p1.GENDER_CD = 'M' THEN 'Male'
			WHEN p1.GENDER_CD = 'F' Then 'Female'
			WHEN p1.GENDER_CD IS NULL THEN 'NotSpecified'
			ELSE 'Other'
			END)
  LEFT JOIN ECA_Dev_Local_Copy.dbo.maritalstatus m ON (m.status = CASE
								WHEN p1.MARITAL_STATUS IS NULL THEN 'N'
 								ELSE p1.marital_status
								END )
  LEFT JOIN ECA_Dev_Local_Copy.dbo.location l ON (l.locationtypeid = 5 AND l.locationname = CASE WHEN p1.BIRTH_CITY IS NULL THEN p1.BIRTH_COUNTRY ELSE p1.BIRTH_CITY END )
    LEFT JOIN ECA_Dev_Local_Copy.dbo.person p2 ON 
   (((p1.[First_Name] IS NULL AND p2.firstname IS NULL) OR (p2.firstname = p1.[First_Name])) AND
    ((p1.[Last_Name] IS NULL AND p2.lastname IS NULL) OR (p2.lastname = p1.[Last_Name])) AND
    ((p1.[PREFIX_CD] IS NULL AND p2.nameprefix IS NULL) OR (p2.nameprefix = SUBSTRING(p1.[PREFIX_CD],1,10))) AND
    ((p1.[SUFFIX_CD] IS NULL AND p2.namesuffix IS NULL) OR (p2.namesuffix = SUBSTRING(p1.[SUFFIX_CD],1,10))) AND
    ((p1.[Middle_Name] IS NULL AND p2.middlename IS NULL) OR (p2.middlename = p1.[Middle_Name])) AND
    ((p1.BIRTH_DATE IS NULL AND p2.dateofbirth = CAST(N'2015-04-11' AS Date)) OR (CONVERT(date,p2.dateofbirth,1) = CONVERT(date,p1.BIRTH_DATE,1))) AND
	((p1.birth_city IS NULL AND p1.birth_country IS NULL AND l.locationid IS NULL) OR (p2.PlaceOfBirth_LocationId = l.locationid)) AND
	(g.genderid = p2.genderid)) AND (m.MaritalStatusId = p2.MaritalStatusId)
  LEFT JOIN ce_participant_sevis ps ON (ps.participant_id = p.PARTICIPANT_ID)
  LEFT JOIN ECA_Dev_Local_Copy.dbo.participanttype pt ON (UPPER(pt.name) = UPPER(p.role))
  GROUP BY p2.personid,pt.ParticipantTypeId,ps.sevis_dhs_id
  ORDER BY p2.personid
  


GO






