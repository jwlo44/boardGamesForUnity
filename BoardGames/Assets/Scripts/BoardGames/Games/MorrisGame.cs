using BoardGames.Games.Pieces;
using BoardGames.Games.Players;
using System;
using System.Linq;

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
            POUND,
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
            return activePlayer1 ? player1 : player2;
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
                        MorrisPlayer currentPlayer = (MorrisPlayer)getActivePlayer();
                        if (space.piece != null)
                        {
                            if (space.piece.owner == currentPlayer)
                            {
                                selectedSpace = space;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (!selectedSpace.adjacentSpaces.Contains(space) && !currentPlayer.isFlying())
                        {
                            break;
                        }
                        // do the move
                        movedPiece.Invoke(from: selectedSpace, to: space);
                        space.piece = selectedSpace.piece;
                        selectedSpace.piece = null;
                        if (isMill(space))
                        {
                            turnState = TurnStates.POUND;
                            updateInstructions();
                            break;
                        }
                        else
                        {
                            takeTurn();
                            break;
                        }
                    }
                case TurnStates.POUND:
                    {
                        MorrisPlayer currentPlayer = (MorrisPlayer)getActivePlayer();
                        if (space.piece == null || space.piece.owner == currentPlayer)
                        {
                            break;
                        }
                        piecePounded.Invoke(space);
                        MorrisPlayer opponent = (MorrisPlayer)space.piece.owner;
                        opponent.losePiece((MorrisPiece)space.piece);
                        takeTurn();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

        }

        private bool isMill(Space space)
        {
            return space.adjacentSpaces.Where((s) => s.piece != null && s.piece.owner == getActivePlayer()).Count() >= 2;
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
                turnState = TurnStates.SELECT;
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
                    else if (turnState == TurnStates.SELECT)
                    {
                        // phase 2: move pieces
                        instruction += "Select a piece to move";
                        if (currentPlayer.isFlying())
                        {
                            // phase 3: fly!
                            instruction += " anywhere!";
                        }
                    }
                    else // POUND!
                    {
                        instruction += "Select an opponent's piece to pound!";
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

        public delegate void MoveEvent(Space from, Space to);
        public event MoveEvent movedPiece;

        public event SpaceTapHandler.SpaceEvent piecePounded;

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
