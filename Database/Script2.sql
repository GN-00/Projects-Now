DECLARE @myName AS VARCHAR(100)='Script2';

IF ((SELECT COUNT(*) FROM [ProjectsNow].[Control].[History] where [Name] = @myName) = 0)
Begin
Insert Into [ProjectsNow].[Control].[History] ([Name]) values (@myName);
------------------------------------------------------------------------
Create Table [User]._Users
(
[Id] int  Not Null,
[UserName] nvarchar(20) Not Null,
[Password] nvarchar(20) Not Null,
[UserCode] nvarchar(20) Not Null,

[Administrator] bit Null,

[InquiryId] int Null,
[QuotationId] int Null,
[JobOrderId] int Null,
[CustomerId] int Null,
[ContactId] int Null,
[ConsultantId] int Null,
[SupplierId] int Null,
[AccountId] int Null,
[FinanceOrderId] int Null,

[AccessTendaring] int Null,
[AccessProjects] int Null,
[AccessItems] int Null,
[AccessFinance] int Null,
)
---------------------------------------------------------------------------
End
else
PRINT @myName + ' Is Already Done';