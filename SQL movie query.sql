-----------------------------------
-- CREATE DATABASE
-----------------------------------
CREATE DATABASE Movies;
GO

USE Movies;
GO

-----------------------------------
-- CREATE TABLE: Users
-----------------------------------
CREATE TABLE Users (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    UserName  NVARCHAR(50) NOT NULL,
    Email     NVARCHAR(200) NOT NULL UNIQUE,
    [Password] NVARCHAR(200) NOT NULL
);
GO

-----------------------------------
-- CREATE TABLE: Movies
-----------------------------------
CREATE TABLE Movies (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Title       NVARCHAR(200)    NOT NULL,
    Rating      FLOAT            NOT NULL,
    Income      DECIMAL(18,2)    NOT NULL,
    ReleaseYear INT              NOT NULL,
    Duration    INT              NOT NULL,
    [Language]  NVARCHAR(50)     NOT NULL,
    [Description] NVARCHAR(MAX)  NOT NULL,
    Genre       NVARCHAR(100)    NOT NULL,
    PhotoUrl    NVARCHAR(500)    NOT NULL
);
GO

-----------------------------------
-- CREATE TABLE: Cast
-----------------------------------
CREATE TABLE [Cast] (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    [Name]       NVARCHAR(100) NOT NULL,
    Role         NVARCHAR(200) NULL,
    DateOfBirth  DATE NULL,
    PhotoUrl     NVARCHAR(500) NULL,
    CONSTRAINT CK_Cast_DateOfBirth CHECK (DateOfBirth <= GETDATE())
);
GO

-----------------------------------
-- CREATE TABLE: MovieCast (Many-to-Many)
-----------------------------------
CREATE TABLE MovieCast (
    MovieId INT NOT NULL,
    CastId  INT NOT NULL,
    RoleName NVARCHAR(200) NULL,

    CONSTRAINT PK_MovieCast PRIMARY KEY (MovieId, CastId),

    CONSTRAINT FK_MovieCast_Movies FOREIGN KEY (MovieId)
        REFERENCES Movies(Id) ON DELETE CASCADE,

    CONSTRAINT FK_MovieCast_Cast FOREIGN KEY (CastId)
        REFERENCES [Cast](Id) ON DELETE CASCADE
);
GO

-----------------------------------
-- INSERT Users
-----------------------------------
INSERT INTO Users (UserName, Email, [Password]) VALUES
(N'admin', N'admin@example.com', N'1234'),
(N'lital', N'lital@example.com', N'l12345'),
(N'shay',  N'shay@example.com',  N'pass789');
GO

-----------------------------------
-- INSERT Movies
-----------------------------------
INSERT INTO Movies
(Title, Rating, Income, ReleaseYear, Duration, [Language], [Description], Genre, PhotoUrl)
VALUES
(N'Inception', 8.8, 825532764.00, 2010, 148, N'English',
 N'חלומות בתוך חלומות וסיפור מדע בדיוני עוצמתי',
 N'Sci-Fi',
 N'https://example.com/inception.jpg'),

(N'The Dark Knight', 9.0, 1004558444.00, 2008, 152, N'English',
 N'באטמן נגד הג׳וקר בסרט פעולה אגדי',
 N'Action',
 N'https://example.com/darkknight.jpg'),

(N'Titanic', 7.9, 2187463944.00, 1997, 195, N'English',
 N'סיפור אהבה טראגי בספינה הטיטאנית',
 N'Drama',
 N'https://example.com/titanic.jpg');
GO

-----------------------------------
-- INSERT Cast
-----------------------------------
INSERT INTO [Cast] ([Name], Role, DateOfBirth, PhotoUrl)
VALUES
(N'Leonardo DiCaprio', N'Actor', '1974-11-11', N'https://example.com/leo.jpg'),
(N'Christian Bale',    N'Actor', '1974-01-30', N'https://example.com/bale.jpg'),
(N'Kate Winslet',      N'Actress', '1975-10-05', N'https://example.com/winslet.jpg');
GO

-----------------------------------
-- INSERT MovieCast
-----------------------------------
INSERT INTO MovieCast (MovieId, CastId, RoleName) VALUES
(1, 1, N'Cob'),
(2, 2, N'Batman'),
(3, 1, N'Jack Dawson'),
(3, 3, N'Rose DeWitt Bukater');
GO

-----------------------------------
-- STORED PROCEDURE: GetAllMovies
-----------------------------------
CREATE OR ALTER PROCEDURE sp_GetAllMovies
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Movies;
END
GO

-----------------------------------
-- STORED PROCEDURE: InsertMovie
-----------------------------------
CREATE OR ALTER PROCEDURE sp_InsertMovie
    @Title        NVARCHAR(200),
    @Rating       FLOAT,
    @Income       DECIMAL(18,2),
    @ReleaseYear  INT,
    @Duration     INT,
    @Language     NVARCHAR(50),
    @Description  NVARCHAR(MAX),
    @Genre        NVARCHAR(100),
    @PhotoUrl     NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Movies
    (Title, Rating, Income, ReleaseYear, Duration, [Language], [Description], Genre, PhotoUrl)
    VALUES
    (@Title, @Rating, @Income, @ReleaseYear, @Duration, @Language, @Description, @Genre, @PhotoUrl);

    SELECT SCOPE_IDENTITY() AS NewMovieId;
END
GO

-----------------------------------
-- STORED PROCEDURE: GetAllCast
-----------------------------------
CREATE OR ALTER PROCEDURE sp_GetAllCast
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [Cast];
END
GO

-----------------------------------
-- STORED PROCEDURE: RegisterUser
-----------------------------------
CREATE OR ALTER PROCEDURE sp_RegisterUser
    @UserName  NVARCHAR(50),
    @Email     NVARCHAR(200),
    @Password  NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
    BEGIN
        RAISERROR(N'Email already exists', 16, 1);
        RETURN;
    END

    INSERT INTO Users (UserName, Email, [Password])
    VALUES (@UserName, @Email, @Password);

    SELECT SCOPE_IDENTITY() AS NewUserId;
END
GO

-----------------------------------
-- STORED PROCEDURE: LoginUser
-----------------------------------
CREATE OR ALTER PROCEDURE sp_LoginUser
    @Email     NVARCHAR(200),
    @Password  NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, UserName, Email
    FROM Users
    WHERE Email = @Email
      AND [Password] = @Password;
END
GO
