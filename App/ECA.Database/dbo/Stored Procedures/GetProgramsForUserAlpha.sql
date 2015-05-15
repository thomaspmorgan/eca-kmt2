CREATE PROCEDURE GetProgramsForUserAlpha
(@UserId int)
AS

select * from program 
where programstatusid = 1
OR programStatusID = 4 and History_CreatedBy = @userID
order by name
