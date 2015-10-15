/* Update IVLP Language Mappings */

/* One update */
UPDATE ivlp_xref.dbo.local_ivlp_Project_Mapping_xref_interim
SET KMT_MappedProjectLanguage =
CASE
WHEN LOWER(PROJECT_LANGUAGE) LIKE '%armenian%'  
   THEN 'Armenian'
WHEN LOWER(PROJECT_LANGUAGE) = 'chinese (mandarin)'
   THEN 'Chinese, Mandarin'
WHEN LOWER(PROJECT_LANGUAGE) = 'chinese (cantonese)'
   THEN 'Chinese, Yue'
WHEN LOWER(PROJECT_LANGUAGE) = 'chinese (taiwanese)'
   THEN 'Chinese, Taiwanese'
WHEN LOWER(PROJECT_LANGUAGE) = 'mandarin'
   THEN 'Chinese, Mandarin'
WHEN LOWER(PROJECT_LANGUAGE) = 'nepali-nepalese'
   THEN 'Nepali'
ELSE Project_Language
END




