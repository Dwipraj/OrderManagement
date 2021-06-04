USE [ORDER_MANAGEMENT]
GO
/****** Object:  Table [dbo].[ORDER]    Script Date: 04-06-2021 21:37:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ORDER](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EMAIL] [nvarchar](100) NOT NULL,
	[FIRST_NAME] [nvarchar](50) NOT NULL,
	[LAST_NAME] [nvarchar](50) NULL,
	[ORDER_DATE] [datetimeoffset](7) NOT NULL,
	[TOTAL] [decimal](18, 2) NOT NULL,
	[IS_DELETED] [bit] NOT NULL,
	[CREATED_AT] [datetimeoffset](7) NULL,
	[UPDATED_AT] [datetimeoffset](7) NULL,
	[DELETED_AT] [datetimeoffset](7) NULL,
 CONSTRAINT [PK_ORDER] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[SP_ORDER]    Script Date: 04-06-2021 21:37:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ORDER]
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
            UPDATE [ORDER] SET [TOTAL]=@Total WHERE ID=@Id and [IS_DELETED] = 0 AND
		(@Username is null OR RTRIM(LTRIM(@Username)) = '' OR [EMAIL] = @Username)
        END  
    END  
    ELSE IF @ActionType = 'DeleteData'  
    BEGIN  
        UPDATE [ORDER] SET [IS_DELETED] = @IsDeleted WHERE ID=@Id AND
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
GO
