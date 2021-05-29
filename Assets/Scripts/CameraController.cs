using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera mainCam, drawingCam;

    public SpriteRenderer background;

    [SerializeField]
    private float zoomStep = 1.0f, minCamSize = 1.0f, maxCamSize = 20.0f;

    private Vector3 dragOrigin;

    public MeshCollider cameraConstraintMesh;
    private Bounds cameraConstraint;

    private float camWidth, camHeight;

    // Start is called before the first frame update
    void Start()
    {
        cameraConstraint = cameraConstraintMesh.bounds;
        camHeight = 2 * Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        PanCamera();
        Zoom();

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, cameraConstraint.min.x + camWidth / 2, cameraConstraint.max.x - camWidth / 2);
        pos.y = Mathf.Clamp(pos.y, cameraConstraint.min.y + camHeight / 2, cameraConstraint.max.y - camHeight / 2);
        transform.position = pos;
    }

    void LateUpdate()
    {
        float increment = ((mainCam.orthographicSize - 5) / 0.25f) * 0.05f + 1f;
        //brushLayerSprite.transform.localScale = new Vector3(increment * 1.89f, increment * 1.89f, 1);
    }

    private void PanCamera()
    {
        // save position of mouse in world space when drag starts (first time clicked)
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = mainCam.ScreenToWorldPoint(Input.mousePosition);
        }

        // calculate distance between drag origin and new position if it is still held down
        if (Input.GetMouseButton(2))
        {
            Vector3 difference = dragOrigin - mainCam.ScreenToWorldPoint(Input.mousePosition);

            Vector3 newPos = transform.position + new Vector3(difference.x, difference.y, 0);

            // move the camera by that distance
            transform.position = newPos;
        }
    }

    public void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float orthoSize = mainCam.orthographicSize;

        //brushLayerSprite.transform.localScale = Vector3.one * ScreenSize.GetScreenToWorldWidth;

        if (scroll < 0f)
        {
            orthoSize = mainCam.orthographicSize - zoomStep;
        }

        if (scroll > 0f)
        {
            orthoSize = mainCam.orthographicSize + zoomStep;
        }

        orthoSize = Mathf.Clamp(orthoSize, minCamSize, maxCamSize);
        mainCam.orthographicSize = orthoSize;
        //drawingCam.orthographicSize = orthoSize;
        //background.size = -Vector2.one * 5f * orthoSize;
    }

    void OnDrawGizmos()
    {
    }
}
