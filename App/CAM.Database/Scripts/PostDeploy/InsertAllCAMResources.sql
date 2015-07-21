
DECLARE @officeName VARCHAR(10) = 'office'
DECLARE @programName VARCHAR(10) = 'program'
DECLARE @projectName VARCHAR(10) = 'project'

--INSERT office resources
INSERT INTO CAM.Resource 
(ForeignResourceId, ParentResourceId, ResourceTypeId)
SELECT
OrganizationId AS ForeignResourceId,
NULL AS ParentResourceId,
(SELECT ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @officeName) AS ResourceTypeId

FROM organization
WHERE organizationid not in (
	SELECT ForeignResourceId 
	FROM CAM.Resource 
	WHERE ResourceTypeId = (SELECT ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @officeName)
) AND organizationtypeid IN (1,2,3)


--INSERT program resources
INSERT INTO CAM.Resource 
(ForeignResourceId, ParentResourceId, ResourceTypeId)
SELECT 
programid AS ForeignResourceId,
(SELECT ResourceId FROM CAM.Resource 
	WHERE ForeignResourceId = Program.Owner_OrganizationId
	AND ResourceTypeId = (SELECT ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @officeName)) AS ParentResourceId,
(SELECT ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @programName) AS ResourceTypeId

FROM Program
WHERE ProgramId NOT IN (
	SELECT ForeignResourceId 
	FROM CAM.Resource 
	WHERE ResourceTypeId = (SELECT ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @programName)
)

--INSERT project resources
INSERT INTO CAM.Resource 
(ForeignResourceId, ParentResourceId, ResourceTypeId)
SELECT 
ProjectId AS ForeignResourceId,
(SELECT ResourceId FROM CAM.Resource 
	WHERE ForeignResourceId = project.programid
	AND resourcetypeID = (SELECT ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @programName)) AS ParentResourceId,
(SELECT ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @projectName) AS ResourceTypeId

FROM Project
WHERE ProjectId NOT IN (
	SELECT ForeignResourceId 
	FROM CAM.Resource 
	WHERE ResourceTypeId = (SELECT ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @projectName)
)
