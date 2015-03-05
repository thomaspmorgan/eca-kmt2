USE AFCP
GO

/* Creates the row to capture Moneyflow From Program to Project */
INSERT 
  INTO eca_dev.eca_dev.dbo.moneyflow
(value,recipientaccommodationid,history_createdby,history_createdon,history_revisedby,history_revisedon,parent_moneyflowid,recipientorganizationid,
SourceorganizationID,moneyflowstatusid,moneyflowtypeid,TransactionDate,FiscalYear,sourcetypeid,recipienttypeid,SourceProgramID,recipientprogramid,
SourceProjectID,recipientprojectid,SourceParticipantID,RecipientParticipantID,SourceItineraryStopID,RecipientItineraryStopID,RecipientTransportationID,
 Description)
SELECT a.post_contribution  value,
	NULL   recipientaccommodationid,
	0      history_createdby, 
	CAST(N'2015-02-23T00:00:00.0000000-05:00' AS DateTimeOffset) history_createdon,
	0  history_revisedby, 
	CAST(N'2015-02-23T00:00:00.0000000-05:00' AS DateTimeOffset) history_revisedon,
	NULL   parent_moneyflowid,
	NULL   recipientorganizationid,
        /*o.organizationid*/
        NULL    SourceorganizationID,
	3      moneyflowstatusid,
	3      moneyflowtypeid,
	CASE
	  WHEN y.programyear IS NULL THEN NULL
 	  ELSE N'9/30/'+CAST(y.programyear AS Nvarchar)+' 12:00:00 AM -05:00'
	END    TransactionDate,
	CASE
	  WHEN y.programyear IS NULL THEN NULL
	  ELSE y.programyear
	END    FiscalYear,
	9      sourcetypeid,
	3      recipienttypeid,
	NULL   SourceProgramID,
	NULL   recipientprogramid,
	NULL   SourceProjectID,
	p.projectid   recipientprojectid,
	NULL   SourceParticipantID,
	NULL   RecipientParticipantID,
	NULL   SourceItineraryStopID,
	NULL   RecipientItineraryStopID,
	NULL   RecipientTransportationID,
	N'Contribution from Post: '+p1.post   Description
FROM afcp a
LEFT JOIN years y ON (y.id = a.yearid)
LEFT JOIN eca_dev.eca_dev.dbo.project p ON (p.name = a.title AND (p.description = a.summary OR a.summary IS NULL))
LEFT JOIN countries c ON (c.id = a.countryid)
LEFT JOIN posts p1 ON (p1.id = c.post_id)
--LEFT JOIN organizationtype ot ON (ot.organizationtype = 'Post')
--LEFT JOIN organization o ON (o.organizationtypeid = ot.organizationtypeid AND o.description = p1.post)
WHERE a.post_contribution > 0

GO


 


