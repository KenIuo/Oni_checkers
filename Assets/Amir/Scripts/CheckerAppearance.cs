using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class CheckerAppearance : MonoBehaviour
{
    [SerializeField] MeshRenderer _bodyMaterial;
    [SerializeField] VisualEffect _chargeVFX;
    [SerializeField] Image _markerImage;
    [SerializeField] Color _playerColor;



    void Awake()
    {
        _markerImage.color = _playerColor;
        _bodyMaterial.material.color = _playerColor;
        _chargeVFX.SetVector4("Color", _playerColor);

        //collision.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
    }
}
