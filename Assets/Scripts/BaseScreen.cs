using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseScreen : MonoBehaviour
{
    public event Action Opened;
    public event Action Closed;
    public event Action<bool> Switched;

    public abstract BaseScreen Parent { get; protected set; }

    public bool IsOpened { get; protected set; }

    public void Open() => SetOpened(true);
    public void Close() => SetOpened(false);
    public void Switch() => SetOpened(!IsOpened);

    public void SetOpened(bool value)
    {
        if (value == IsOpened) return;
        SetOpenedInternal(value);
        IsOpened = value;
        (value ? Opened : Closed)?.Invoke();
        Switched?.Invoke(value);
    }

    protected abstract void SetOpenedInternal(bool value);
}