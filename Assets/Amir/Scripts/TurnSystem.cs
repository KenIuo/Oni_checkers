using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public List<GameObject> _playersQueue = new(4);
    public GameObject _playerCheckerObject;

    List<bool> _is_ready_list = new(4);
    byte _currentPlayer = 0;



    /// <summary>
    ///  ������ ���������� ��� ������ ���� (�.�. ����� ������� �� ������ ������ � ��. ����)
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

        NewTurn();
    }

    public void CheckConditions()
    {

        byte _is_all_ready = 0;

        foreach (GameObject player in _playersQueue)
        {
            if (!player.activeSelf || player.GetComponent<CheckerState>()._isStopped)
                _is_all_ready++;
        }

        if (_is_all_ready == _playersQueue.Count) // ���� ������� ��������, �� �������� ����� ���
        {
            gameObject.GetComponent<DeathByFall>().CheckQueue(); // ���������� ����� ��� �������� ?
            NewTurn();
        }

        if (_playersQueue.Count == 1)
        {
            if (_playersQueue[0] == _playerCheckerObject)
                GameObject.Find("Win").SetActive(true);
            else
                GameObject.Find("Lose").SetActive(true);
        }
    }



    void NewTurn()
    {
        // ������ ��������� (�������� ������� + ��������� ������ ������ ������ + ��������� ������ ������� + ������� ������ �������� �������)
        // 
        // ��������� �������� ���� �� ������
        // ���� ������� - �����
        // �������������� ����������
        // �����
        // �������������� ��������� �� ��� �������� ������� ������ ������� � ��������
    }



    private void Start()
    {
        //
    }
}
