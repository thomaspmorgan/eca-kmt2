begin tran t1
update location set region_locationid = 5 where locationname in ( 
  

'Afghanistan'
 ,
'Bangladesh'
 ,
'Bhutan'
 ,
'India'
 ,
'Kazakhstan'
 ,
'Kyrgyzstan'
 ,
'Maldives'
 ,
'Nepal'
 ,
'Pakistan'
 ,
'Sri Lanka'
 ,
'Tajikistan'
 ,
'Turkmenistan'
 ,
'Uzbekistan'



)
commit tran t1
 
