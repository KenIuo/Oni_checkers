using UnityEngine;

public class PointerSystemController : MonoBehaviour
{
    [SerializeField] GameObject _playerChecker;
    [SerializeField] GameObject _pointingArrow; // настраивается только местоположение (на середине шашки игрока) и поворот (изначально смотрит вперёд)
    [SerializeField] GameObject _tensionForce; // настраивается только длина (Z координата) (изначально на максимальной или почти максимальной длине)
    [SerializeField] LayerMask _layerMask;

    Camera _camera;
    CheckerController _playerCheckerController;
    Vector3 _playerPos;
    float _currentRadius;
    float _speed = 2f;



    void DrawArrow(Vector3 hit_position)
    {
        _playerPos = new Vector3(_playerChecker.transform.position.x,
                                 0.1f,
                                 _playerChecker.transform.position.z);

        Vector3 direction_move = GetDirectionVector(hit_position);

        if (Input.GetMouseButton(0))
        {
            _currentRadius = _playerCheckerController.GetCurrentRadius(hit_position);

            if (_currentRadius != 0)
                TransformPointingArrow(direction_move);
            else
                _pointingArrow.SetActive(false);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (_currentRadius != 0)
                _playerCheckerController.LaunchChecker(_currentRadius, direction_move);

            _pointingArrow.SetActive(false);
        }
    }

    Vector3 GetDirectionVector(Vector3 hit_position)
    {
        hit_position.y = _playerPos.y;

        Vector3 direction_move = _playerPos - hit_position;
        direction_move.Normalize();

        return direction_move;
    }

    void TransformPointingArrow(Vector3 direction_move)
    {
        _pointingArrow.SetActive(true);
        _pointingArrow.transform.position = _playerPos;
        _pointingArrow.transform.rotation = Quaternion.LookRotation(direction_move);

        _playerChecker.transform.rotation = Quaternion.LookRotation(direction_move);

        _tensionForce.transform.localScale = new Vector3(1, 1, _currentRadius / 5);
    }

    void RotateCamera()
    {
        if (Input.GetMouseButton(1))
            gameObject.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * _speed);
    }



    void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
        _playerCheckerController = _playerChecker.GetComponent<CheckerController>();
    }

    void Update()
    {
        // (_playerCheckerController._gameStarted && !TurnSystem.Instance._didMoved)
        if (_playerCheckerController._state.Equals(CheckerState.Turning))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (TurnSystem.Instance._playerID == TurnSystem.Instance._currentPlayer
            &&  Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, _layerMask))
            {
                if (_playerChecker.activeInHierarchy)
                    DrawArrow(hit.point);
            }
            else
                _pointingArrow.SetActive(false);
        }

        RotateCamera();
    }
}
