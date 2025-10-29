using MoneyManager.Data.Models.Configuration;

namespace MoneyManager.Data.Interface.Configuration
{
    public interface IConfiguration
    {
        string UpdateConfiguration(ConfigurationModel configuration);
        ConfigurationModel GetConfiguration();
    }
}
