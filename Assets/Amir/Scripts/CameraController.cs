using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;
    [SerializeField] float _rotationSpeed;
    [SerializeField] Mesh _cameraMesh;



    void Update()
    {
        if (Input.GetMouseButton(1))
            gameObject.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * _rotationSpeed);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawSphere(gameObject.transform.position, 0.4f);
        Gizmos.DrawLine(gameObject.transform.position, _mainCamera.transform.position);
        Gizmos.DrawMesh(_cameraMesh, _mainCamera.transform.position, _mainCamera.transform.rotation, new Vector3(0.4f, 0.4f, 0.4f));
    }
}
