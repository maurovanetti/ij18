using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Localization : MonoBehaviour
{
    //public string sceneName;
    public string DefaultLanguage = "it";
    public static Action<Language> LanguageChanged;
    private static string _currentLanguageCode;

    private const string LanguageKey = "Language";

    public enum Language
    {
        English,
        Italian
    }
    public static Language language;

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
        _currentLanguageCode = languageCode;
        PlayerPrefs.SetString(LanguageKey, languageCode);
        Debug.Log("Set language with current code " + _currentLanguageCode);
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

        LanguageChanged?.Invoke(language);
    }

    public void Start()
    {
        Debug.Log("Localization start");

        if (PlayerPrefs.HasKey(LanguageKey))
            DefaultLanguage = PlayerPrefs.GetString(LanguageKey, DefaultLanguage);

        DontDestroyOnLoad(this);
        StartCoroutine(SetLanguageWithDelay(DefaultLanguage));
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene current, Scene next)
    {
        if (next.name.Equals("Game"))
        {
            Debug.Log("Enter game scene");
            StartCoroutine(SetLanguageWithDelay(_currentLanguageCode));
        }
    }

    //set default language with a small delay
    //to make sure each object subscribes to the LanguageChanged event
    private IEnumerator SetLanguageWithDelay(string language)
    {
        Debug.Log("Set language with delay with value " + language);
        yield return new WaitForSeconds(0.2f);
        SetLanguage(language);
    }
}
