using System.Configuration;

namespace PostCodeAPI.Data
{
    public interface IConnectionStringProvider
    {
       string ConnectionString { get;  }
    }

    public class ConnectionStringProvider : IConnectionStringProvider
    {
        public string ConnectionString => ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
    }
}