using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInput : MonoBehaviour {

    Manager manager = new Manager();
    char thisPlayer;
    char myEnemy;

	// Use this for initialization
	void Start () {
		
	}

    void Init(char myOption, char otherOption)
    {
        thisPlayer = myOption;
        myEnemy = otherOption;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    int FindTheBestMove(char[,] board, char currentPlayer)
    {
        if (DoWeHaveAWiner(board, currentPlayer))
        {
            if (currentPlayer == thisPlayer)
                return 10;
            else
                return -10;
        }
        else
        {
            return 0;
        }

        List<Casa> resultados = new List<Casa>();
        Casa c = new Casa();
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                c.x = x;
                c.y = y;

                if (board[x, y] == '0')
                {
                    board[x, y] = currentPlayer;
                    if (currentPlayer == thisPlayer)
                        c.val = FindTheBestMove(board, myEnemy);
                    else
                        c.val = FindTheBestMove(board, thisPlayer);
                    board[x, y] = '0';
                    resultados.Add(c);

                }
            }
        }

        int MelhorValor = 0;
        if (currentPlayer == thisPlayer)
        {
            int bVal = -100000;
            for (int x; x < resultados.Contains; x++)
                if (resultados[x].val > bVal)
                {
                    MelhorValor = x;
                    bVal = resultados[x].val;
                }
        }
        else
        {
            int bVal = 100000;
            for (int x; x < resultados.Contains; x++)
                if (resultados[x].val < bVal)
                {
                    MelhorValor = x;
                    bVal = resultados[x].val;
                }
        }
        return resultados[MelhorValor];
    }

    //Verificacao se ha um vencedor
    public bool DoWeHaveAWiner(char[,] board, char player)
    {
        if ((board[0, 0] == player && board[1, 1] == player && board[2, 2] == player) ||
            (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player))
        {
            Debug.Log("Diagonal: " + player + " : " + board[2, 0]);
            return true;

        }
        else
        {

            for (int i = 0; i < 3; i++)
                if ((board[i, 2] == player && board[i, 1] == player && board[i, 0] == player) ||
                   (board[2, i] == player && board[1, i] == player && board[0, i] == player))
                {
                    Debug.Log("Linha: " + i + " - " + player + " - " + board[0, i]);
                    return true;
                }
        }
        return false;
    }

}