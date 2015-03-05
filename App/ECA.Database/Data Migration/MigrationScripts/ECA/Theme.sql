/* creates test Region data for Location Table  */

/*
--Professional Fellows "On Demand" Programs
Civil Society
Culture/Sports/American Society
Democracy/Good Governance/Rule of Law
Diversity
Education
Entrepreneurship/Job Creation
Environment
Women's Empowerment
Youth Engagement

--Youth Leadership Programs (YLP)
Civil Society
Culture/Sports/American Society
Democracy/Good Governance/Rule of Law
Education
Science and Technology
Youth Engagement

--Institute For Representative Government
Culture/Sports/American Society
Democracy/Good Governance/Rule of Law

--J-1 Visa Exchange Visitor Program: Secondary School Student
Culture/Sports/American Society
Diversity
Education
Youth Engagement

--Partners of the Americas
Culture/Sports/American Society
Diversity
Education
Environment
Human Rights

--Fulbright English Teaching Assistant Program (ETA) - AF
Culture/Sports/American Society
Education
Markets and Competitiveness
Youth Engagement

--Academic Exchange Programs - East Asian & Pacific Programs
Civil Society
Culture/Sports/American Society
Education

--National Youth Science Camp of the Americas
Culture/Sports/American Society
Education
Science and Technology
Youth Engagement

--Iraq Cultural Heritage Initiative
Conflict Prevention, Mitigation, and Response
Diversity
Trade & Investment
Transitions in Frontline States
Transnational Threats - Crime, Narcotics, Trafficking in Person
Travel and Tourism

--American Center for International Labor Solidarity
Democracy/Good Governance/Rule of Law
Human Rights
Trade & Investment

--Academic Exchange Programs - European Programs
Education
Entrepreneurship/Job Creation

--Critical Language Scholarships (CLS)
Education
Youth Engagement

--English Access Microscholarship Program
Civil Society
Culture/Sports/American Society
Democracy/Good Governance/Rule of Law
Diversity
Education
Women's Empowerment
Youth Engagement

--Hubert H. Humphrey Fellowship
Civil Society
Diversity
Education
Entrepreneurship/Job Creation
Environment

--Fulbright Regional Network for Applied Research (NEXUS) Program
Education
Environment
Innovation
Science and Technology

--International Vistor Leadership Program (IVLP)
Civil Society
Civilian Security
Conflict Prevention, Mitigation, and Response
Culture/Sports/American Society
CVE/Counterterrorism
Democracy/Good Governance/Rule of Law
Diversity
Economic Statecraft
Education
Energy Security
Entrepreneurship/Job Creation
Environment
Financial Sector
Food Security/Agriculture
Global Health
Human Rights
Humanitarian Assistance, Disaster Mitigation
Innovation
Intellectual Property Rights (IPR)/Anti-Piracy
Markets and Competitiveness
Military and Security Cooperation/Reform
Muslim Engagement
Regional Economic Integration
Religious Engagement
Science and Technology
Smart Sanctions
Sustainable Economic Growth & Well-Being
Trade & Investment
Transitions in Frontline States
Transnational Threats - Crime, Narcotics, Trafficking in Person
Travel and Tourism
Women's Empowerment
Youth Engagement

--Cultural Property Protection
Conflict Prevention, Mitigation, and Response
Humanitarian Assistance, Disaster Mitigation
Sustainable Economic Growth & Well-Being
Trade & Investment
Transnational Threats - Crime, Narcotics, Trafficking in Person

--Professional Fellows Program
Civil Society
Culture/Sports/American Society
Democracy/Good Governance/Rule of Law
Diversity
Education
Entrepreneurship/Job Creation
Environment
Women's Empowerment
Youth Engagement

--Fulbright Scholar-in-Residence Program
Diversity
Education
Women's Empowerment

--Institutes for Student Leaders
Civil Society
Culture/Sports/American Society
Democracy/Good Governance/Rule of Law
Diversity
Education
Entrepreneurship/Job Creation
Environment
Women's Empowerment
Youth Engagement

--English Language Fellows
Diversity
Education

--Tunisia Community College Scholarship Program
Education
Science and Technology

--Arts in Collaboration: Youth-Urban Outreach
Civil Society
Culture/Sports/American Society
Regional Economic Integration
Youth Engagement

--Sister Cities International
Civil Society
Culture/Sports/American Society

--Academic Exchange Programs - South & Central Asia Programs
Civil Society
Education
Transitions in Frontline States

--Fulbright U.S. Scholar Program
Civilian Security
Culture/Sports/American Society
Diversity
Entrepreneurship/Job Creation
Environment
Women's Empowerment
Youth Engagement


*/

begin tran t1


insert into theme
(ThemeName,History_createdby,history_createdon,history_revisedby,history_revisedon)
values
('Civil Society',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Civilian Security',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Conflict Prevention, Mitigation, and Response',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Culture/Sports/American Society',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('CVE/Counterterrorism',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Democracy/Good Governance/Rule of Law',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Diversity',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Economic Statecraft',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Education',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Energy Security',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Entrepreneurship/Job Creation',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Environment',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Financial Sector',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Food Security/Agriculture',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Global Health',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Human Rights',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Humanitarian Assistance, Disaster Mitigation',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Innovation',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Intellectual Property Rights (IPR)/Anti-Piracy',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Markets and Competitiveness',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Military and Security Cooperation/Reform',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Muslim Engagement',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Regional Economic Integration',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Religious Engagement',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Science and Technology',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Smart Sanctions',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Sustainable Economic Growth & Well-Being',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Trade & Investment',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Transitions in Frontline States',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Transnational Threats - Crime, Narcotics, Trafficking in Person',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Travel and Tourism',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Women''s Empowerment',0,sysdatetimeoffset(),0,sysdatetimeoffset()),
('Youth Engagement',0,sysdatetimeoffset(),0,sysdatetimeoffset())





commit tran t1
GO

