using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AIMove : MonoBehaviour
{
    private const int ROWS = 6;
    private const int COLS = 7;
    private const int WIN_LENGTH = 4;
    private int currentPlayer = 1; // player1 starts

    // ...

    private bool IsValidLocation(int[,] board, int column)
    {
        return board[ROWS - 1, column] == 0;
    }

    private int[] ValidLocations(int[,] board)
    {

        //int count = 0;
        List<int> valid = new List<int>();
        for(int col = 0; col < COLS; col++)
        {
            if (IsValidLocation(board, col)) valid.Add(col);
        }

        int[] valid_locations = valid.ToArray();
        return valid_locations;

        /*for (int col = 0; col < COLS; col++)
        {
            if (IsValidLocation(board, col))
            {
                valid_locations[count] = col;
                //Debug.Log(valid_locations[count]);
                count++;
            }
        }
        return valid_locations;*/
 
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

        if (count_player == 4) score += 100;
        else if (count_player == 3 && count_empty == 1) score += 5;
        else if (count_player == 2 && count_empty == 2) score += 2;
        if (count_opp == 3 && count_empty == 1) score -= 4;

        return score;

        /*if (player2Count == WIN_LENGTH)
        {
            return int.MaxValue;
        }
        else if (player1Count == WIN_LENGTH - 1)
        {
            return int.MinValue;
        }
        else
        {
            int score =  player2Count - player1Count;
            if (player1Count > 0 && player2Count > 0)
            {
                score = 0; // La fenêtre n'a aucune valeur si elle contient des jetons des deux joueurs
            }
            if (player1Count > 0)
            {
                score *= -1; // Inverse le score si la fenêtre contient des jetons de l'adversaire
            }
            return score;
        }*/
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
        
        //if(score == int.MinValue) Debug.Log(score);

        /*if (depth == 0 || score == int.MaxValue || score == int.MinValue)
        {
            return score;
        }*/

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

        /*int bestScore = maximizingPlayer ? int.MinValue : int.MaxValue;

        for (int col = 0; col < COLS; col++)
        {
            if (!IsColumnFull(col, board))
            {
                int row = GetNextEmptyRow(col, board);
                board[row, col] = maximizingPlayer ? 2 : 1;

                int[] childScore = Minimax(depth - 1, alpha, beta, !maximizingPlayer, board);

                if (maximizingPlayer)
                {
                    bestScore = Mathf.Max(bestScore, childScore[1]);
                    alpha = Mathf.Max(alpha, childScore[1]);
                }
                else
                {
                    bestScore = Mathf.Min(bestScore, childScore[1]);
                    beta = Mathf.Min(beta, childScore[1]);
                }

                board[row, col] = 0;

                if (beta <= alpha)
                {
                    break;
                }
            }
        }

        return bestScore;*/
    }

    public int GetBestMove(int[,] board)
    {

        int[] value_minimax = Minimax(6, int.MinValue, int.MaxValue, true, board);

        return value_minimax[0];

        /*int bestScore = int.MinValue;
        List<int> bestMoves = new List<int>();


        for (int col = 0; col < COLS; col++)
        {
            if (!IsColumnFull(col, board))
            {
                
                int row = GetNextEmptyRow(col, board);
                board[row, col] = 2;

                int score = Minimax(7, int.MinValue, int.MaxValue, true, board); // depth of 4

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMoves.Clear();
                    bestMoves.Add(col);
                }
                else if (score == bestScore)
                {
                    bestMoves.Add(col);
                }

                board[row, col] = 0;
            }
        }

        return bestMoves[Random.Range(0, bestMoves.Count)];*/
    }
}
