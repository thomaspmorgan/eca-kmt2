/* Loads Visiting Scholar Birth Location data as a "City" Type in the Location table */
USE VisitingScholar
GO

DECLARE @CityLocationTypeID int = 0; 

/* get the correct ID for the region location type */ 
SELECT @CityLocationTypeID = locationtypeid 
  FROM ECA_Dev_Backup.dbo.locationtype 
 WHERE locationtypename = 'City'

/* output to be sure */
SELECT @CityLocationTypeID AS 'City Location Type ID'

/* ALWAYS RUN THIS - THIS WILL POPULATE ALL CORRECTLY */
INSERT INTO ECA_Dev_Backup.dbo.location
	(LocationTypeId,LocationName,Country_LocationId,Region_LocationId,
	History_createdby,history_createdon,history_revisedby,history_revisedon)
SELECT @CityLocationTypeID,vs.[Birth City],cx.locationid,cx.region_locationid,
       0, CAST(N'2015-04-02T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-04-02T00:00:00.0000000-05:00' AS DateTimeOffset) 
  FROM VisitingScholarData vs
  LEFT JOIN DataMigrationXREF.dbo.Vw_CountryCodeXREF cx ON (cx.ISOCode2 = vs.[Birth Country Name])
 GROUP BY vs.[Birth City],cx.locationid,cx.region_locationid


GO


