using System.Windows.Forms;
using Rage;

namespace DynamicWeather;

internal static class Settings
{
    internal static InitializationFile iniFile;
    internal static Keys GlobalModifierKey = Keys.None;
    internal static Keys ShowForecastKey = Keys.Oemtilde;
    internal static int TimeInterval = 3;
    internal static bool EnableAlwaysOnUI = true;
    
    
    
    internal static void ReadSettings()
    {
        iniFile = new InitializationFile(@"Plugins/DynamicWeather/DynamicWeather.ini");
        iniFile.Create();

        GlobalModifierKey = iniFile.ReadEnum("General", "GlobalModifierKey", GlobalModifierKey);    
        ShowForecastKey = iniFile.ReadEnum("General", "ShowForecastKey", ShowForecastKey);
        TimeInterval = iniFile.ReadInt32("General", "TimeInterval", TimeInterval);
        EnableAlwaysOnUI = iniFile.ReadBoolean("General", "EnableAlwaysOnUI", EnableAlwaysOnUI);
    }
 }