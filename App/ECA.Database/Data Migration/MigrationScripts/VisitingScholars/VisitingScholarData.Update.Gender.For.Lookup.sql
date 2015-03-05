USE visitingscholar
GO

/* Reset the Gender values so there is a match */
UPDATE dbo.VisitingScholarData 
   SET gender = (CASE gender
                    WHEN 'M' THEN N'Male'
                    WHEN 'F' THEN N'Female'
                    WHEN NULL THEN N'Not Specified'
                    ELSE N'Unknown'
                 END)
GO 