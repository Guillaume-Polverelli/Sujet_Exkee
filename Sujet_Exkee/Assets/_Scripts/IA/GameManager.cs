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
    [SerializeField] private AIMove _aiMove;

    private const int ROWS = 6;
    private const int COLS = 7;
    private const int WIN_LENGTH = 4;


    private GameObject fallingPiece;
    private GameObject[,] _piecePositions;

    private bool _isPlayerTurn = false;
    private bool _hasGameEnded = false;

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

        _piecePositions = new GameObject[ROWS, COLS];

        StartCoroutine(PlayIACoroutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuHandler.Instance.HandlePauseInput();
            if (MenuHandler.Instance.getPause())
            {
                _player1Ghost.SetActive(false);
                _player2Ghost.SetActive(false);
            }
        }
    }

    public void HoverColumn(int column)
    {
        if(_board.IsColumnNotFull(column) && (fallingPiece == null || fallingPiece.GetComponent<Rigidbody>().velocity == Vector3.zero) && !_hasGameEnded && !MenuHandler.Instance.getPause())
        {
            if (_isPlayerTurn)
            {
                _player1Ghost.SetActive(true);
                _player1Ghost.transform.position = _spawnLocations[column].transform.position;
            }
        }
    }


    public void SelectColumn(int column)
    {
        if(_isPlayerTurn && !MenuHandler.Instance.getPause() && (fallingPiece == null || fallingPiece.GetComponent<Rigidbody>().velocity == Vector3.zero))
        {
            PlayerTurn(column);
        }
    }

    private void PlayerTurn(int column)
    {
        if (!_hasGameEnded && _board.UpdateBoardState(column, _isPlayerTurn))
        {           
              fallingPiece = Instantiate(_player1, _spawnLocations[column].transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
              _piecePositions[_board.GetLastRowNotEmpty(column), column] = fallingPiece;
              fallingPiece.GetComponent<Rigidbody>().velocity = new Vector3(0, 0.1f, 0);
              _isPlayerTurn = !_isPlayerTurn;
              _player1Ghost.SetActive(false);

              if (DidWin(_board.GetBoardState(), 1))
              {
                  _hasGameEnded = true;
                  Debug.Log("Player 1 won !");
              }
              else if (DidDraw(_board.GetBoardState()))
              {
                  Debug.Log("Draw !");
              }

              StartCoroutine(PlayIACoroutine()); 
        }
    }
    
    IEnumerator PlayIACoroutine()
    {
        if (!_hasGameEnded)
        {
            int column = _aiMove.GetBestMove(_board.GetBoardState());
            if (_board.UpdateBoardState(column, _isPlayerTurn))
            {

                yield return new WaitForSeconds(2);
                _player2Ghost.SetActive(true);
                _player2Ghost.transform.position = _spawnLocations[column].transform.position;

                yield return new WaitForSeconds(0.7f);

                fallingPiece = Instantiate(_player2, _spawnLocations[column].transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
                _piecePositions[_board.GetLastRowNotEmpty(column), column] = fallingPiece;
                fallingPiece.GetComponent<Rigidbody>().velocity = new Vector3(0, 0.1f, 0);
                _isPlayerTurn = !_isPlayerTurn;
                _player2Ghost.SetActive(false);

                if (DidWin(_board.GetBoardState(), 2))
                {
                    _hasGameEnded = true;
                    Debug.Log("Player 2 won !");
                }
                else if (DidDraw(_board.GetBoardState()))
                {
                    Debug.Log("Draw !");
                }
            }
        }
    }

    private bool DidWin(int[,] board, int player)
    {
        // Check rows
        for (int row = 0; row < ROWS; row++)
        {
            for (int col = 0; col <= COLS - WIN_LENGTH; col++)
            {
                if (board[row, col] == player && board[row, col + 1] == player && board[row, col + 2] == player && board[row, col + 3] == player)
                {
                    _piecePositions[row, col].GetComponent<Outline>().enabled = true;
                    _piecePositions[row, col+1].GetComponent<Outline>().enabled = true;
                    _piecePositions[row, col+2].GetComponent<Outline>().enabled = true;
                    _piecePositions[row, col+3].GetComponent<Outline>().enabled = true;
                    return true;
                }
            }
        }

        // Check columns
        for (int row = 0; row <= ROWS - WIN_LENGTH; row++)
        {
            for (int col = 0; col < COLS; col++)
            {
                if (board[row, col] == player && board[row + 1, col] == player && board[row + 2, col] == player && board[row + 3, col] == player)
                {
                    _piecePositions[row, col].GetComponent<Outline>().enabled = true;
                    _piecePositions[row+1, col].GetComponent<Outline>().enabled = true;
                    _piecePositions[row+2, col].GetComponent<Outline>().enabled = true;
                    _piecePositions[row+3, col].GetComponent<Outline>().enabled = true;
                    return true;
                }
            }
        }

        // Check diagonals (positive slope)
        for (int row = 0; row <= ROWS - WIN_LENGTH; row++)
        {
            for (int col = 0; col <= COLS - WIN_LENGTH; col++)
            {
                if (board[row, col] == player && board[row + 1, col + 1] == player && board[row + 2, col + 2] == player && board[row + 3, col + 3] == player)
                {
                    _piecePositions[row, col].GetComponent<Outline>().enabled = true;
                    _piecePositions[row+1, col+1].GetComponent<Outline>().enabled = true;
                    _piecePositions[row+2, col+2].GetComponent<Outline>().enabled = true;
                    _piecePositions[row+3, col+3].GetComponent<Outline>().enabled = true;
                    return true;
                }
            }
        }

        // Check diagonals (negative slope)
        for (int row = 0; row <= ROWS - WIN_LENGTH; row++)
        {
            for (int col = WIN_LENGTH - 1; col < COLS; col++)
            {
                if (board[row, col] == player && board[row + 1, col - 1] == player && board[row + 2, col - 2] == player && board[row + 3, col - 3] == player)
                {
                    _piecePositions[row, col].GetComponent<Outline>().enabled = true;
                    _piecePositions[row+1, col-1].GetComponent<Outline>().enabled = true;
                    _piecePositions[row+2, col-2].GetComponent<Outline>().enabled = true;
                    _piecePositions[row+3, col-3].GetComponent<Outline>().enabled = true;
                    return true;
                }
            }
        }
        return false;
    }

    private bool DidDraw(int[,] board)
    {
        for(int col = 0; col < COLS; col++)
        {
            if (board[ROWS - 1, col] == 0) return false;
        }
        return true;
    }
}
