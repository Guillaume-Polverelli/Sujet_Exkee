using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove2 : MonoBehaviour
{
    private const int ROWS = 6;
    private const int COLS = 7;
    private const int WIN_LENGTH = 4;

    private const int AI_PIECE = 2;
    private const int PLAYER_PIECE = 1;

   

    private void EvaluateBoard(int[,] board, int player)
    {
        for(int i = 0; i < ROWS; i++)
        {
            int[] row_array = new int[COLS];
            for (int j = 0; j < COLS; j++)
            {
                row_array[j] = board[i, j];
            }
            for(int c = 0; c < COLS - 3; c++)
            {

            }
        }
    }
}
