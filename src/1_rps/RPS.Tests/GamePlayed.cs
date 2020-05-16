using System;
using System.Collections.Generic;

namespace RPS.Tests
{
    public class GamePlayed : IEvent
    {
        public GamePlayed When(IEvent @event) => @event switch
        {
            RoundEnded e => RoundEndedEvent(e),
            _ => this,
        };

        public GamePlayed RoundEndedEvent(RoundEnded @event)
        {
            this.Winner = @event.Winner;
            this.Looser = @event.Looser;

            return this;
        }

        public Guid GameId { get; set; }
        public int Rounds { get; set; }
        public string Winner { get; set; }
        public string Looser { get; set; }
        public string SourceId => GameId.ToString();
        public IDictionary<string, string> Meta { get; set; }
    }

}
