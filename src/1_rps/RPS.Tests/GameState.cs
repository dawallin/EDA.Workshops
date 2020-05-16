namespace RPS.Tests
{
    public class GameState
    {
        public GameState When(IEvent @event) => @event switch
        {
            GameCreated e => new GameState { Status = GameStatus.ReadyToStart },
            GameStarted e => new GameState { Status = GameStatus.Started },
            RoundStarted e => new GameState { Status = GameStatus.Started },
            HandShown e => new GameState { Status = GameStatus.Started },
            RoundEnded e => new GameState { Status = GameStatus.Started },
            GameEnded e => new GameState { Status = GameStatus.Ended },
            _ => this,
        };

        public GameStatus Status { get; set; }
    }

}
