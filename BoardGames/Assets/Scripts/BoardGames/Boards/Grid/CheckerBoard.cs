using UnityEngine;
using System.Collections;

public class CheckerBoard : GridBoard {
    [SerializeField]
    protected Sprite checkeredSprite;

	// Use this for initialization
	override protected void Start () {
        base.Start();
        bool checkered = true;
        for (int i = 0; i < numberOfRows; i++)
        {
            for (int j = 0; j < numberOfColumns; j++)
            {
                if (checkered)
                {
                    spaces[i, j].gameObject.GetComponent<SpriteRenderer>().sprite = checkeredSprite;
                }
                checkered = !checkered;
            }
        }
	}

	// Update is called once per frame
	void Update () {
	
	}
}
