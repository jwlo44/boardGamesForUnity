using UnityEngine;
using System.Collections;

public class MorrisCamera
{

    Camera cam = Camera.main;
    public MorrisBoard board;
    // the gutter size is the extra padding we add to the camera's viewport around the board.
    const float gutterSize = 2.0f;


    public void initCamera(MorrisBoard board)
    {
        this.board = board;
        cam.orthographic = true;
        float width = board.width;
        cam.orthographicSize = width / 2.0f + gutterSize;
        cam.transform.position = new Vector3(0, 0, -10);
    }

}
