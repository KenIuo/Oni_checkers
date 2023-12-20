using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class CheckerAppearance : MonoBehaviour
{
    [SerializeField] Material _bodyMaterial;
    [SerializeField] VisualEffect _chargeVFX;
    [SerializeField] Image _markerImage;
    [SerializeField] Color _playerColor;



    void Awake()
    {
        _markerImage.color = _playerColor;
        _bodyMaterial.SetVector("Base_color", _playerColor);
        //_bodyMaterial.SetVectorArray("Color", new List<Vector4> { _playerColor, _playerColor });
        _chargeVFX.SetVector4("Color", _playerColor);

        //collision.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
    }
}
