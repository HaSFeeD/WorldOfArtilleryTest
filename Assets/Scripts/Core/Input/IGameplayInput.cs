using UnityEngine;

public interface IGameplayInput
{
    Vector2 Aim { get; }
    bool Fire { get; }
    float Zoom { get; }
}
