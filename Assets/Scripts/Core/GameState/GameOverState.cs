using Zenject;
using UnityEngine;

public class GameOverState : IGameState
{
    private readonly UIManager _ui;

    public GameOverState(UIManager ui) => _ui = ui;

    public void Enter()
    {
        Time.timeScale = 0f;
        _ui.ShowGameOver();
    }
}
