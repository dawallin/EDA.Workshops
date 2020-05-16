using System;
using System.Linq;
using Xunit;

namespace RPS.Tests
{
    public class HighScoreViewTests
    {
        [Fact]
        public void Highscore()
        {
            var playerOne = "alex@rpsgame.com";
            var playerTwo = "lisa@rpsgame.com";
            var playerThree = "julie@rpsgame.com";
            var state = Enumerable
                .Range(0, 20)
                .SelectMany((x, i) => GameEvents(Guid.NewGuid(), $"Game_{i}", playerOne, playerTwo))
                .Concat(Enumerable.Range(0, 10)
                .SelectMany((x, i) => GameEvents(Guid.NewGuid(), $"Game_{i + 20}", playerTwo, playerOne)))
                .Concat(Enumerable.Range(0, 15)
                .SelectMany((x, i) => GameEvents(Guid.NewGuid(), $"Game_{i + 30}", playerThree, playerOne)))
                .Concat(Enumerable.Range(0, 5)
                .SelectMany((x, i) => GameEvents(Guid.NewGuid(), $"Game_{i + 45}", playerTwo, playerThree)))
                .Rehydrate<HighScoreView>();

            Assert.Equal(25, state.Rows.OrderBy(r => r.Rank).First().GamesWon);
            Assert.Equal(playerOne, state.Rows.OrderBy(r => r.Rank).First().PlayerId);
            Assert.Equal(50, state.Rows.Sum(x => x.GamesWon));
        }

        public static IEvent[] GameEvents(Guid gameId, string title, string loosingPlayer, string winningPlayer)
            => new IEvent[] {
                new GameCreated { GameId = gameId, PlayerId = loosingPlayer, Rounds = 1, Title = title },
                new GameStarted { GameId = gameId, PlayerId = winningPlayer },
                new RoundStarted { GameId = gameId, Round = 1 },
                new HandShown { GameId = gameId, Hand = Hand.Scissors, PlayerId = loosingPlayer },
                new HandShown { GameId = gameId, Hand = Hand.Rock, PlayerId = winningPlayer },
                new RoundEnded { GameId = gameId, Round = 1, Looser = loosingPlayer, Winner = winningPlayer },
                new RoundStarted { GameId = gameId, Round = 2 },
                new HandShown { GameId = gameId, Hand = Hand.Paper, PlayerId = loosingPlayer },
                new HandShown { GameId = gameId, Hand = Hand.Scissors, PlayerId = winningPlayer },
                new RoundEnded { GameId = gameId, Round = 2, Looser = loosingPlayer, Winner = winningPlayer },
                new GameEnded { GameId = gameId }
            };
    }
}
