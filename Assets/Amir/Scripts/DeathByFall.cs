using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathByFall : MonoBehaviour
{
    public GameObject _LoseScreen;

    List<GameObject> _eliminationQueue = new();
    MoveSystem _moveSystem;



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
            if (_eliminationQueue[0] == _moveSystem._playerCheckerObject)
                GameObject.Find("Lose").SetActive(true);

            // _eliminationQueue[0].transform.position; // воспроизведение звука смерти
            // исчезновение из системы ходов
            _moveSystem._playersQueue.Remove(_eliminationQueue[0]);

            // обновление очереди игроков в интерфейсе

            _eliminationQueue.Clear();
        }
    }

    public void OnEnter(GameObject other)
    {
        _eliminationQueue.Add(other);
    }



    void Awake()
    {
        _moveSystem = gameObject.GetComponent<MoveSystem>();
    }
}
