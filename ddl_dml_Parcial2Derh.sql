﻿﻿CREATE DATABASE Parcial2Derh;
GO
USE [master]
GO
CREATE LOGIN [usrparcial2] WITH PASSWORD = N'12345678',
 DEFAULT_DATABASE = [Parcial2Derh],
 CHECK_EXPIRATION = OFF,
 CHECK_POLICY = ON
GO
USE [Parcial2Derh]
GO
CREATE USER [usrparcial2] FOR LOGIN [usrparcial2]
GO
ALTER ROLE [db_owner] ADD MEMBER [usrparcial2]
GO

DROP TABLE Serie;

CREATE TABLE Serie (
id INT PRIMARY KEY IDENTITY(1,1),
titulo VARCHAR(250) NOT NULL,
sinopsis VARCHAR(5000) NOT NULL,
director VARCHAR(100) NOT NULL,
episodios INT NOT NULL,
fechaEstreno DATE,
urlPortada VARCHAR(500) NOT NULL,
idiomaOriginal VARCHAR(50) NOT NULL,
estado SMALLINT NOT NULL DEFAULT 1
);

ALTER TABLE Serie ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Serie ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
GO

CREATE PROC paSerieListar @parametro VARCHAR(100)
AS
SELECT * FROM Serie
WHERE estado <> -1 AND (
      titulo LIKE '%' + REPLACE(@parametro, ' ', '%') + '%'
   OR director LIKE '%' + REPLACE(@parametro, ' ', '%') + '%'
)


 GO
 EXEC paSerieListar 'Naruto';

INSERT INTO Serie(titulo, sinopsis, director, episodios, fechaEstreno,urlPortada,idiomaOriginal)
VALUES ('Naruto','un joven ninja','Akira Toriyama', 120, '2001-01-01','https://www.youtube.com/','idionma');