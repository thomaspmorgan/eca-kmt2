begin tran t1
update location set region_locationid = 2 where locationname in ( 
 

'Australia'
,
'Brunei'
, 
'Burma'
, 
'Cambodia'
, 
'China'
, 
'Fiji'
, 
'Indonesia'
, 
'Japan'
, 
'Kiribati'
, 
'Korea, North'
, 
'Korea, South'
, 
'Laos'
, 
'Malaysia'
, 
'Marshall Islands'
, 
'Micronesia, Federated States of'
, 
'Mongolia'
, 
'Nauru'
, 
'New Zealand'
, 
'Palau'
, 
'Papua New Guinea'
, 
'Philippines'
, 
'Samoa'
, 
'Singapore'
, 
'Solomon Islands'
, 
'Taiwan'
, 
'Thailand'
, 
'Timor-Leste'
, 
'Tonga'
, 
'Tuvalu'
, 
'Vanuatu'
, 
'Vietnam'
)
commit tran t1
 
