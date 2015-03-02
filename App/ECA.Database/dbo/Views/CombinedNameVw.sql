

CREATE VIEW [dbo].[CombinedNameVw]
	AS SELECT np1.PersonId,
	          np1.Value AS LastName,
              np2.Value AS FirstName,
              np3.Value AS Prefix,
              np4.Value AS Suffix 
         FROM NamePart np1
         LEFT OUTER JOIN NamePart np2 ON (np2.PersonId = np1.PersonId and np2.NameTypeid = 2)
         LEFT OUTER JOIN NamePart np3 ON (np3.PersonId = np1.PersonId and np3.NameTypeid = 3)
         LEFT OUTER JOIN NamePart np4 ON (np4.PersonId = np1.PersonId and np4.NameTypeid = 4)
        WHERE np1.NameTypeid = 1

