using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using PostCodeAPI.Data;
using Swashbuckle.Application;

namespace PostCodeAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);
            builder.RegisterType<ConnectionStringProvider>().As<IConnectionStringProvider>();
            builder.RegisterType<PostCodeRepository>().As<IPostCodeRepository>();
            var container = builder.Build();
            var config = GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "PostCodeApi");
                c.IncludeXmlComments(string.Format(@"{0}\bin\PostCodeAPI.XML",
                    System.AppDomain.CurrentDomain.BaseDirectory));

            }).EnableSwaggerUi();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}

