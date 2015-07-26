CREATE TABLE [dbo].[OfficeSetting]
(
	[OfficeSettingId] INT NOT NULL IDENTITY (1,1), 
    [OfficeId] INT NOT NULL, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Value] NVARCHAR(50) NOT NULL,
	CONSTRAINT [PK_dbo.OfficeSetting] PRIMARY KEY CLUSTERED ([OfficeSettingId] ASC),
    CONSTRAINT [FK_dbo.OfficeSettings_dbo.OfficeId] FOREIGN KEY ([OfficeId]) REFERENCES [dbo].[Organization] ([OrganizationId]) ON DELETE CASCADE
) 
