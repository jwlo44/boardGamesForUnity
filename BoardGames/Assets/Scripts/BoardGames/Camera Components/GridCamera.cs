using UnityEngine;
using System.Collections;

public class GridCamera {

	Camera cam = Camera.main;
    public GridBoard board;


    public void initCamera(GridBoard board) {
        this.board = board;
        cam.orthographic = true;
        int width = board.width;
        int height = board.height;
        if (height > width)
        {
            cam.orthographicSize = height / 2.0f;
        }
        else
        {
            cam.orthographicSize = width / 2.0f;
        }
        cam.transform.position = new Vector3(width / 2.0f, height / 2.0f, -10);

    }

}
