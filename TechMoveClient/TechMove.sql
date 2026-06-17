IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Clients] (
    [ClientId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [ContactDetails] nvarchar(max) NOT NULL,
    [Region] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Clients] PRIMARY KEY ([ClientId])
);

CREATE TABLE [Contracts] (
    [ContractId] int NOT NULL IDENTITY,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NOT NULL,
    [Status] int NOT NULL,
    [ServiceLevel] nvarchar(max) NOT NULL,
    [ClientId] int NOT NULL,
    CONSTRAINT [PK_Contracts] PRIMARY KEY ([ContractId]),
    CONSTRAINT [FK_Contracts_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([ClientId]) ON DELETE CASCADE
);

CREATE TABLE [ServiceRequests] (
    [ServiceRequestId] int NOT NULL IDENTITY,
    [Description] nvarchar(max) NOT NULL,
    [Cost] decimal(18,2) NOT NULL,
    [Status] int NOT NULL,
    [ContractId] int NOT NULL,
    CONSTRAINT [PK_ServiceRequests] PRIMARY KEY ([ServiceRequestId]),
    CONSTRAINT [FK_ServiceRequests_Contracts_ContractId] FOREIGN KEY ([ContractId]) REFERENCES [Contracts] ([ContractId]) ON DELETE CASCADE
);

CREATE INDEX [IX_Contracts_ClientId] ON [Contracts] ([ClientId]);

CREATE INDEX [IX_ServiceRequests_ContractId] ON [ServiceRequests] ([ContractId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260414164919_InitialCreate', N'10.0.5');

COMMIT;
GO

BEGIN TRANSACTION;
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260414184232_FixContractEntityKey', N'10.0.5');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [ServiceRequests] ADD [OriginalAmount] decimal(18,2) NOT NULL DEFAULT 0.0;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260415183148_FixDecimalPrecision', N'10.0.5');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [Contracts] ADD [AgreementFilePath] nvarchar(max) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260418110742_AddAgreementFileToContract', N'10.0.5');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [ServiceRequests] ADD [Currency] nvarchar(max) NOT NULL DEFAULT N'';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260422064127_AddCurrencyToServiceRequest', N'10.0.5');

COMMIT;
GO

