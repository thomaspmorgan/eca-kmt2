﻿DECLARE @officeResourceTypeName VARCHAR(25) = 'office';
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



--update existing program parent resources
UPDATE
r
SET 
r.ParentResourceId = pr.ResourceId

FROM cam.Resource r

JOIN Program prog
on r.ForeignResourceId = prog.ProgramId AND r.ResourceTypeId = @programResourceTypeId

JOIN Organization o
ON prog.Owner_OrganizationId = o.OrganizationId

JOIN cam.Resource pr
ON o.OrganizationId = pr.ForeignResourceId AND pr.ResourceTypeId = @officeResourceTypeId

WHERE r.ParentResourceId <> pr.ResourceId
PRINT 'Updated ' + CAST(@@RowCount AS VARCHAR(10)) + ' program parent resources.';


--update existing project parent resources
UPDATE
r
SET 
r.ParentResourceId = pr.ResourceId

FROM cam.Resource r

JOIN Project proj
on r.ForeignResourceId = proj.ProjectId AND r.ResourceTypeId = @projectResourceTypeId

JOIN Program prog
ON proj.ProgramId = prog.ProgramId

JOIN cam.Resource pr
ON prog.ProgramId = pr.ForeignResourceId AND pr.ResourceTypeId = @programResourceTypeId
WHERE r.ParentResourceId <> pr.ResourceId
PRINT 'Updated ' + CAST(@@RowCount AS VARCHAR(10)) + ' project parent resources.';

GO