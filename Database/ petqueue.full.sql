```sql
/* =========================================================
   PetQueue – Full Database Initialization Script
   ========================================================= */

------------------------------------------------------------
-- 1. Database
------------------------------------------------------------
IF DB_ID('PetQueue') IS NULL
    CREATE DATABASE PetQueue;
GO

USE PetQueue;
GO

------------------------------------------------------------
-- 2. Tables
------------------------------------------------------------

-- Users
IF OBJECT_ID('dbo.Users', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users (
        UserId INT IDENTITY PRIMARY KEY,
        Username NVARCHAR(100) NOT NULL,
        PasswordHash NVARCHAR(MAX) NOT NULL,
        FirstName NVARCHAR(100) NOT NULL
    );
END
GO

-- DogTypes
IF OBJECT_ID('dbo.DogTypes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.DogTypes (
        TypeId INT IDENTITY PRIMARY KEY,
        TypeName NVARCHAR(40) NOT NULL,
        Duration INT NOT NULL,
        Price DECIMAL(10,2) NOT NULL
    );
END
GO

-- Appointments
IF OBJECT_ID('dbo.Appointments', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Appointments (
        AppointmentId INT IDENTITY PRIMARY KEY,
        UserId INT NULL,
        TypeId INT NULL,
        ScheduledTime DATETIME NOT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
        FinalPrice DECIMAL(10,2) NOT NULL
    );
END
GO

------------------------------------------------------------
-- 3. Indexes
------------------------------------------------------------

CREATE UNIQUE INDEX IX_Users_Username
ON dbo.Users (Username);
GO

CREATE INDEX IX_Appointments_UserId
ON dbo.Appointments (UserId);
GO

CREATE INDEX IX_Appointments_TypeId
ON dbo.Appointments (TypeId);
GO

CREATE INDEX IX_Appointments_ScheduledTime
ON dbo.Appointments (ScheduledTime);
GO

------------------------------------------------------------
-- 4. Foreign Keys
------------------------------------------------------------

ALTER TABLE dbo.Appointments
ADD CONSTRAINT FK_Appointments_Users
    FOREIGN KEY (UserId)
    REFERENCES dbo.Users(UserId)
    ON DELETE SET NULL;
GO

ALTER TABLE dbo.Appointments
ADD CONSTRAINT FK_Appointments_DogTypes
    FOREIGN KEY (TypeId)
    REFERENCES dbo.DogTypes(TypeId)
    ON DELETE SET NULL;
GO

------------------------------------------------------------
-- 5. Views
------------------------------------------------------------

IF OBJECT_ID('dbo.ClientAppointments_View', 'V') IS NOT NULL
    DROP VIEW dbo.ClientAppointments_View;
GO

CREATE VIEW dbo.ClientAppointments_View AS
SELECT 
    A.AppointmentId,
    U.FirstName AS ClientName,
    U.UserId,
    D.TypeName AS DogSize,
    A.ScheduledTime,
    A.CreatedAt,
    A.FinalPrice
FROM dbo.Appointments A
JOIN dbo.Users U ON A.UserId = U.UserId
JOIN dbo.DogTypes D ON A.TypeId = D.TypeId;
GO

------------------------------------------------------------
-- 6. Stored Procedures (5)
------------------------------------------------------------

/* 1️ Create User */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.spCreateUser
    @Username NVARCHAR(50),
    @PasswordHash NVARCHAR(MAX),
    @FirstName NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Users (Username, PasswordHash, FirstName)
    VALUES (@Username, @PasswordHash, @FirstName);

    SELECT @@ROWCOUNT;
END
GO

/* 2️ Get User By Username */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.spGetUserByUsername
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Users
    WHERE Username = @Username;
END
GO

/* 3️ Create Appointment (with discount logic) */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.spCreateAppointment
    @UserId INT,
    @TypeId INT,
    @ScheduledTime DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Price DECIMAL(10,2);
    DECLARE @PastCount INT;
    DECLARE @FinalPrice DECIMAL(10,2);

    SELECT @Price = Price
    FROM dbo.DogTypes
    WHERE TypeId = @TypeId;

    SELECT @PastCount = COUNT(*)
    FROM dbo.Appointments
    WHERE UserId = @UserId;

    IF @PastCount > 3
        SET @FinalPrice = @Price * 0.9;
    ELSE
        SET @FinalPrice = @Price;

    INSERT INTO dbo.Appointments (UserId, TypeId, ScheduledTime, FinalPrice)
    VALUES (@UserId, @TypeId, @ScheduledTime, @FinalPrice);
END
GO

/* 4️ Update Appointment */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.spSetAppointment
   @AppointmentId INT,
    @UserId INT,
    @TypeId INT,
    @ScheduledTime DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @price DECIMAL(10,2);
    DECLARE @FinalPrice DECIMAL(10,2);
    DECLARE @PastCount INT;


    select @price = Price from DogTypes
    where TypeId = @TypeId

  SELECT @PastCount = COUNT(*) FROM Appointments WHERE UserId = @UserId;

    IF @PastCount > 3
        SET @FinalPrice = @Price * 0.9; -- 10% discount
    ELSE
        SET @FinalPrice = @Price;

    UPDATE Appointments 
    SET TypeId = @TypeId, 
        ScheduledTime = @ScheduledTime,
        FinalPrice = @FinalPrice
    WHERE AppointmentId = @AppointmentId 
      AND UserId = @UserId;

    SELECT @@ROWCOUNT;
    END
GO

/* 5️ Delete Appointment */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.spDeleteAppointment
    @AppointmentId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.Appointments
    WHERE AppointmentId = @AppointmentId;

    SELECT @@ROWCOUNT;
END
GO

------------------------------------------------------------
-- END OF FILE
------------------------------------------------------------
```
