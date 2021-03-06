/* Add missing Themes (with IsActive flag) */
INSERT INTO [dbo].[Theme]
           ([ThemeName]
           ,[History_CreatedBy]
           ,[History_CreatedOn]
           ,[History_RevisedBy]
           ,[History_RevisedOn]
           ,[IsActive])
     VALUES
('Adaptation',0,GETDATE(),0,GETDATE(),1),
('African American History/Issues',0,GETDATE(),0,GETDATE(),1),
('American History and Studies',0,GETDATE(),0,GETDATE(),1),
('Anti-Americanism',0,GETDATE(),0,GETDATE(),1),
('Anti-Crime',0,GETDATE(),0,GETDATE(),1),
('Anti-Gang',0,GETDATE(),0,GETDATE(),1),
('Anti-Money Laundering',0,GETDATE(),0,GETDATE(),1),
('Architecture and Urban Planning',0,GETDATE(),0,GETDATE(),1),
('Arms Control',0,GETDATE(),0,GETDATE(),1),
('Biodiversity',0,GETDATE(),0,GETDATE(),1),
('Biotech',0,GETDATE(),0,GETDATE(),1),
('Border Management',0,GETDATE(),0,GETDATE(),1),
('Business Science and Technology',0,GETDATE(),0,GETDATE(),1),
('City and Local Government',0,GETDATE(),0,GETDATE(),1),
('Civics: Democracy, Pluralism, Federalism',0,GETDATE(),0,GETDATE(),1),
('Civil Rights and Basic Freedoms',0,GETDATE(),0,GETDATE(),1),
--('Civilian Security',0,GETDATE(),0,GETDATE(),1),   /* Duplicate */
('Clean Energy Solutions/Alternative Energy',0,GETDATE(),0,GETDATE(),1),
('Climate Change and Climate Change Education',0,GETDATE(),0,GETDATE(),1),
('Combatting WMD and Detabilizing Conventional Weapons',0,GETDATE(),0,GETDATE(),1),
('Community College and Continuing Education',0,GETDATE(),0,GETDATE(),1),
('Community Development',0,GETDATE(),0,GETDATE(),1),
('Community Policing and Law Enforcement',0,GETDATE(),0,GETDATE(),1),
--('Conflict Prevention, Mitigation, and Response',0,GETDATE(),0,GETDATE(),1),   /* Duplicate */
('Conservation/Natural Resources',0,GETDATE(),0,GETDATE(),1),
('Consular Services',0,GETDATE(),0,GETDATE(),1),
('Counseling and Mental Health',0,GETDATE(),0,GETDATE(),1),
('Countering Violent Extremism (CVE)',0,GETDATE(),0,GETDATE(),1),
('Counter-Narcotics',0,GETDATE(),0,GETDATE(),1),
('Counterterrorism',0,GETDATE(),0,GETDATE(),1),
('Cultural and Sports Programs',0,GETDATE(),0,GETDATE(),1),
('Cultural Heritage Preservation (Preservation, Anthropology, Archeology, History)',0,GETDATE(),0,GETDATE(),1),
('Cultural Heritage Protection (Theft, Profiteering)',0,GETDATE(),0,GETDATE(),1),
('Curriculum Development',0,GETDATE(),0,GETDATE(),1),
('Defense Policy',0,GETDATE(),0,GETDATE(),1),
('Diplomatic Services',0,GETDATE(),0,GETDATE(),1),
('Distance Education',0,GETDATE(),0,GETDATE(),1),
('Diversity (Religious, ethnic, disabled, LGBT, underserved, marginalized)',0,GETDATE(),0,GETDATE(),1),
('e-Commerce',0,GETDATE(),0,GETDATE(),1),
('Economic Opportunity and Empowerment',0,GETDATE(),0,GETDATE(),1),
('Educational Opportunity',0,GETDATE(),0,GETDATE(),1),
--('Energy Security',0,GETDATE(),0,GETDATE(),1),          /* Duplicate */
('Engineering, Robotics, and Transportation',0,GETDATE(),0,GETDATE(),1),
('Entrepreneurship and Competitiveness',0,GETDATE(),0,GETDATE(),1),
('Environment and Environment Education',0,GETDATE(),0,GETDATE(),1),
('Environmental Science and Technology',0,GETDATE(),0,GETDATE(),1),
('Financial/Banking Sector',0,GETDATE(),0,GETDATE(),1),
('Food Security/Sustainable Agriculture',0,GETDATE(),0,GETDATE(),1),
('Foreign Policy and Politics',0,GETDATE(),0,GETDATE(),1),
('Free and Fair Elections',0,GETDATE(),0,GETDATE(),1),
('Freedom of the Press (Communications and Journalism)',0,GETDATE(),0,GETDATE(),1),
('Gender Issues and Gender Based Violence',0,GETDATE(),0,GETDATE(),1),
('Healthcare Science and Administration',0,GETDATE(),0,GETDATE(),1),
('HIV',0,GETDATE(),0,GETDATE(),1),
('Human Resources/Workforce Development',0,GETDATE(),0,GETDATE(),1),
('Human Rights and Human Rights Education',0,GETDATE(),0,GETDATE(),1),
('Humanitarian Assistance/ Disaster Mitigation',0,GETDATE(),0,GETDATE(),1),
('Immigration Enforcement',0,GETDATE(),0,GETDATE(),1),
('Infectious Diseases',0,GETDATE(),0,GETDATE(),1),
('Information Technology',0,GETDATE(),0,GETDATE(),1),
('Intellectual Property Rights/Anti-Piracy',0,GETDATE(),0,GETDATE(),1),
('Investment Ecosystem and Business Climate',0,GETDATE(),0,GETDATE(),1),
('Leadership Development',0,GETDATE(),0,GETDATE(),1),
('Library and Information Science',0,GETDATE(),0,GETDATE(),1),
('Mathematics',0,GETDATE(),0,GETDATE(),1),
('Military Cooperation and Reform',0,GETDATE(),0,GETDATE(),1),
('Monetary Policy',0,GETDATE(),0,GETDATE(),1),
--('Museum Administration',0,GETDATE(),0,GETDATE(),1),           /* Duplicate */
('NGO Advocacy and Management',0,GETDATE(),0,GETDATE(),1),
('Oceans/Water',0,GETDATE(),0,GETDATE(),1),
('Park Management/Eco Tourism',0,GETDATE(),0,GETDATE(),1),
('Peacekeeping',0,GETDATE(),0,GETDATE(),1),
('Post-Secondary Education',0,GETDATE(),0,GETDATE(),1),
('Preventing Violent Extremism (PVE)',0,GETDATE(),0,GETDATE(),1),
('Primary Education',0,GETDATE(),0,GETDATE(),1),
('Promoting U.S. Higher Education',0,GETDATE(),0,GETDATE(),1),
('Promoting U.S. Study Education',0,GETDATE(),0,GETDATE(),1),
('Public Administration',0,GETDATE(),0,GETDATE(),1),
('Public Health and Safety',0,GETDATE(),0,GETDATE(),1),
--('Regional Economic Integration',0,GETDATE(),0,GETDATE(),1),        /* Duplicate */
('Research',0,GETDATE(),0,GETDATE(),1),
('Responsible Independent Judiciaries',0,GETDATE(),0,GETDATE(),1),
('Responsible Independent Legislatures',0,GETDATE(),0,GETDATE(),1),
('Reversing Land Emissions',0,GETDATE(),0,GETDATE(),1),
--('Rule of Law',0,GETDATE(),0,GETDATE(),1),                   /* Duplicate */
('Secondary Education',0,GETDATE(),0,GETDATE(),1),
('Security and Regional Stability',0,GETDATE(),0,GETDATE(),1),
--('Smart Sanctions',0,GETDATE(),0,GETDATE(),1),                  /* Duplicate */
('Sociology and Social Work',0,GETDATE(),0,GETDATE(),1),
('STEM Education',0,GETDATE(),0,GETDATE(),1),
('Sustainable Economic Growth and Diversity',0,GETDATE(),0,GETDATE(),1),
('Trade',0,GETDATE(),0,GETDATE(),1),
('Trafficking in Persons',0,GETDATE(),0,GETDATE(),1),
('Transparent and Accountable Government',0,GETDATE(),0,GETDATE(),1),
('U.S. Elections Programs',0,GETDATE(),0,GETDATE(),1),
('University Administration',0,GETDATE(),0,GETDATE(),1),
('Victim Assistance',0,GETDATE(),0,GETDATE(),1),
('Vocational Education',0,GETDATE(),0,GETDATE(),1),
('Volunteerism',0,GETDATE(),0,GETDATE(),1),
('Waste Management',0,GETDATE(),0,GETDATE(),1),
--('Women''s Empowerment',0,GETDATE(),0,GETDATE(),1),       /* Duplicate */
--('Women''s Rights',0,GETDATE(),0,GETDATE(),1),            /* Duplicate */
('Women''s Studies',0,GETDATE(),0,GETDATE(),1)


/* Add missing Themes (without IsActive flag) */
INSERT INTO [dbo].[Theme]
           ([ThemeName]
           ,[History_CreatedBy]
           ,[History_CreatedOn]
           ,[History_RevisedBy]
           ,[History_RevisedOn]
           ,[IsActive])
     VALUES
('Adaptation',0,GETDATE(),0,GETDATE()),
('African American History/Issues',0,GETDATE(),0,GETDATE()),
('American History and Studies',0,GETDATE(),0,GETDATE()),
('Anti-Americanism',0,GETDATE(),0,GETDATE()),
('Anti-Crime',0,GETDATE(),0,GETDATE()),
('Anti-Gang',0,GETDATE(),0,GETDATE()),
('Anti-Money Laundering',0,GETDATE(),0,GETDATE()),
('Architecture and Urban Planning',0,GETDATE(),0,GETDATE()),
('Arms Control',0,GETDATE(),0,GETDATE()),
('Biodiversity',0,GETDATE(),0,GETDATE()),
('Biotech',0,GETDATE(),0,GETDATE()),
('Border Management',0,GETDATE(),0,GETDATE()),
('Business Science and Technology',0,GETDATE(),0,GETDATE()),
('City and Local Government',0,GETDATE(),0,GETDATE()),
('Civics: Democracy, Pluralism, Federalism',0,GETDATE(),0,GETDATE()),
('Civil Rights and Basic Freedoms',0,GETDATE(),0,GETDATE()),
--('Civilian Security',0,GETDATE(),0,GETDATE()),                   /* Duplicate */
('Clean Energy Solutions/Alternative Energy',0,GETDATE(),0,GETDATE()),
('Climate Change and Climate Change Education',0,GETDATE(),0,GETDATE()),
('Combatting WMD and Detabilizing Conventional Weapons',0,GETDATE(),0,GETDATE()),
('Community College and Continuing Education',0,GETDATE(),0,GETDATE()),
('Community Development',0,GETDATE(),0,GETDATE()),
('Community Policing and Law Enforcement',0,GETDATE(),0,GETDATE()),
--('Conflict Prevention, Mitigation, and Response',0,GETDATE(),0,GETDATE()),             /* Duplicate */
('Conservation/Natural Resources',0,GETDATE(),0,GETDATE()),
('Consular Services',0,GETDATE(),0,GETDATE()),
('Counseling and Mental Health',0,GETDATE(),0,GETDATE()),
('Countering Violent Extremism (CVE)',0,GETDATE(),0,GETDATE()),
('Counter-Narcotics',0,GETDATE(),0,GETDATE()),
('Counterterrorism',0,GETDATE(),0,GETDATE()),
('Cultural and Sports Programs',0,GETDATE(),0,GETDATE()),
('Cultural Heritage Preservation (Preservation, Anthropology, Archeology, History)',0,GETDATE(),0,GETDATE()),
('Cultural Heritage Protection (Theft, Profiteering)',0,GETDATE(),0,GETDATE()),
('Curriculum Development',0,GETDATE(),0,GETDATE()),
('Defense Policy',0,GETDATE(),0,GETDATE()),
('Diplomatic Services',0,GETDATE(),0,GETDATE()),
('Distance Education',0,GETDATE(),0,GETDATE()),
('Diversity (Religious, ethnic, disabled, LGBT, underserved, marginalized)',0,GETDATE(),0,GETDATE()),
('e-Commerce',0,GETDATE(),0,GETDATE()),
('Economic Opportunity and Empowerment',0,GETDATE(),0,GETDATE()),
('Educational Opportunity',0,GETDATE(),0,GETDATE()),
--('Energy Security',0,GETDATE(),0,GETDATE()),                /* Duplicate */
('Engineering, Robotics, and Transportation',0,GETDATE(),0,GETDATE()),
('Entrepreneurship and Competitiveness',0,GETDATE(),0,GETDATE()),
('Environment and Environment Education',0,GETDATE(),0,GETDATE()),
('Environmental Science and Technology',0,GETDATE(),0,GETDATE()),
('Financial/Banking Sector',0,GETDATE(),0,GETDATE()),
('Food Security/Sustainable Agriculture',0,GETDATE(),0,GETDATE()),
('Foreign Policy and Politics',0,GETDATE(),0,GETDATE()),
('Free and Fair Elections',0,GETDATE(),0,GETDATE()),
('Freedom of the Press (Communications and Journalism)',0,GETDATE(),0,GETDATE()),
('Gender Issues and Gender Based Violence',0,GETDATE(),0,GETDATE()),
('Healthcare Science and Administration',0,GETDATE(),0,GETDATE()),
('HIV',0,GETDATE(),0,GETDATE()),
('Human Resources/Workforce Development',0,GETDATE(),0,GETDATE()),
('Human Rights and Human Rights Education',0,GETDATE(),0,GETDATE()),
('Humanitarian Assistance/ Disaster Mitigation',0,GETDATE(),0,GETDATE()),
('Immigration Enforcement',0,GETDATE(),0,GETDATE()),
('Infectious Diseases',0,GETDATE(),0,GETDATE()),
('Information Technology',0,GETDATE(),0,GETDATE()),
('Intellectual Property Rights/Anti-Piracy',0,GETDATE(),0,GETDATE()),
('Investment Ecosystem and Business Climate',0,GETDATE(),0,GETDATE()),
('Leadership Development',0,GETDATE(),0,GETDATE()),
('Library and Information Science',0,GETDATE(),0,GETDATE()),
('Mathematics',0,GETDATE(),0,GETDATE()),
('Military Cooperation and Reform',0,GETDATE(),0,GETDATE()),
('Monetary Policy',0,GETDATE(),0,GETDATE()),
--('Museum Administration',0,GETDATE(),0,GETDATE()),             /* Duplicate */
('NGO Advocacy and Management',0,GETDATE(),0,GETDATE()),
('Oceans/Water',0,GETDATE(),0,GETDATE()),
('Park Management/Eco Tourism',0,GETDATE(),0,GETDATE()),
('Peacekeeping',0,GETDATE(),0,GETDATE()),
('Post-Secondary Education',0,GETDATE(),0,GETDATE()),
('Preventing Violent Extremism (PVE)',0,GETDATE(),0,GETDATE()),
('Primary Education',0,GETDATE(),0,GETDATE()),
('Promoting U.S. Higher Education',0,GETDATE(),0,GETDATE()),
('Promoting U.S. Study Education',0,GETDATE(),0,GETDATE()),
('Public Administration',0,GETDATE(),0,GETDATE()),
('Public Health and Safety',0,GETDATE(),0,GETDATE()),
--('Regional Economic Integration',0,GETDATE(),0,GETDATE()),           /* Duplicate */
('Research',0,GETDATE(),0,GETDATE()),
('Responsible Independent Judiciaries',0,GETDATE(),0,GETDATE()),
('Responsible Independent Legislatures',0,GETDATE(),0,GETDATE()),
('Reversing Land Emissions',0,GETDATE(),0,GETDATE()),
--('Rule of Law',0,GETDATE(),0,GETDATE()),                           /* Duplicate */
('Secondary Education',0,GETDATE(),0,GETDATE()),
('Security and Regional Stability',0,GETDATE(),0,GETDATE()),
--('Smart Sanctions',0,GETDATE(),0,GETDATE()),                      /* Duplicate */
('Sociology and Social Work',0,GETDATE(),0,GETDATE()),
('STEM Education',0,GETDATE(),0,GETDATE()),
('Sustainable Economic Growth and Diversity',0,GETDATE(),0,GETDATE()),
('Trade',0,GETDATE(),0,GETDATE()),
('Trafficking in Persons',0,GETDATE(),0,GETDATE()),
('Transparent and Accountable Government',0,GETDATE(),0,GETDATE()),
('U.S. Elections Programs',0,GETDATE(),0,GETDATE()),
('University Administration',0,GETDATE(),0,GETDATE()),
('Victim Assistance',0,GETDATE(),0,GETDATE()),
('Vocational Education',0,GETDATE(),0,GETDATE()),
('Volunteerism',0,GETDATE(),0,GETDATE()),
('Waste Management',0,GETDATE(),0,GETDATE()),
--('Women''s Empowerment',0,GETDATE(),0,GETDATE()),             /* Duplicate */
--('Women''s Rights',0,GETDATE(),0,GETDATE()),                  /* Duplicate */
('Women''s Studies',0,GETDATE(),0,GETDATE())


/* Update the Theme Names where there are logical matches */
UPDATE dbo.theme SET ThemeName = 'American History and Studies' WHERE ThemeName = 'American Studies'
UPDATE dbo.theme SET ThemeName = 'Anti-Americanism' WHERE ThemeName = 'How American Citizens are Viewed Abroad'
UPDATE dbo.theme SET ThemeName = 'Business Science and Technology' WHERE ThemeName = 'Science and Technology'
UPDATE dbo.theme SET ThemeName = 'City and Local Government' WHERE ThemeName = 'Government'
UPDATE dbo.theme SET ThemeName = 'Countering Violent Extremism (CVE)' WHERE ThemeName = 'CVE/Counterterrorism'
UPDATE dbo.theme SET ThemeName = 'Cultural and Sports Programs' WHERE ThemeName = 'Culture/Sports/American Society'
UPDATE dbo.theme SET ThemeName = 'Curriculum Development' WHERE ThemeName = 'Education & Curriculum Development'
UPDATE dbo.theme SET ThemeName = 'Diversity (Religious, ethnic, disabled, LGBT, underserved, marginalized)' WHERE ThemeName = 'Diversity'
UPDATE dbo.theme SET ThemeName = 'Economic Opportunity and Empowerment' WHERE ThemeName = 'Economic Statecraft'
UPDATE dbo.theme SET ThemeName = 'Educational Opportunity' WHERE ThemeName = 'Education'
UPDATE dbo.theme SET ThemeName = 'Entrepreneurship and Competitiveness' WHERE ThemeName = 'Entrepreneurship/Job Creation'
UPDATE dbo.theme SET ThemeName = 'Environment and Environment Education' WHERE ThemeName = 'Environment'
UPDATE dbo.theme SET ThemeName = 'Financial/Banking Sector' WHERE ThemeName = 'Financial Sector'
UPDATE dbo.theme SET ThemeName = 'Food Security/Sustainable Agriculture' WHERE ThemeName = 'Food Security/Agriculture'
UPDATE dbo.theme SET ThemeName = 'Healthcare Science and Administration' WHERE ThemeName = 'Global Health'
UPDATE dbo.theme SET ThemeName = 'HIV' WHERE ThemeName = 'HIV/AIDS'
UPDATE dbo.theme SET ThemeName = 'Human Rights and Human Rights Education' WHERE ThemeName = 'Human Rights'
UPDATE dbo.theme SET ThemeName = 'Humanitarian Assistance/ Disaster Mitigation' WHERE ThemeName = 'Humanitarian Assistance, Disaster Mitigation'
UPDATE dbo.theme SET ThemeName = 'Intellectual Property Rights/Anti-Piracy' WHERE ThemeName = 'Intellectual Property Rights (IPR)/Anti-Piracy'
UPDATE dbo.theme SET ThemeName = 'Military Cooperation and Reform' WHERE ThemeName = 'Military and Security Cooperation/Reform'
UPDATE dbo.theme SET ThemeName = 'Peacekeeping' WHERE ThemeName = 'Peace & Security'
UPDATE dbo.theme SET ThemeName = 'Sustainable Economic Growth and Diversity' WHERE ThemeName = 'Sustainable Economic Growth & Well-Being'
UPDATE dbo.theme SET ThemeName = 'Trade' WHERE ThemeName = 'Trade & Investment'
UPDATE dbo.theme SET ThemeName = 'Trafficking in Persons' WHERE ThemeName = 'Transnational Threats - Crime, Narcotics, Trafficking in Person'
UPDATE dbo.theme SET ThemeName = 'Transparent and Accountable Government' WHERE ThemeName = 'Good Governance'


DELETE FROM dbo.THEME WHERE ThemeName IN (
'Arts',
'Arts & Culture',
'Arts and Culture',
'Civil Society',
'Dance',
'Democracy/Good Governance/Rule of Law',
'Democratic Institutions',
'Disability Advocacy',
'Disability Issues',
'Diversity & Pluralism',
'Film',
'Human & Civil Rights',
'Innovation',
'Markets and Competitiveness',
'Media',
'Minority Issues & Non-Traditional Audiences',
'Music',
'Muslim Engagement',
'Muslim Outreach',
'Mutual Understanding',
'Mutual Understanding and Education',
'Native American Issues',
'Other',
'Outreach to Youth',
'Performing Arts',
'Political Science',
'Politics & Parties',
'Public Affairs',
'Religious Engagement',
'Sport and Health',
'Sports',
'Theater',
'Theater & Dance',
'Transitions in Frontline States',
'Travel and Tourism',
'Visual Arts',
'Youth Engagement'
)

 
 
 


