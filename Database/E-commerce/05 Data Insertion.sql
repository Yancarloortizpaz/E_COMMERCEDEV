USE [DB_ECOMMERCE]
GO

INSERT INTO [SQM_CATALOGS].[Tbl_Status](statusName,statusCreatorId,statusCreationDate,statusStatusId)
VALUES
('ACTIVO', 1, GETDATE(), 1),
('INACTIVO', 1, GETDATE(), 1),
('BLOQUEADO', 1, GETDATE(), 1),
('PROCESADO', 1, GETDATE(), 1),
('ENTREGADO', 1, GETDATE(), 1),
('ANULADO', 1, GETDATE(), 1);

OPEN SYMMETRIC KEY KEY_HASH
DECRYPTION BY CERTIFICATE CERT_ECOMMERCE;

  DECLARE
	@UserPassword VARCHAR(256),
	@UserPasswordEncrypted VARBINARY(256)
  
  SET @UserPassword = '12345'
  SET @UserPasswordEncrypted = SQM_SECURITY.Fn_EncryptByKey(@UserPassword)
  


INSERT INTO [SQM_SECURITY].[Tbl_Users](userFullName,userName,userPassword,userEmail,userPhoneNumber,userCountryId,userGenderId,userBirthDay,userCreatorId,userCreationDate,userStatusId)
VALUES
('HECTOR JOSE CALERO ALANIZ', 'HCALERO', @UserPasswordEncrypted, 'hcalero@dominio.local','88888888', 1, 2,'2000-05-12', 1, GETDATE(), 1),
('DAVID JOSHUA JIMENEZ URBINA', 'DURBINA', @UserPasswordEncrypted, 'durbina@dominio.local','99999999', 1, 2,'1995-10-21', 1, GETDATE(), 1);

CLOSE SYMMETRIC KEY KEY_HASH;
GO

INSERT INTO [SQM_GENERAL].[Tbl_UserAddress](userAddressUserId,userAddressCountryId,userAddressZIPCode,userAddressDescription,userAddressIsPrincipal,userAddressCreatorId,userAddressCreationDate,userAddressStatusId)
VALUES 
(1,1,'1001','Ciudad el Doral, Mateare', 1, 1, GETDATE(), 1),
(2,1,'1002','Ciudad Sandino, Managua', 1, 1, GETDATE(), 1);

INSERT INTO [SQM_CATALOGS].[Tbl_Categories](categoryName,categoryDescription,categoryCreatorId,categoryCreationDate,categoryStatusId)
VALUES
('CALZADO', 'CALZADO EN GENERAL', 1, GETDATE(), 1),
('ROPA', 'ROPA EN GENERAL', 1, GETDATE(), 1),
('TECNOLOGIA', 'TECNOLOGIA EN GENERAL', 1, GETDATE(), 1),
('ACCESORIOS', 'ACCESORIOS EN GENERAL', 1, GETDATE(), 1);

INSERT INTO [SQM_CATALOGS].[Tbl_SubCategories](subCategoryName,subCategoryDescription,subCategoryCreatorId,subCategoryCreationDate,subCategoryStatusId)
VALUES
('MASCULINO', 'ROPA MASCULINA', 1, GETDATE(), 1),
('FEMENINO', 'ROPA FEMENINA', 1, GETDATE(), 1),
('NIÑOS', 'ROPA PARA NIÑOS', 1, GETDATE(), 1),
('NIÑAS', 'ROPA PARA NIÑAS', 1, GETDATE(), 1),
('CELULARES', 'CELULARES EN GENERAL', 1, GETDATE(), 1),
('COMPUTADORAS', 'COMPUTADORAS EN GENERAL', 1, GETDATE(), 1);

INSERT INTO [SQM_CATALOGS].[Tbl_Segments](segmentName,segmentDescription,segmentCreatorId,segmentCreationDate,segmentStatusId)
VALUES
('DEPORTIVO', 'SEGMENTO DEPORTIVO', 1, GETDATE(), 1),
('CASUAL', 'SEGMENTO CASUAL', 1, GETDATE(), 1),
('ELEGANTE', 'SEGMENTO ELEGANTE', 1, GETDATE(), 1),
('OFICINA', 'SEGMENTO OFICINA', 1, GETDATE(), 1),
('GAMING', 'SEGMENTO GAMING', 1, GETDATE(), 1),
('HOGAR', 'SEGMENTO HOGAR', 1, GETDATE(), 1),
('LAPTOS', 'SEGMENTO LAPTOS', 1, GETDATE(), 1),
('REPUESTOS', 'SEGMENTO REPUESTOS', 1, GETDATE(), 1);

INSERT INTO [SQM_CATALOGS].[Tbl_ProductIdentificators](productIdentificatorCategoryId,productIdentificatorSubCategoryId,productIdentificatorSegmentId,productIdentificatorCreatorId,productIdentificatorCreationDate,productIdentificatorStatusId)
VALUES
(1,1,1,1,GETDATE(),1),
(1,2,1,1,GETDATE(),1),
(1,3,1,1,GETDATE(),1),
(1,4,1,1,GETDATE(),1),
(2,1,1,1,GETDATE(),1),
(2,2,1,1,GETDATE(),1),
(2,3,1,1,GETDATE(),1),
(2,4,1,1,GETDATE(),1),
(3,5,4,1,GETDATE(),1),
(3,5,5,1,GETDATE(),1),
(3,6,4,1,GETDATE(),1),
(3,6,5,1,GETDATE(),1),
(3,6,7,1,GETDATE(),1);
GO

INSERT INTO [SQM_CATALOGS].[Tbl_StockMovementTypes] 
(stockMovementTypeName, stockMovementTypeDescription, stockMovementTypeCreatorId, stockMovementTypeCreationDate, stockMovementTypeStatusId)
VALUES 
('ENTRADA POR COMPRA', 'Ingreso de mercadería de un proveedor', 1, GETDATE(), 1),
('SALIDA POR VENTA', 'Despacho de mercadería por orden de cliente', 1, GETDATE(), 1),
('AJUSTE POSITIVO', 'Suma al inventario por descuadre o devolución', 1, GETDATE(), 1),
('AJUSTE NEGATIVO', 'Resta al inventario por merma, daño o pérdida', 1, GETDATE(), 1);
GO

-----------------------------------------
-- --------- NUEVAS INSERCIONES ---------
-----------------------------------------

-- 1. PROVEEDORES (Providers)
INSERT INTO [SQM_CATALOGS].[Tbl_Providers](providerName, providerDescription, providerCreatorId, providerCreationDate, providerStatusId)
VALUES 
('DISTRIBUIDORA ADIDAS', 'Proveedor mayorista de calzado y ropa Adidas', 1, GETDATE(), 1),
('DISTRIBUIDORA NIKE', 'Proveedor oficial de productos Nike', 1, GETDATE(), 1),
('COMPU SOLUCIONES', 'Proveedor de hardware y laptops', 1, GETDATE(), 1);
GO

-- 2. MARCAS (Marks)
INSERT INTO [SQM_CATALOGS].[Tbl_Marks](markName, markDescription, markCreatorId, markCreationDate, markStatusId)
VALUES 
('ADIDAS', 'Marca Adidas global', 1, GETDATE(), 1),
('NIKE', 'Marca Nike global', 1, GETDATE(), 1),
('DELL', 'Marca de computadoras Dell', 1, GETDATE(), 1);
GO

-- 3. RELACION MARCA Y PROVEEDOR (MarkByProviders)
INSERT INTO [SQM_CATALOGS].[Tbl_MarkByProviders](markByProviderMarkId, markByProviderProviderId, markByProviderCreatorId, markByProviderCreationDate, markByProviderStatusId)
VALUES 
(1, 1, 1, GETDATE(), 1), -- Adidas con Adidas Distribuidora
(2, 2, 1, GETDATE(), 1), -- Nike con Nike Distribuidora
(3, 3, 1, GETDATE(), 1); -- Dell con Compu Soluciones
GO

-- 4. MONEDAS (Currencies)
INSERT INTO [SQM_CATALOGS].[Tbl_Currencies](currencyName, currencyISO, currencyCode, currencyDescription, currencyCreatorId, currencyCreationDate, currencyStatusId)
VALUES 
('DOLAR ESTADOUNIDENSE', 'USD', 840, 'Dólar de los Estados Unidos', 1, GETDATE(), 1),
('CORDOBA NICARAGUENSE', 'NIO', 558, 'Córdoba Oro de Nicaragua', 1, GETDATE(), 1);
GO

-- 5. PRODUCTOS (Products)
INSERT INTO [SQM_GENERAL].[Tbl_Products](productName, productDescription, productProductIdentificatorId, productMarkByProviderId, productCreatorId, productCreationDate, productStatusId)
VALUES 
('RUNNING ULTRABOOST 22', 'Zapatos para correr Ultraboost 22', 1, 1, 1, GETDATE(), 1), 
('AIR MAX 90 CASUAL', 'Zapatos Nike Air Max 90', 2, 2, 1, GETDATE(), 1), 
('DELL INSPIRON 15', 'Laptop Dell Inspiron 15 Core i5', 13, 3, 1, GETDATE(), 1); 
GO

-- 6. VARIANTES DE PRODUCTO (ProductVariables)
INSERT INTO [SQM_GENERAL].[Tbl_ProductVariables](productVariableProductId, productVariableValue, productVariablePrice, productVariableCurrencyId, productVariableCreatorId, productVariableCreationDate, productVariableStatusId)
VALUES 
(1, 'TALLA 42 (9.5 US)', 180.00, 1, 1, GETDATE(), 1), 
(1, 'TALLA 43 (10 US)', 180.00, 1, 1, GETDATE(), 1),  
(2, 'TALLA 38 (7 US)', 140.00, 1, 1, GETDATE(), 1),   
(3, '8GB RAM - 512GB SSD', 650.00, 1, 1, GETDATE(), 1); 
GO

-- 7. STOCK DE INVENTARIO (Stocks)
INSERT INTO [SQM_GENERAL].[Tbl_Stocks](stockProductVariableId, stockQuantity, stockFactoryDate, stockExpirationDate, stockCreatorId, stockCreationDate, stockStatusId)
VALUES 
(1, 25, '2025-01-01', '2030-01-01', 1, GETDATE(), 1),
(2, 30, '2025-01-01', '2030-01-01', 1, GETDATE(), 1),
(3, 15, '2025-02-01', '2030-02-01', 1, GETDATE(), 1),
(4, 10, '2025-03-01', '2030-03-01', 1, GETDATE(), 1);
GO

-- 8. TIPOS DE METODOS DE PAGO (PaymentMethodTypes)
INSERT INTO [SQM_CATALOGS].[Tbl_PaymentMethodTypes](paymentMethodTypeName, paymentMethodTypeDescription, paymentMethodTypeCreatorId, paymentMethodTypeCreationDate, paymentMethodTypeStatusId)
VALUES 
('TARJETA DE CREDITO', 'Visa/Mastercard de crédito', 1, GETDATE(), 1),
('TARJETA DE DEBITO', 'Visa/Mastercard de débito', 1, GETDATE(), 1);
GO

-- 9. METODOS DE PAGO DE USUARIO (UserPaymentMethods)
OPEN SYMMETRIC KEY KEY_HASH DECRYPTION BY CERTIFICATE CERT_ECOMMERCE;

INSERT INTO [SQM_GENERAL].[Tbl_UserPaymentMethods](userPaymentMethodUserId, userPaymentMethodPaymentMethodTypeId, userPaymentMethodCardNumber, userPaymentMethodExpirationDate, userPaymentMethodCVV, userPaymentMethodCardHolderName, userPaymentMethodCreatorId, userPaymentMethodCreationDate, userPaymentMethodStatusId)
VALUES 
(1, 1, SQM_SECURITY.Fn_EncryptByKey('1234567812345678'), SQM_SECURITY.Fn_EncryptByKey('12/28'), SQM_SECURITY.Fn_EncryptByKey('123'), 'HECTOR JOSE CALERO ALANIZ', 1, GETDATE(), 1),
(2, 2, SQM_SECURITY.Fn_EncryptByKey('8765432187654321'), SQM_SECURITY.Fn_EncryptByKey('06/29'), SQM_SECURITY.Fn_EncryptByKey('456'), 'DAVID JOSHUA JIMENEZ URBINA', 1, GETDATE(), 1);

CLOSE SYMMETRIC KEY KEY_HASH;
GO

-- 10. ORDENES DE PAGO (PaymentOrders)
INSERT INTO [SQM_GENERAL].[Tbl_PaymentOrders](orderUserId, orderDeliveryAddress, orderPaymentMethodId, orderSubtotal, orderDiscount, orderShipping, orderTAX, orderTotal, orderCurrencyId, orderCreatorId, orderCreationDate, orderStatusId)
VALUES 
(1, 1, 1, 360.00, 10.00, 5.00, 52.50, 407.50, 1, 1, GETDATE(), 4), 
(2, 2, 2, 650.00, 00.00, 10.00, 97.50, 757.50, 1, 1, GETDATE(), 4); 
GO

-- 11. DETALLES DE ORDEN (PaymentOrderDetails)
INSERT INTO [SQM_GENERAL].[Tbl_PaymentOrderDetails](orderDetailOrderId, orderDetailProductVariableId, orderDetailPrice, orderDetailQuantity, orderDetailDiscount, orderDetailSubTotal, orderDetailTAX, orderDetailTotal, orderDetailCurrencyId, orderDetailCreatorId, orderDetailCreationDate, orderDetailStatusId)
VALUES 
(1, 1, 180.00, 2, 10.00, 350.00, 52.50, 402.50, 1, 1, GETDATE(), 1), 
(2, 4, 650.00, 1, 00.00, 650.00, 97.50, 747.50, 1, 1, GETDATE(), 1); 
GO
