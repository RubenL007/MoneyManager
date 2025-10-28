using MoneyManager.Data.Models;

namespace MoneyManager.Data.Interface
{
    public interface IConfiguration
    {
        string UpdateConfiguration(ConfigurationModel configuration);
        List<ConfigurationModel> SearchConfiguration();
    }
}
