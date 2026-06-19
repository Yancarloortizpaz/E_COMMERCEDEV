USE [DB_ECOMMERCE]
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_PaymentMethodTypes_Delete]
    @paymentMethodTypeId INT, @paymentMethodTypeModificatorId INT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_PaymentMethodTypes] SET paymentMethodTypeStatusId = 0, paymentMethodTypeModificatorId = @paymentMethodTypeModificatorId, paymentMethodTypeModificationDate = GETDATE() WHERE paymentMethodTypeId = @paymentMethodTypeId;
END
GO
