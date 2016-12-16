using UnityEngine;
using BoardGames.Games.Pieces;
using System.Collections.Generic;

public class Space {
    int row;
    int column;
    public GameObject gameObject;
    public Space(int row, int column) { this.row = row; this.column = column; }
    public int getRow() { return row; }
    public int getColumn() { return column; }
    public Piece piece;
    public HashSet<Space> adjacentSpaces;
}
