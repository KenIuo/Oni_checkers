using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class DeathByFall
{
    public GameObject _LoseScreen;

    List<GameObject> _eliminationQueue = new();

    public void CheckQueue()
    {
        /*if (_eliminationQueue.Count > 1
        &&  _eliminationQueue.Contains(TurnSystem.Instance._playersQueue[TurnSystem.Instance._currentPlayer]))
        {
            foreach (GameObject player in _eliminationQueue)
                player.transform.position = player.GetComponent<CheckerController>()._standardPosition.position;

            _eliminationQueue.Clear();
        }
        else if (_eliminationQueue.Count > 0)
        {
            if (_eliminationQueue[0] == TurnSystem.Instance._playerCheckerObject)
                GameObject.Find("Lose").SetActive(true);

            // _eliminationQueue[0].transform.position; // воспроизведение звука смерти
            // исчезновение из системы ходов
            TurnSystem.Instance._playersQueue.RemoveAt(TurnSystem.Instance._playersQueue.IndexOf(_eliminationQueue[0]));

            // обновление очереди игроков в интерфейсе

            _eliminationQueue.Clear();
        }*/
    }

    public void OnEnter(GameObject other)
    {
        _eliminationQueue.Add(other);
    }
}
