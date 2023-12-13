using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;
    [SerializeField] float _rotationSpeed;

    void Update()
    {
        if (Input.GetMouseButton(1))
            gameObject.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * _rotationSpeed);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(gameObject.transform.position, 0.5f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(gameObject.transform.position, _mainCamera.transform.position);
    }
}
