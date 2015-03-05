USE AFCP
GO

/* Creates the row to capture Moneyflow From Program to Project */
INSERT 
  INTO eca_dev.eca_dev.dbo.moneyflow
(value,recipientaccommodationid,history_createdby,history_createdon,history_revisedby,history_revisedon,parent_moneyflowid,recipientorganizationid,
SourceorganizationID,moneyflowstatusid,moneyflowtypeid,TransactionDate,FiscalYear,sourcetypeid,recipienttypeid,SourceProgramID,recipientprogramid,
SourceProjectID,recipientprojectid,SourceParticipantID,RecipientParticipantID,SourceItineraryStopID,RecipientItineraryStopID,RecipientTransportationID,
 Description)
SELECT a.award  value,
	NULL   recipientaccommodationid,
	0      history_createdby, 
	CAST(N'2015-02-23T00:00:00.0000000-05:00' AS DateTimeOffset) history_createdon,
	0  history_revisedby, 
	CAST(N'2015-02-23T00:00:00.0000000-05:00' AS DateTimeOffset) history_revisedon,
	NULL   parent_moneyflowid,
	p2.organizationid   recipientorganizationid,
        NULL     SourceorganizationID,
	3      moneyflowstatusid,
	2      moneyflowtypeid,
	CASE
	  WHEN y.programyear IS NULL THEN NULL
 	  ELSE N'9/30/'+CAST(y.programyear AS Nvarchar)+' 12:00:00 AM -05:00'
	END    TransactionDate,
	CASE
	  WHEN y.programyear IS NULL THEN NULL
	  ELSE y.programyear
	END    FiscalYear,
	3      sourcetypeid,
	4      recipienttypeid,
        NULL    SourceProgramID,
	NULL   recipientprogramid,
	p.projectid   SourceProjectID,
	NULL   recipientprojectid,
	NULL   SourceParticipantID,
	p2.participantid   RecipientParticipantID,
	NULL   SourceItineraryStopID,
	NULL   RecipientItineraryStopID,
	NULL   RecipientTransportationID,
	NULL   Description
FROM afcp a
LEFT JOIN years y ON (y.id = a.yearid)
LEFT JOIN eca_dev.eca_dev.dbo.project p ON (p.name = a.title AND p.description = a.summary)
LEFT JOIN participants p1 ON (p1.project_id = a.id)
LEFT OUTER JOIN eca_dev.eca_dev.dbo.combinednamevw c 
             ON ((c.lastname = p1.[Name Last] OR (c.lastname IS NULL AND p1.[Name Last] IS NULL)) AND  
                 (c.firstname = p1.[Name First] OR (c.firstname IS NULL AND p1.[Name First] IS NULL)) AND
		   (c.prefix = p1.prefix OR (c.prefix IS NULL AND p1.prefix IS NULL)) AND 
		   (c.suffix = p1.suffix OR (c.suffix IS NULL AND p1.suffix IS NULL)))
LEFT JOIN eca_dev.eca_dev.dbo.participant p2 ON (p2.personid = c.personid)
WHERE a.award > 0 AND p1.reciprole = 'Recipient' AND p1.reciptype = 'Individual'

GO




 


