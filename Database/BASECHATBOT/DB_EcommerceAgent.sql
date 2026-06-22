CREATE DATABASE [DB_EcommerceAgent]
GO

USE [DB_EcommerceAgent]
GO

CREATE TABLE ReglasChatbot (
    ReglaID INT PRIMARY KEY IDENTITY(1,1),
    NombreRegla VARCHAR(100) NOT NULL,
    AccionDinamica BIT NOT NULL,     
    AccionPython VARCHAR(100) NULL,         
    Activo BIT
);

CREATE TABLE PalabrasClaveRegla (
    PalabraClaveID INT PRIMARY KEY IDENTITY(1,1),
    ReglaID INT NOT NULL REFERENCES ReglasChatbot(ReglaID),
    PalabraClave VARCHAR(100) NOT NULL,
	Activo BIT
);

CREATE TABLE PlantillasRespuesta (
    PlantillaID INT IDENTITY(1,1) PRIMARY KEY,
    ReglaID INT NOT NULL REFERENCES ReglasChatbot(ReglaID),
    TextoRespuesta NVARCHAR(MAX) NOT NULL, 
    Activo BIT
);

CREATE TABLE HistorialConversaciones
(
	ConversacionID BIGINT IDENTITY(1,1) PRIMARY KEY,
	UsuarioID VARCHAR(100) NOT NULL,
	FechaInicio DATETIME NOT NULL,
	FechaFin DATETIME NULL,
	Activo BIT
)

CREATE TABLE HistorialMensajes (
    MensajeID BIGINT IDENTITY(1,1) PRIMARY KEY,
	ConversacionID BIGINT REFERENCES HistorialConversaciones (ConversacionID) NOT NULL,
    ChatBot BIT NOT NULL,           
    Texto VARCHAR(1000) NOT NULL,             
    FechaHora DATETIME NOT NULL,     
    ReglaActivadaID INT REFERENCES ReglasChatbot(ReglaID),               
);
GO