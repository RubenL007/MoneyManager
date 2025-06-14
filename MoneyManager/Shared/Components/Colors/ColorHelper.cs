using MoneyManager.Shared.Components.Colors;

namespace MoneyManager.Shared.Components
{
    public class ColorHelper
    {
        public static string ColorEnumToHTML(ColorsEnum colorByEnum, ColorsPercentageEnum? colorPercentage = null)
        {
            return $"var(--{colorByEnum}{(colorPercentage != null ? $"-{(int)colorPercentage}" : string.Empty)})";
        }
    }
}
