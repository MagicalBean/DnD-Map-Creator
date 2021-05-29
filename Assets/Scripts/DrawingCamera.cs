using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingCamera : MonoBehaviour
{
    public Camera drawingCam;
    public SpriteRenderer overlayImage;
    public RawImage rawImage;

    WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

    public IEnumerator RenderBrushStrokes()
    {
        yield return frameEnd;

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = drawingCam.targetTexture;

        //drawingCam.cullingMask = 1 << LayerMask.NameToLayer("Brushes");
        //drawingCam.clearFlags = CameraClearFlags.Nothing;

        drawingCam.Render();

        Texture2D image = new Texture2D(Screen.width, Screen.height);
        image.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        image.Apply();

        RenderTexture.active = currentRT;

        overlayImage.sprite = Sprite.Create(image, new Rect(0.0f, 0.0f, image.width, image.height), new Vector2(0.5f, 0.5f), 100);
    }

    // Start is called before the first frame update
    void Start()
    {
        drawingCam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(RenderBrushStrokes());
    }
}
