using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BoardGames.util
{
    public static class LineDrawer
    {

        private static Sprite lineSprite = Resources.Load<Sprite>("line");

        /// <summary>
        /// makes a square out of lines centered at centerX, centerY, 0 with a side length of 2 * width and a line thickness of thickness
        /// </summary>
        /// <returns>the ring gameobjecct</returns>
        public static GameObject MakeRing(float centerX, float centerY, float width, float thickness)
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

        public static GameObject MakeLine(float x, float y, float x2, float y2, float thickness)
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
    }
}
