USE ECA_DEV
GO
/* creates test Region data for Location Table  */

DECLARE @regionLocationID int = 0; 

/* get the correct ID for the region location type */ 
SELECT @regionLocationID = locationtypeid 
  FROM locationtype 
 WHERE locationTypeName = 'Region'

/* output to be sure */
SELECT @regionLocationID AS 'Region Location ID'

/* Insert test data */
INSERT INTO location
 (LocationTypeId,LocationName,LocationIso,
     History_createdby,history_createdon,history_revisedby,history_revisedon)
values
 (@regionLocationid,'Africa','AF',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
 (@regionLocationid,'East Asia and the Pacific','EAP',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
 (@regionLocationid,'Europe and Eurasia','EUR',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
 (@regionLocationid,'Near East','NEA',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
 (@regionLocationid,'South and Central Asia','SCA',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
 (@regionLocationid,'Western Hemisphere','WHA',0,sysdatetimeoffset(),0,sysdatetimeoffset())
 (@regionLocationid,'UN and Other International Organizations','IO',0,sysdatetimeoffset(),0,sysdatetimeoffset(),
 (@regionLocationid,'United States','US',0,sysdatetimeoffset(),0,sysdatetimeoffset())

GO



