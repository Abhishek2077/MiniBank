CREATE DATABASE MiniBankDB;

USE MiniBankDB

CREATE TABLE Bank_Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL, 
    Gender NVARCHAR(10) NOT NULL,
    PasswordHash NVARCHAR(MAX) NOT NULL, 
    [Role] NVARCHAR(10) NOT NULL, 
    CreatedAt DATETIME2 DEFAULT GETDATE() 
);

USE MiniBankDB;
DROP TABLE IF EXISTS CreditApplications;


CREATE TABLE CreditApplications (
    ApplicationID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT FOREIGN KEY REFERENCES Bank_Users(UserID),
    CompanyStructure NVARCHAR(50) NOT NULL,
    BusinessSince DATE NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL,
    StreetAddress NVARCHAR(255) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    [State] NVARCHAR(100) NOT NULL,
    ZipCode NVARCHAR(10) NOT NULL,
    BankName NVARCHAR(100) NULL,
    BankPhone NVARCHAR(20) NULL,
    BankAddress NVARCHAR(255) NULL,
    LogoImagePath NVARCHAR(MAX) NULL,
    [Status] NVARCHAR(20) DEFAULT 'Pending',
    SubmittedAt DATETIME2 DEFAULT GETDATE()
   );
-----------------------------------------------------------------------
CREATE PROCEDURE Bank_Users_Insert
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @Gender NVARCHAR(10),
    @PasswordHash NVARCHAR(MAX),
    @Role NVARCHAR(10)
AS
BEGIN
    INSERT INTO Bank_Users (FirstName, LastName, Email, Gender, PasswordHash, [Role])
    VALUES (@FirstName, @LastName, @Email, @Gender, @PasswordHash, @Role);
END
--------------------------------------------------------------------------
CREATE PROCEDURE Bank_Users_Update
    @UserID INT,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @Gender NVARCHAR(10)
AS
BEGIN
    UPDATE Bank_Users SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Gender = @Gender
    WHERE UserID = @UserID;
END
-----------------------------------------------------------------------
CREATE PROCEDURE Bank_Users_Delete
    @UserID INT
AS
BEGIN
    DELETE FROM Bank_Users WHERE UserID = @UserID;
END
-----------------------------------------------------------------------
CREATE PROCEDURE Bank_Users_GetByID
    @UserID INT
AS
BEGIN
    SELECT UserID, FirstName, LastName, Email, Gender, [Role] FROM Bank_Users WHERE UserID = @UserID;
END
-----------------------------------------------------------------------
CREATE PROCEDURE Bank_Users_GetByEmail
    @Email NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Bank_Users WHERE Email = @Email;
END
-----------------------------------------------------------------------

---- Procedures for the CreditApplications Table-------

CREATE PROCEDURE CreditApplications_Insert
    @UserID INT,
    @CompanyStructure NVARCHAR(50),
    @BusinessSince DATE,
    @PhoneNumber NVARCHAR(20),
    @StreetAddress NVARCHAR(255),
    @City NVARCHAR(100),
    @State NVARCHAR(100),
    @ZipCode NVARCHAR(10),
    @BankName NVARCHAR(100),
    @BankPhone NVARCHAR(20),
    @BankAddress NVARCHAR(255),
    @LogoImagePath NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO CreditApplications (UserID, CompanyStructure, BusinessSince, PhoneNumber, StreetAddress, City, [State], ZipCode, BankName, BankPhone, BankAddress, LogoImagePath)
    VALUES (@UserID, @CompanyStructure, @BusinessSince, @PhoneNumber, @StreetAddress, @City, @State, @ZipCode, @BankName, @BankPhone, @BankAddress, @LogoImagePath);
END

---------------------------------------------------------------------------------
CREATE PROCEDURE CreditApplications_Update
    @ApplicationID INT,
    @CompanyStructure NVARCHAR(50),
    @BusinessSince DATE,
    @PhoneNumber NVARCHAR(20),
    @StreetAddress NVARCHAR(255),
    @City NVARCHAR(100),
    @State NVARCHAR(100),
    @ZipCode NVARCHAR(10),
    @BankName NVARCHAR(100),
    @BankPhone NVARCHAR(20),
    @BankAddress NVARCHAR(255)
AS
BEGIN
    UPDATE CreditApplications SET CompanyStructure = @CompanyStructure, BusinessSince = @BusinessSince, PhoneNumber = @PhoneNumber, StreetAddress = @StreetAddress, City = @City, [State] = @State, ZipCode = @ZipCode, BankName = @BankName, BankPhone = @BankPhone, BankAddress = @BankAddress
    WHERE ApplicationID = @ApplicationID;
END
-----------------------------------------------------------------------------------
CREATE PROCEDURE CreditApplications_Delete
    @ApplicationID INT
AS
BEGIN
    DELETE FROM CreditApplications WHERE ApplicationID = @ApplicationID;
END
------------------------------------------------------------------------------------
CREATE PROCEDURE CreditApplications_GetByID
    @ApplicationID INT
AS
BEGIN
    SELECT * FROM CreditApplications WHERE ApplicationID = @ApplicationID;
END
------------------------------------------------------------------------------------
CREATE PROCEDURE CreditApplications_GetByUserID
    @UserID INT
AS
BEGIN
    SELECT * FROM CreditApplications WHERE UserID = @UserID;
END
------------------------------------------------------------------------------------
CREATE PROCEDURE CreditApplications_GetAll
AS
BEGIN
    SELECT * FROM CreditApplications;
END
------------------------------------------------------------------------------------
USE MiniBankDB;
GO

CREATE PROCEDURE CreditApplications_GetDetailsByID
    @ApplicationID INT
AS
BEGIN
    SELECT 
        app.*, 
        usr.FirstName, 
        usr.LastName, 
        usr.Email
    FROM 
        CreditApplications AS app
    JOIN 
        Bank_Users AS usr ON app.UserID = usr.UserID
    WHERE 
        app.ApplicationID = @ApplicationID;
END
GO
-----------------------------------------------------------------------------
CREATE PROCEDURE CreditApplications_UpdateStatus
    @ApplicationID INT,
    @Status NVARCHAR(20)
AS
BEGIN
    UPDATE CreditApplications
    SET [Status] = @Status
    WHERE ApplicationID = @ApplicationID;
END


select * from CreditApplications

----------------------------------------------------------------------------
ALTER TABLE CreditApplications
DROP COLUMN BankPhone;

ALTER TABLE CreditApplications
DROP COLUMN BankAddress;

ALTER TABLE CreditApplications
ADD AccountNumber NVARCHAR(18) NOT NULL DEFAULT'';

ALTER TABLE CreditApplications
ADD IFSCCode NVARCHAR(20) NOT NULL DEFAULT'';

ALTER TABLE CreditApplications
ADD BankBranch NVARCHAR(100) NOT NULL DEFAULT'';

ALTER TABLE CreditApplications
ADD BankState NVARCHAR(100) NOT NULL DEFAULT'';

ALTER TABLE CreditApplications
ADD BankCity NVARCHAR(100) NOT NULL DEFAULT'';

ALTER TABLE CreditApplications
ADD BankZipCode NVARCHAR(10) NOT NULL DEFAULT'';
--------------------------------------------------------------------------------

ALTER PROCEDURE CreditApplications_Insert
    @UserID INT,
    @CompanyStructure NVARCHAR(50),
    @BusinessSince DATE,
    @PhoneNumber NVARCHAR(20),
    @StreetAddress NVARCHAR(255),
    @City NVARCHAR(100),
    @State NVARCHAR(100),
    @ZipCode NVARCHAR(10),
    @BankName NVARCHAR(100),
    @AccountNumber NVARCHAR(18),
    @IFSCCode NVARCHAR(20),
    @BankBranch NVARCHAR(100),
    @BankState NVARCHAR(100),
    @BankCity NVARCHAR(100),
    @BankZipCode NVARCHAR(10),
    @LogoImagePath NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO CreditApplications (UserID, CompanyStructure, BusinessSince, PhoneNumber, StreetAddress, City, [State], ZipCode, BankName, AccountNumber, IFSCCode, BankBranch, BankState, BankCity, BankZipCode, LogoImagePath)
    VALUES (@UserID, @CompanyStructure, @BusinessSince, @PhoneNumber, @StreetAddress, @City, @State, @ZipCode, @BankName, @AccountNumber, @IFSCCode, @BankBranch, @BankState, @BankCity, @BankZipCode, @LogoImagePath);
END
----------------------------------------------------------------------------------------------------------------

ALTER PROCEDURE CreditApplications_Update
    @ApplicationID INT,
    @UserID INT, -- Add this line
    @CompanyStructure NVARCHAR(50),
    @BusinessSince DATE,
    @PhoneNumber NVARCHAR(20),
    @StreetAddress NVARCHAR(255),
    @City NVARCHAR(100),
    @State NVARCHAR(100),
    @ZipCode NVARCHAR(10),
    @BankName NVARCHAR(100),
    @AccountNumber NVARCHAR(18),
    @IFSCCode NVARCHAR(20),
    @BankBranch NVARCHAR(100),
    @BankState NVARCHAR(100),
    @BankCity NVARCHAR(100),
    @BankZipCode NVARCHAR(10),
    @LogoImagePath NVARCHAR(255)
AS
BEGIN
    UPDATE CreditApplications SET 
        CompanyStructure = @CompanyStructure, 
        BusinessSince = @BusinessSince, 
        PhoneNumber = @PhoneNumber, 
        StreetAddress = @StreetAddress, 
        City = @City, 
        [State] = @State, 
        ZipCode = @ZipCode, 
        BankName = @BankName, 
        AccountNumber = @AccountNumber, 
        IFSCCode = @IFSCCode, 
        BankBranch = @BankBranch, 
        BankState = @BankState, 
        BankCity = @BankCity, 
        BankZipCode = @BankZipCode,
        LogoImagePath = @LogoImagePath
    WHERE ApplicationID = @ApplicationID AND UserID = @UserID; -- Use UserID for extra safety
END
------------------------------------------------------------------------
ALTER PROCEDURE CreditApplications_Delete
    @ApplicationID INT,
    @UserID INT = NULL
AS
BEGIN
    IF @UserID IS NULL
    BEGIN
        -- Employee: delete by ApplicationID only
        DELETE FROM CreditApplications WHERE ApplicationID = @ApplicationID;
    END
    ELSE
    BEGIN
        -- User: delete only their own pending application
        DELETE FROM CreditApplications WHERE ApplicationID = @ApplicationID AND UserID = @UserID AND Status = 'Pending';
    END
END