/* This script creates moneyflow for AFCP top-level program to AFCP sub-program */

INSERT 
  INTO /*eca_dev.eca_dev.*/dbo.moneyflow
(value,recipientaccommodationid,history_createdby,history_createdon,history_revisedby,history_revisedon,parent_moneyflowid,recipientorganizationid,
SourceorganizationID,moneyflowstatusid,moneyflowtypeid,TransactionDate,FiscalYear,sourcetypeid,recipienttypeid,SourceProgramID,recipientprogramid,
SourceProjectID,recipientprojectid,SourceParticipantID,RecipientParticipantID,SourceItineraryStopID,RecipientItineraryStopID,RecipientTransportationID,
 Description)
SELECT m.afcp_contribution  value,
	NULL   recipientaccommodationid,
	0      history_createdby, 
	CAST(N'2015-03-24T00:00:00.0000000-05:00' AS DateTimeOffset) history_createdon,
	0  history_revisedby, 
	CAST(N'2015-03-24T00:00:00.0000000-05:00' AS DateTimeOffset) history_revisedon,
	NULL   parent_moneyflowid,
	NULL   recipientorganizationid,
	NULL    SourceorganizationID,
	3      moneyflowstatusid,
	3      moneyflowtypeid,
	CASE
	  WHEN m.programyear IS NULL THEN NULL
 	 ELSE  N'9/30/'+CAST(m.programyear AS Nvarchar)+' 12:00:00 AM -05:00'
	END    TransactionDate,
	CASE
	  WHEN m.programyear IS NULL THEN NULL
	  ELSE m.programyear
	END    FiscalYear,
	2      sourcetypeid,
	2      recipienttypeid,
	1038 SourceProgramID,
	m.programid   recipientprogramid,
	NULL   SourceProjectID,
	NULL   recipientprojectid,
	NULL   SourceParticipantID,
	NULL   RecipientParticipantID,
	NULL   SourceItineraryStopID,
	NULL   RecipientItineraryStopID,
	NULL   RecipientTransportationID,
	CASE
	  WHEN m.programid = 1008 THEN 'AFCP/AFCP-Small Grants Competition'
	  WHEN m.programid = 1009 THEN 'AFCP/AFCP-Large Grants Program'
	  WHEN m.programid = 1010 THEN 'AFCP/AFCP-Other Projects'
	  ELSE NULL
	END    Description
FROM (SELECT SUM(value) afcp_contribution,
     	     SourceProgramId programid,
	     fiscalyear programyear
	FROM MoneyFlow
       WHERE SourceProgramId IN (1008,1009,1010)
       GROUP BY SourceProgramId,FiscalYear) m
WHERE m.afcp_contribution > 0
ORDER BY m.programid,m.programyear
