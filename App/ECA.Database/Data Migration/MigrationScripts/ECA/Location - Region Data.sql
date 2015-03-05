/* creates test Region data for Location Table  */

/* Do this if countryid column not created */
ALTER TABLE dbo.location ADD Country_LocationId int NULL ;
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Location_dbo.Location_Country_LocationId] FOREIGN KEY([Country_LocationId])
REFERENCES [dbo].[Location] ([LocationId])
GO

begin tran t1

DECLARE @regionLocationID int = 0; 

/* get the correct ID for the region location type */ 
SELECT @regionLocationID = locationtypeid 
  FROM locationtype 
 WHERE locationTypeName = 'Region'

/* output to be sure */
SELECT @regionLocationID AS 'Region Location ID'

/* Insert test data */
insert into location
 (LocationTypeId,LocationName,LocationIso,
     History_createdby,history_createdon,history_revisedby,history_revisedon)
values
 (@regionLocationid,'Africa','AF',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
 (@regionLocationid,'East Asia and the Pacific','EAP',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
 (@regionLocationid,'Europe and Eurasia','EUR',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
 (@regionLocationid,'Near East','NEA',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
 (@regionLocationid,'South and Central Asia','SCA',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
 (@regionLocationid,'Western Hemisphere','WHA',0,sysdatetimeoffset(),0,sysdatetimeoffset())
 (@regionLocationid,'UN and Other International Organizations','IO',0,sysdatetimeoffset(),0,sysdatetimeoffset())

commit tran t1
GO



