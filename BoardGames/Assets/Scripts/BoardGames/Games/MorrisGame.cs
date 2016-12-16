using BoardGames.Games.Players;

namespace BoardGames.Games
{
    public class MorrisGame : IGame
    {
        private MorrisPlayer player1;
        private MorrisPlayer player2;
        private bool activePlayer1 = false;
        public string instruction = "";

        public Player getActivePlayer()
        {
            return activePlayer1 ? player1 : player1;
        }

        public Player getWinner()
        {
            if (!isOver()) { return null; }
            else
            {
                return player1.lost() ? player2 : player1;
            }
        }

        public bool isOver()
        {
            return player1.lost() || player2.lost();
        }

        public void takeTurn()
        {
            activePlayer1 = !activePlayer1;
            MorrisPlayer currentPlayer = (MorrisPlayer)getActivePlayer();
            instruction = "Player " + (activePlayer1 ? "1: " : "2: ");
            // what phase are we in?
            if (currentPlayer.isPlacingPieces())
            {
                // phase 1: place pieces
                instruction += "Tap an empty space to place a piece.";
            }
            else if (!currentPlayer.isFlying())
            {
                // phase 2: move pieces
                instruction += "Select a piece to move.";
            }
            else
            {
                // phase 3: fly!
            }
        }

        public void init()
        {
            player1 = new MorrisPlayer();
            player2 = new MorrisPlayer();
            player1.init();
            player2.init();
        }
    }
}
