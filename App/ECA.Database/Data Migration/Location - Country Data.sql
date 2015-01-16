/* creates test Region data for Location Table  */
GO
begin tran t1

DECLARE @countryLocationTypeID int = 0; 

/* get the correct ID for the region location type */ 
SELECT @countryLocationTypeID = locationtypeid 
  FROM locationtype 
 WHERE locationtypename = 'Country'

/* output to be sure */
SELECT @countryLocationTypeID AS 'Country Location Type ID'

/* insert the test data */
insert into dbo.location 
        (LocationType_LocationTypeId,latitude,longitude,History_createdby,history_createdon,history_revisedby,history_revisedon,
        locationname,locationiso,city) 
values 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Afghanistan','AF','Kabul'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Albania','AL','Tirana'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Algeria ','AG','Algiers'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Andorra ','AN','Andorra la Vella'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Angola ','AO','Luanda'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Antigua and Barbuda','AC','Saint John''s'),
		
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Argentina','AR','Buenos Aires'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Armenia','AM','Yerevan'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Australia','AS','Canberra'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Austria','AU','Vienna'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Azerbaijan','AJ','Baku'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Bahamas, The','BF','Nassau'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Bahrain','BA','Manama'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Bangladesh','BG','Dhaka'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Barbados','BB','Bridgetown'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Belarus','BO','Minsk'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Belgium','BE','Brussels'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Belize','BH','Belmopan'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Benin','BN','Porto-Novo'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Bhutan','BT','Thimphu'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Bolivia','BL','La Paz (administrative) Sucre (legislative/judiciary)'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Bosnia and Herzegovina','BK','	Sarajevo'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Botswana','BC','Gaborone'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Brazil','BR','Brasília'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Brunei','BX','Bandar Seri Begawan'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Bulgaria','BU','Sofia'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Burkina Faso','UV','Ouagadougou'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Burma','BM','Rangoon Nay Pyi Taw (administrative)'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Burundi','BY','Bujumbura'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Cabo Verde','CV','Praia'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Cambodia','CB','Phnom Penh'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Cameroon','CM','Yaoundé'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Canada','CA','Ottawa'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Central African Republic','CT','Bangui'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Chad','CD','N''Djamena'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Chile','CI','Santiago'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'China','CH','Beijing'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Colombia','CO','Bogotá'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Comoros','CN','Moroni'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Congo, Rebublic of the','CF','Brazzaville'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Congo, Democratic Rebublic of the','CG','Kinshasa'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Costa Rica','CS','San José'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Côte d''Ivoire','IV','Yamoussoukro'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Croatia','HR','Zagreb'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Cuba','CU','Havana'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Cyprus','CY','Nicosia'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Czech Republic','EZ','Prague'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Denmark','DA','Copenhagen'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Djibouti','DJ','Djibouti'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Dominica','DO','Roseau'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Dominican Republic','DR','Santo Domingo'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Ecuador','EC','Quito'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Egypt','EG','Cairo'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'El Salvador','ES','San Salvador'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Equatorial Guinea','EK','Malabo'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Eritrea','ER','Asmara'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Estonia','EN','Tallinn'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Ethiopia','ET','Addis Ababa'),
		
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Fiji','FJ','Suva'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Finland','FI','Helsinki'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'France','FR','Paris'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Gabon','GB','Libreville'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Gambia, The','GA','Banjul'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Georgia','GG','Tbilisi'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Germany','GM','Berlin'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Ghana','GH','Accra'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Greece','GR','Athens'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Grenada','GJ','Saint George''s'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Guatemala','GT','Guatemala'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Guinea','GV','Conakry'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Guinea-Bissau','PU','Bissau'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Guyana','GY','Georgetown'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Haiti','HA','Port-au-Prince'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Holy See','VT','Vatican City'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Honduras','HO','Tegucigalpa'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Hungary','HU','Budapest'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Iceland','IC','Reykjavík'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'India','IN','New Delhi'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Indonesia','ID','Jakarta'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Iran','IR','Tehran'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Iraq','IZ','Baghdad'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Ireland','EI','Dublin'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Israel','IS','Jerusalem (see note 5)'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Italy','IT','Rome'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Jamaica','JM','Kingston'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Japan','JA','Tokyo'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Jordan','JO','Amman'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Kazakhstan','KZ','Astana'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Kenya','KE','Nairobi'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Kiribati','KR','Tarawa'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Korea, North','KN','Pyongyang'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Korea, South','KS','Seoul'),
        
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Kosovo','KV','Pristina'),
        
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Kuwait','KU','Kuwait'),
        
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Kyrgyzstan','KG','Bishkek'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Laos','LA','Vientiane'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Latvia','LG','Riga'),
         
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Lebanon','LE','Beirut'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Lesotho','LT','Maseru'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Liberia','LI','Monrovia'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Libya','LY','Tripoli'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Liechtenstein','LS','Vaduz'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Lithuania','LH','Vilnius'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Luxembourg','LU','Luxembourg'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Macedonia','MK','Skopje'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Madagascar','MA','Antananarivo'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Malaw','MI','Lilongwe'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Malaysia','MY','Kuala Lumpur'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Maldives','MV','Male'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Mali','ML','Bamako'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Malta','MT','Valletta'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Marshall Islands','RM','Majuro'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Mauritania','MR','Nouakchott'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Mauritius','MP','Port Louis'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Mexico','MX','Mexico'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Micronesia, Federated States of','FM','Palikir'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Moldova','MD','Chisinau'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Monaco','MN','Monaco'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Mongolia','MG','Ulaanbaatar'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Montenegro','MJ','Podgorica'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Morocco','MO','Rabat'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Mozambique','MZ','Maputo'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Namibia','WA','Windhoek'),
  
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Nauru','NR','Yaren District (no capital city)'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Nepal','NP','Kathmandu'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Netherlands','NL','Amsterdam The Hague (seat of gov''t)'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'New Zealand','NZ','Wellington'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Nicaragua','NU','Managua'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Niger','NG','Niamey'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Nigeria','NI','Abuja'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Norway','NO','Oslo'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Oman','MU','Muscat'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Pakistan','PK','Islamabad'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Palau','PS','Melekeok'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Panama','PM','Panama'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Papua New Guinea','PP','Port Moresby'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Paraguay','PA','Asunción'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Peru','PE','Lima'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Philippines','RP','Manila'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Poland','PL','Warsaw'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Portugal','PO','Lisbon'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Qatar','QA','Doha'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Romania','RO','Bucharest'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Russia','RS','Moscow'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Rwanda','RW','Kigali'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Saint Kitts and Nevis','SC','Basseterre'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Saint Lucia','ST','Castries'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Saint Vincent and the Grenadines','VC','Kingstown'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Samoa','WS','Apia'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'San Marino','SM','San Marino'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Sao Tome and Principe','TP','São Tomé'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Saudi Arabia','SA','Riyadh'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Senegal','SG','Dakar'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Serbia','RI','Belgrade'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Seychelles','SE','Victoria'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Sierra Leone','SL','Freetown'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Singapore','SN','Singapore'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Slovakia','LO','Bratislava'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Slovenia','SI','Ljubljana'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Solomon Islands','BP','Honiara'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Somalia','SO','Mogadishu'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'South Africa','SF','Pretoria (administrative) Cape Town (legislative) Bloemfontein (judiciary)'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'South Sudan','OD','Juba'),
  
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Spain','SP','Madrid'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Sri Lanka','CE','Colombo Sri Jayewardenepura Kotte (legislative)'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Sudan','SU','Khartoum'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Suriname','NS','Paramaribo'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Swaziland','WZ','Mbabane (administrative) Lobamba (legislative)'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Sweden','SW','Stockholm'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Switzerland','SZ','Bern'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Syria','SY','Damascus'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Tajikistan','TI','Dushanbe'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Tanzania','TZ','Dar es Salaam Dodoma (legislative)'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Thailand','TH','Bangkok'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Timor-Leste','TT','Dili'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Togo','TO','Lomé'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Tonga','TN','Nuku''alofa'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Trinidad and Tobago','TD','Port of Spain'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Tunisia','TS','Tunis'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Turkey','TU','Ankara'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Turkmenistan','TX','Ashgabat'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Tuvalu','TV','Funafuti'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Uganda','UG','Kampala'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Ukraine','UP','Kyiv'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'United Arab Emirates','AE','Abu Dhabi'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'United Kingdom','UK','London'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'United States','US','Washington, DC'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Uruguay','UY','Montevideo'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Uzbekistan','UZ','Tashkent'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Vanuatu','NH','Port-Vila'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Venezuela','VE','Caracas'),
   
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Vietnam','VM','Hanoi'),
  
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Yemen','YM','Sanaa'),

 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Zambia','ZA','Lusaka'),
   
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),        'Zimbabwe','ZI','Harare'),
 
 (@countryLocationTypeID,0,0,0,getdate(),0,getdate(),         'Taiwan','TW','Taipei')




commit tran t1
END

