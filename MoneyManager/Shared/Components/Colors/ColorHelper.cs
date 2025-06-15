using MoneyManager.Shared.Components.Colors;

namespace MoneyManager.Shared.Components
{
    public class ColorHelper
    {
        public static string ColorEnumToHTML(ColorsEnum colorByEnum, ColorsPercentageEnum? colorPercentage = null)
        {
            return $"var(--{colorByEnum}{(colorPercentage != null ? $"-{(int)colorPercentage}" : string.Empty)})";
        }

        public static string RandomHexColor()
        {
            var random = new Random();

            int r = random.Next(100, 256);
            int g = random.Next(100, 256);
            int b = random.Next(100, 256);

            return $"#{r:X2}{g:X2}{b:X2}";
        }
    }
}
