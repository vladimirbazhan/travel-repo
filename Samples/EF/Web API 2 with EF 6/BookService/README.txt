 *****  MS Sample: Using Web API 2 with Entity Framework 6  ******
 BookService
 (http://www.asp.net/web-api/overview/creating-web-apis/using-web-api-with-entity-framework/part-1)



 Windows Azure Site Settigs:
    Subscription limit - 50$/month
    Site name - BookServiceOnGo.azurewebsites.net
                (bookserviceongo.scm.azurewebsites.net:443)
                (Data Source=tcp:f20z2unc5e.database.windows.net,1433;Initial Catalog=bookserviceongo_db;User Id=sshev4enko@f20z2unc5e;Password=hefy2corEPAM)
        Database username - sshev4enko
        Database password - h....2...EP..    
    MS Azure Control Panel - https://manage.windowsazure.com/@sergiishevchenkoepam.onmicrosoft.com#Workspaces/WebsiteExtension/websites

 IIS (local) Site Settigs:
    Site name - http://localhost:9810/
 
 Frameworks & Tools:
    .NET 4.5
    Visual Studio 2013 Update 2
    Web API 2.1
    Entity Framework 6.0.0
    Knockout.js 3.1
    jQuery 1.10.2
    Bootstrap styling 3.0.0 (http://getbootstrap.com/)
    
 Design:
    - single-page application (SPA)

Using:
    Web API - http://localhost:62407/api/Books  etc.



                                            B U G S

1) Clean build from Git --> DB does not create (test on HOME env.)
2) 


                                            T O   D O

MS site End working on:
________________________________________________________
http://www.asp.net/web-api/overview/creating-web-apis/using-web-api-with-entity-framework/part-10
beginning..


0) Publish to Azure !!!
1) Read about EF Migration.
2) 


                                            H O W    T O

0) How to Seed the Database via EF Migrations:
    - open Package Manager Console window
    - [on 1st setup] run command 'Enable-Migrations' (adds a folder named Migrations to your project + Configuration.cs)
    - [on 1st setup] run command 'Add-Migration Initial' (generates code that creates DB e.g. ..\BookService\Migrations\201407101307443_Initial.cs)
    - run command 'Update-Database' (executes that generated code above: EF created the DB *.mdf and call the Seed() method)

1) How to change default Web API string to   e.g. /api/version2/AuthorLalala ??
    - see API mapping in WebApiConfig.cs (like  routeTemplate: "api/{controller}/{id}")
    - use [Route] and [RoutePrefix] attribute for class or methods
    - http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2

2) How to host ASP.NET project in IIS using VS Publish method
    - http://dotnetmentors.com/web-api/host-asp-net-web-api-in-iis-using-visual-studio-publish.aspx
    - DO NOT FORGET to change Identity of Application pool ".NET v4.5"
      from 'ApplicationPoolIdentity' to 'LocalSystem'
    - DO NOT FORGET to copy App_Data folder with *.mdf files to folder where we was published the site

3) How to enable ASP.NET 4.5 support in IIS (HTTP Error 500.19)
    - http://serverfault.com/questions/514091/iis-8-asp-net-mvc-http-error-500-19
