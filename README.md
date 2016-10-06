Swashbuckle
=========

Seamlessly adds a [Swagger](http://swagger.io/) to API's that are built with AspNet Core! It combines the built in metadata functionality ([ApiExplorer](https://github.com/aspnet/Mvc/tree/dev/src/Microsoft.AspNetCore.Mvc.ApiExplorer)) and Swagger/swagger-ui to provide a rich discovery, documentation and playground experience to your API consumers.

In addition to its [Swagger](http://swagger.io/specification/) generator, Swashbuckle also contains an embedded version of the [swagger-ui](https://github.com/swagger-api/swagger-ui) which it will automatically serve up once Swashbuckle is installed. This means you can complement your API with a slick discovery UI to assist consumers with their integration efforts. Best of all, it requires minimal coding and maintenance, allowing you to focus on building an awesome API

And that's not all ...

Once you have a Web API that can describe itself in Swagger, you've opened the treasure chest of Swagger-based tools including a client generator that can be targeted to a wide range of popular platforms. See [swagger-codegen](https://github.com/swagger-api/swagger-codegen) for more details.

**Swashbuckle Core Features:**

* Auto-generated [Swagger 2.0](https://github.com/swagger-api/swagger-spec/blob/master/versions/2.0.md)
* Seamless integration of swagger-ui
* Reflection-based Schema generation for describing API types
* Extensibility hooks for customizing the generated Swagger doc
* Extensibility hooks for customizing the swagger-ui
* Out-of-the-box support for leveraging Xml comments
* Support for describing ApiKey, Basic Auth and OAuth2 schemes ... including UI support for the Implicit OAuth2 flow

**\*Swashbuckle 6.0.0**

Because Swashbuckle 6.0.0 is built on top of the next-gen implementation of .NET and ASP.NET (AspNet Core), the source code and public interface deviate significantly from previous versions. Once a stable release of AspNet Core (RC2 at time of writing) becomes available, I'll add a transition guide for Swashbuckle. In the meantime, you'll need to figure this out yourself. Hopefully, the examples [here](https://github.com/domaindrivendev/Ahoy/tree/master/test/WebSites) and the remainder of this README will get you there!


## Getting Started ##

Currently, Swashbuckle consists of two components (with more planned for future iterations) - __Swashbuckle.SwaggerGen__ and __Swashbuckle.SwaggerUi__. The former provides functionality to generate one or more Swagger documents directly from your API implementation and expose them as JSON endpoints. The latter provides an embedded version of the excellent [swagger-ui](https://github.com/swagger-api/swagger-ui) tool that can be served by your application and powered by the generated Swagger documents to describe your API.

These can be installed as separate Nuget packages if you need one without the other. If you want both, the simplest way to get started is by installing the meta-package __"Swashbuckle"__ which bundles them together:

	Install-Package Swashbuckle -Pre
    
Next, you'll need to configure Swagger in your _Startup.cs_.

    public void ConfigureServices(IServiceCollection services)
    {
    	... Configure MVC services ...

		// Inject an implementation of ISwaggerProvider with defaulted settings applied
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
		... Enable MVC middleware ...

		// Enable middleware to serve generated Swagger as a JSON endpoint
        app.UseSwagger();
        
        // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
        app.UseSwaggerUi();
    }
    

Once this is done, you should be able to spin up you app and browse the following Swagger JSON and UI endpoints respectively:

***\<your-root-url\>/swagger/v1/swagger.json***

***\<your-root-url\>/swagger/ui***
    

## Customizing the Generated Swagger Docs ##

The above snippet demonstrates the minimum configuration required to get the Swagger docs and swagger-ui up and running. However, these methods expose a range of configuration and extensibility options that you can pick and choose from, combining the convenience of sensible defaults with the flexibility to customize where you see fit. Read on to learn more.

Sorry :( - still in progress but coming real soon! For now, take a look at the sample projects for inspiration:

- https://github.com/domaindrivendev/Ahoy/tree/master/test/WebSites/Basic
- https://github.com/domaindrivendev/Ahoy/tree/master/test/WebSites/CustomizedUi
- https://github.com/domaindrivendev/Ahoy/tree/master/test/WebSites/MultipleVersions
- https://github.com/domaindrivendev/Ahoy/tree/master/test/WebSites/SecuritySchemes
- https://github.com/domaindrivendev/Ahoy/tree/master/test/WebSites/VirtualDirectory
