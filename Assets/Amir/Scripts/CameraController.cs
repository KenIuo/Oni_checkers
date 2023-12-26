using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;
    [SerializeField] Mesh _cameraMesh;
    [SerializeField] Transform _playerTransform,
                               _targetTransform;
    [SerializeField] float _smoothSpeed = 0.05f;

    internal Vector3 target,
                     offset;



    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawSphere(gameObject.transform.position, 0.4f);
        Gizmos.DrawLine(gameObject.transform.position, _mainCamera.transform.position);
        Gizmos.DrawMesh(_cameraMesh, _mainCamera.transform.position, _mainCamera.transform.rotation, new Vector3(0.4f, 0.4f, 0.4f));
    }

    void LateUpdate()
    {
        if (_targetTransform != null && _playerTransform != null)
        {
            float distance = Vector3.Distance(_playerTransform.position, _targetTransform.position);

            target = GetCentralPosition(distance);
            transform.position = Vector3.Lerp(transform.position, target, _smoothSpeed);

            offset = GetCameraDistance(distance);
            _mainCamera.transform.localPosition = Vector3.Lerp(_mainCamera.transform.localPosition, offset, _smoothSpeed);
        }
    }



    Vector3 GetCameraDistance(float _distance)
    {
        if (_distance >= 3)
            return new Vector3(0, 0, -_distance - 2);
        else
            return new Vector3(0, 0, -5);
    }

    Vector3 GetCentralPosition(float _distance)
    {
        Vector3 direction_move = _playerTransform.position - _targetTransform.position;

        return _playerTransform.position - direction_move.normalized * _distance / 2;
    }

    public void SetPlayerTransform(Transform transform)
    {
        _playerTransform = transform;
    }

    public void SetTargetTransform(Transform transform)
    {
        _targetTransform = transform;
    }
}
