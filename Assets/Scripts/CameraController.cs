using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform targetCamera;

    public Vector2 ZoomRange;
    public int ZoomLevel;
    public float ZoomUnitLength;
    public float ZoomDuration;
    public float RotateDuration;
    public float MoveSpeed;

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
            StartCoroutine(Rotate(90, RotateDuration));
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Rotate(-90, RotateDuration));
            return;
        }
    }

    private void FixedUpdate()
    {
        var verticalInput = Input.GetAxis("Vertical");
        var horizontalInput = Input.GetAxis("Horizontal");
        var direction = new Vector3(targetCamera.forward.x, 0, targetCamera.forward.z) * verticalInput;
        direction += new Vector3(targetCamera.right.x, 0, targetCamera.right.z) * horizontalInput;
        targetCamera.localPosition += direction * Time.deltaTime * MoveSpeed;
    }

    private void UpdateCameraPosition()
    {
        StartCoroutine(Move(ZoomDuration));
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
        if (ZoomLevel <= ZoomRange.x)
        {
            return;
        }
        ZoomLevel--;
    }

    private void AddZoomLevel()
    {
        if (ZoomLevel >= ZoomRange.y)
        {
            return;
        }
        ZoomLevel++;
    }

    private Vector3 GetZoomPoint()
    {
        var lookingPoint = GetLookingPoint();
        lookingPoint -= targetCamera.forward * ZoomUnitLength * ZoomLevel;
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
