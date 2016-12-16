using BoardGames.Games.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoardGames.Games.Players
{
    public class MorrisPlayer : Player
    {
        int placedPieces;
        int unplacedPieces;
        int deadPieces;

        public bool isPlacingPieces()
        {
            return unplacedPieces > 0;
        }
        public bool isFlying()
        {
            return placedPieces == 3;
        }
        
        public void placePiece()
        {
            unplacedPieces--;
            placedPieces++;
        }

        public void losePiece()
        {
            placedPieces--;
            deadPieces++;
        }

        public void init( int totalPieces = 9)
        {
            placedPieces = 0;
            deadPieces = 0;
            unplacedPieces = totalPieces;
        }

        public void takeTurn()
        {

        }

        public bool lost()
        {
            return placedPieces < 3 && unplacedPieces == 0;
        }
    }
}
