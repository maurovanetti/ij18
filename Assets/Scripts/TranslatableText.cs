using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TranslatableText : MonoBehaviour
{
    public Text text;
    public TextMeshProUGUI textMeshPro;
    public TranslatedText[] Values;

    private void OnEnable()
    {
        Localization.LanguageChanged += OnLanguageChanged;
    }

    private void OnDisable()
    {
        Localization.LanguageChanged -= OnLanguageChanged;
    }

    private void OnLanguageChanged(Localization.Language language)
    {
        Debug.Log(gameObject.name + " On language changed with value " + language.ToString());
        string value = Values.SingleOrDefault(v => v.Index == language).Value;

        if (text != null)
            text.text = value;
        else if (textMeshPro != null)
            textMeshPro.text = value;
        else
            Debug.LogError($"No Text or TextMeshPro Component specified for TranslatableText on {gameObject.name}");
    }
}

[Serializable]
public class TranslatedText
{
    public Localization.Language Index;
    public string Value;
}
