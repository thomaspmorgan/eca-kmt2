CREATE TABLE [dbo].[OrganizationOrganizationRole]
(
	[OrganizationId] INT NOT NULL,
    [OrganizationRoleId] INT NOT NULL,
	CONSTRAINT [FK_OrganizationOrganizationRole_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [Organization]([OrganizationId]), 
    CONSTRAINT [FK_OrganizationOrganizationRole_OrganizationRole] FOREIGN KEY ([OrganizationRoleId]) REFERENCES [OrganizationRole]([OrganizationRoleId]), 
    PRIMARY KEY ([OrganizationId], [OrganizationRoleId])
)
