using BoardGames.Games.Pieces;
using BoardGames.Games.Players;
using System;

namespace BoardGames.Games
{
    // game handles game logic
    public class MorrisGame : IGame
    {
        private MorrisPlayer player1;
        private MorrisPlayer player2;
        private bool activePlayer1 = false;
        public string instruction = "";
        public int piecesPerPlayer = 9;
        private enum TurnStates
        {
            PLACE,
            SELECT,
            MOVE,
            NONE,
        }

        private TurnStates turnState = TurnStates.NONE;
        private Space selectedSpace;

        public MorrisGame(int piecesPerPlayer)
        {
            this.piecesPerPlayer = piecesPerPlayer;
        }

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

        public void HandleSpaceClicked(Space space)
        {
           if (isOver())
            {
                return;
            }
           switch(turnState)
            {
                case TurnStates.PLACE:
                    {
                        if (space.piece != null)
                        {
                            break;
                        }
                        MorrisPlayer currentPlayer = (MorrisPlayer)getActivePlayer();
                        space.piece = new MorrisPiece();
                        space.piece.owner = currentPlayer;
                        currentPlayer.placePiece();
                        piecePlaced.Invoke(space, activePlayer1 ? 1 : 2);
                        takeTurn();
                        break;
                    }
                case TurnStates.SELECT:
                    {
                        if (space.piece == null || space.piece.owner != getActivePlayer())
                        {
                            break;
                        }
                        selectedSpace = space;
                        turnState = TurnStates.MOVE;
                        updateInstructions();
                        break;
                    }
                case TurnStates.MOVE:
                    {
                        if (space.piece != null)
                        {
                            if (space.piece.owner == getActivePlayer())
                            {
                                selectedSpace = space;
                            }
                            else
                            {
                                break;
                            }
                        }
                        // do the move
                        space.piece = selectedSpace.piece;
                        selectedSpace.piece = null;
                        // todo: update position
                        takeTurn();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

        }

        public void takeTurn()
        {
            activePlayer1 = !activePlayer1;
            // update game state

            MorrisPlayer currentPlayer = (MorrisPlayer)getActivePlayer();
            if (currentPlayer.isPlacingPieces())
            {
                turnState = TurnStates.PLACE;
            }
            else
            {
                turnState = TurnStates.MOVE;
            }

            updateInstructions();
        }

        public void updateInstructions()
        {
            if (isOver())
            {
                setInstruction(string.Format("Game over! Player {0} won!", getWinner() == player1 ? "1" : "2"));
            }
            else
            {
                MorrisPlayer currentPlayer = (MorrisPlayer)getActivePlayer();
                instruction = "Player " + (activePlayer1 ? "1: " : "2: ");
                // what phase are we in?
                if (currentPlayer.isPlacingPieces())
                {
                    // phase 1: place pieces
                    instruction += "Tap an empty space to place a piece.";
                }
                else
                {
                    if (turnState == TurnStates.MOVE)
                    {
                        instruction += "Select an empty space to move to.";
                    }
                    else
                    {
                        // phase 2: move pieces
                        instruction += "Select a piece to move";
                        if (currentPlayer.isFlying())
                        {
                            // phase 3: fly!
                            instruction += " anywhere!";
                        }
                    }
                }
                setInstruction(instruction);
            }
        }

        public event Action instructionUpdated;

        private void setInstruction(string instruction)
        {
            this.instruction = instruction;
            if (instructionUpdated != null)
            {
                instructionUpdated.Invoke();
            }
        }
        public delegate void SpacePlayerEvent(Space space, int PlayerNumber);
        public event SpacePlayerEvent piecePlaced;

        public void init()
        {
            player1 = new MorrisPlayer();
            player2 = new MorrisPlayer();
            player1.init(piecesPerPlayer);
            player2.init(piecesPerPlayer);
            takeTurn();
        }
    }
}
