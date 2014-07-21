 *****  MS Sample: Using Web API 2 with Entity Framework 6  ******
 BookService
 (http://www.asp.net/web-api/overview/creating-web-apis/using-web-api-with-entity-framework/part-1)



 Windows Azure Site Settigs:
	Subscription limit - 50$/month
	Site name - BookServiceOnGo.azurewebsites.net
 
 Used Frameworks:
	Web API 2.1
	Visual Studio 2013 Update 2
	Entity Framework 6
	.NET 4.5
	Knockout.js 3.1
	Bootstrap styling (http://getbootstrap.com/)
	
 Design:
	- single-page application (SPA) design

Using:
	Web API - http://localhost:62407/api/Books  etc.



											B U G S

1) Clean build from Git --> DB does not dreate
2) 


											T O   D O

MS site End working on:
________________________________________________________
http://www.asp.net/web-api/overview/creating-web-apis/using-web-api-with-entity-framework/part-7
beginning..


1) Read about EF Migration.
2) 


											H O W    T O

1) How to change default Web API string to   e.g. /api/version2/AuthorLalala ??
	- see API mapping in Global.asax or WebApiConfig.cs (like  routeTemplate: "api/{controller}/{id}")
	- use [Route] and [RoutePrefix] attribute for class or methods
	- http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
2) 