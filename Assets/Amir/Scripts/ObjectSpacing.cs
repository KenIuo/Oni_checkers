using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpacing : MonoBehaviour
{
    [Min(2)]
    [SerializeField] List<GameObject> _gameObjects = new List<GameObject>();
    [SerializeField] float _radius;

    Vector3 _center = Vector3.zero;

    public void RebuildScene()
    {
        // перемещение в нужное положение и поворот объектов
    }



    void OnDrawGizmosSelected()
    {
        // отображение коллайдеров в новой позиции
    }
}
