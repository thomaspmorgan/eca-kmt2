﻿CREATE TABLE [dbo].[PersonProminentCategory]
(
	[PersonId] INT NOT NULL , 
    [ProminentCategoryId] INT NOT NULL, 
    CONSTRAINT [FK_PersonProminentCategory_Person] FOREIGN KEY ([PersonId]) REFERENCES [Person]([PersonId]), 
    CONSTRAINT [FK_PersonProminentCategory_ProminentCategory] FOREIGN KEY ([ProminentCategoryId]) REFERENCES [ProminentCategory]([ProminentCategoryId]), 
    PRIMARY KEY ([ProminentCategoryId], [PersonId])
)
