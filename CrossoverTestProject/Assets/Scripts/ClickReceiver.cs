using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickReceiver : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected bool _pointerInBounds = false;

    [SerializeField]
    protected UnityEvent _onLeftClick = new UnityEvent();

    [SerializeField]
    protected UnityEvent _onRightClick = new UnityEvent();

    [SerializeField]
    protected UnityEvent _onMiddleClick = new UnityEvent();

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_pointerInBounds)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    Debug.Log("LMB");
                    _onLeftClick.Invoke();
                    break;

                case PointerEventData.InputButton.Right:
                    Debug.Log("RMB");
                    _onRightClick.Invoke();
                    break;

                case PointerEventData.InputButton.Middle:
                    Debug.Log("MMB");
                    _onMiddleClick.Invoke();
                    break;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                Debug.Log("LMB");
                _onLeftClick.Invoke();
                break;

            case PointerEventData.InputButton.Right:
                Debug.Log("RMB");
                _onRightClick.Invoke();
                break;

            case PointerEventData.InputButton.Middle:
                Debug.Log("MMB");
                _onMiddleClick.Invoke();
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // NA
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _pointerInBounds = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _pointerInBounds = false;
    }
}
