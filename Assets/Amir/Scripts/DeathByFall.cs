using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathByFall : MonoBehaviour
{
    [SerializeField] GameObject _playerCheckerObject;
    [SerializeField] GameObject _LoseScreen;

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
        else if (_eliminationQueue.Count == 1) // дополнить условие на проверку был ли водящий в списке
        {
            if (_eliminationQueue[0] == _playerCheckerObject)
                _LoseScreen.SetActive(true);

            // _eliminationQueue[0].transform.position; // воспроизведение звука смерти
            // исчезновение из системы ходов
            _eliminationQueue.Clear();
        }
    }



    public void OnEnter(Collider other)
    {
        _eliminationQueue.Add(other.gameObject);

        if (other.TryGetComponent(out DefaultState default_state))
            default_state._isCheckable = false;

        CheckQueue(); // убрать отсюда и вызывать из перехода хода
    }
}
