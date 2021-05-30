using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localScale = Vector3.one * ScreenSize.GetScreenToWorldWidth;
    }

    struct ScreenSize
    {
        public static float GetScreenToWorldHeight
        {
            get
            {
                Vector2 topRightCorner = new Vector2(1, 1);
                Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);

                var height = edgeVector.y * 2;
                return height;
            }
        }

        public static float GetScreenToWorldWidth
        {
            get
            {
                Vector2 topRightCorner = new Vector2(1, 1);
                Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner); var width = edgeVector.x * 2;
                return width;
            }
        }
    }
}
