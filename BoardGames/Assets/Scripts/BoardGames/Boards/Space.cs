﻿using UnityEngine;
using System.Collections;

public class Space {
    int row;
    int column;
    public GameObject gameObject;
    public Space(int row, int column) { this.row = row; this.column = column; }
    public int getRow() { return row; }
    public int getColumn() { return column; }
}
