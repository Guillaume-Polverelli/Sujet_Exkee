using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    //3 states for each cell : 0 is empty, is player 1, 2 is player 2
    private int[,] _boardState;

  //[5,0]       [5,6]
    //0 0 0 0 0 0 0 
    //0 0 0 0 0 0 0 
    //0 0 0 0 0 0 0 
    //0 0 0 0 0 0 0 
    //0 0 0 0 0 0 0 
    //0 0 0 0 0 0 0 
  //[0,0]       [0,6]


    [SerializeField] private int _heightOfBoard;
    [SerializeField] private int _widthOfBoard;

    // Start is called before the first frame update
    void Start()
    {
        _boardState = new int[_heightOfBoard, _widthOfBoard];
    }

    public int[,] GetBoardState()
    {
        return _boardState;
    }
    public int GetWidth()
    {
        return _widthOfBoard;
    }
    public int GetHeight()
    {
        return _heightOfBoard;
    }

    public bool IsColumnNotFull(int column) {
        return _boardState[_heightOfBoard - 1, column] == 0;
    }

    public bool UpdateBoardState(int column, bool isPlayerTurn)
    {
        if (column == -1)
        {
            Debug.Log("La partie est finie");
            return false;
        }
        for(int row = 0; row < _heightOfBoard; row++)
        {
            if(_boardState[row, column] == 0)
            {
                if (isPlayerTurn)
                {
                    _boardState[row, column] = 1;
                }
                else
                {
                    _boardState[row, column] = 2;
                }
                Debug.Log("Piece being spawned at (" + row + " , " + column + ")");
                return true;
            }   
        }
        Debug.LogWarning("Column : " + column + " is full");
        return false;
    }
}
