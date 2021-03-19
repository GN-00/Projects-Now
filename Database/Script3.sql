DECLARE @myName AS VARCHAR(100)='Script2';

IF ((SELECT COUNT(*) FROM [ProjectsNow].[Control].[History] where [Name] = @myName) = 0)
Begin
Insert Into [ProjectsNow].[Control].[History] ([Name]) values (@myName);
------------------------------------------------------------------------
Insert into [ProjectsNow].[User].[_Users]
(Id, UserName, Password, UserCode, Administrator, AccessTendaring, AccessProjects, AccessItems, AccessFinance)
Select 
UserID,  
UserName, 
Password,
UserCode, 
Administrator, 
AccessTendaring, 
AccessProjects, 
AccessItems, 
AccessFinance 
From [User].[Users];

Alter Table 
---------------------------------------------------------------------------
End
else
PRINT @myName + ' Is Already Done';