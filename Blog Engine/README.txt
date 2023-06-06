IDE used Visual studio 2022
Data base used was SQL server.
I used swagger to test the api components. I is the first page to open when you run the program.

Set up 
You will likely need to reset the areas where I use “localhost:7107” in code to match your system.
I used it in
	postController .cs
		found in the Controllers folder.
			at lines 
				17
				50
				68
				93
				113
				141
	categoriesController .cs
		found in the Controllers folder.
			at lines 
				16
				41
				80
				101
				124
				141
	_Layout.cshtml
		Found in the Shared folder in the views directory
			At lines
				21


Import System.Runtime.InteropServices

' In SDK-style projects such as this one, several assembly attributes that were historically
' defined in this file are now automatically added during build and populated with
' values defined in project properties. For details of which attributes are included
' and how to customise this process see: https://aka.ms/assembly-info-properties


' Setting ComVisible to false makes the types in this assembly not visible to COM
' components.  If you need to access a type in this assembly from COM, set the ComVisible
' attribute to true on that type.

<Assembly: ComVisible(False)> 

' The following GUID is for the ID of the typelib if this project is exposed to COM.

<Assembly: Guid("97cf83a8-f286-4628-8460-582bd42466e8")> 
