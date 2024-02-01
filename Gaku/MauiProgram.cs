using Microsoft.Extensions.Logging;

namespace Gaku
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("banana-grotesk-regular.ttf", "BananaRegular");
                    fonts.AddFont("banana-grotesk-semibold.ttf", "BananaSemibold");
                    fonts.AddFont("banana-grotesk-thin.ttf", "BananaThin");
                    fonts.AddFont("banana-grotesk-bold.ttf", "BananaBold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
