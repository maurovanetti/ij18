using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DefaultButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
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
        Background.color = White;
        Border.color = Grey;
        Text.color = Grey;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Background.color = Grey;
        Border.color = White;
        Text.color = White;
    }
}
