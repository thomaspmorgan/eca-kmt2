/****** Object: View [dbo].[Vw_CountryCodeXREF] Script Date: 3/6/2015 3:43:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[Vw_CountryCodeXREF]
	AS SELECT l.locationid,l.locationName,l.locationiso,l.region_locationid,cx.countryname,cx.ISOCode3,cx.ISOCode2,cx.UNCodeM49
         FROM eca_dev.eca_dev.dbo.locationtype lt
         JOIN eca_dev.eca_dev.dbo.location l ON (l.locationtypeid = lt.locationtypeid)
         LEFT JOIN CountryCodeXREF cx ON (cx.ISOCode3 = l.locationiso)
        WHERE lt.locationtypename = 'Country'

GO
