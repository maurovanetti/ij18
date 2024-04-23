using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour, ICurtains
{
    [SerializeField] private Animator _animator;

#if UNITY_EDITOR
    [SerializeField] private bool _debugGameOver;
#endif

    public void ChangeTo(Curtain curtain)
    {
        if (curtain == Curtain.GameOver || curtain == Curtain.Victory)
        {
            _animator.SetTrigger("Show");
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        DestroyLocalization();
        SceneManager.LoadScene("AppStart");
    }

    private void DestroyLocalization()
    {
        Localization loc = FindObjectOfType<Localization>();

        if (loc != null)
            Destroy(loc.gameObject);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (_debugGameOver)
        {
            _debugGameOver = false;
            _animator.SetTrigger("Show");
        }
    }
#endif
}
