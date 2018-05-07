using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {


    AIInput ai = new AIInput();

    public char[,] TicTacToeGrid;
	public short SIZE = 3;
	bool isPlayerOne = true;
	public Sprite[] spt;
	public GameObject block;
	public GameObject[,] Squares;

    bool aWinner = false;
	Image myImageComponent;
	public bool isHuman = true;
	int gameStep;
    bool isMultiplayer;

	// Use this for initialization
	void Start () {
        TicTacToeGrid = new char[SIZE, SIZE];
        BuildBoard ();
		CamPos ();
		char[,] dummy = new char[SIZE, SIZE];
        gameStep = (SIZE*SIZE)-1;
		dummy = TicTacToeGrid;
        isMultiplayer = true;

        PlaceXorO (dummy);

	}

    //Construcao da matriz visual do jogo
	void BuildBoard()
	{
		TicTacToeGrid = new char[SIZE,SIZE];
		Squares = new GameObject[SIZE, SIZE];
		int i = 1;
		//foreach (bool a in TicTacToeGrid)
		for (short y = 0; y < SIZE; y++)
			for (short x = 0; x < SIZE; x++) {
                //yield return new WaitForSeconds(0);
				Squares[x,y] = Instantiate (block, new Vector2 (x, y), Quaternion.identity) as GameObject;
				Squares [x, y].name = "grid" + i;
				Debug.Log("Build: "+x+" - "+y);
				TicTacToeGrid [x, y] = '0';
				i++;
			}
	}

    //Inicializacao do board
	void PlaceXorO(char[,] myDummy)
	{
		int count = 0;
		for (int x = 0; x < SIZE; x++)
			for (int y = 0; y < SIZE; y++)
				if (myDummy [x, y] == '0')
					count++;
	}
	
	// Update is called once per frame
	void Update () {

        if (!aWinner)
            if (isMultiplayer)
            {
                if (isHuman)
                {
                    if (GetHumanInput())
                    {
                        isHuman = false;
                        gameStep--;
                        if (DoWeHaveAWiner(TicTacToeGrid, isPlayerOne ? '2' : '1'))
                        {
                            aWinner = true;
                            if (isPlayerOne) Debug.Log("Vitoria do jogador 2"); else Debug.Log("Vitoria do jogador 1");
                        }
                        else
                        {
                            if (gameStep < 0)
                                Debug.Log("EMPATE");
                        }
                    }
                }
                else
                {
                    
                    Vector2 play = ai.GetAIInput(TicTacToeGrid, isPlayerOne ? '1' : '2');
                    Debug.Log(play);
                    PlaceXOnBoard(isPlayerOne ? '1' : '2', play);
                    if (DoWeHaveAWiner(TicTacToeGrid, isPlayerOne ? '1' : '2'))
                    {
                        aWinner = true;
                        if (!isPlayerOne) Debug.Log("Vitoria do jogador 2"); else Debug.Log("Vitoria do jogador 1");
                    }
                    else
                    {
                        if (gameStep < 0)
                            Debug.Log("EMPATE");
                    }
                    isPlayerOne = !isPlayerOne;
                    isHuman = true;
                    gameStep--;
                }
            }
    }

    bool GetHumanInput()
    {
        if (Input.GetMouseButtonUp(0))
        {
            bool shouldCheckWinner = false;
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            foreach (GameObject go in Squares)
                if (go.GetComponent<BoxCollider2D>().OverlapPoint(wp))
                {
                    shouldCheckWinner = BoxClick(go);
                    PrintBoard(TicTacToeGrid);
                }
            return shouldCheckWinner;
        }
        return false;
    }

    //Debug do board, para ter certeza que esta tudo bem.
    void PrintBoard(char[,] board)
    {
        string brd = "";
        for (int i = 0; i < SIZE; i++)
        {
            
            for (int j = 0; j < SIZE; j++)
            {
                brd += "|" + board[j, i];
            }
            brd += "\n";
        }
        Debug.Log(brd);

    }

    int CalculateScore(char[,] board, bool currentPlayer){

		if(currentPlayer)
		{
			if(DoWeHaveAWiner(board, '2'))
				return 1;
		}else
		{
			if(DoWeHaveAWiner(board, '1'))
				return -1;
		}
		return 0;

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


    //Tratamento do clique no grid
    public bool BoxClick(GameObject goBox)
    {
        bool madePlay = false;
        //Pega o ID do box
        string piece = goBox.name.Substring(4, 1);
        int i = int.Parse(piece);
        Debug.Log( ((int)(i - 1) % SIZE)+" , " + ((int)(i - 1) / SIZE));

        //verifica se a casa está vazia
        if (TicTacToeGrid[(int)(i - 1) % SIZE, (int)(i - 1) / SIZE] == '0') {
            Sprite img = goBox.transform.GetComponent<Sprite>();
            if (isPlayerOne) {
                PlaceXOnBoard('1', i);
                madePlay = true;
            } else {
                PlaceXOnBoard('2', i);
                madePlay = true;
            }
        }
        return madePlay;

    }

    //Place the item
    void PlaceXOnBoard(char playerType, int square)
    {
        Debug.Log(playerType + " - " + square);
        TicTacToeGrid[(int)(square - 1) % SIZE, (int)(square - 1) / SIZE] = playerType;
        Squares[(int)(square - 1) % SIZE, (int)(square - 1) / SIZE].transform.GetComponent<SpriteRenderer>().sprite = spt[(int.Parse(playerType.ToString()) - 1)];
        isPlayerOne = !isPlayerOne;
    }

    //Place the item
    void PlaceXOnBoard(char playerType, Vector2 v2)
    {
        Debug.Log("AI: "+playerType + " - " + v2);
        TicTacToeGrid[(int)v2.x, (int)v2.y] = playerType;
        Squares[(int)v2.x, (int)v2.y].transform.GetComponent<SpriteRenderer>().sprite = spt[(int.Parse(playerType.ToString()) - 1)];
        PrintBoard(TicTacToeGrid);
    }


    //Ajuste da camera para multiplos tamanhos de tabuleiro
    void CamPos()
    {
        float z = (float)(SIZE - 1) / 2;
        transform.position = new Vector3(z, z, -10);
        GetComponent<Camera>().orthographicSize = (float)SIZE / 2;
    }

}
