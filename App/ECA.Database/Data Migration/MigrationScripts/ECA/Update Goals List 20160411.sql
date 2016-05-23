
/* Insert official goals (no IsActive flag) */
INSERT INTO [dbo].[Goal]
           ([GoalName]
           ,[History_CreatedBy]
           ,[History_CreatedOn]
           ,[History_RevisedBy]
           ,[History_RevisedOn])
     VALUES
('Developing Strategic Partners',0,GETDATE(),0,GETDATE()),
('Digital/Virtual/Blended Exchanges',0,GETDATE(),0,GETDATE()),
('Education Diplomacy, Scholarship, and Research',0,GETDATE(),0,GETDATE()),
('Treaty/Bilateral Obligations (CHC)',0,GETDATE(),0,GETDATE()),
('Outreach: Alumni',0,GETDATE(),0,GETDATE()),
('Outreach: At-Risk Audiences',0,GETDATE(),0,GETDATE()),
('Outreach: Increasing Participant Diversity', 0,GETDATE(),0,GETDATE()),
('Outreach: Women',0,GETDATE(),0,GETDATE()), 
('Outreach: Youth',0,GETDATE(),0,GETDATE()),
('Promoting English',0,GETDATE(),0,GETDATE()),
('Promoting Foreign Language Study', 0,GETDATE(),0,GETDATE()),
('Promoting Internationalization of Higher Ed/Scholarly Collaboration',0,GETDATE(),0,GETDATE()),
('Promoting U.S. Higher Education',0,GETDATE(),0,GETDATE()),
('Promoting U.S. Study Abroad',0,GETDATE(),0,GETDATE())


/* Insert official goals (with IsActive flag) */
INSERT INTO [dbo].[Goal]
           ([GoalName]
           ,[History_CreatedBy]
           ,[History_CreatedOn]
           ,[History_RevisedBy]
           ,[History_RevisedOn]
           ,[IsActive])
     VALUES
('Developing Strategic Partners',0,GETDATE(),0,GETDATE(),1),
('Digital/Virtual/Blended Exchanges',0,GETDATE(),0,GETDATE(),1),
('Education Diplomacy, Scholarship, and Research',0,GETDATE(),0,GETDATE(),1),
('Treaty/Bilateral Obligations (CHC)',0,GETDATE(),0,GETDATE(),1),
('Outreach: Alumni',0,GETDATE(),0,GETDATE(),1),
('Outreach: At-Risk Audiences',0,GETDATE(),0,GETDATE(),1),
('Outreach: Increasing Participant Diversity', 0,GETDATE(),0,GETDATE(),1),
('Outreach: Women',0,GETDATE(),0,GETDATE(),1), 
('Outreach: Youth',0,GETDATE(),0,GETDATE(),1),
('Promoting English',0,GETDATE(),0,GETDATE(),1),
('Promoting Foreign Language Study', 0,GETDATE(),0,GETDATE(),1),
('Promoting Internationalization of Higher Ed/Scholarly Collaboration',0,GETDATE(),0,GETDATE(),1),
('Promoting U.S. Higher Education',0,GETDATE(),0,GETDATE(),1),
('Promoting U.S. Study Abroad',0,GETDATE(),0,GETDATE(),1)
