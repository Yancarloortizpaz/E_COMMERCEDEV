USE [DB_ECOMMERCE]
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_UserPaymentMethods_Delete]
    @userPaymentMethodId INT, @userPaymentMethodModificatorId INT
AS
BEGIN
    UPDATE [SQM_GENERAL].[Tbl_UserPaymentMethods]
    SET userPaymentMethodStatusId = 0, userPaymentMethodModificatorId = @userPaymentMethodModificatorId, userPaymentMethodModificationDate = GETDATE()
    WHERE userPaymentMethodId = @userPaymentMethodId;
END
GO
