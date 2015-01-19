/* creates test Region data for Location Table  */

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
        (LocationTypeId,History_createdby,history_createdon,history_revisedby,history_revisedon,
        locationname,locationiso,city) 
values 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Afghanistan','AF','Kabul'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Albania','AL','Tirana'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Algeria ','AG','Algiers'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Andorra ','AN','Andorra la Vella'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Angola ','AO','Luanda'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Antigua and Barbuda','AC','Saint John''s'),
		
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Argentina','AR','Buenos Aires'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Armenia','AM','Yerevan'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Australia','AS','Canberra'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Austria','AU','Vienna'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Azerbaijan','AJ','Baku'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Bahamas, The','BF','Nassau'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Bahrain','BA','Manama'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Bangladesh','BG','Dhaka'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Barbados','BB','Bridgetown'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Belarus','BO','Minsk'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Belgium','BE','Brussels'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Belize','BH','Belmopan'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Benin','BN','Porto-Novo'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Bhutan','BT','Thimphu'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Bolivia','BL','La Paz (administrative) Sucre (legislative/judiciary)'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Bosnia and Herzegovina','BK','Sarajevo'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Botswana','BC','Gaborone'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Brazil','BR','Brasília'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Brunei','BX','Bandar Seri Begawan'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Bulgaria','BU','Sofia'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Burkina Faso','UV','Ouagadougou'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Burma','BM','Rangoon Nay Pyi Taw (administrative)'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Burundi','BY','Bujumbura'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Cabo Verde','CV','Praia'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Cambodia','CB','Phnom Penh'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Cameroon','CM','Yaoundé'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Canada','CA','Ottawa'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Central African Republic','CT','Bangui'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Chad','CD','N''Djamena'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Chile','CI','Santiago'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'China','CH','Beijing'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Colombia','CO','Bogotá'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Comoros','CN','Moroni'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Congo, Rebublic of the','CF','Brazzaville'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Congo, Democratic Rebublic of the','CG','Kinshasa'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Costa Rica','CS','San José'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Côte d''Ivoire','IV','Yamoussoukro'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Croatia','HR','Zagreb'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Cuba','CU','Havana'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Cyprus','CY','Nicosia'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Czech Republic','EZ','Prague'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Denmark','DA','Copenhagen'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Djibouti','DJ','Djibouti'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Dominica','DO','Roseau'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Dominican Republic','DR','Santo Domingo'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Ecuador','EC','Quito'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Egypt','EG','Cairo'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'El Salvador','ES','San Salvador'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Equatorial Guinea','EK','Malabo'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Eritrea','ER','Asmara'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Estonia','EN','Tallinn'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Ethiopia','ET','Addis Ababa'),
		
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Fiji','FJ','Suva'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Finland','FI','Helsinki'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'France','FR','Paris'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Gabon','GB','Libreville'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Gambia, The','GA','Banjul'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Georgia','GG','Tbilisi'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Germany','GM','Berlin'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Ghana','GH','Accra'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Greece','GR','Athens'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Grenada','GJ','Saint George''s'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Guatemala','GT','Guatemala'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Guinea','GV','Conakry'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Guinea-Bissau','PU','Bissau'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Guyana','GY','Georgetown'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Haiti','HA','Port-au-Prince'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Holy See','VT','Vatican City'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Honduras','HO','Tegucigalpa'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Hungary','HU','Budapest'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Iceland','IC','Reykjavík'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'India','IN','New Delhi'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Indonesia','ID','Jakarta'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Iran','IR','Tehran'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Iraq','IZ','Baghdad'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Ireland','EI','Dublin'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Israel','IS','Jerusalem (see note 5)'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Italy','IT','Rome'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Jamaica','JM','Kingston'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Japan','JA','Tokyo'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Jordan','JO','Amman'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Kazakhstan','KZ','Astana'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Kenya','KE','Nairobi'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Kiribati','KR','Tarawa'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Korea, North','KN','Pyongyang'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Korea, South','KS','Seoul'),
        
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Kosovo','KV','Pristina'),
        
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Kuwait','KU','Kuwait'),
        
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Kyrgyzstan','KG','Bishkek'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Laos','LA','Vientiane'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Latvia','LG','Riga'),
         
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Lebanon','LE','Beirut'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Lesotho','LT','Maseru'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Liberia','LI','Monrovia'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Libya','LY','Tripoli'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Liechtenstein','LS','Vaduz'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Lithuania','LH','Vilnius'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Luxembourg','LU','Luxembourg'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Macedonia','MK','Skopje'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Madagascar','MA','Antananarivo'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Malaw','MI','Lilongwe'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Malaysia','MY','Kuala Lumpur'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Maldives','MV','Male'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Mali','ML','Bamako'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Malta','MT','Valletta'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Marshall Islands','RM','Majuro'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Mauritania','MR','Nouakchott'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Mauritius','MP','Port Louis'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Mexico','MX','Mexico'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Micronesia, Federated States of','FM','Palikir'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Moldova','MD','Chisinau'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Monaco','MN','Monaco'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Mongolia','MG','Ulaanbaatar'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Montenegro','MJ','Podgorica'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Morocco','MO','Rabat'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Mozambique','MZ','Maputo'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Namibia','WA','Windhoek'),
  
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Nauru','NR','Yaren District (no capital city)'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Nepal','NP','Kathmandu'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Netherlands','NL','Amsterdam The Hague (seat of gov''t)'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'New Zealand','NZ','Wellington'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Nicaragua','NU','Managua'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Niger','NG','Niamey'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Nigeria','NI','Abuja'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Norway','NO','Oslo'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Oman','MU','Muscat'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Pakistan','PK','Islamabad'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Palau','PS','Melekeok'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Panama','PM','Panama'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Papua New Guinea','PP','Port Moresby'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Paraguay','PA','Asunción'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Peru','PE','Lima'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Philippines','RP','Manila'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Poland','PL','Warsaw'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Portugal','PO','Lisbon'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Qatar','QA','Doha'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Romania','RO','Bucharest'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Russia','RS','Moscow'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Rwanda','RW','Kigali'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Saint Kitts and Nevis','SC','Basseterre'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Saint Lucia','ST','Castries'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Saint Vincent and the Grenadines','VC','Kingstown'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Samoa','WS','Apia'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'San Marino','SM','San Marino'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Sao Tome and Principe','TP','São Tomé'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Saudi Arabia','SA','Riyadh'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Senegal','SG','Dakar'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Serbia','RI','Belgrade'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Seychelles','SE','Victoria'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Sierra Leone','SL','Freetown'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Singapore','SN','Singapore'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Slovakia','LO','Bratislava'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Slovenia','SI','Ljubljana'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Solomon Islands','BP','Honiara'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Somalia','SO','Mogadishu'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'South Africa','SF','Pretoria (administrative) Cape Town (legislative) Bloemfontein (judiciary)'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'South Sudan','OD','Juba'),
  
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Spain','SP','Madrid'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Sri Lanka','CE','Colombo Sri Jayewardenepura Kotte (legislative)'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Sudan','SU','Khartoum'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Suriname','NS','Paramaribo'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Swaziland','WZ','Mbabane (administrative) Lobamba (legislative)'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Sweden','SW','Stockholm'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Switzerland','SZ','Bern'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Syria','SY','Damascus'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Tajikistan','TI','Dushanbe'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Tanzania','TZ','Dar es Salaam Dodoma (legislative)'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Thailand','TH','Bangkok'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Timor-Leste','TT','Dili'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Togo','TO','Lomé'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Tonga','TN','Nuku''alofa'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Trinidad and Tobago','TD','Port of Spain'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Tunisia','TS','Tunis'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Turkey','TU','Ankara'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Turkmenistan','TX','Ashgabat'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Tuvalu','TV','Funafuti'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Uganda','UG','Kampala'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Ukraine','UP','Kyiv'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'United Arab Emirates','AE','Abu Dhabi'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'United Kingdom','UK','London'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'United States','US','Washington, DC'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Uruguay','UY','Montevideo'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Uzbekistan','UZ','Tashkent'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Vanuatu','NH','Port-Vila'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Venezuela','VE','Caracas'),
   
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Vietnam','VM','Hanoi'),
  
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Yemen','YM','Sanaa'),

 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Zambia','ZA','Lusaka'),
   
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),        'Zimbabwe','ZI','Harare'),
 
 (@countryLocationTypeID,0,sysdatetimeoffset(),0,sysdatetimeoffset(),         'Taiwan','TW','Taipei')




commit tran t1
GO

