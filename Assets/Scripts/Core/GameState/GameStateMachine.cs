using Zenject;

public class GameStateMachine : IInitializable
{
    private readonly PlayingState _playing;
    private readonly GameOverState _gameOver;

    public GameStateMachine(
        PlayingState playing, GameOverState gameOver)
    {
        _playing = playing;
        _gameOver = gameOver;
    }

    public void Initialize() => _playing.Enter();

    public void EnterGameOver() => _gameOver.Enter();
}