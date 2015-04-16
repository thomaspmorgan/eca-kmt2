/* Loads IVLP Birth Location data as a "City" Type in the Location table */
USE IVLP
GO

DECLARE @CityLocationTypeID int = 0; 

/* get the correct ID for the region location type */ 
SELECT @CityLocationTypeID = locationtypeid 
  FROM ECA_Dev_test.dbo.locationtype 
 WHERE locationtypename = 'City'

/* output to be sure */
SELECT @CityLocationTypeID AS 'City Location Type ID'

/* ALWAYS RUN THIS - THIS WILL POPULATE ALL CORRECTLY */
INSERT INTO ECA_Dev_test.dbo.location
	(LocationTypeId,LocationName,Country_LocationId,Region_LocationId,
	History_createdby,history_createdon,history_revisedby,history_revisedon)
SELECT @CityLocationTypeID,ip.BIRTH_CITY,l1.locationid,l1.region_locationid,
       0, CAST(N'2015-04-03T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-04-03T00:00:00.0000000-05:00' AS DateTimeOffset)
  FROM IVLP_Person ip
  LEFT JOIN ECA_Dev_test.dbo.location l ON (l.locationtypeid = 5 AND l.locationname = ip.BIRTH_CITY  )
  LEFT JOIN ECA_Dev_test.dbo.location l1 ON (l1.locationtypeid = 3 AND l1.locationname = ip.BIRTH_COUNTRY  )
 WHERE birth_city IS NOT NULL AND birth_country IS NOT NULL AND l.locationid IS NULL
 GROUP BY ip.Birth_City,l1.locationid,l1.region_locationid

GO


