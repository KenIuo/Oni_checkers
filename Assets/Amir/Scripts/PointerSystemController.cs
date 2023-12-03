using UnityEngine;

public class PointerSystemController : MonoBehaviour
{
    [SerializeField] GameObject _pointingArrow; // настраивается только местоположение (на середине шашки игрока) и поворот (изначально смотрит вперёд)
    [SerializeField] GameObject _tensionForce; // настраивается только длина (Z координата) (изначально на максимальной или почти максимальной длине)
    [SerializeField] Camera _camera;

    CheckerController _playerCheckerController;
    Vector3 _playerPos;
    float _currentRadius;
    Vector3 direction_move;



    void DrawArrow(Vector3 hit_position)
    {
        _playerPos = new Vector3(transform.position.x, 0.1f, transform.position.z);

        direction_move = GetDirectionVector(hit_position);

        if (Input.GetMouseButton(0))
        {
            CursorManager.Instance.SetHandCursor();

            _currentRadius = _playerCheckerController.GetCurrentRadius(hit_position);

            if (_currentRadius != 0)
            {
                transform.rotation = Quaternion.LookRotation(direction_move);

                TransformPointingArrow(direction_move);
            }
            else
                _pointingArrow.SetActive(false);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (_currentRadius != 0)
                _playerCheckerController.LaunchChecker(_currentRadius, direction_move);

            CursorManager.Instance.SetStandartCursor();
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

        _tensionForce.transform.localScale = new Vector3(1, 1, _currentRadius / 5);
    }



    void Awake()
    {
        _playerCheckerController = gameObject.GetComponent<CheckerController>();
    }

    void Update()
    {
        if (_playerCheckerController._state.Equals(CheckerState.Turning))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, LayersTags.ARROW_LAYER)
            && !TurnSystem.Instance._isOnPause
            &&  gameObject.activeInHierarchy)
                DrawArrow(hit.point);
            else
            {
                CursorManager.Instance.SetStandartCursor();
                _pointingArrow.SetActive(false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_playerPos, _playerPos + (direction_move * 2));
    }
}
