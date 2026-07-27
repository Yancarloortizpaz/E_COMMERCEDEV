USE [DB_EcommerceAgent]
GO

CREATE OR ALTER PROCEDURE dbo.sp_GuardarConversacion
    @ConversacionID BIGINT = NULL, -- Puede venir NULL si es una conversación nueva
    @UsuarioID VARCHAR(100),
    @ChatBot BIT,
    @Texto VARCHAR(1000),
    @ReglaActivadaID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IDFinal BIGINT = @ConversacionID;

    -- 1. Si no viene ID de conversación o no existe en la BD, se crea automáticamente
    IF @IDFinal IS NULL OR NOT EXISTS (SELECT 1 FROM dbo.HistorialConversaciones WHERE ConversacionID = @IDFinal)
    BEGIN
        INSERT INTO dbo.HistorialConversaciones (UsuarioID, FechaInicio, Activo)
        VALUES (@UsuarioID, GETDATE(), 1);
        
        -- Capturamos el ID autogenerado por la propiedad IDENTITY de SQL Server
        SET @IDFinal = SCOPE_IDENTITY();
    END

    -- 2. Insertamos el mensaje ligado al ID autogenerado
    INSERT INTO dbo.HistorialMensajes (ConversacionID, ChatBot, Texto, FechaHora, ReglaActivadaID)
    VALUES (@IDFinal, @ChatBot, @Texto, GETDATE(), @ReglaActivadaID);

    -- 3. Retornamos el ID final para que FastAPI se lo envíe al Frontend
    SELECT @IDFinal AS ConversacionID;
END;
GO