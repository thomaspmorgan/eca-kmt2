DECLARE @sevisUsername VARCHAR(10) = 'esayya9302'
DECLARE @orgId VARCHAR(15) = 'P-1-19833'

DECLARE @thomasMorganAdGuid UNIQUEIDENTIFIER  = convert(uniqueidentifier, '178C83B3-1061-436E-89FE-59073312950F');
DECLARE @brianGibowskiAdGuid UNIQUEIDENTIFIER  = convert(uniqueidentifier, '86101ACC-10B0-4E10-AEEA-FFCC7B42BBC5');
DECLARE @brandonTuckerAdGuid UNIQUEIDENTIFIER  = convert(uniqueidentifier, 'E8FEAB56-B7EC-409A-BAF2-0F9F780027DD');

DECLARE @thomasMorganUserId INT = 0;
DECLARE @brianGibowskiUserId INT = 0;
DECLARE @brandonTuckerUserId INT = 0;

SELECT @thomasMorganUserId = PrincipalId FROM cam.UserAccount WHERE AdGuid = @thomasMorganAdGuid;
SELECT @brianGibowskiUserId = PrincipalId FROM cam.UserAccount WHERE AdGuid = @brianGibowskiAdGuid;
SELECT @brandonTuckerUserId = PrincipalId FROM cam.UserAccount WHERE AdGuid = @brandonTuckerAdGuid;

DECLARE @tblTempTable TABLE (
	[PrincipalId] int,
	[OrgId] nvarchar(15),
	[Username] nvarchar(10)
)

INSERT INTO @tblTempTable(PrincipalId, OrgId, Username) VALUES (@thomasMorganUserId, @orgId, @sevisUsername);
INSERT INTO @tblTempTable(PrincipalId, OrgId, Username) VALUES (@brianGibowskiUserId, @orgId, @sevisUsername);
INSERT INTO @tblTempTable(PrincipalId, OrgId, Username) VALUES (@brandonTuckerUserId, @orgId, @sevisUsername);
BEGIN
	MERGE cam.SevisAccount AS target
	USING @tblTempTable AS source
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