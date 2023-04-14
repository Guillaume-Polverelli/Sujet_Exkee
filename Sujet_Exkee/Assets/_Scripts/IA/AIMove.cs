using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AIMove : MonoBehaviour
{
    private const int ROWS = 6;
    private const int COLS = 7;
    private const int WIN_LENGTH = 4;

    private bool IsValidLocation(int[,] board, int column)
    {
        return board[ROWS - 1, column] == 0;
    }

    private int[] ValidLocations(int[,] board)
    {
        List<int> valid = new List<int>();
        for(int col = 0; col < COLS; col++)
        {
            if (IsValidLocation(board, col)) valid.Add(col);
        }

        int[] valid_locations = valid.ToArray();
        return valid_locations;
 
    }

    private int GetNextEmptyRow(int column, int[,] board)
    {
        for (int i = 0; i <= board.GetLength(0) - 1; i++)
        {
            if (board[i, column] == 0)
            {
                return i;
            }
        }
        return -1; // Column is full
    }

    private bool IsColumnFull(int column, int[,] board)
    {
        return board[ROWS - 1, column] != 0;
    }

    private bool WinningMove(int[,] board, int player)
    {
        // Check rows
        for (int row = 0; row < ROWS; row++)
        {
            for (int col = 0; col <= COLS - WIN_LENGTH; col++)
            {
                if (board[row, col] == player && board[row, col + 1] == player && board[row, col + 2] == player && board[row, col + 3] == player)
                {
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
                    return true;
                }
            }
        }
        return false;
    }

    private int EvaluateWindow(int startRow, int startCol, int deltaRow, int deltaCol, int[,] board, int player)
    {
        int score = 0;
        int opp_player = 1;
        int count_player = 0;
        int count_empty = 0;
        int count_opp = 0;

        if(player == 1)
        {
            opp_player = 2;
        }

        for (int i = 0; i < WIN_LENGTH; i++)
        {
            int row = startRow + i * deltaRow;
            int col = startCol + i * deltaCol;

            if (board[row, col] == player)
            {
                count_player++;
            }
            else if (board[row, col] == opp_player)
            {
                count_opp++;
            }
            else
            {
                count_empty++;
            }

        }
        if (player == 2)
        {
            if (count_player == 4) score += 10000;
            else if (count_player == 3 && count_empty == 1) score += 100;
            else if (count_player == 2 && count_empty == 2) score += 10;
            if (count_opp == 2 && count_empty == 2) score -= 10;
            else if (count_opp == 3 && count_empty == 1) score -= 100;
            else if (count_opp == 4) score -= 10000;
        }
        else
        {
            if (count_player == 4) score -= 10000;
            else if (count_player == 3 && count_empty == 1) score -= 100;
            else if (count_player == 2 && count_empty == 2) score -= 10;
            if (count_opp == 2 && count_empty == 2) score += 10;
            else if (count_opp == 3 && count_empty == 1) score += 100;
            else if (count_opp == 4) score += 10000;
        }

        return score;
    }

    private int EvaluateBoard(int[,] board, int player)
    {
        int score = 0;

        // Check center column
        int[] centerArray = Enumerable.Range(0, ROWS).Select(i => board[i, COLS / 2]).ToArray();

        int centerCount = centerArray.Count(p => p == player);

        score += centerCount * 3;

        // Check rows
        for (int row = 0; row < ROWS; row++)
        {
            for (int col = 0; col <= COLS - WIN_LENGTH; col++)
            {
                score += EvaluateWindow(row, col, 0, 1, board, player);
            }
        }

        // Check columns
        for (int row = 0; row <= ROWS - WIN_LENGTH; row++)
        {
            for (int col = 0; col < COLS; col++)
            {
                score += EvaluateWindow(row, col, 1, 0, board, player);
            }
        }

        // Check diagonals (positive slope)
        for (int row = 0; row <= ROWS - WIN_LENGTH; row++)
        {
            for (int col = 0; col <= COLS - WIN_LENGTH; col++)
            {
                score += EvaluateWindow(row, col, 1, 1, board, player);             
            }
        }

        // Check diagonals (negative slope)
        for (int row = 0; row <= ROWS - WIN_LENGTH; row++)
        {
            for (int col = WIN_LENGTH - 1; col < COLS; col++)
            {
                score += EvaluateWindow(row, col, 1, -1, board, player);               
            }
        }

        return score;
    }

    private bool IsTerminalNode(int[,] board)
    {
        return WinningMove(board, 1) || WinningMove(board, 2) || ValidLocations(board).Count() == 0;
    }

    private int[] Minimax(int depth, int alpha, int beta, bool maximizingPlayer, int[,] board)
    {


        bool isTerminal = IsTerminalNode(board);

        if(depth == 0 || isTerminal)
        {
            
            if (isTerminal)
            {
                if (WinningMove(board, 2))
                {
                    int[] val = { -1, int.MaxValue };
                    return val;
                }
                else if (WinningMove(board, 1))
                {
                    int[] val = { -1, int.MinValue };
                    return val;
                }
                else
                {
                    int[] val = { -1, 0 };
                    return val;
                }
            }
            else
            {
                int[] val = { -1,EvaluateBoard(board, 2) };
                return val;
            }
        }

        if (maximizingPlayer)
        {
            int score = int.MinValue;
            int[] valid_locations = ValidLocations(board);
            int column = valid_locations[Random.Range(0, valid_locations.Count())];
            for (int col = 0; col < valid_locations.Count(); col++)
            {
                int row = GetNextEmptyRow(valid_locations[col], board);
                int[,] b_copy = board.Clone() as int[,];
                b_copy[row, col] = 2;
                int new_score = Minimax(depth - 1, alpha, beta, false, b_copy)[1];
                if(new_score > score)
                {
                    score = new_score;
                    column = valid_locations[col];
                }
                alpha = Mathf.Max(alpha, score);
                if (alpha >= beta) break;
            }
            int[] val = { column, score };
            return val;
        }
        else
        {
            int score = int.MaxValue;
            int[] valid_locations = ValidLocations(board);
            int column = valid_locations[Random.Range(0, valid_locations.Count())];
            for (int col = 0; col < valid_locations.Count(); col++)
            {
                int row = GetNextEmptyRow(valid_locations[col], board);
                int[,] b_copy = board.Clone() as int[,];
                b_copy[row, col] = 1;
                int new_score = Minimax(depth - 1, alpha, beta, true, b_copy)[1];
                if (new_score < score)
                {
                    score = new_score;
                    column = valid_locations[col];
                }
                beta = Mathf.Min(beta, score);
                if (alpha >= beta) break;
            }
            int[] val = { column, score };
            return val;
        }
    }

    public int GetBestMove(int[,] board)
    {

        int[] value_minimax = Minimax(6, int.MinValue, int.MaxValue, true, board);

        return value_minimax[0];
    }
}
