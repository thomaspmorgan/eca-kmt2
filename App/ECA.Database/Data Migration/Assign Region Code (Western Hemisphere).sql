begin tran t1
update location set region_locationid = 6 where locationname in ( 
 
'Antigua and Barbuda'
 ,
'Argentina'
 ,
'Aruba'
 ,
'Bahamas, The'
 ,
'Barbados'
 ,
'Belize'
 ,
'Bermuda'
 ,
'Bolivia'
 ,
'Brazil'
 ,
'Canada'
 ,
'Cayman Islands'
 ,
'Chile'
 ,
'Colombia'
 ,
'Costa Rica'
 ,
'Cuba'
 ,
'Curacao'
 ,
'Dominica'
 ,
'Dominican Republic'
 ,
'Ecuador'
 ,
'El Salvador'
 ,
'Grenada'
 ,
'Guatemala'
 ,
'Guyana'
 ,
'Haiti'
 ,
'Honduras'
 ,
'Jamaica'
 ,
'Mexico'
 ,
'Nicaragua'
 ,
'Panama'
 ,
'Paraguay'
 ,
'Peru'
 ,
'Saint Kitts and Nevis'
 ,
'Saint Lucia'
 ,
'St. Maarten'
 ,
'Saint Vincent and the Grenadines'
 ,
'Suriname'
 ,
'Trinidad and Tobago'
 ,
'Uruguay'
 ,
'Venezuela'



)
commit tran t1
 
