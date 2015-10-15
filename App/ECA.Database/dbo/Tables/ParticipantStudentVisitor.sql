﻿CREATE TABLE [dbo].[ParticipantStudentVisitor]
(
	[ParticipantId] INT NOT NULL PRIMARY KEY, 
    [IssueReasonId] INT NOT NULL , 
    [EducationLevelId] INT NOT NULL, 
    [EducationLevelOtherRemarks] NVARCHAR(255) NULL, 
    [PrimaryMajorId] INT NOT NULL, 
    [SecondaryMajorId] INT NULL, 
    [MinorId] INT NULL, 
    [LengthOfStudy] INT NULL, 
    [IsEnglishProficiencyReqd] BIT NOT NULL , 
    [IsEnglishProficiencyMet] BIT NOT NULL, 
    [EnglishProficiencyNotReqdReason] NVARCHAR(255) NULL, 
    [TuitionExpense] DECIMAL(12, 2) NULL, 
    [LivingExpense] DECIMAL(12, 2) NULL, 
    [DependentExpense] DECIMAL(12, 2) NULL, 
    [OtherExpense] DECIMAL(12, 2) NULL, 
    [ExpenseRemarks] NVARCHAR(255) NULL, 
    [PersonalFunding] DECIMAL(12, 2) NULL, 
    [SchoolFunding] DECIMAL(12, 2) NULL, 
    [SchoolFundingRemarks] NVARCHAR(255) NULL, 
    [OtherFunding] DECIMAL(12, 2) NULL, 
    [OtherFundingRemarks] NVARCHAR(255) NULL, 
    [EmploymentFunding] DECIMAL(12, 2) NULL, 
    [History_CreatedBy] INT NOT NULL, 
    [History_CreatedOn] DATETIMEOFFSET NOT NULL, 
    [History_RevisedBy] INT NOT NULL, 
    [History_RevisedOn] DATETIMEOFFSET NOT NULL, 
    CONSTRAINT [FK_ParticipantStudentVisitor_ToParticipantPerson] FOREIGN KEY ([ParticipantId]) REFERENCES [ParticipantPerson]([ParticipantId]), 
    CONSTRAINT [FK_ParticipantStudentVisitor_ToSevisStudentCreation] FOREIGN KEY ([IssueReasonId]) REFERENCES [sevis].[StudentCreation]([StudentCreationId]), 
    CONSTRAINT [FK_ParticipantStudentVisitor_ToSevisEducationLevel] FOREIGN KEY ([EducationLevelId]) REFERENCES [sevis].[EducationLevel]([EducationLevelId]), 
    CONSTRAINT [FK_ParticipantStudentVisitorPrimaryMajor_ToSevisFieldOfStudy] FOREIGN KEY ([PrimaryMajorId]) REFERENCES [sevis].[FieldOfStudy]([FieldOfStudyId]),
	CONSTRAINT [FK_ParticipantStudentVisitorSecondaryMajor_ToSevisFieldOfStudy] FOREIGN KEY ([SecondaryMajorId]) REFERENCES [sevis].[FieldOfStudy]([FieldOfStudyId]),
	CONSTRAINT [FK_ParticipantStudentVisitorMinor_ToSevisFieldOfStudy] FOREIGN KEY ([MinorId]) REFERENCES [sevis].[FieldOfStudy]([FieldOfStudyId])
)
