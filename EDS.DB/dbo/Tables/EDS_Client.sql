CREATE TABLE [dbo].[EDS_Client] (
    [Client_ID]       INT            IDENTITY (1, 1) NOT NULL,
    [Client_Name]     NVARCHAR (255) NOT NULL,
    [Client_Code]     NVARCHAR (50)  NOT NULL,
    [Organization_ID] INT            NOT NULL,
    [Created_By]      NVARCHAR (MAX) NOT NULL,
    [Created_At]      DATETIME2 (7)  NOT NULL,
    [Updated_By]      NVARCHAR (MAX) NULL,
    [Updated_At]      DATETIME2 (7)  NULL,
    [Is_Active]       BIT            NOT NULL,
    CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED ([Client_ID] ASC),
    CONSTRAINT [FK_Client_Org_OrgsOrganizationID] FOREIGN KEY ([Organization_ID]) REFERENCES [dbo].[EDS_Organization] ([Organization_ID])
);

