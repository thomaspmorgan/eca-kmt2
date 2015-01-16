begin tran t1
update location set region_locationid = 3 where locationname in ( 
 
'Albania'
 ,
'Andorra'
 ,
'Armenia'
, 
'Austria'
 ,
'Azerbaijan'
, 
'Belarus'
, 
'Belgium'
, 
'Bosnia and Herzegovina'
 ,
'Bulgaria'
 ,
'Croatia'
 ,
'Cyprus'
, 
'Czech Republic'
 ,
'Denmark'
, 
'Estonia'
, 
'European Union'
, 
'Finland'
, 
'France'
 ,
'Georgia'
, 
'Germany'
 ,
'Greece'
, 
'Holy See'
 ,
'Hungary'
, 
'Iceland'
 ,
'Ireland'
, 
'Italy'
 ,
'Kosovo'
, 
'Latvia'
 ,
'Liechtenstein'
, 
'Lithuania'
 ,
'Luxembourg'
, 
'Macedonia'
 ,
'Malta'
, 
'Moldova'
 ,
'Monaco'
, 
'Montenegro'
 ,
'Netherlands'
 ,
'Norway'
 ,
'Poland'
 ,
'Portugal'
, 
'Romania'
 ,
'Russia'
, 
'San Marino'
 ,
'Serbia'
 ,
'Slovakia'
 ,
'Slovenia'
 ,
'Spain'
 ,
'Sweden'
 ,
'Switzerland'
 ,
'Turkey'
 ,
'Ukraine'
 ,
'United Kingdom'

)
commit tran t1
 
