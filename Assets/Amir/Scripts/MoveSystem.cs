using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveSystem : MonoBehaviour
{
    public List<GameObject> _playersQueue = new(4);
    public GameObject _playerCheckerObject;

    byte _currentPlayer = 0;



    /// <summary>
    ///  должен вызываться при старте игры (т.е. после нажатия на кнопку играть в гл. меню)
    /// </summary>
    public void ShuffleListOfPlayers()
    {
        _playersQueue.Shuffle();

        foreach (GameObject player in _playersQueue)
        {
            if (player != _playerCheckerObject)
            {
                //p.AddComponent<AIScript>();
            }
        }
    }

    public void CheckConditions()
    {

    }



    private void Start()
    {
        gameObject.GetComponent<DeathByFall>().CheckQueue(); // убрать отсюда и вызывать из перехода хода
    }
}
