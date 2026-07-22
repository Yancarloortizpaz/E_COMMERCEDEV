CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Categories_Update]
(
    @categoryId INT,
    @categoryName VARCHAR(50),
    @categoryDescription VARCHAR(100),
    @categoryModificatorId INT,
    @categoryStatusId BIT,
    @ForzarRecuperacion BIT = 0,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Categories_Delete]
(
    @categoryId INT,
    @categoryModificatorId INT,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS




CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Categories_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    DECLARE @SearchId INT = TRY_CAST(@SearchTerm AS INT);

    SELECT categoryId, categoryName, categoryDescription, categoryStatusId


    CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Categories_Create]
(
    @categoryName VARCHAR(50),
    @categoryDescription VARCHAR(100),
    @categoryCreatorId INT,
    @categoryStatusId BIT,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Categories_List]
AS BEGIN
    SELECT categoryId, categoryName, categoryDescription, categoryCreatorId, categoryStatusId
   