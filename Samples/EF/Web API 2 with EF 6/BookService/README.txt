 *****  MS Sample: Using Web API 2 with Entity Framework 6  ******
 BookService
 (http://www.asp.net/web-api/overview/creating-web-apis/using-web-api-with-entity-framework/part-1)



 Windows Azure Site Settigs:
	Subscription limit - 50$/month
	Site name - BookServiceOnGo.azurewebsites.net

 IIS (local) Site Settigs:
	Site name - http://localhost:9810/
 
 Used Frameworks:
	Web API 2.1
	Visual Studio 2013 Update 2
	Entity Framework 6
	.NET 4.5
	Knockout.js 3.1
	jQuery JavaScript Library v1.10.2
	Bootstrap styling (http://getbootstrap.com/)
	
 Design:
	- single-page application (SPA) design

Using:
	Web API - http://localhost:62407/api/Books  etc.



											B U G S

1) Clean build from Git --> DB does not create (test on HOME env.)
2) 


											T O   D O

MS site End working on:
________________________________________________________
http://www.asp.net/web-api/overview/creating-web-apis/using-web-api-with-entity-framework/part-9
beginning..


1) Read about EF Migration.
2) 


											H O W    T O

1) How to change default Web API string to   e.g. /api/version2/AuthorLalala ??
	- see API mapping in Global.asax or WebApiConfig.cs (like  routeTemplate: "api/{controller}/{id}")
	- use [Route] and [RoutePrefix] attribute for class or methods
	- http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
2) How to host ASP.NET project in IIS using VS Publish method
	- http://dotnetmentors.com/web-api/host-asp-net-web-api-in-iis-using-visual-studio-publish.aspx
	- DO NOT FORGET to change Identity of Application pool ".NET v4.5"
	  from 'ApplicationPoolIdentity' to 'LocalSystem'
	- DO NOT FORGET to copy App_Data folder with *.mdf files to folder where we was published the site
3) How to enable ASP.NET 4.5 support in IIS (HTTP Error 500.19)
	- http://serverfault.com/questions/514091/iis-8-asp-net-mvc-http-error-500-19
