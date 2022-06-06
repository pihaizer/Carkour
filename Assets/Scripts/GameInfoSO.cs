using UnityEngine;

[CreateAssetMenu]
public class GameInfoSO : ScriptableObject
{
    public Mutable<float> timer = new();
}