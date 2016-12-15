using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MorrisBoard : Board
{

    [SerializeField]
    int numberOfRings = 3;
    [SerializeField]
    bool diagonalBisections = false;
    [SerializeField]
    bool bisectCenterRing = false;
    Space[,] spaces;
    const int spacesPerRing = 8;
    [SerializeField]
    protected Sprite spaceSprite;
    [SerializeField]
    protected Sprite ringSprite;
    [SerializeField]
    protected Sprite lineSprite;
    [SerializeField]
    protected float relativeRingScale = 3;
    [SerializeField]
    protected float lineThickness = 5;

    public float width { get { return numberOfRings * relativeRingScale * 2; } }

    // a space in morris is determined by that space's ring and the position on the ring.
    // ring indices start at 0 for the innermost ring


    // Use this for initialization
    void Start()
    {
        spaces = new Space[numberOfRings, spacesPerRing];
        for (int i = 0; i < numberOfRings; i++)
        {
            // draw the ring
            GameObject ring = MakeRing(0, 0, (i + 1) * relativeRingScale, lineThickness);
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
            MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);

            // horizontal
            x = 0;
            x2 = 0;
            y = relativeRingScale * numberOfRings;
            y2 = -relativeRingScale * numberOfRings;
            MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);

            // while we're here, make the center space
            GameObject centerspace = makeSpace(0, 0, this.gameObject);
            centerspace.name = "center space";
            centerspace.transform.position = Vector3.zero;
        }
        else
        {
            // make four lines -:-

            // left
            x = -relativeRingScale;
            y = 0;
            y2 = 0;
            x2 = numberOfRings * relativeRingScale * -1;
            MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);

            // right
            x = relativeRingScale;
            y = 0;
            y2 = 0;
            x2 = numberOfRings * relativeRingScale;
            MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);

            // down
            y = -relativeRingScale;
            x = 0;
            x2 = 0;
            y2 = numberOfRings * relativeRingScale * -1;
            MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);

            // up
            y = relativeRingScale;
            x = 0;
            x2 = 0;
            y2 = numberOfRings * relativeRingScale;
            MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);
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
                MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);

                // up right to down left
                x = -relativeRingScale * numberOfRings;
                x2 = relativeRingScale * numberOfRings;
                y = relativeRingScale * numberOfRings;
                y2 = -relativeRingScale * numberOfRings;
                MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);
            }
            else
            {
                // up right
                y = relativeRingScale;
                x = relativeRingScale;
                x2 = numberOfRings * relativeRingScale;
                y2 = numberOfRings * relativeRingScale;
                MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);

                // up left
                y = relativeRingScale;
                x = -relativeRingScale;
                x2 = numberOfRings * -relativeRingScale;
                y2 = numberOfRings * relativeRingScale;
                MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);

                // down right
                y = -relativeRingScale;
                x = relativeRingScale;
                x2 = numberOfRings * relativeRingScale;
                y2 = numberOfRings * -relativeRingScale;
                MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);

                // down left
                y = -relativeRingScale;
                x = -relativeRingScale;
                x2 = numberOfRings * -relativeRingScale;
                y2 = numberOfRings * -relativeRingScale;
                MakeLine(x, y, x2, y2, lineThickness).transform.SetParent(this.transform);
            }
        }

        // finally, init camera
        MorrisCamera cam = new MorrisCamera();
        cam.initCamera(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// makes a square out of lines centered at centerX, centerY, 0 with a side length of 2 * width and a line thickness of thickness
    /// </summary>
    /// <returns>the ring gameobjecct</returns>
    GameObject MakeRing(float centerX, float centerY, float width, float thickness)
    {
        GameObject ring = new GameObject("ring");
        GameObject leftLine = MakeLine(centerX - width, centerY - width, centerX - width, centerY + width, thickness);
        leftLine.transform.SetParent(ring.transform);
        GameObject rightLine = MakeLine(centerX + width, centerY - width, centerX + width, centerY + width, thickness);
        rightLine.transform.SetParent(ring.transform);
        GameObject topLine = MakeLine(centerX - width, centerY - width, centerX + width, centerY - width, thickness);
        topLine.transform.SetParent(ring.transform);
        GameObject bottomLine = MakeLine(centerX - width, centerY + width, centerX + width, centerY + width, thickness);
        bottomLine.transform.SetParent(ring.transform);
        return ring;
    }

    GameObject MakeLine(float x, float y, float x2, float y2, float thickness)
    {
        GameObject line = new GameObject(string.Format("line from {0},{1} to {2},{3}", x, y, x2, y2));
        SpriteRenderer spr = line.AddComponent<SpriteRenderer>();
        spr.sprite = lineSprite;
        Vector2 start = new Vector2(x, y);
        Vector2 end = new Vector2(x2, y2);
        Vector2 direction = end - start;
        float magnitude = direction.magnitude;
        float angle = -90f + (Mathf.Rad2Deg * (Mathf.Atan2(direction.y, direction.x)));
        line.transform.localScale = new Vector3(thickness, magnitude, 1);
        line.transform.position = new Vector3(start.x + 0.5f, start.y + 0.5f, 0);
        line.transform.Rotate(new Vector3(0, 0, 1), angle);
        return line;
    }

    private GameObject makeSpace(int i, int j, GameObject ring)
    {
        // draw the spaces on the ring
        Space space = new Space(i, j);
        GameObject spaceObject = new GameObject(string.Format("Space {0} on ring {1}", j, i));
        space.gameObject = spaceObject;
        spaces[i, j] = space;
        int xPosition, yPosition;
        if (j < 3)
        {
            yPosition = 1;
            xPosition = j % 3 - 1;
        }
        else if (j < 5)
        {
            yPosition = 0;
            if (j == 3)
            {
                xPosition = -1;
            }
            else
            {
                xPosition = 1;
            }
        }
        else
        {
            yPosition = -1;
            xPosition = j % 3 - 1;
        }
        SpriteRenderer spr2 = spaceObject.AddComponent<SpriteRenderer>();
        spr2.sprite = spaceSprite;
        spr2.sortingOrder = 1;
        spaceObject.transform.position = new Vector3(xPosition * relativeRingScale * (i + 1), yPosition * relativeRingScale * (i + 1), 0);
        spaceObject.transform.SetParent(ring.transform);
        return spaceObject;
    }
}
