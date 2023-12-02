using UnityEngine;
using UnityEngine.UI;

public class CheckerAppearance : MonoBehaviour
{
    [SerializeField] MeshRenderer _bodyMaterial;
    [SerializeField] Image _markerImage;
    [SerializeField] Color _playerColor;



    void Awake()
    {
        _markerImage.color = _playerColor;
        _bodyMaterial.material.color = _playerColor;
        gameObject.GetComponent<CheckerController>()._chargeVFX.SetVector4("Color", _playerColor);

        //collision.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
    }
}
