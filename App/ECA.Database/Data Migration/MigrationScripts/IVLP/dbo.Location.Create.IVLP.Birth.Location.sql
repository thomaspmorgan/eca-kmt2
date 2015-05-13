/* Loads IVLP Birth Location data as a "City" Type in the Location table */
USE IVLP
GO

DECLARE @CityLocationTypeID int = 0; 

/* get the correct ID for the region location type */ 
SELECT @CityLocationTypeID = locationtypeid 
  FROM ECA_Dev.eca_dev.dbo.locationtype 
 WHERE locationtypename = 'City'

/* output to be sure */
SELECT @CityLocationTypeID AS 'City Location Type ID'

/* ALWAYS RUN THIS - THIS WILL POPULATE ALL CORRECTLY ... WRONG! */

/* HERE IS NEW CODE WHICH WORKS!!! */
INSERT INTO ECA_Dev.eca_dev.dbo.location
	(LocationTypeId,LocationName,Country_LocationId,Region_LocationId,
	History_createdby,history_createdon,history_revisedby,history_revisedon)
SELECT @CityLocationTypeID,ip.BIRTH_CITY,l1.locationid,l1.region_locationid,
       0, CAST(N'2015-05-11T00:00:00.0000000-00:00' AS DateTimeOffset), 0, CAST(N'2015-05-11T00:00:00.0000000-00:00' AS DateTimeOffset)
  FROM IVLP_Person ip
  LEFT JOIN ECA_Dev.eca_dev.dbo.location l1 ON (l1.locationtypeid = 3 AND l1.locationname = ip.BIRTH_COUNTRY  )
  LEFT JOIN ECA_Dev.eca_dev.dbo.location l ON (l.locationtypeid = 5 AND 
        /* Birth City Null and birth country Exists */
     	(l.locationname IS NULL AND ip.birth_city IS NULL AND l.country_locationid = l1.locationid) OR
        /* Birth City Exists and Birth Country does not exist OR Birth Country DOES exist */      
	(l.locationname = ip.BIRTH_CITY AND ((l.country_locationid IS NULL AND l1.locationid IS NULL) OR l.country_locationid = l1.locationid)))
 WHERE ((ip.birth_city IS NOT NULL OR ip.birth_country IS NOT NULL) AND l.locationid IS NULL) AND
       (ip.birth_city IS NOT NULL OR l1.locationid IS NOT NULL OR l1.region_locationid IS NOT NULL)
 GROUP BY ip.Birth_City,l1.locationid,l1.region_locationid

GO



