/****** Script for SelectTopNRows command from SSMS  ******/
SELECT p.*
,p2.*
  FROM [ECA_Local].[dbo].[Person] p
  
  left outer join [ECA_Local].[dbo].[Person] p2 on (p2.FullName = p.FullName and p2.personid <> p.personid)
  where p.personid > 63206
  order by p.personid desc


  select * from location where locationid in (2139,2087,2134,101447,101446,101448)
  select * from location where locationid in (150,199)

/* Create a list of current fulbright participants */
select p.personid,p2.participantid,
* 
from FulbrightDataset2_Import f
 join eca_kmt_qa.eca_kmt_qa.dbo.person p on (
(p.lastname is null and f.[Last Name] is null or p.lastname = f.[Last Name]) and
(p.firstname is null and f.[First Name] is null or p.firstname = f.[First Name]) and
(p.middlename is null and f.[Middle Name] is null or p.middlename = f.[Middle Name]) and
(p.nameprefix is null and f.Prefix is null or p.nameprefix = f.Prefix) and
(p.namesuffix is null and f.Suffix is null or p.namesuffix = f.Suffix) and
(p.dateofbirth is null and f.[Date of Birth] is null or convert(date,p.dateofbirth) = convert(date,f.[Date of Birth]))
)
join eca_kmt_qa.eca_kmt_qa.dbo.participant p2 on (p2.personid = p.personid)
order by category,subcategory



/* Remap participants to projects */
update eca_kmt_qa.eca_kmt_qa.dbo.participant set projectid = 7371 WHERE participantid in (1437,1440)
update eca_kmt_qa.eca_kmt_qa.dbo.participant set projectid = 7372 WHERE participantid in (1405,1389,1415)
update eca_kmt_qa.eca_kmt_qa.dbo.participant set projectid = 7373 WHERE projectid = 1521


 /* Some queries to help */
  select * from participant where personid in (614,617)

  select * from project where name like '%Fulbright%Lecturer%'

  update participant set projectid = 7368 where participantid in (1437,1440)

  select * from participant where personid in (566,592,582,620)

  select * from project where name like '%Fulbright%Lecturer%'

  update participant set projectid = 7369 where participantid in (1389,1405,1415,1443)

  select * from participant where personid in (606,63257,539,573,596,585,604,532,530,548,583,556,612,601,613,568,616,551,533,578,584,544,543,588,569,528,579,557,522,605,554,610,597,574,607,571,540,550,609,603,602,577,523,590,580,593,559,531,589,611,546,608,567,537,525,545,527,534,562,536,591,561,572,619
,542,552,581,555,541,594,529,587,570,595,600,63258,526,524,565,553,535,615,575,576,598,564,563,538,547,599,63259,586,549,558,618,560)

select * from project where name like '%Fulbright%Researcher%'

update participant set projectid = 7370 where participantid in (select participantid from participant where personid in (606,63257,539,573,596,585,604,532,530,548,583,556,612,601,613,568,616,551,533,578,584,544,543,588,569,528,579,557,522,605,554,610,597,574,607,571,540,550,609,603,602,577,523,590,580,593,559,531,589,611,546,608,567,537,525,545,527,534,562,536,591,561,572,619
,542,552,581,555,541,594,529,587,570,595,600,63258,526,524,565,553,535,615,575,576,598,564,563,538,547,599,63259,586,549,558,618,560) and projectid = 1521)