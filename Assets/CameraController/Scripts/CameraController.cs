using UnityEngine;

public class CameraController : MonoBehaviour
{
    internal bool _blockLerp { get; private set; }

    [SerializeField] Transform _mainTarget,
                               _otherTarget;
    [Space]
    [SerializeField] Camera _mainCamera;
    [Min(0)]
    [SerializeField] float _smoothSpeed = 0.05f;
    [Space]
    [Min(0)]
    [SerializeField] float _minDesiredDistanceTogether = 5;
    [Min(0)]
    [SerializeField] float _minDesiredDistanceAlone = 10;
    [Min(0)]
    [SerializeField] float _distanceOffset = 2;
    [Space]
    [SerializeField] bool _showGizmo = true;

    Mesh _cameraMesh;



    void OnDrawGizmos()
    {
        if (!_showGizmo)
            return;

        if (_cameraMesh == null)
            _cameraMesh = Resources.Load<Mesh>("Meshes/camera");

        Gizmos.color = Color.white;

        Gizmos.DrawSphere(gameObject.transform.position, 0.4f);
        Gizmos.DrawLine(gameObject.transform.position,
                        _mainCamera.transform.position);
        Gizmos.DrawMesh(_cameraMesh,
                        _mainCamera.transform.position,
                        _mainCamera.transform.rotation,
                        new Vector3(0.4f, 0.4f, 0.4f));

        if (_mainTarget == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, _mainTarget.position);

        if (_otherTarget == null)
            return;

        Gizmos.DrawLine(transform.position, _otherTarget.position);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(_mainTarget.position, _otherTarget.position);
    }

    void LateUpdate()
    {
        if (_mainTarget == null)
            return;

        Vector3 target = _mainTarget.position,
                offset = new Vector3(0, 0, -_minDesiredDistanceAlone);

        if (_otherTarget != null)
            SetTwoTargets(ref target, ref offset);

        transform.position = Vector3.Lerp(transform.position, target, _smoothSpeed); // Mathf.Log10()
        _mainCamera.transform.localPosition = Vector3.Lerp(_mainCamera.transform.localPosition, offset, _smoothSpeed);
    }



    void SetTwoTargets(ref Vector3 target, ref Vector3 offset)
    {
        float distance = Vector3.Distance(_mainTarget.position, _otherTarget.position);

        target = GetCentralPosition(distance);
        offset = GetCameraDistance(distance);
    }

    Vector3 GetCameraDistance(float _distance)
    {
        if (_distance >= _minDesiredDistanceTogether - _distanceOffset)
            return new Vector3(0, 0, -_distance - _distanceOffset);
        else
            return new Vector3(0, 0, -_minDesiredDistanceTogether);
    }

    Vector3 GetCentralPosition(float _distance)
    {
        Vector3 direction_move = _mainTarget.position - _otherTarget.position;

        return _mainTarget.position - direction_move.normalized * _distance / 2;
    }



    public void SetPlayerTransform(Transform transform)
    {
        _mainTarget = transform;
    }

    public void SetTargetTransform(Transform transform)
    {
        _otherTarget = transform;
    }
}
