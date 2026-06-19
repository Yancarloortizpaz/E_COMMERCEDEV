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
