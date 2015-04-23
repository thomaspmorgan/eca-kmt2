/* Loads CE Birth Location data as a "City" Type in the Location table - will not add a location that already exists */
USE CE
GO

DECLARE @CityLocationTypeID int = 0; 

/* get the correct ID for the region location type */ 
SELECT @CityLocationTypeID = locationtypeid 
  FROM ECA_Dev_local_copy.dbo.locationtype 
 WHERE locationtypename = 'City'

/* output to be sure */
SELECT @CityLocationTypeID AS 'City Location Type ID'

/* ALWAYS RUN THIS - THIS WILL POPULATE ALL CORRECTLY */
INSERT INTO ECA_Dev_local_copy.dbo.location
	(LocationTypeId,LocationName,Country_LocationId,Region_LocationId,
	History_createdby,history_createdon,history_revisedby,history_revisedon)
SELECT @CityLocationTypeID,ip.BIRTH_CITY,l1.locationid,l1.region_locationid,
       0, CAST(N'2015-04-11T00:00:00.0000000-05:00' AS DateTimeOffset), 0, CAST(N'2015-04-11T00:00:00.0000000-05:00' AS DateTimeOffset)
  FROM CE_Person ip
  LEFT JOIN ECA_Dev_local_copy.dbo.location l1 ON (l1.locationtypeid = 3 AND l1.locationname = ip.BIRTH_COUNTRY  )
  LEFT JOIN ECA_Dev_local_copy.dbo.location l ON (l.locationtypeid = 5 AND l.locationname = ip.BIRTH_CITY AND (l.country_locationid IS NULL OR l.Country_LocationId = l1.locationid ))
 WHERE ip.birth_city IS NOT NULL AND ip.birth_country IS NOT NULL AND l.locationid IS NULL
 GROUP BY ip.Birth_City,l1.locationid,l1.region_locationid

GO


/* Create the update statements to switch birthlocation assignments */
select 'UPDATE ECA_Dev_Local_Copy.dbo.person SET placeofbirth_locationid = '+CAST(l1.locationid AS nvarchar)+
  ' WHERE placeofbirth_locationid = '+CAST(l.locationid AS Nvarchar) /*,
  p.placeofbirth_locationid,l.locationid,l.locationname,l.locationtypeid ,
  l1.locationid,l1.locationname,l1.locationtypeid*/
  from ECA_Dev_Local_Copy.dbo.person p
  join ECA_Dev_Local_Copy.dbo.location l ON (l.locationid = p.placeofbirth_locationid)
  left join ECA_Dev_Local_Copy.dbo.location l1 ON (l1.locationtypeid = 5 AND l1.locationname = l.locationname)
  where l.locationtypeid <> 5 

/* results */
UPDATE ECA_Dev_Local_Copy.dbo.person SET placeofbirth_locationid = 4902 WHERE placeofbirth_locationid = 39
UPDATE ECA_Dev_Local_Copy.dbo.person SET placeofbirth_locationid = 4903 WHERE placeofbirth_locationid = 42
UPDATE ECA_Dev_Local_Copy.dbo.person SET placeofbirth_locationid = 4904 WHERE placeofbirth_locationid = 67
UPDATE ECA_Dev_Local_Copy.dbo.person SET placeofbirth_locationid = 4905 WHERE placeofbirth_locationid = 70
UPDATE ECA_Dev_Local_Copy.dbo.person SET placeofbirth_locationid = 4906 WHERE placeofbirth_locationid = 90
UPDATE ECA_Dev_Local_Copy.dbo.person SET placeofbirth_locationid = 4907 WHERE placeofbirth_locationid = 107
UPDATE ECA_Dev_Local_Copy.dbo.person SET placeofbirth_locationid = 4908 WHERE placeofbirth_locationid = 145
UPDATE ECA_Dev_Local_Copy.dbo.person SET placeofbirth_locationid = 4909 WHERE placeofbirth_locationid = 146
UPDATE ECA_Dev_Local_Copy.dbo.person SET placeofbirth_locationid = 4910 WHERE placeofbirth_locationid = 176
UPDATE ECA_Dev_Local_Copy.dbo.person SET placeofbirth_locationid = 4911 WHERE placeofbirth_locationid = 202
UPDATE ECA_Dev_Local_Copy.dbo.person SET placeofbirth_locationid = 4912 WHERE placeofbirth_locationid = 185
UPDATE ECA_Dev_Local_Copy.dbo.person SET placeofbirth_locationid = 4913 WHERE placeofbirth_locationid = 192
UPDATE ECA_Dev_Local_Copy.dbo.person SET placeofbirth_locationid = 4913 WHERE placeofbirth_locationid = 192
UPDATE ECA_Dev_Local_Copy.dbo.person SET placeofbirth_locationid = 4914 WHERE placeofbirth_locationid = 194
