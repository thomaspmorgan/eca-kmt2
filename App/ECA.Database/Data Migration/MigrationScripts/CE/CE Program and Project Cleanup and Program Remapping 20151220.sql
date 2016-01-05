--select * from program where programid in (
--update program set programstatusid = 6 where programid in (
--select * from program where parentprogram_programid in (
--delete from program where parentprogram_programid in (
--select * from project where programid in (
--delete from project where programid in (
--delete from project where programid in (
--select * from program where parentprogram_programid in (
--delete from program where parentprogram_programid in (
--select programid from program where parentprogram_programid in (
--select * from program where parentprogram_programid in (
--delete from program where parentprogram_programid IN (
--select programid from program where programid IN (
select * from program where programid in (
--delete from program where programid in (

160,1046,161,156,157,162,154,155,163,151,150,164,159,1052,1054,1055


)
)
)

select * from program where programid = 1082
select * from program where programid = 1128
select * from project where projectid = 1521
update project set programid = 1153 where projectid = 1521

select * from program where programid = 1038
update program set parentprogram_programid = null where programid = 1038


/* Professional Fellows */
/* determine what is to be deleted */
select * from program where programid in (160,1046,161,156,157,162,154,155,163,151,150,164,159/*,1052,1054,1055*/)
select * from project where programid in (160,1046,161,156,157,162,154,155,163,151,150,164,159/*,1052,1054,1055*/)
select * from program where parentprogram_programid in (select programid from program where programid in (160,1046,161,156,157,162,154,155,163,151,150,164,159/*,1052,1054,1055*/))
select * from project where programid IN (select programid from program where parentprogram_programid in (select programid from program where programid in (160,1046,161,156,157,162,154,155,163,151,150,164,159/*,1052,1054,1055*/)))

/* delete Extraneous PF Programs and Projects*/
delete from project where programid IN (select programid from program where parentprogram_programid in (select programid from program where programid in (160,1046,161,156,157,162,154,155,163,151,150,164,159/*,1052,1054,1055*/)))
delete from program where parentprogram_programid in (select programid from program where programid in (160,1046,161,156,157,162,154,155,163,151,150,164,159/*,1052,1054,1055*/))
delete from project where programid in (160,1046,161,156,157,162,154,155,163,151,150,164,159/*,1052,1054,1055*/)
delete from program where programid in (160,1046,161,156,157,162,154,155,163,151,150,164,159/*,1052,1054,1055*/)


/* Youth  - PY */
select * from program where programid in (136,3,149,148,81,1081,1075,1076,10,9)
select * from project where programid in (136,3,149,148,81,1081,1075,1076,10,9)
select * from program where parentprogram_programid in (select programid from program where programid in (136,3,149,148,81,1081,1075,1076,10,9))
select * from project where programid IN (select programid from program where parentprogram_programid in (select programid from program where programid in (136,3,149,148,81,1081,1075,1076,10,9)))

/* Delete extraneous programs and projects */
delete from project where programid IN (select programid from program where parentprogram_programid in (select programid from program where programid in (136,3,149,148,81,1081,1075,1076,10,9)))
delete from program where parentprogram_programid in (select programid from program where programid in (136,3,149,148,81,1081,1075,1076,10,9))
delete from project where programid in (136,3,149,148,81,1081,1075,1076,10,9)
delete from program where programid in (136,3,149,148,81,1081,1075,1076,10,9)

/* Sports - SU */
select * from program where programid in (1053)
select * from project where programid in (1053)
select * from program where parentprogram_programid in (select programid from program where programid in (1053))
select * from project where programid IN (select programid from program where parentprogram_programid in (select programid from program where programid in (1053)))

/* Delete extraneous programs and projects */
delete from project where programid IN (select programid from program where parentprogram_programid in (select programid from program where programid in (1053)))
delete from program where parentprogram_programid in (select programid from program where programid in (1053))
delete from project where programid in (1053)
delete from program where programid in (1053)

/* Culture - CU */
select * from program where programid in (1043,1051)
select * from project where programid in (1043,1051)
select * from program where parentprogram_programid in (select programid from program where programid in (1043,1051))
select * from project where programid IN (select programid from program where parentprogram_programid in (select programid from program where programid in (1043,1051)))

/* Delete extraneous programs and projects */
delete from project where programid IN (select programid from program where parentprogram_programid in (select programid from program where programid in (1043,1051)))
delete from program where parentprogram_programid in (select programid from program where programid in (1043,1051))
delete from project where programid in (1043,1051)
delete from program where programid in (1043,1051)

/* Added American Music Abroad FY14 Project */
     INSERT 
      INTO dbo.program
          (ProgramStatusId,Name,Description,StartDate,EndDate,
           History_CreatedBy,History_CreatedOn,History_RevisedBy,History_RevisedOn,
           ParentProgram_ProgramId,Owner_OrganizationId) 
      VALUES (1,'American Music Abroad FY14','Showcases music from a wide variety of traditional American Genres including jazz, blues, zydeco, hip hop, bluegrass, country, and roots music during ten multi-country tours annually to 40-50 countries.  American ensembles of 3-to-5 members are selected via an open nation-wide application and audition process for their artistic excellence and ability to connect with audiences, teach, and share musical traditions.  Annual AMA countries are selected via consultation with regional public diplomacy bureaus. This is administered via a public-private partnership.?'
,CAST(N'2015-12-20T00:00:00.0000000-00:00' AS DateTimeOffset),
              NULL,1,CAST(N'2015-12-20T00:00:00.0000000-00:00' AS DateTimeOffset),
              1,CAST(N'2015-09-17T00:00:00.0000000-00:00' AS DateTimeOffset),1298,45)

select top 1 * from program order by programid desc

/* Reassign projects */
UPDATE project set programid = 1308 where projectid = 1565
UPDATE project set programid = 1341 where projectid = 1566


/* ECA/PE/C */

select * from program where programid in (74,96,93,91,98,100,101,89,99,97,92,8,175,173,172,174,72,187,188,189)
select * from project where programid in (74,96,93,91,98,100,101,89,99,97,92,8,175,173,172,174,72,187,188,189)
select * from program where parentprogram_programid in (select programid from program where programid in (74,96,93,91,98,100,101,89,99,97,92,8,175,173,172,174,72,187,188,189))
select * from project where programid IN (select programid from program where parentprogram_programid in (select programid from program where programid in (74,96,93,91,98,100,101,89,99,97,92,8,175,173,172,174,72,187,188,189)))

/* Delete extraneous programs and projects */
delete from project where programid IN (select programid from program where parentprogram_programid in (select programid from program where programid in (74,96,93,91,98,100,101,89,99,97,92,8,175,173,172,174,72,187,188,189)))
delete from program where parentprogram_programid in (select programid from program where programid in (74,96,93,91,98,100,101,89,99,97,92,8,175,173,172,174,72,187,188,189))
delete from project where programid in (74,96,93,91,98,100,101,89,99,97,92,8,175,173,172,174,72,187,188,189)
delete from program where programid in (74,96,93,91,98,100,101,89,99,97,92,8,175,173,172,174,72,187,188,189)

delete from moneyflow where recipientprogramid in (74,96,93,91,98,100,101,89,99,97,92,8,175,173,172,174,72,187,188,189)




