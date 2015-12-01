/* Update IVLP Birth Country Mappings */

/* One update */
UPDATE dbo.ECA_IVLP_KMT_Person_XREF_Interim
SET IVLP_Mapped_BirthCountryName =
CASE
WHEN LOWER(IVLP_BIRTH_COUNTRY) = LOWER('Congo')  
   THEN 'Congo (Brazzaville)'
WHEN LOWER(IVLP_BIRTH_COUNTRY) = LOWER('Congo, Democratic Republic of the')  
   THEN 'Congo (Kinshasa)'
WHEN LOWER(IVLP_BIRTH_COUNTRY) = LOWER('Federated States of Micronesia')  
   THEN 'Micronesia, Federated States of'
WHEN LOWER(IVLP_BIRTH_COUNTRY) = LOWER('Germany, Federal Republic of')  
   THEN 'Germany'
WHEN LOWER(IVLP_BIRTH_COUNTRY) = LOWER('Korea, Democratic People''s Republic of')  
   THEN 'Korea, North'
WHEN LOWER(IVLP_BIRTH_COUNTRY) = LOWER('Kosovo, Republic of')  
   THEN 'Kosovo'
WHEN LOWER(IVLP_BIRTH_COUNTRY) = LOWER('People''s Republic of China')  
   THEN 'China'
WHEN LOWER(IVLP_BIRTH_COUNTRY) = LOWER('Republic of Korea')  
   THEN 'Korea, South'
WHEN LOWER(IVLP_BIRTH_COUNTRY) = LOWER('South Sudan, Republic of')  
   THEN 'South Sudan'
WHEN LOWER(IVLP_BIRTH_COUNTRY) = LOWER('St. Lucia')  
   THEN 'Saint Lucia'
WHEN LOWER(IVLP_BIRTH_COUNTRY) = LOWER('Tanzania, United Republic of')  
   THEN 'Tanzania'
WHEN LOWER(IVLP_BIRTH_COUNTRY) = LOWER('Union of Soviet Socialist Republics')  
   THEN 'USSR'
WHEN LOWER(IVLP_BIRTH_COUNTRY) = LOWER('United States of America')  
   THEN 'United States'
WHEN LOWER(IVLP_BIRTH_COUNTRY) = LOWER('Vietnam, Republic of')  
   THEN 'Vietnam'
ELSE IVLP_BIRTH_COUNTRY
END





