DECLARE @myName AS VARCHAR(100)='Script2';

IF ((SELECT COUNT(*) FROM [ProjectsNow].[Control].[History] where [Name] = @myName) = 0)
Begin
Insert Into [ProjectsNow].[Control].[History] ([Name]) values (@myName);
------------------------------------------------------------------------

---------------------------------------------------------------------------
End
else
PRINT @myName + ' Is Already Done';