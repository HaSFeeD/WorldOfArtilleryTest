using Zenject;
using UnityEngine;
public class PlayingState : IGameState
{
    private readonly UIManager _ui;

    public PlayingState(UIManager ui) => _ui = ui;

    public void Enter()
    {
        Time.timeScale = 1f;
        _ui.HideGameOver();
        _ui.ShowHUD();
    }
}
