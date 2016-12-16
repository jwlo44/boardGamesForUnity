using BoardGames.Games.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoardGames.Games.Players
{
    public class MorrisPlayer : Player
    {
        HashSet<MorrisPiece> placedPieces;
        HashSet<MorrisPiece> unplacedPieces;
        HashSet<MorrisPiece> deadPieces;

        public bool isPlacingPieces()
        {
            return unplacedPieces.Count > 0;
        }
        public bool isFlying()
        {
            return placedPieces.Count == 3;
        }

        public void init( int totalPieces = 9)
        {
            placedPieces = new HashSet<MorrisPiece>();
            deadPieces = new HashSet<MorrisPiece>();
            unplacedPieces = new HashSet<MorrisPiece>();
            for (int i = 0; i < totalPieces; i++)
            {
                unplacedPieces.Add(new MorrisPiece());
            }
        }

        public void takeTurn()
        {

        }

        public bool lost()
        {
            return placedPieces.Count < 3 && unplacedPieces.Count == 0;
        }
    }
}
