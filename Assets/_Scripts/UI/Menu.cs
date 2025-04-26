using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class Menu : MonoBehaviour
{
    [field: Header("Menu")]
    [field: SerializeField] public MenuType MenuType { get; private set; }

    // Override this for initialization logic (happens during MenuManager Start to all Menus).
    public virtual void Init() { }

    // Do not call Open, Close, or SetInteractivity directly, the only place they should be called is from their respective methods in MenuManager!
    public void Open()
    {
        gameObject.SetActive(true);
        OnOpen();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        OnClose();
    }
    // Override these if you want something to happen OnOpen or OnClose.
    protected virtual void OnOpen() { }

    protected virtual void OnClose() { }
}
