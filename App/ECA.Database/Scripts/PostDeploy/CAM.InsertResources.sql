DECLARE @officeResourceTypeName VARCHAR(25) = 'office';
DECLARE @programResourceTypeName VARCHAR(25) = 'program';
DECLARE @projectResourceTypeName VARCHAR(25) = 'project';

DECLARE @officeResourceTypeId INT;
DECLARE @programResourceTypeId INT;
DECLARE @projectResourceTypeId INT;

SELECT @officeResourceTypeId = ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @officeResourceTypeName
SELECT @programResourceTypeId = ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @programResourceTypeName
SELECT @projectResourceTypeId = ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @projectResourceTypeName

--insert office resources
INSERT into CAM.Resource 
(ForeignResourceId, ParentResourceId, ResourceTypeId)
SELECT
OrganizationId as ForeignResourceId,
null as ParentResourceId,
(@officeResourceTypeId) as ResourceTypeId

FROM Organization
WHERE OrganizationId not in (
	SELECT ForeignResourceId 
	FROM CAM.Resource 
	WHERE ResourceTypeId = (@officeResourceTypeId)
) AND OrganizationTypeId in (1,2,3)
PRINT 'Inserted ' + CAST(@@RowCount AS VARCHAR(10)) + ' office cam resources.';


--insert program resources
INSERT into CAM.Resource 
(ForeignResourceId, ParentResourceId, ResourceTypeId)
SELECT 
programid as ForeignResourceId,
(SELECT ResourceId FROM CAM.Resource 
	WHERE ForeignResourceId = Program.Owner_OrganizationId 
	AND ResourceTypeId = (@officeResourceTypeId)) as ParentResourceId,
(@programResourceTypeId) as ResourceTypeId

FROM program
WHERE programid not in (
	SELECT ForeignResourceId 
	FROM CAM.Resource 
	WHERE ResourceTypeId = (@programResourceTypeId)
)
PRINT 'Inserted ' + CAST(@@RowCount AS VARCHAR(10)) + ' program cam resources.';


--insert project resources
INSERT into CAM.Resource 
(ForeignResourceId, ParentResourceId, ResourceTypeId)
SELECT 
projectid as ForeignResourceId,
(SELECT ResourceId FROM CAM.Resource 
	WHERE ForeignResourceId = project.programid
	AND ResourceTypeId = (@programResourceTypeId)) as ParentResourceId,
(SELECT ResourceTypeId FROM CAM.ResourceType WHERE ResourceTypeName = @projectResourceTypeName) as ResourceTypeId

FROM Project
WHERE ProjectId not in (
	SELECT ForeignResourceId 
	FROM CAM.Resource 
	WHERE ResourceTypeId = (@projectResourceTypeId)
)
PRINT 'Inserted ' + CAST(@@RowCount AS VARCHAR(10)) + ' project cam resources.';

GO