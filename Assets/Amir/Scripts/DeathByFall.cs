using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathByFall : MonoBehaviour
{
    /*public GameObject _LoseScreen;

    List<GameObject> _eliminationQueue = new();
    TurnSystem _turnSystem;



    public void CheckQueue()
    {
        if (_eliminationQueue.Count > 1
        &&  _eliminationQueue.Contains(_turnSystem._playersQueue[_turnSystem._currentPlayer]))
        {
            foreach (GameObject player in _eliminationQueue)
                player.transform.position = player.GetComponent<CheckerState>()._standardPosition.position;

            _eliminationQueue.Clear();
        }
        else if (_eliminationQueue.Count > 0)
        {
            if (_eliminationQueue[0] == _turnSystem._playerCheckerObject)
                GameObject.Find("Lose").SetActive(true);

            // _eliminationQueue[0].transform.position; // воспроизведение звука смерти
            // исчезновение из системы ходов
            _turnSystem._playersQueue.RemoveAt(_turnSystem._playersQueue.IndexOf(_eliminationQueue[0]));

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
        _turnSystem = gameObject.GetComponent<TurnSystem>();
    }*/
}
