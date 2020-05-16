using System;
using System.Collections.Generic;
using System.Linq;

namespace RPS.Tests
{
    public class HighScoreView
    {
        private static List<(Guid GameId, string Winner)> Games = new List<(Guid, string)> {};

        public HighScoreView When(IEvent @event)
        {
            return @event switch
            {
                RoundEnded e => RoundEndedEvent(e),
                GameEnded e => GameEnded(e),
                GamePlayed e => GamePlayedEvent(e),
                _ => this,
            };
        }

        public HighScoreView RoundEndedEvent(RoundEnded @event)
        {
            var winnerScoreRow = GetPlayer(@event.Winner);
            var looserScoreRow = GetPlayer(@event.Looser);

            winnerScoreRow.RoundsPlayed++;
            winnerScoreRow.RoundsWon++;

            looserScoreRow.RoundsPlayed++;

            Games.Add((@event.GameId, @event.Winner));

            return this;
        }

        public HighScoreView GameEnded(GameEnded @event)
        {
            var game = Games
                .Where(g => g.GameId == @event.GameId)
                .Select(g => g.Winner);

            var winner = game.First();

            var winnerScoreRow = GetPlayer(winner);

            winnerScoreRow.GamesWon++;

            SetRank();

            return this;
        }

        public HighScoreView GamePlayedEvent(GamePlayed @event)
        {
            var winnerScoreRow = GetPlayer(@event.Winner);

            winnerScoreRow.GamesWon++;

            SetRank();

            return this;
        }

        private void SetRank()
        {
            this.Rows = this
                .Rows
                .OrderByDescending(r => r.GamesWon)
                .Select((r, i) =>
                {
                    r.Rank = i;
                    return r;
                })
                .ToArray();
        }

        private ScoreRow GetPlayer(string player)
        {
            if (this.Rows == null)
                this.Rows = new ScoreRow[0];

            var scoreRow = this.Rows.FirstOrDefault(r => r.PlayerId == player);
            if (scoreRow == null)
            {
                scoreRow = new ScoreRow() {PlayerId = player};
                this.Rows = this.Rows.Concat(new List<ScoreRow>()
                {
                    scoreRow,
                }).ToArray();
            }

            return scoreRow;
        }

        public ScoreRow[] Rows { get; set; }
        public class ScoreRow
        {
            public int Rank { get; set; }
            public string PlayerId { get; set; }
            public int GamesWon { get; set; }
            public int RoundsWon { get; set; }
            public int GamesPlayed { get; set; }
            public int RoundsPlayed { get; set; }
        }
    }
}
