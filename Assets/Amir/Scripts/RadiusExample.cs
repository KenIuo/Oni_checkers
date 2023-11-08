using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour // подлежит удалению
{
    // adjust in the inspector
    public float maxRadius = 2;

    private Vector3 startPosition;

    [SerializeField] private LineRenderer line;
    [SerializeField] private Collider collider;
    [SerializeField] private Camera camera;

    private void Awake()
    {
        line.positionCount = 0;

        line = GetComponentInChildren<LineRenderer>();
        collider = GetComponent<Collider>();
        camera = Camera.main;
    }

    // wherever you dragging starts
    private void OnMouseDown()
    {
        line.positionCount = 2;

        startPosition = collider.ClosestPoint(camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z)));

        var positions = new[] { startPosition, startPosition };

        line.SetPositions(positions);
    }

    // while dragging
    private void OnMouseDrag()
    {
        var currentPosition = GetComponent<Collider>().ClosestPoint(camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z)));

        // get vector between positions
        var difference = currentPosition - startPosition;

        // normalize to only get a direction with magnitude = 1
        var direction = difference.normalized;

        // here you "clamp" use the smaller of either
        // the max radius or the magnitude of the difference vector
        var distance = Mathf.Min(maxRadius, difference.magnitude);


        // and finally apply the end position
        var endPosition = startPosition + direction * distance;

        line.SetPosition(1, endPosition);
    }
}
