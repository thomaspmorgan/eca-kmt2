DECLARE @officeResourceTypeName VARCHAR(25) = 'office';
DECLARE @programResourceTypeName VARCHAR(25) = 'program';
DECLARE @projectResourceTypeName VARCHAR(25) = 'project';


--insert office resources
INSERT into CAM.Resource 
(ForeignResourceId, ParentResourceId, ResourceTypeId)
SELECT
OrganizationId as ForeignResourceId,
null as ParentResourceId,
(SELECT ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @officeResourceTypeName) as ResourceTypeId

FROM Organization
WHERE OrganizationId not in (
	SELECT ForeignResourceId 
	FROM CAM.Resource 
	WHERE ResourceTypeId = (SELECT ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @officeResourceTypeName)
) AND OrganizationTypeId in (1,2,3)
PRINT 'Inserted ' + CAST(@@RowCount AS VARCHAR(10)) + ' office cam resources.';


--insert program resources
INSERT into CAM.Resource 
(ForeignResourceId, ParentResourceId, ResourceTypeId)
SELECT 
programid as ForeignResourceId,
(SELECT ResourceId FROM CAM.Resource 
	WHERE ForeignResourceId = Program.Owner_OrganizationId 
	AND ResourceTypeId = (SELECT ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @officeResourceTypeName)) as ParentResourceId,
(SELECT ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @programResourceTypeName) as ResourceTypeId

FROM program
WHERE programid not in (
	SELECT ForeignResourceId 
	FROM CAM.Resource 
	WHERE ResourceTypeId = (SELECT ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @programResourceTypeName)
)
PRINT 'Inserted ' + CAST(@@RowCount AS VARCHAR(10)) + ' program cam resources.';


--insert project resources
INSERT into CAM.Resource 
(ForeignResourceId, ParentResourceId, ResourceTypeId)
SELECT 
projectid as ForeignResourceId,
(SELECT ResourceId FROM CAM.Resource 
	WHERE ForeignResourceId = project.programid
	AND ResourceTypeId = (SELECT ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @programResourceTypeName)) as ParentResourceId,
(SELECT ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @projectResourceTypeName) as ResourceTypeId

FROM Project
WHERE ProjectId not in (
	SELECT ForeignResourceId 
	FROM CAM.Resource 
	WHERE ResourceTypeId = (SELECT ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @projectResourceTypeName)
)
PRINT 'Inserted ' + CAST(@@RowCount AS VARCHAR(10)) + ' project cam resources.';