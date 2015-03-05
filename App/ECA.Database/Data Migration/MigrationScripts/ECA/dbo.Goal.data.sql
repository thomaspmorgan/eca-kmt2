/* created Goals from AFCP RatCat - Strategic Goals only */
INSERT INTO eca_dev.eca_dev.dbo.goal
    (goalname,
     history_createdby,
     history_createdon,
     history_revisedby,
     history_revisedon)
SELECT ratcat,
       0,
       N'2/3/2015 12:00:00 AM -05:00',
       0,
       N'2/3/2015 12:00:00 AM -05:00' 
  FROM ratcat 
 ORDER BY ratcat
GO


/* Insert list of Strategic Goals (from Janice-provided list */
SET IDENTITY_INSERT [dbo].[Goal] ON
insert into dbo.goal
(goalid,goalname,[History_CreatedBy], [History_CreatedOn], [History_RevisedBy], [History_RevisedOn])
VALUES
  (1, N'Strengthen America''s Economic Reach and Positive Economic Impact', 0, N'2/4/2015 12:00:00 AM -05:00', 0, N'2/4/2015 12:00:00 AM -05:00'), 
  (2, N'Strengthen America''s Foreign Policy Impact on Our Strategic Challenges', 0, N'2/4/2015 12:00:00 AM -05:00', 0, N'2/4/2015 12:00:00 AM -05:00'), 
  (3, N'Promote the Transition to a Low-Emission, Climate-Resilient World while Expanding Global Access to Sustainable Energy', 0, N'2/4/2015 12:00:00 AM -05:00', 0, N'2/4/2015 12:00:00 AM -05:00'), 
  (4, N'Protect Core U.S. Interests by Advancing Democracy and Human Rights and Strengthening Civil Society', 0, N'2/4/2015 12:00:00 AM -05:00', 0, N'2/4/2015 12:00:00 AM -05:00'), 
  (5, N'Modernize the Way We Do Diplomacy and Development', 0, N'2/4/2015 12:00:00 AM -05:00', 0, N'2/4/2015 12:00:00 AM -05:00') 
SET IDENTITY_INSERT [dbo].[Goal] OFF
GO

/*
Strategic Goal 1 – Strengthen America’s Economic Reach and Positive Economic Impact
Strategic Goal 2 – Strengthen America’s Foreign Policy Impact on Our Strategic Challenges
Strategic Goal 3 – Promote the Transition to a Low-Emission, Climate-Resilient World while Expanding Global Access to Sustainable Energy
Strategic Goal 4 – Protect Core U.S. Interests by Advancing Democracy and Human Rights and Strengthening Civil Society
Strategic Goal 5 – Modernize the Way We Do Diplomacy and Development
*/
