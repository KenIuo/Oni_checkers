using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathByFall : MonoBehaviour
{
    List<GameObject> _eliminationQueue = new List<GameObject>();



    public void CheckQueue()
    {
        if (_eliminationQueue.Count > 1)
        {
            foreach (GameObject go in _eliminationQueue)
            {
                if (go.TryGetComponent(out DefaultState default_state))
                    go.transform.position = default_state._standardPosition.position;
            }

            _eliminationQueue.Clear();
        }
        else if (_eliminationQueue.Count == 1)
        {
            _eliminationQueue[0].SetActive(false);
            _eliminationQueue.Clear();
        }
    }



    void OnTriggerEnter(Collider other)
    {
        _eliminationQueue.Add(other.gameObject);
    }
}
