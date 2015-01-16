begin tran t1
update location set region_locationid = 4 where locationname in ( 
  
'Algeria'
, 
'Bahrain'
, 
'Egypt'
, 
'Iran'
, 
'Iraq'
, 
'Israel'
, 
'Jordan'
, 
'Kuwait'
, 
'Lebanon'
, 
'Libya'
, 
'Morocco'
, 
'Oman'
, 
'Palestinian Territories'
, 
'Qatar'
, 
'Saudi Arabia'
, 
'Syria'
, 
'Tunisia'
, 
'United Arab Emirates'
, 
'Yemen'


)
commit tran t1
 
