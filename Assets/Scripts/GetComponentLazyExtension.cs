using UnityEngine;

public static class GetComponentLazyExtension
{
    public static T GetComponentLazy<T>(this Component component, ref T backingField) where T : Component
    {
        if (backingField == null) backingField = component.GetComponent<T>();
        return backingField;
    }
}