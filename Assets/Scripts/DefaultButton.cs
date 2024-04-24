using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DefaultButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool PreventOnPointerEvents;
    public Color Grey;
    public Color White;

    public AudioClip ClickSound;
    public Image Background;
    public Image Border;
    public Text Text;

    [Space]
    public UnityEvent OnClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySingleSound(ClickSound);
        OnClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (PreventOnPointerEvents)
            return;

        UpdateGraphics(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (PreventOnPointerEvents)
            return;

        UpdateGraphics(false);
    }

    public void UpdateGraphics(bool onPointerEnter)
    {
        Background.color = onPointerEnter? White : Grey;
        Border.color = onPointerEnter ? Grey : White;
        Text.color = onPointerEnter ? Grey : White;
    }
}
