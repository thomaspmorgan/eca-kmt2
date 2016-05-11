DECLARE @sevisUserAccount TABLE(	
	[OrgId] nvarchar(15),
	[Username] nvarchar(10),
	[EmailAddress] NVARCHAR(200) NOT NULL
)

--CREATE BE Sevis Account Info
DECLARE @sevisUsername VARCHAR(10) = 'esayya9302'
DECLARE @orgId VARCHAR(15) = 'P-1-19833'

--Add All BE Accounts Here
INSERT INTO @sevisUserAccount (OrgId, Username, EmailAddress) VALUES (@orgId, @sevisUsername, 'gibowskibr@statedept.us');
INSERT INTO @sevisUserAccount (OrgId, Username, EmailAddress) VALUES (@orgId, @sevisUsername, 'tuckerb@statedept.us');
INSERT INTO @sevisUserAccount (OrgId, Username, EmailAddress) VALUES (@orgId, @sevisUsername, 'comptonwa@statedept.us');
INSERT INTO @sevisUserAccount (OrgId, Username, EmailAddress) VALUES (@orgId, @sevisUsername, 'sayyade@statedept.us');
INSERT INTO @sevisUserAccount (OrgId, Username, EmailAddress) VALUES (@orgId, @sevisUsername, 'boustaniw@statedept.us');
INSERT INTO @sevisUserAccount (OrgId, Username, EmailAddress) VALUES (@orgId, @sevisUsername, 'brian.gibowski@buchanan-edwards.com');
INSERT INTO @sevisUserAccount (OrgId, Username, EmailAddress) VALUES (@orgId, @sevisUsername, 'brandon.tucker@buchanan-edwards.com');
INSERT INTO @sevisUserAccount (OrgId, Username, EmailAddress) VALUES (@orgId, @sevisUsername, 'alan.compton@buchanan-edwards.com');
INSERT INTO @sevisUserAccount (OrgId, Username, EmailAddress) VALUES (@orgId, @sevisUsername, 'wassim.boustani@buchanan-edwards.com');
INSERT INTO @sevisUserAccount (OrgId, Username, EmailAddress) VALUES (@orgId, @sevisUsername, 'jefferson.baker@buchanan-edwards.com');
INSERT INTO @sevisUserAccount (OrgId, Username, EmailAddress) VALUES (@orgId, @sevisUsername, 'eiad.sayyad@buchanan-edwards.com');
INSERT INTO @sevisUserAccount (OrgId, Username, EmailAddress) VALUES (@orgId, @sevisUsername, 'mak.mendelson@buchanan-edwards.com');
INSERT INTO @sevisUserAccount (OrgId, Username, EmailAddress) VALUES (@orgId, @sevisUsername, 'mycal.rolland@buchanan-edwards.com');

--Add All ECA Accounts Here
INSERT INTO @sevisUserAccount (OrgId, Username, EmailAddress) VALUES (@orgId, 'sshiel1521', 'ShieldsSD@statedept.us');
INSERT INTO @sevisUserAccount (OrgId, Username, EmailAddress) VALUES (@orgId, 'sgrant5540', 'grantsb@statedept.us');
INSERT INTO @sevisUserAccount (OrgId, Username, EmailAddress) VALUES (@orgId, 'tmorga3488', 'thomas.morgan@buchanan-edwards.com');
INSERT INTO @sevisUserAccount (OrgId, Username, EmailAddress) VALUES (@orgId, 'tmorga3488', 'morgant@statedept.us');
INSERT INTO @sevisUserAccount (OrgId, Username, EmailAddress) VALUES (@orgId, 'dgusta9934', 'gustafsondp@statedept.us');
INSERT INTO @sevisUserAccount (OrgId, Username, EmailAddress) VALUES (@orgId, 'jnelso7231', 'nelsonjg2@statedept.us');
INSERT INTO @sevisUserAccount (OrgId, Username, EmailAddress) VALUES (@orgId, 'rmurph8772', 'murphyrm@statedept.us');


--END ADDING ALL ACCOUNTS THAT WILL BE UPSERTED...


--Declare a temp table to hold sevis accounts to insert.
DECLARE @principalSevisAccount TABLE (
	[PrincipalId] int NULL,
	[OrgId] nvarchar(15),
	[Username] nvarchar(10)
)

--Insert into to temp table the sevis accounts that will be upserted.
INSERT INTO @principalSevisAccount(PrincipalId, OrgId, Username)
SELECT DISTINCT
	(SELECT PrincipalId FROM cam.UserAccount WHERE EmailAddress = a.EmailAddress) As PrincipalId,
	a.OrgId as OrgId,
	a.Username as Username
FROM @sevisUserAccount a;

--Delete sevisUserAccounts that are not found/null.  This may happen when the user has not yet registered in this environment.
DELETE @principalSevisAccount WHERE PrincipalId = 0 OR PrincipalId IS NULL;

DELETE FROM cam.SevisAccount WHERE NOT EXISTS (SELECT * FROM @principalSevisAccount psa WHERE cam.SevisAccount.PrincipalId = psa.PrincipalId)

--Upsert sevis user accounts
BEGIN
	MERGE cam.SevisAccount AS target
	USING @principalSevisAccount AS source
	ON target.PrincipalId = source.PrincipalId
	WHEN MATCHED THEN
		UPDATE SET 
			OrgId = source.OrgId,
			Username = source.Username

	WHEN NOT MATCHED THEN
		INSERT (PrincipalId, OrgId, Username)
		VALUES(source.PrincipalId, source.OrgId, source.Username);
END
GO