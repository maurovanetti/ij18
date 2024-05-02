using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public CanvasGroup MainCanvasGroup;
    public CanvasGroup ButtonsContainer;
    public CanvasGroup CreditsContainer;
    public CanvasGroup LoadingContainer;
    public DefaultButton ItButton;
    public DefaultButton EnButton;
    public DefaultButton PlayButton;
    public DefaultButton CreditsButton;
    public DefaultButton QuitCreditButton;

    private void OnEnable()
    {
        Localization.LanguageChanged += OnLanguageChanged;
    }

    private void OnDisable()
    {
        Localization.LanguageChanged -= OnLanguageChanged;
    }

    //show buttons only after text is set
    private void OnLanguageChanged(Localization.Language language)
    {
        UpdateButtonLanguage();
        ToggleCanvasGroup(true, ref ButtonsContainer);
        ToggleCanvasGroup(false, ref CreditsContainer);
        ToggleCanvasGroup(false, ref LoadingContainer);
        MainCanvasGroup.alpha = 1f;
    }

    public void Play()
    {
        StartCoroutine(LoadGameSceneAsync());
        ToggleCanvasGroup(false, ref ButtonsContainer);
        ToggleCanvasGroup(false, ref CreditsContainer);
        ToggleCanvasGroup(true, ref LoadingContainer);
    }

    private IEnumerator LoadGameSceneAsync()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    public void ToggleCanvasGroup(bool show, ref CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = Convert.ToInt32(show);
        canvasGroup.interactable = show;
        canvasGroup.blocksRaycasts = show;
    }

    public void ToggleView(bool openCredits)
    {
        //reset button graphics before changing view
        if (openCredits)
        {
            PlayButton.UpdateGraphics(false);
            CreditsButton.UpdateGraphics(false);
        }
        else
        {
            QuitCreditButton.UpdateGraphics(false);
        }

        ToggleCanvasGroup(!openCredits, ref ButtonsContainer);
        ToggleCanvasGroup(openCredits, ref CreditsContainer);
    }

    public void UpdateButtonLanguage()
    {
        if (Localization.language == Localization.Language.Italian)
        {
            ItButton.UpdateGraphics(true);
            EnButton.UpdateGraphics(false);
        }
        else
        {
            ItButton.UpdateGraphics(false);
            EnButton.UpdateGraphics(true);
        }
    }
}
