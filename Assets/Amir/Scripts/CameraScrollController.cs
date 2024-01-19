using UnityEngine;
using UnityEngine.EventSystems;

public class CameraScrollController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] CameraController _camera;
    [SerializeField] Transform _arenasParent;
    [Space]
    [SerializeField] float _sensitivity = 1;

    Transform _arenasScroll;
    int _arenasCount = 0,
        _spacing = 50;
    bool _onHover = false;

    internal int _selectedArena { get; private set; } = 1;



    void Awake()
    {
        _arenasCount = _arenasParent.childCount - 1;

        _arenasScroll = Instantiate(_arenasParent.transform);
        _arenasScroll.name = $"Empty Transform";
        _arenasScroll.parent = _arenasParent;

        if (_arenasScroll.childCount > 0)
            while (_arenasScroll.childCount != 0)
                DestroyImmediate(_arenasScroll.GetChild(0).gameObject);

        _camera.SetPlayerTransform(_arenasScroll);
    }

    void Update()
    {
        if (!_onHover)
            return;

        if (!Input.GetMouseButton(0))
            return;

        float x_input;

        if (Input.GetAxis(AxisTags.MOUSE_X) != 0)
            x_input = -Input.GetAxis(AxisTags.MOUSE_X) * _sensitivity;
        else if (Input.GetAxis(AxisTags.MOUSE_WHEEL) != 0)
            x_input = Input.GetAxis(AxisTags.MOUSE_WHEEL) * _sensitivity;
        else
            return;

        _arenasScroll.localPosition += new Vector3(x_input, 0, 0);
    }

    void LateUpdate()
    {
        if (_arenasScroll.localPosition.x >= _arenasCount * _spacing)
            _arenasScroll.localPosition = Vector3.right * _arenasCount * _spacing;
        else if (_arenasScroll.localPosition.x <= 0)
            _arenasScroll.localPosition = Vector3.zero;
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        _onHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _onHover = false;
    }
}
