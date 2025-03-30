
CREATE TABLE [dbo].[DrugIndications] 
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [DrugName] NVARCHAR(200) NOT NULL,
    [Indication] NVARCHAR(500) NOT NULL,
    [Setid] NVARCHAR(80) NOT NULL
);


CREATE TABLE [dbo].[Users]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Username] NVARCHAR(100) NOT NULL,
    [PasswordHash] NVARCHAR(200) NOT NULL,
    [Role] NVARCHAR(50) NULL
);


CREATE TABLE [dbo].[ProgramData] (
    [ProgramId]                  INT            IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [ProgramName]                NVARCHAR(200)  NOT NULL,
    [CoverageEligibilitiesJson]  NVARCHAR(MAX)  NULL,
    [ProgramType]                NVARCHAR(100)  NULL,
    [RequirementsJson]           NVARCHAR(MAX)  NULL,
    [BenefitsJson]               NVARCHAR(MAX)  NULL,
    [FormsJson]                  NVARCHAR(MAX)  NULL,
    [FundingEvergreen]           BIT            NULL,
    [FundingCurrentFundingLevel] NVARCHAR(200)  NULL,
    [DetailsJson]                NVARCHAR(MAX)  NULL
);
