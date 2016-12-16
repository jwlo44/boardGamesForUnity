using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BoardGames.util;
using BoardGames.Games;
using UnityEngine.UI;


// board controls visuals and input
public class MorrisBoard : Board
{
    [SerializeField]
    int numberOfRings = 3;
    [SerializeField]
    bool diagonalBisections = false;
    [SerializeField]
    bool bisectCenterRing = false;
    [SerializeField]
    int piecesPerPlayer = 9;
    Space[,] spaces;
    const int spacesPerRing = 8;
    [SerializeField]
    protected GameObject spacePrefab;
    [SerializeField]
    protected GameObject player1PiecePrefab;
    [SerializeField]
    protected GameObject player2PiecePrefab;
    [SerializeField]
    protected Sprite lineSprite;
    [SerializeField]
    protected float relativeRingScale = 3;
    [SerializeField]
    protected float lineThickness = 5;
    [SerializeField]
    protected Text instructions;

    public float width { get { return numberOfRings * relativeRingScale * 2; } }

    public MorrisGame game;

    private GameObject centerSpace = null;
    // Use this for initialization
    void Start()
    {
        game = new MorrisGame(piecesPerPlayer);

        spaces = new Space[numberOfRings, spacesPerRing];
        for (int i = 0; i < numberOfRings; i++)
        {
            // draw the ring
            GameObject ring = LineDrawer.MakeRing(0, 0, (i + 1) * relativeRingScale, lineThickness);
            ring.name += " " + i;
            ring.transform.SetParent(this.transform);

            for (int j = 0; j < spacesPerRing; j++)
            {
                makeSpace(i, j, ring);
            }
        }
        // draw some edges
        float x, y, x2, y2;
        if (bisectCenterRing)
        {
            // go inside center of the one ring
            // vertical
            x = relativeRingScale * numberOfRings;
            x2 = -relativeRingScale * numberOfRings;
            y = 0;
            y2 = 0;
            LineDrawer.MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);

            // horizontal
            x = 0;
            x2 = 0;
            y = relativeRingScale * numberOfRings;
            y2 = -relativeRingScale * numberOfRings;
            LineDrawer.MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);

            // while we're here, make the center space
            makeCenterSpace(this.gameObject);
        }
        else
        {
            // make four lines -:-

            // left
            x = -relativeRingScale;
            y = 0;
            y2 = 0;
            x2 = numberOfRings * relativeRingScale * -1;
            LineDrawer.MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);

            // right
            x = relativeRingScale;
            y = 0;
            y2 = 0;
            x2 = numberOfRings * relativeRingScale;
            LineDrawer.MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);

            // down
            y = -relativeRingScale;
            x = 0;
            x2 = 0;
            y2 = numberOfRings * relativeRingScale * -1;
            LineDrawer.MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);

            // up
            y = relativeRingScale;
            x = 0;
            x2 = 0;
            y2 = numberOfRings * relativeRingScale;
            LineDrawer.MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);
        }

        if (diagonalBisections)
        {
            // draw diagonals
            if (bisectCenterRing)
            {
                // go inside center of the one ring
                // up left to down right
                x = relativeRingScale * numberOfRings;
                x2 = -relativeRingScale * numberOfRings;
                y = relativeRingScale * numberOfRings;
                y2 = -relativeRingScale * numberOfRings;
                LineDrawer.MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);

                // up right to down left
                x = -relativeRingScale * numberOfRings;
                x2 = relativeRingScale * numberOfRings;
                y = relativeRingScale * numberOfRings;
                y2 = -relativeRingScale * numberOfRings;
                LineDrawer.MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);
            }
            else
            {
                // up right
                y = relativeRingScale;
                x = relativeRingScale;
                x2 = numberOfRings * relativeRingScale;
                y2 = numberOfRings * relativeRingScale;
                LineDrawer.MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);

                // up left
                y = relativeRingScale;
                x = -relativeRingScale;
                x2 = numberOfRings * -relativeRingScale;
                y2 = numberOfRings * relativeRingScale;
                LineDrawer.MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);

                // down right
                y = -relativeRingScale;
                x = relativeRingScale;
                x2 = numberOfRings * relativeRingScale;
                y2 = numberOfRings * -relativeRingScale;
                LineDrawer.MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);

                // down left
                y = -relativeRingScale;
                x = -relativeRingScale;
                x2 = numberOfRings * -relativeRingScale;
                y2 = numberOfRings * -relativeRingScale;
                LineDrawer.MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);
            }
        }

        //  init camera
        MorrisCamera cam = new MorrisCamera();
        cam.initCamera(this);

        // subscribe to events
        game.instructionUpdated += OnInstructionUpdatedHandler;
        game.piecePlaced += OnPiecePlacedHandler;
        game.movedPiece += OnPieceMovedHandler;

        // init game
        game.init();
    }

    void OnDestroy()
    {
        // unsubscribe
        game.instructionUpdated -= OnInstructionUpdatedHandler;
        game.piecePlaced -= OnPiecePlacedHandler;
        game.movedPiece -= OnPieceMovedHandler;
    }

    private void OnInstructionUpdatedHandler()
    {
        instructions.text = game.instruction;
    }


    private void OnSpaceClickHandler(Space space)
    {
        game.HandleSpaceClicked(space);
    }

    private void OnPiecePlacedHandler(Space space, int playerNumber)
    {
        GameObject placedPiece;
        if (playerNumber == 1)
        {
            // put a piece at that space
            placedPiece = GameObject.Instantiate(player1PiecePrefab);
        }
        else
        {
            placedPiece = GameObject.Instantiate(player2PiecePrefab);
        }
        placedPiece.transform.position = space.gameObject.transform.position;
        placedPiece.transform.SetParent(space.gameObject.transform);
        space.piece.gameObject = placedPiece;
    }

    private void OnPieceMovedHandler(Space from, Space to)
    {
        from.piece.gameObject.transform.SetParent(to.gameObject.transform);
        from.piece.gameObject.transform.position = to.gameObject.transform.position;
    }

    private void makeCenterSpace(GameObject centerRing)
    {
        Space space = new Space(-1, -1);
        centerSpace = GameObject.Instantiate(spacePrefab);
        centerSpace.name = "center space";
        centerSpace.transform.position = Vector3.zero;
        SpriteRenderer spr2 = centerSpace.GetComponent<SpriteRenderer>();
        spr2.sortingOrder = 1;
        centerSpace.transform.SetParent(centerRing.transform);
        SpaceTapHandler tap = centerSpace.GetComponent<SpaceTapHandler>();
        tap.setSpace(space);
        tap.onClick += OnSpaceClickHandler;
        space.adjacentSpaces = getAdjacentSpaces(space);
    }

    private GameObject makeSpace(int i, int j, GameObject ring)
    {
        // draw the spaces on the ring
        Space space = new Space(i, j);
        GameObject spaceObject = GameObject.Instantiate(spacePrefab);
        spaceObject.name = string.Format("Space {0} on ring {1}", j, i);
        space.gameObject = spaceObject;
        spaces[i, j] = space;
        // 0 1 2
        // 7   3
        // 6 5 4
        int xPosition, yPosition;
        if (j < 3)
        {
            yPosition = 1;
            xPosition = j % 3 - 1;
        }

        else if (j > 3 &&  j < 7)
        {
            yPosition = -1;
            xPosition = -(j - 5);
        }
        else // 7 or 3
        {
            yPosition = 0;
            if (j == 3)
            {
                xPosition = 1;
            }
            else // 7
            {
                xPosition = -1;
            }
        }
        SpriteRenderer spr2 = spaceObject.GetComponent<SpriteRenderer>();
        spr2.sortingOrder = 1;
        spaceObject.transform.position = new Vector3(xPosition * relativeRingScale * (i + 1), yPosition * relativeRingScale * (i + 1), 0);
        spaceObject.transform.SetParent(ring.transform);
        SpaceTapHandler tap = spaceObject.GetComponent<SpaceTapHandler>();
        tap.setSpace(space);
        tap.onClick += OnSpaceClickHandler;
        space.adjacentSpaces = getAdjacentSpaces(space);
        return spaceObject;
    }

    private HashSet<Space> getAdjacentSpaces(Space space)
    {
        HashSet<Space> adjacentSpaces = new HashSet<Space>();
        if (isCenterSpace(space))
        {
            if (diagonalBisections)
            {
                for (int i = 0; i < spacesPerRing; i++)
                {
                    adjacentSpaces.Add(spaces[0, i]);
                }
            }
            else
            {
                for (int i = 1; i < spacesPerRing; i += 2)
                {
                    adjacentSpaces.Add(spaces[0, i]);
                }
            }
        }
        else
        {
            if (diagonalBisections || !isCornerSpace(space))
            {
                // rows (rings)
                int row = space.getRow() + 1;
                if (row < numberOfRings)
                {
                    adjacentSpaces.Add(spaces[row, space.getColumn()]);
                }
                row = space.getRow() - 1;
                if (row >= 0 || (bisectCenterRing && row == -1))
                {
                    adjacentSpaces.Add(spaces[row, space.getColumn()]);
                }

            }
            // columns (within ring)
            int column = space.getColumn() + 1;
            if (column < spacesPerRing)
            {
                adjacentSpaces.Add(spaces[space.getRow(), column]);
            }
            column = space.getColumn() - 1;
            if (column >= 0 || (column == -1 && bisectCenterRing))
            {
                adjacentSpaces.Add(spaces[space.getRow(), column]);
            }
        }
        return adjacentSpaces;
    }

    private bool isCornerSpace(Space space)
    {
        return space.getRow() % 2 == 0;
    }
    private bool isCenterSpace(Space space)
    {
        return space.getColumn() == -1;
    }
}
