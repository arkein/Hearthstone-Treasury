using System.Collections.Generic;

namespace Hearthstone_Treasury.Utils
{
    public static class Localization
    {
        public static string[] GetSupportedLocales()
        {
            return new string[] {
                "en-US",
                "fr-FR",
                "de-DE",
                "ko-KR",
                "es-ES",
                "es-MX",
                "ru-RU",
                "zh-TW",
                "zh-CN",
                "it-IT",
                "pt-BR",
                "pl-PL",
                "ja-JP",
                "th-TH",
            };
        }
        public static Dictionary<string, string> WindowsToBlizzardLocale = new Dictionary<string, string>
        {
            { "en-US", "enUS" },
            { "fr-FR", "frFR" },
            { "de-DE", "deDE" },
            { "ko-KR", "koKR" },
            { "es-ES", "esES" },
            { "es-MX", "esMX" },
            { "ru-RU", "ruRU" },
            { "zh-TW", "zhTW" },
            { "zh-CN", "zhCN" },
            { "it-IT", "itIT" },
            { "pt-BR", "ptBR" },
            { "pl-PL", "plPL" },
            { "ja-JP", "jaJP" },
            { "th-TH", "thTH" }
        };

        public static Dictionary<string, string> BlizzardToWindowsLocale = new Dictionary<string, string>
        {
            { "enUS", "en-US" },
            { "frFR", "fr-FR" },
            { "deDE", "de-DE" },
            { "koKR", "ko-KR" },
            { "esES", "es-ES" },
            { "esMX", "es-MX" },
            { "ruRU", "ru-RU" },
            { "zhTW", "zh-TW" },
            { "zhCN", "zh-CN" },
            { "itIT", "it-IT" },
            { "ptBR", "pt-BR" },
            { "plPL", "pl-PL" },
            { "jaJP", "ja-JP" },
            { "thTH", "th-TH" },
        };
    }
}
