# BlogEngine

#OVERVIEW
Build a minimal Blog Engine / CMS backend API, that allows to create, edit and publish textbased
posts, with an approval flow where two different user types may interact.

# Dependencies
Install the following programs in case you do not have the correct operation of the application.

Visual Studio 2019 URL: https://visualstudio.microsoft.com/es/thank-you-downloading-visual-studio/?sku=Community&rel=16
.NET Framework 5.0 URL: https://dotnet.microsoft.com/download/dotnet-framework/net48
Sql Server URL: https://go.microsoft.com/fwlink/?linkid=866662

Management Studio: https://aka.ms/ssmsfullsetup

# STEPS TO OPERATE THE APPLICATION
1.- Execute script called (SCRIPT DB BlogEngine) hosted in folder (SCRIPT) from SQL Management Studio.

2.- How to run the application
WEB API
Open the WebAPI solution (WebApiBlogEngine) found in the WebApiBlogEngine folder from the WebApiBlogEngine.sln file

In solution explorer right click on the application and then click on the Compile option.

Then run solution by pressing the key combination (Ctrl + F5)

.NET CORE APPLICATION.
Open the application solution (AppBlogEngine) found in the AppBlogEngine folder from the AppBlogEngine.sln file

In solution explorer right click on the application and then click on the Compile option.

Then run solution by pressing the key combination (Ctrl + F5). IMPORTANT do not close the Web API previously raised.


To access the application, do so with the default users and passwords provided below:

ADMINISTRATOR
User: administrator
Password: AdminBlog *

WRITER
User: writer
Password: writer *

EDITOR
User: editor
Password: editor *

The administrator user has an administrator role, so with this you can access all the functionalities, create new users and assign them a role.
