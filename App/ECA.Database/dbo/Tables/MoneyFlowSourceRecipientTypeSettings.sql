CREATE TABLE [dbo].[MoneyFlowSourceRecipientTypeSettings]
(
	[MoneyFlowSourceRecipientTypeId] INT NOT NULL,
	[PeerMoneyFlowSourceRecipientTypeId] INT NOT NULL,
	[IsSource] BIT NOT NULL, 
    [IsRecipient] BIT NOT NULL, 
    CONSTRAINT [PK_dbo.MoneyFlowSourceRecipientTypeSettings] PRIMARY KEY CLUSTERED ([MoneyFlowSourceRecipientTypeId] ASC, [PeerMoneyFlowSourceRecipientTypeId] ASC),
	CONSTRAINT [FK_dbo.MoneyFlowSourceRecipientTypeSettings_dbo.MoneyFlowSourceRecipientType_MoneyFlowSourceRecipientTypeId] FOREIGN KEY ([MoneyFlowSourceRecipientTypeId]) REFERENCES [dbo].[MoneyFlowSourceRecipientType] ([MoneyFlowSourceRecipientTypeId]),
    CONSTRAINT [FK_dbo.MoneyFlowSourceRecipientTypeSettings_dbo.MoneyFlowSourceRecipientType_PeerMoneyFlowSourceRecipientTypeId] FOREIGN KEY ([PeerMoneyFlowSourceRecipientTypeId]) REFERENCES [dbo].[MoneyFlowSourceRecipientType] ([MoneyFlowSourceRecipientTypeId])
)
