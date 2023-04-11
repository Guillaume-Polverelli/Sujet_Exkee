using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField] private GameObject _player1;
    [SerializeField] private GameObject _player2;

    [SerializeField] private GameObject _player1Ghost;
    [SerializeField] private GameObject _player2Ghost;

    [SerializeField] private GameObject[] _spawnLocations;
    [SerializeField] private GameGrid _board;

    private bool _isPlayerTurn = true;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        Debug.Assert(_player1 != null);
        Debug.Assert(_player2 != null);
        Debug.Assert(_player1Ghost != null);
        Debug.Assert(_player2Ghost != null);
        Debug.Assert(_board != null);
    }

    private void Start()
    {
        _player1Ghost.SetActive(false);
        _player2Ghost.SetActive(false);
    }

    public void HoverColumn(int column)
    {
        if (_isPlayerTurn)
        {
            _player1Ghost.SetActive(true);
            _player1Ghost.transform.position = _spawnLocations[column].transform.position;
        }
        else
        {
            _player2Ghost.SetActive(true);
            _player2Ghost.transform.position = _spawnLocations[column].transform.position;

        }
    }


    public void SelectColumn(int column)
    {
        Debug.Log("GameManager column : " + column);
        PlayerTurn(column);
    }

    private void PlayerTurn(int column)
    {

        if(_board.UpdateBoardState(column, _isPlayerTurn))
        {
            if (_isPlayerTurn)
            {
                Instantiate(_player1, _spawnLocations[column].transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
                _isPlayerTurn = !_isPlayerTurn;
                _player1Ghost.SetActive(false);
            }
            else
            {
                Instantiate(_player2, _spawnLocations[column].transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
                _isPlayerTurn = !_isPlayerTurn;
                _player2Ghost.SetActive(false);

            }
        }
    }
}
