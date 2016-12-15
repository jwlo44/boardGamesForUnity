using UnityEngine;
using System.Collections;

// a grid board is a 2D grid of square spaces
public class GridBoard : MonoBehaviour {
    [SerializeField]
    protected int spaceSize = 1;
    [SerializeField]
    protected int numberOfRows = 8;
    [SerializeField]
    protected int numberOfColumns = 8;
    [SerializeField]
    protected Sprite spaceSprite;

    protected Space[,] spaces;

    public Space getSpace(int row, int column) {
        return spaces[row,column];
    }

    public int height {
        get { return spaces.GetLength(1) * spaceSize; }
    }
    public int width {
        get { return spaces.GetLength(0) * spaceSize; }
    }

	// Use this for initialization
	protected virtual void Start () {
        spaces = new Space[numberOfRows, numberOfColumns];
        for (int i = 0; i < numberOfRows; i++)
        {
            for (int j = 0; j < numberOfColumns; j++)
            {
                // make a grid of spaces
                spaces[i, j] = new Space(i, j);
                GameObject space = new GameObject(string.Format("space {0}, {1}", i, j));
                SpriteRenderer spr = space.AddComponent<SpriteRenderer>();
                spr.sprite = spaceSprite;
                space.transform.position = new Vector3(i, j, 0);
                space.transform.SetParent(this.transform);
                spaces[i, j].gameObject = space;
            }
        }
        // finally, init the camera.
        GridCamera camera = new GridCamera();
        camera.initCamera(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
