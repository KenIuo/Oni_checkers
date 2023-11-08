using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSystem : MonoBehaviour
{
    public List<GameObject> _playersQueue = new List<GameObject>();
    ushort _currentPlayer = 0;



    /// <summary>
    ///  должен вызываться при старте игры
    /// </summary>
    public void ShuffleListOfPlayers()
    {
        _playersQueue.Shuffle();

        foreach (GameObject p in _playersQueue)
        {
            //p.AddComponent<DefaultState>();
            //p.AddComponent<>();
        }
    }



    private void Start()
    {
        
    }
}
