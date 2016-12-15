using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// Board menu is a unity editor window for editing a board class
/// </summary>
public class BoardMenu : EditorWindow {
    string boardName = "Hello, Board!";
    bool shouldDeleteOldBoard = true;
    enum BoardTypes
    {
        GRID_BOARD,
        CHECKER_BOARD,
        MORRIS_BOARD
    }
    BoardTypes selectedBoardType = BoardTypes.GRID_BOARD;
    GameObject board;


    // Add menu named "My Window" to the Window menu
    [MenuItem("Board Games/Board Menu")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        BoardMenu window = (BoardMenu)EditorWindow.GetWindow(typeof(BoardMenu));
        window.Show();
    }

    // create a new board gameobject
    void Generate()
    {
        if (shouldDeleteOldBoard && board != null)
        {
            DestroyImmediate(board);
        }
        board = new GameObject(boardName);
        switch(selectedBoardType)
        {
            case BoardTypes.GRID_BOARD:
                {
                    board.AddComponent<GridBoard>();
                    break;
                }
            case BoardTypes.CHECKER_BOARD:
                {
                    board.AddComponent<CheckerBoard>();
                    break;
                }
            case BoardTypes.MORRIS_BOARD:
                {
                    board.AddComponent<MorrisBoard>();
                    break;
                }
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        boardName = EditorGUILayout.TextField("Text Field", boardName);
        selectedBoardType = (BoardTypes) EditorGUILayout.EnumPopup("Board Type", selectedBoardType);
        EditorGUILayout.Toggle("Replace previous board", shouldDeleteOldBoard);
        if (GUILayout.Button("Generate Board"))
        {
            Generate();
        }

    }

}
