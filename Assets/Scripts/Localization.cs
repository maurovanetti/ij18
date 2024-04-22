using UnityEngine;

public class Localization : MonoBehaviour
{
    //public string sceneName;

    public enum Language
    {
        English,
        Italian
    }
    private static Language language;

    public static int LanguageIndex
    {
        get
        {
            switch (language)
            {
                case Language.Italian:
                    return 1;

                default:
                case Language.English:
                    return 0;

            }
        }
    }

    public static string FormatDecimalSeparator(string unformatted)
    {
        switch (language)
        {
            case Language.Italian:
                return unformatted.Replace(".", ",");

            default:
            case Language.English:
                return unformatted.Replace(",", ".");

        }

    }

    public void SetLanguage(string languageCode)
    {
        switch (languageCode)
        {
            case "it":
                Localization.language = Language.Italian;
                break;

            default:
            case "en":
                Localization.language = Language.English;
                break;

        }
        //SceneManager.LoadScene(sceneName);
    }

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
