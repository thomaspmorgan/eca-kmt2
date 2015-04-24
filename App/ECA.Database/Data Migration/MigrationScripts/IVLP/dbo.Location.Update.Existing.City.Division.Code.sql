/* Updates Division (State) data on Location table using IVLP Itinerary city data */

/* Query to see what should be updated - just for checking */
--select is1.city,is1.state_cd,l.* 
--from ivlp_itinerary_stop is1
--left join ECA_Dev_local_copy.dbo.location l ON (l.locationtypeid = 5 AND l.locationname = is1.city)
--order by is1.city

/* now do the update */
UPDATE ECA_Dev.ECA_Dev.dbo.location 
SET division = i.state_cd
FROM (SELECT city,state_cd FROM ivlp_itinerary_stop) i
WHERE locationtypeid = 5 AND locationname = i.city

GO