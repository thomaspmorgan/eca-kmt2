-- =============================================
-- Author:		Marc Setien
-- Create date: 5/10/2016
-- Description:	Return a text value with all diacritics replaced.
-- =============================================
CREATE Function [dbo].[NoDiacritics](@string VARCHAR(2000))
RETURNS VARCHAR(2000)
AS
BEGIN
	RETURN CAST (@string AS varchar(2000)) Collate SQL_Latin1_General_CP1253_CI_AI 
END