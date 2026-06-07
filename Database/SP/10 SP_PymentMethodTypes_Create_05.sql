USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_PaymentMethodTypes_Create]
    @paymentMethodTypeName VARCHAR(50), @paymentMethodTypeDescription VARCHAR(100), @paymentMethodTypeCreatorId INT, @paymentMethodTypeStatusId BIT
AS BEGIN
    INSERT INTO [SQM_CATALOGS].[Tbl_PaymentMethodTypes] (paymentMethodTypeName, paymentMethodTypeDescription, paymentMethodTypeCreatorId, paymentMethodTypeCreationDate, paymentMethodTypeStatusId)
    VALUES (@paymentMethodTypeName, @paymentMethodTypeDescription, @paymentMethodTypeCreatorId, GETDATE(), @paymentMethodTypeStatusId);
END
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_PaymentMethodTypes_Update]
    @paymentMethodTypeId INT, @paymentMethodTypeName VARCHAR(50), @paymentMethodTypeDescription VARCHAR(100), @paymentMethodTypeModificatorId INT, @paymentMethodTypeStatusId BIT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_PaymentMethodTypes] SET paymentMethodTypeName = @paymentMethodTypeName, paymentMethodTypeDescription = @paymentMethodTypeDescription, paymentMethodTypeModificatorId = @paymentMethodTypeModificatorId, paymentMethodTypeModificationDate = GETDATE(), paymentMethodTypeStatusId = @paymentMethodTypeStatusId
    WHERE paymentMethodTypeId = @paymentMethodTypeId;
END
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_PaymentMethodTypes_Delete]
    @paymentMethodTypeId INT, @paymentMethodTypeModificatorId INT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_PaymentMethodTypes] SET paymentMethodTypeStatusId = 0, paymentMethodTypeModificatorId = @paymentMethodTypeModificatorId, paymentMethodTypeModificationDate = GETDATE() WHERE paymentMethodTypeId = @paymentMethodTypeId;
END
GO

-- 4. LISTAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_PaymentMethodTypes_List]
AS BEGIN
    SELECT paymentMethodTypeId, paymentMethodTypeName, paymentMethodTypeDescription, paymentMethodTypeCreatorId, paymentMethodTypeCreationDate, paymentMethodTypeModificatorId, paymentMethodTypeModificationDate, paymentMethodTypeStatusId
    FROM [SQM_CATALOGS].[Tbl_PaymentMethodTypes] (NOLOCK);
END
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_PaymentMethodTypes_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT paymentMethodTypeId, paymentMethodTypeName, paymentMethodTypeDescription, paymentMethodTypeStatusId
    FROM [SQM_CATALOGS].[Tbl_PaymentMethodTypes] (NOLOCK)
    WHERE (@SearchTerm IS NULL OR paymentMethodTypeName LIKE '%' + @SearchTerm + '%') AND (@StatusId IS NULL OR paymentMethodTypeStatusId = @StatusId);
END
GO