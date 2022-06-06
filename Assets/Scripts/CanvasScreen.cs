using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CanvasScreen : BaseScreen
{
    public override BaseScreen Parent { get; protected set; }
    public Canvas Canvas => this.GetComponentLazy(ref _canvas);

    public RectTransform RectTransform => this.GetComponentLazy(ref _rectTransform);

    private Canvas _canvas;
    private RectTransform _rectTransform;

    protected virtual void Awake()
    {
        IsOpened = Canvas.enabled;
    }

    protected override void SetOpenedInternal(bool value)
    {
        if (Canvas.enabled == value) return;
        Canvas.enabled = value;
    }
}