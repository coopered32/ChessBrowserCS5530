using System;

namespace ChessTools
{
    public class ChessGame
    {
        public string Event { get; set; }
        public string Site { get; set; }
        public string Round { get; set; }
        public Player WhitePlayer { get; set; }
        public Player BlackPlayer { get; set; }
        public string Result { get; set; }
        public string EventDate { get; set; }
        public string Date { get; set; }
        public string Moves { get; set; }
        public string ECO { get; set; }
    }

    public class Player
    {
        public string Name { get; set; }
        public int Elo { get; set; }
    }
}
