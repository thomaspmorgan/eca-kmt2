/* Sports - SU */
select * from program where programid in (1190,1191,1189,1187,1188,1186,1194,1192,1193,1053)
select * from project where programid in (1190,1191,1189,1187,1188,1186,1194,1192,1193,1053)
select * from program where parentprogram_programid in (select programid from program where programid in (1190,1191,1189,1187,1188,1186,1194,1192,1193,1053))
select * from project where programid IN (select programid from program where parentprogram_programid in (select programid from program where programid in (1190,1191,1189,1187,1188,1186,1194,1192,1193,1053)))



/* Delete extraneous programs and projects */
delete from project where programid IN (select programid from program where parentprogram_programid in (select programid from program where programid in (1190,1191,1189,1187,1188,1186,1194,1192,1193)))
delete from program where parentprogram_programid in (select programid from program where programid in (1190,1191,1189,1187,1188,1186,1194,1192,1193))
delete from project where programid in (1190,1191,1189,1187,1188,1186,1194,1192,1193)
delete from program where programid in (1190,1191,1189,1187,1188,1186,1194,1192,1193)



(1171,1167,1168,1153,1179,1174,1172,1043,1169,1175,1176,1170,1173,1051,1178,1177)

/* Sports - SU */
select * from program where programid in (1171,1167,1168,1153,1179,1174,1172,1043,1169,1175,1176,1170,1173,1051,1178,1177)
select * from project where programid in (1171,1167,1168,1153,1179,1174,1172,1043,1169,1175,1176,1170,1173,1051,1178,1177,1048)
select * from program where parentprogram_programid in (select programid from program where programid in (1171,1167,1168,1153,1179,1174,1172,1043,1169,1175,1176,1170,1173,1051,1178,1177))
select * from project where programid IN (select programid from program where parentprogram_programid in (select programid from program where programid in (1171,1167,1168,1153,1179,1174,1172,1043,1169,1175,1176,1170,1173,1051,1178,1177)))

update program set programstatusid = 1 where programid = 1048
update project set programid = 1155 where projectid in (1565,1566)

/* Delete extraneous programs and projects */
delete from project where programid IN (select programid from program where parentprogram_programid in (select programid from program where programid in (1171,1167,1168,1153,1179,1174,1172,1043,1169,1175,1176,1170,1173,1051,1178,1177)))
delete from program where parentprogram_programid in (select programid from program where programid in (1171,1167,1168,1153,1179,1174,1172,1043,1169,1175,1176,1170,1173,1051,1178,1177))
delete from project where programid in (1171,1167,1168,1153,1179,1174,1172,1043,1169,1175,1176,1170,1173,1051,1178,1177)
delete from program where programid in (1171,1167,1168,1153,1179,1174,1172,1043,1169,1175,1176,1170,1173,1051,1178,1177)



/* PF */
select * from program where programid in (1109,1113,1114,1115,1112,1046,1106,1107,1110,1108,1052,1111,1054,1118,1117,1121,1119,1120,1116)
select * from project where programid in (1109,1113,1114,1115,1112,1046,1106,1107,1110,1108,1052,1111,1054,1118,1117,1121,1119,1120,1116)
select * from program where parentprogram_programid in (select programid from program where programid in (1109,1113,1114,1115,1112,1046,1106,1107,1110,1108,1052,1111,1054,1118,1117,1121,1119,1120,1116))
select * from project where programid IN (select programid from program where parentprogram_programid in (select programid from program where programid in (1109,1113,1114,1115,1112,1046,1106,1107,1110,1108,1052,1111,1054,1118,1117,1121,1119,1120,1116)))

update program set programstatusid = 1,name = 'Traditional Public Private Partnerships (TPPP)' where programid = 1055
update program set programstatusid = 1,name = 'TPPP: ACILS',parentprogram_programid = 1055 where programid = 1102
update program set programstatusid = 1,name = 'TPPP: American Council of Young Political Leaders (ACYPL)',parentprogram_programid = 1055 where programid = 1101
update program set programstatusid = 1,name = 'TPPP: Institute for Representative Government',parentprogram_programid = 1055 where programid = 1105
update program set programstatusid = 1,name = 'TPPP: Partners of the Americas',parentprogram_programid = 1055 where programid = 1103
update program set programstatusid = 1,name = 'TPPP: Sister Cities International',parentprogram_programid = 1055 where programid = 1104
update project set programid = 1086 where projectid in (1728,1729,1730)
update project set programid = 1105 where projectid in (1670)


/* Delete extraneous programs and projects */
delete from project where programid IN (select programid from program where parentprogram_programid in (select programid from program where programid in (1109,1113,1114,1115,1112,1046,1106,1107,1110,1108,1052,1111,1054,1118,1117,1121,1119,1120,1116)))
delete from program where parentprogram_programid in (select programid from program where programid in (1109,1113,1114,1115,1112,1046,1106,1107,1110,1108,1052,1111,1054,1118,1117,1121,1119,1120,1116))
delete from project where programid in (1109,1113,1114,1115,1112,1046,1106,1107,1110,1108,1052,1111,1054,1118,1117,1121,1119,1120,1116)
delete from program where programid in (1109,1113,1114,1115,1112,1046,1106,1107,1110,1108,1052,1111,1054,1118,1117,1121,1119,1120,1116)


/* PY */
select * from program where programid in (1132,1140,81,1143,10,1126,1145,1147,1146,1148,1122,1142,1134,1152,1150,1081,1138,1128,1149,1075,1076,1144,9,1141,1139,1151,136,3,1133,149,148,147)
select * from project where programid in (1132,1140,81,1143,10,1126,1145,1147,1146,1148,1122,1142,1134,1152,1150,1081,1138,1128,1149,1075,1076,1144,9,1141,1139,1151,136,3,1133,149,148,147)
select * from program where parentprogram_programid in (select programid from program where programid in (1132,1140,81,1143,10,1126,1145,1147,1146,1148,1122,1142,1134,1152,1150,1081,1138,1128,1149,1075,1076,1144,9,1141,1139,1151,136,3,1133,149,148,147))
select * from project where programid IN (select programid from program where parentprogram_programid in (select programid from program where programid in (1132,1140,81,1143,10,1126,1145,1147,1146,1148,1122,1142,1134,1152,1150,1081,1138,1128,1149,1075,1076,1144,9,1141,1139,1151,136,3,1133,149,148,147)))

select * from program where programid in (1126,1049)
select * from project where programid in (1126,1049)
select * from organization where organizationid = 1405
select * from program where programid in (1050,1122)
select * from project where programid in (1050,1122)
select * from organization where organizationid = 1405
select * from program where programid in (84,1134)
select * from project where programid in (84,1134)
select * from organization where organizationid = 1405
select * from program where programid in (12,1128)
select * from project where programid in (12,1128)
select * from organization where organizationid = 1407
select * from program where programid in (11,1132)
select * from project where programid in (11,1132)
select * from organization where organizationid = 1407

select * from project where programid in (1133,1056)
select * from program


update program set parentprogram_programid = 1049 where programid in (1135,1137,1136)
update program set parentprogram_programid = NULL where programid in (1056)

/* Delete extraneous programs and projects */
delete from project where programid IN (select programid from program where parentprogram_programid in (select programid from program where programid in (1132,1140,81,1143,10,1126,1145,1147,1146,1148,1122,1142,1134,1152,1150,1081,1138,1128,1149,1075,1076,1144,9,1141,1139,1151,136,3,1133,149,148,147)))
delete from program where parentprogram_programid in (select programid from program where programid in (1132,1140,81,1143,10,1126,1145,1147,1146,1148,1122,1142,1134,1152,1150,1081,1138,1128,1149,1075,1076,1144,9,1141,1139,1151,136,3,1133,149,148,147))
delete from project where programid in (1132,1140,81,1143,10,1126,1145,1147,1146,1148,1122,1142,1134,1152,1150,1081,1138,1128,1149,1075,1076,1144,9,1141,1139,1151,136,3,1133,149,148,147)
delete from program where programid in (1132,1140,81,1143,10,1126,1145,1147,1146,1148,1122,1142,1134,1152,1150,1081,1138,1128,1149,1075,1076,1144,9,1141,1139,1151,136,3,1133,149,148,147)


select * from organization where officesymbol like 'ECA/PE/C%' order by officesymbol 
