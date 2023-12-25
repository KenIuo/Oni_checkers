using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveControl : MonoBehaviour
{
    [SerializeField] List<MeshRenderer> _checkerMeshes;
    [SerializeField] List<Material> _checkerMaterials;
    [Space]
    [SerializeField] VisualEffect _spawnVFX;

    float dissolveRate = 0.0125f;
    float refreshRate = 0.025f;
    float currentDissolve = 0;

    void Start()
    {
        _checkerMeshes[0].material = _checkerMaterials[0];

        for (byte i = 1; i < _checkerMeshes.Count; i++)
            _checkerMeshes[i].material = _checkerMaterials[1];

        SetDissolve(1);
        StartCoroutine(SpawnCoroutine());
    }

    public void SetDissolve(float dissolve_amount)
    {
        currentDissolve = dissolve_amount;

        foreach (var mat in _checkerMaterials)
            mat.SetFloat(NamesTags.DISSOLVE, currentDissolve);
    }

    /*public IEnumerator DeathCoroutine ()
    {
        if(VFXDeath != null)
            VFXDeath.Play();

        if (skinnedMaterials.Length > 0)
        {
            while (currentDissolve < 1)
            {
                currentDissolve += dissolveRate;

                for (int i = 0; i < skinnedMaterials.Length; i++)
                {
                    skinnedMaterials[i].SetFloat(_dissolveValue, currentDissolve);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }*/

    public IEnumerator SpawnCoroutine()
    {
        if (_spawnVFX != null)
            _spawnVFX.Play();

        while (currentDissolve > 0)
        {
            currentDissolve -= dissolveRate;

            foreach (Material mat in _checkerMaterials)
            {
                mat.SetFloat(NamesTags.DISSOLVE, currentDissolve);
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
