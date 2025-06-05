using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Graphic))]
public class UICollider : MonoBehaviour
{
    [SerializeField]
    private Graphic graphic;
    public Graphic Graphic => graphic;

    [SerializeField]
    private UIColliderPadding padding = new();
    public Vector4 Padding
    {
        get { return padding.ToVector4(); }
        set { padding = new UIColliderPadding(value); }
    }

    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    private void OnEnable()
    {
        if (IsValid() == false)
        {
            return;
        }

        GraphicRegistry.RegisterUIColliderForCanvas(canvas, this);
    }

    private void OnDisable()
    {
        if (IsValid() == false)
        {
            return;
        }

        GraphicRegistry.UnregisterUIColliderForCanvas(canvas, this);
    }

    private bool IsValid()
    {
        if (canvas && graphic)
        {
            return true;
        }

        Debug.LogWarning($"Reference is missing. {nameof(UICollider)} requires both a {nameof(Canvas)} in parent and a {nameof(Graphic)} component.");
        return false;
    }
}