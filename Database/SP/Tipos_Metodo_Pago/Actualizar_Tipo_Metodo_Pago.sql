USE [DB_ECOMMERCE]
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_PaymentMethodTypes_Update]
    @paymentMethodTypeId INT, @paymentMethodTypeName VARCHAR(50), @paymentMethodTypeDescription VARCHAR(100), @paymentMethodTypeModificatorId INT, @paymentMethodTypeStatusId BIT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_PaymentMethodTypes] SET paymentMethodTypeName = @paymentMethodTypeName, paymentMethodTypeDescription = @paymentMethodTypeDescription, paymentMethodTypeModificatorId = @paymentMethodTypeModificatorId, paymentMethodTypeModificationDate = GETDATE(), paymentMethodTypeStatusId = @paymentMethodTypeStatusId
    WHERE paymentMethodTypeId = @paymentMethodTypeId;
END
GO
