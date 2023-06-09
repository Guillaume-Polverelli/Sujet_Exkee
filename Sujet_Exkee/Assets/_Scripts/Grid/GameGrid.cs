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
    void Awake()
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

    public int GetLastRowNotEmpty(int column)
    {
        if (_boardState[0, column] == 0) return 0;
        for (int row = 1; row < _heightOfBoard; row++)
        {
            if (_boardState[row, column] == 0) return row - 1;
        }
        return _heightOfBoard - 1;
    }

    public bool UpdateBoardState(int column, bool isPlayerTurn)
    {
        if (column == -1)
        {
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
                return true;
            }   
        }
        return false;
    }
}
