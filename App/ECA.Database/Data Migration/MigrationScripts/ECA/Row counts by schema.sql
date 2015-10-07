/* This will output row counts for all tables within a schema */
execute sp_MSForEachTable 'DECLARE @t AS VARCHAR(MAX); 
SELECT @t = CAST(COUNT(1) as VARCHAR(MAX)) 
+ CHAR(9) + CHAR(9) + ''?'' FROM ? ; PRINT @t'