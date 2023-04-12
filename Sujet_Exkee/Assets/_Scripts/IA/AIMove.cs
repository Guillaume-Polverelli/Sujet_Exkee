using UnityEngine;
using System.Collections.Generic;

public class AIMove : MonoBehaviour
{
    private const int ROWS = 6;
    private const int COLS = 7;
    private const int WIN_LENGTH = 4;
    private int currentPlayer = 1; // player1 starts

    // ...

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

    private int EvaluateWindow(int startRow, int startCol, int deltaRow, int deltaCol, int[,] board)
    {
        int player1Count = 0;
        int player2Count = 0;

        for (int i = 0; i < WIN_LENGTH; i++)
        {
            int row = startRow + i * deltaRow;
            int col = startCol + i * deltaCol;

            if (board[row, col] == 1)
            {
                player1Count++;
            }
            else if (board[row, col] == 2)
            {
                player2Count++;
            }
        }

        if (player2Count == WIN_LENGTH)
        {
            return int.MaxValue;
        }
        else if (player1Count == WIN_LENGTH)
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
        }
    }

    private int EvaluateBoard(int[,] board)
    {
        int score = 0;

        // Check rows
        for (int row = 0; row < ROWS; row++)
        {
            for (int col = 0; col <= COLS - WIN_LENGTH; col++)
            {
                int windowScore = EvaluateWindow(row, col, 0, 1, board);
                if (windowScore == int.MaxValue)
                {
                    return windowScore;
                }
                else if (windowScore == int.MinValue)
                {
                    return windowScore;
                }
                else
                {
                    score += windowScore;
                }
            }
        }

        // Check columns
        for (int row = 0; row <= ROWS - WIN_LENGTH; row++)
        {
            for (int col = 0; col < COLS; col++)
            {
                int windowScore = EvaluateWindow(row, col, 1, 0, board);
                if (windowScore == int.MaxValue)
                {
                    return windowScore;
                }
                else if (windowScore == int.MinValue)
                {
                    return windowScore;
                }
                else
                {
                    score += windowScore;
                }
            }
        }

        // Check diagonals (positive slope)
        for (int row = 0; row <= ROWS - WIN_LENGTH; row++)
        {
            for (int col = 0; col <= COLS - WIN_LENGTH; col++)
            {
                int windowScore = EvaluateWindow(row, col, 1, 1, board);
                if (windowScore == int.MaxValue)
                {
                    return windowScore;
                }
                else if (windowScore == int.MinValue)
                {
                    return windowScore;
                }
                else
                {
                    score += windowScore;
                }
            }
        }

        // Check diagonals (negative slope)
        for (int row = 0; row <= ROWS - WIN_LENGTH; row++)
        {
            for (int col = WIN_LENGTH - 1; col < COLS; col++)
            {
                int windowScore = EvaluateWindow(row, col, 1, -1, board);
                if (windowScore == int.MaxValue)
                {
                    return windowScore;
                }
                else if (windowScore == int.MinValue)
                {
                    return windowScore;
                }
                else
                {
                    score += windowScore;
                }
            }
        }

        return score;
    }

    private int Minimax(int depth, int alpha, int beta, bool maximizingPlayer, int[,] board)
    {
        int score = EvaluateBoard(board);
        //if(score == int.MinValue) Debug.Log(score);

        if (depth == 0 || score == int.MaxValue || score == int.MinValue)
        {
            return score;
        }

        int bestScore = maximizingPlayer ? int.MinValue : int.MaxValue;

        for (int col = 0; col < COLS; col++)
        {
            if (!IsColumnFull(col, board))
            {
                int row = GetNextEmptyRow(col, board);
                board[row, col] = maximizingPlayer ? 2 : 1;

                int childScore = Minimax(depth - 1, alpha, beta, !maximizingPlayer, board);

                if (maximizingPlayer)
                {
                    bestScore = Mathf.Max(bestScore, childScore);
                    alpha = Mathf.Max(alpha, childScore);
                }
                else
                {
                    bestScore = Mathf.Min(bestScore, childScore);
                    beta = Mathf.Min(beta, childScore);
                }

                board[row, col] = 0;

                if (beta <= alpha)
                {
                    break;
                }
            }
        }

        return bestScore;
    }

    public int GetBestMove(int[,] board)
    {
        int bestScore = int.MinValue;
        List<int> bestMoves = new List<int>();


        for (int col = 0; col < COLS; col++)
        {
            if (!IsColumnFull(col, board))
            {
                
                int row = GetNextEmptyRow(col, board);
                board[row, col] = 2;

                int score = Minimax(4, int.MinValue, int.MaxValue, true, board); // depth of 4

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

        return bestMoves[Random.Range(0, bestMoves.Count)];
    }
}
