USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Carts_Create]
    @cartUserId INT, @cartCreatorId INT, @cartStatusId BIT
AS BEGIN
    INSERT INTO [SQM_GENERAL].[Tbl_Carts] 
    (cartUserId, cartCreatorId, cartCreationDate, cartStatusId)
    VALUES 
    (@cartUserId, @cartCreatorId, GETDATE(), @cartStatusId);
END
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Carts_Update]
    @cartId INT, @cartUserId INT, @cartModificatorId INT, @cartStatusId BIT
AS BEGIN
    UPDATE [SQM_GENERAL].[Tbl_Carts] 
    SET cartUserId = @cartUserId, cartModificatorId = @cartModificatorId, cartModificationDate = GETDATE(), cartStatusId = @cartStatusId
    WHERE cartId = @cartId;
END
GO

-- 3. ELIMINAR (Borrado Físico)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Carts_Delete]
    @cartId INT
AS BEGIN
    -- Al ser un borrado físico, se elimina el registro permanentemente
    DELETE FROM [SQM_GENERAL].[Tbl_Carts] 
    WHERE cartId = @cartId;
END
GO

-- 4. LISTAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Carts_List]
AS BEGIN
    SELECT 
        cartId, cartUserId, cartCreatorId, cartCreationDate, 
        cartModificatorId, cartModificationDate, cartStatusId
    FROM [SQM_GENERAL].[Tbl_Carts] (NOLOCK);
END
GO