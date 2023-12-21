using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveControl : MonoBehaviour
{
    [SerializeField] List<Material> _materials;
    [SerializeField] VisualEffect _spawnVFX;

    float dissolveRate = 0.0125f;
    float refreshRate = 0.025f;
    float currentDissolve = 0;

    void Start()
    {
        SetDissolve(1);

        StartCoroutine(SpawnCoroutine());
    }

    public void SetDissolve(float dissolve_amount)
    {
        currentDissolve = dissolve_amount;

        foreach (var mat in _materials)
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

            foreach (Material mat in _materials)
            {
                mat.SetFloat(NamesTags.DISSOLVE, currentDissolve);
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
