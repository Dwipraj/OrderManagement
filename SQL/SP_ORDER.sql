USE [ORDER_MANAGEMENT]
GO
/****** Object:  StoredProcedure [dbo].[SP_ORDER]    Script Date: 04-06-2021 17:38:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_ORDER]
(  
    @Id INT = NULL,  
    @FirstName NVARCHAR(50) = NULL,  
    @LastName NVARCHAR(50) = NULL,  
	@Email NVARCHAR(100) = NULL,  
	@OrderDate DATETIMEOFFSET(7) = NULL,
	@Total DECIMAL(18,2) = null,
	@IsDeleted BIT = null,
	@CreatedAt DATETIMEOFFSET(7) = NULL,
	@UpdatedAt DATETIMEOFFSET(7) = NULL,
	@DeletedAt DATETIMEOFFSET(7) = NULL,

	@Search NVARCHAR(150) = null,
	@SortByColumn NVARCHAR(150) = null,
	@SortByOrder NVARCHAR(150) = null,
	@Offset INT = null,
	@PageSize INT = null,

	@Username NVARCHAR(100) = NULL,  

    @ActionType VARCHAR(25)  
)  
AS  
BEGIN  
    IF @ActionType = 'SaveData'  
    BEGIN  
        IF NOT EXISTS (SELECT * FROM [ORDER] WHERE Id=@Id)  
        BEGIN  
            INSERT INTO [ORDER] ([EMAIL]
           ,[FIRST_NAME]
           ,[LAST_NAME]
           ,[ORDER_DATE]
           ,[TOTAL]
           ,[IS_DELETED]
           ,[CREATED_AT])  
            VALUES (@Email, @FirstName, @LastName, @OrderDate, @Total, @IsDeleted, @CreatedAt)  

			SELECT [ID] as Id
		  ,[EMAIL] as Email
		  ,[FIRST_NAME] as FirstName
		  ,[LAST_NAME] as LastName
		  ,[ORDER_DATE] as OrderDate
		  ,[TOTAL] as Total
		  ,[IS_DELETED] as IsDeleted
		  ,[CREATED_AT] as CreatedAt
		  ,[UPDATED_AT] as UpdatedAt
		  ,[DELETED_AT] as DeletedAt FROM [ORDER] where [IS_DELETED] = 0 and [ID] = SCOPE_IDENTITY()
        END  
        ELSE  
        BEGIN  
			--Never update EMAIL of the Order
            UPDATE [ORDER] SET [TOTAL]=@Total, [UPDATED_AT]=@UpdatedAt WHERE ID=@Id and [IS_DELETED] = 0 AND
		(@Username is null OR RTRIM(LTRIM(@Username)) = '' OR [EMAIL] = @Username)
        END  
    END  
    ELSE IF @ActionType = 'DeleteData'  
    BEGIN  
        UPDATE [ORDER] SET [IS_DELETED] = @IsDeleted, [DELETED_AT]=@DeletedAt WHERE ID=@Id AND
		(@Username is null OR RTRIM(LTRIM(@Username)) = '' OR [EMAIL] = @Username)
    END  
    ELSE IF @ActionType = 'FetchAllData'  
    BEGIN  
        SELECT [ID] as Id
      ,[EMAIL] as Email
      ,[FIRST_NAME] as FirstName
      ,[LAST_NAME] as LastName
      ,[ORDER_DATE] as OrderDate
      ,[TOTAL] as Total
      ,[IS_DELETED] as IsDeleted
      ,[CREATED_AT] as CreatedAt
      ,[UPDATED_AT] as UpdatedAt
      ,[DELETED_AT] as DeletedAt FROM [ORDER] where [IS_DELETED] = 0 AND
		(@Username is null OR RTRIM(LTRIM(@Username)) = '' OR [EMAIL] = @Username)
    END  
    ELSE IF @ActionType = 'FetchData'  
    BEGIN  
        SELECT [ID] as Id
      ,[EMAIL] as Email
      ,[FIRST_NAME] as FirstName
      ,[LAST_NAME] as LastName
      ,[ORDER_DATE] as OrderDate
      ,[TOTAL] as Total
      ,[IS_DELETED] as IsDeleted
      ,[CREATED_AT] as CreatedAt
      ,[UPDATED_AT] as UpdatedAt
      ,[DELETED_AT] as DeletedAt FROM [ORDER] where [IS_DELETED] = 0 and [ID] = @Id AND
		(@Username is null OR RTRIM(LTRIM(@Username)) = '' OR [EMAIL] = @Username)
    END  
	ELSE IF @ActionType = 'FetchPaginatedData'  
    BEGIN  
        SELECT [ID] as Id
		,[EMAIL] as Email
		,[FIRST_NAME] as FirstName
		,[LAST_NAME] as LastName
		,[ORDER_DATE] as OrderDate
		,[TOTAL] as Total
		,[IS_DELETED] as IsDeleted
		,[CREATED_AT] as CreatedAt
		,[UPDATED_AT] as UpdatedAt
		,[DELETED_AT] as DeletedAt FROM [ORDER] 
		WHERE [IS_DELETED] = 0 AND
		(
			(@Search is null OR LTRIM(RTRIM(@Search)) = '') OR
			(CAST(ISNULL([ID], '') as nvarchar) + CAST(ISNULL([EMAIL], '') as nvarchar) + CAST(ISNULL([FIRST_NAME], '') as nvarchar) + CAST(ISNULL([LAST_NAME], '') as nvarchar) like '%' + @Search + '%')
		) AND
		(@Username is null OR RTRIM(LTRIM(@Username)) = '' OR [EMAIL] = @Username)
		ORDER BY
		CASE
        WHEN @SortByOrder <> 'ASC' THEN 0
        WHEN @SortByColumn = 'Id' THEN ID
        END ASC
,       CASE
        WHEN @SortByOrder <> 'ASC' THEN ''
        WHEN @SortByColumn = 'FirstName' THEN FIRST_NAME
        END ASC
,       CASE
        WHEN @SortByOrder <> 'ASC' THEN ''
        WHEN @SortByColumn = 'LastName' THEN LAST_NAME
        END ASC
,       CASE
        WHEN @SortByOrder <> 'ASC' THEN 0
        WHEN @SortByColumn = 'Total' THEN TOTAL
        END ASC
,       CASE
        WHEN @SortByOrder <> 'DESC' THEN 0
        WHEN @SortByColumn = 'Id' THEN ID
        END DESC
,       CASE
        WHEN @SortByOrder <> 'DESC' THEN ''
        WHEN @SortByColumn = 'FirstName' THEN FIRST_NAME
        END DESC
,       CASE
        WHEN @SortByOrder <> 'DESC' THEN ''
        WHEN @SortByColumn = 'LastName' THEN LAST_NAME
        END DESC
,       CASE
        WHEN @SortByOrder <> 'DESC' THEN 0
        WHEN @SortByColumn = 'Total' THEN TOTAL
        END DESC
		OFFSET @Offset ROWS
		FETCH NEXT @PageSize ROWS ONLY

		SELECT count([Id]) FROM [ORDER] 
		WHERE [IS_DELETED] = 0 AND
		(
			(@Search is not null and LTRIM(RTRIM(@Search)) <> '') AND
			(CAST(ISNULL([ID], '') as nvarchar) + CAST(ISNULL([EMAIL], '') as nvarchar) + CAST(ISNULL([FIRST_NAME], '') as nvarchar) + CAST(ISNULL([LAST_NAME], '') as nvarchar) like '%' + @Search + '%')
		) AND
		(@Username is null OR RTRIM(LTRIM(@Username)) = '' OR [EMAIL] = @Username)

    END 
END  