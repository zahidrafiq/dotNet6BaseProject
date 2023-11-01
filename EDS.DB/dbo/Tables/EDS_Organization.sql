CREATE TABLE [dbo].[EDS_Organization] (
    [Organization_ID]         INT            IDENTITY (1, 1) NOT NULL,
    [Organization_Code]       NVARCHAR (MAX) NOT NULL,
    [ParentOrganization_Code] NVARCHAR (255) NULL,
    [Created_By]              NVARCHAR (MAX) NOT NULL,
    [Created_At]              DATETIME2 (7)  NOT NULL,
    [Updated_By]              NVARCHAR (MAX) NULL,
    [Updated_At]              DATETIME2 (7)  NULL,
    [Is_Active]               BIT            NOT NULL,
    CONSTRAINT [PK_Org] PRIMARY KEY CLUSTERED ([Organization_ID] ASC)
);

