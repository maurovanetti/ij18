using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject ButtonsContainer;
    public GameObject CreditsContainer;
    public GameObject LoadingContainer;
    public DefaultButton ItButton;
    public DefaultButton EnButton;
    public DefaultButton PlayButton;
    public DefaultButton CreditsButton;
    public DefaultButton QuitCreditButton;

    public void Play()
    {
        StartCoroutine(LoadGameSceneAsync());
        ButtonsContainer.SetActive(false);
        CreditsContainer.SetActive(false);
        LoadingContainer.SetActive(true);
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

        ButtonsContainer.SetActive(!openCredits);
        CreditsContainer.SetActive(openCredits);
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
