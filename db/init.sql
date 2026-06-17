-- Author: PROG7311-2026-EMWVL (Lecturer Repository)
-- URL: https://github.com/PROG7311-2026-EMWVL/Hello-PROG7311/tree/main/12%20-%20Docker%20Compose
-- Date: [n.d]
-- Date Accessed: 16 May 2026

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'TechMoveDB')
BEGIN
    CREATE DATABASE TechMoveDB;
END
GO

USE TechMoveDB;
GO

-- Clients Table
IF NOT EXISTS (
    SELECT * FROM sysobjects
    WHERE name = 'Clients' AND xtype = 'U'
)
BEGIN
    CREATE TABLE Clients
    (
        ClientId INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        ContactDetails NVARCHAR(300) NOT NULL,
        Region NVARCHAR(100) NOT NULL
    );
END
GO

-- Contracts Table
IF NOT EXISTS (
    SELECT * FROM sysobjects
    WHERE name = 'Contracts' AND xtype = 'U'
)
BEGIN
    CREATE TABLE Contracts
    (
        ContractId INT IDENTITY(1,1) PRIMARY KEY,
        StartDate DATETIME2 NOT NULL,
        EndDate DATETIME2 NOT NULL,
        Status INT NOT NULL,
        ServiceLevel NVARCHAR(50) NOT NULL,
        AgreementFilePath NVARCHAR(500) NULL,
        ClientId INT NOT NULL,

        CONSTRAINT FK_Contracts_Clients
        FOREIGN KEY (ClientId)
        REFERENCES Clients(ClientId)
        ON DELETE CASCADE
    );
END
GO


-- ServiceRequests Table
IF NOT EXISTS (
    SELECT * FROM sysobjects
    WHERE name = 'ServiceRequests' AND xtype = 'U'
)
BEGIN
    CREATE TABLE ServiceRequests
    (
        ServiceRequestId INT IDENTITY(1,1) PRIMARY KEY,
        Description NVARCHAR(500) NOT NULL,
        Cost DECIMAL(18,2) NOT NULL,
        Status INT NOT NULL,
        ContractId INT NOT NULL,
        OriginalAmount DECIMAL(18,2) NOT NULL,
        Currency NVARCHAR(10) NOT NULL,

        CONSTRAINT FK_ServiceRequests_Contracts
        FOREIGN KEY (ContractId)
        REFERENCES Contracts(ContractId)
        ON DELETE CASCADE
    );
END
GO