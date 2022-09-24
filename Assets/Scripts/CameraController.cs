using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform targetCamera;

    public Vector2 zoomRange;
    public int zoomLevel;
    public float zoomUnitLength;
    public float zoomDuration;
    public float rotateDuration;
    public float moveSpeed;

    void Start()
    {
        UpdateCameraPosition();
    }

    void Update()
    {
        var scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput > 0)
        {
            DecZoomLevel();
            UpdateCameraPosition();
            return;
        }
        if (scrollInput < 0)
        {
            AddZoomLevel();
            UpdateCameraPosition();
            return;
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(Rotate(90, rotateDuration));
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Rotate(-90, rotateDuration));
            return;
        }
    }

    private void FixedUpdate()
    {
        var verticalInput = Input.GetAxis("Vertical");
        var horizontalInput = Input.GetAxis("Horizontal");
        var direction = new Vector3(targetCamera.forward.x, 0, targetCamera.forward.z) * verticalInput;
        direction += new Vector3(targetCamera.right.x, 0, targetCamera.right.z) * horizontalInput;
        targetCamera.localPosition += direction.normalized * Time.deltaTime * moveSpeed;
    }

    private void UpdateCameraPosition()
    {
        StartCoroutine(Move(zoomDuration));
    }

    IEnumerator Rotate(float angle, float time)
    {
        float number = time / Time.fixedDeltaTime;
        float delta = angle / number;

        for (int i = 0; i < number; i++)
        {
            var point = GetLookingPoint();
            targetCamera.RotateAround(point, Vector3.up, delta);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator Move(float time)
    {
        var targetPoint = GetZoomPoint();
        float number = time / Time.fixedDeltaTime;
        float delta = (targetPoint - targetCamera.position).magnitude / number;

        for (int i = 0; i < number; i++)
        {
            targetPoint = GetZoomPoint();
            targetCamera.position = Vector3.MoveTowards(targetCamera.position, targetPoint, delta);
            yield return new WaitForFixedUpdate();
        }
    }

    private void DecZoomLevel()
    {
        if (zoomLevel <= zoomRange.x)
        {
            return;
        }
        zoomLevel--;
    }

    private void AddZoomLevel()
    {
        if (zoomLevel >= zoomRange.y)
        {
            return;
        }
        zoomLevel++;
    }

    private Vector3 GetZoomPoint()
    {
        var lookingPoint = GetLookingPoint();
        lookingPoint -= targetCamera.forward * zoomUnitLength * zoomLevel;
        return lookingPoint;
    }

    private Vector3 GetLookingPoint()
    {
        var point = targetCamera.position;
        var direct = targetCamera.forward;
        var planeNormal = Vector3.up;
        var planePoint = Vector3.zero;
        float d = Vector3.Dot(planePoint - point, planeNormal) / Vector3.Dot(direct.normalized, planeNormal);
        return d * direct.normalized + point;
    }


}