using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

	public char[,] TicTacToeGrid;
	public short SIZE = 3;
	bool isPlayerOne = true;
	public Sprite[] spt;
	public GameObject block;
	public GameObject[,] Squares;

    bool aWinner = false;
	Image myImageComponent;
	bool isHuman;
	int gameStep;

	// Use this for initialization
	void Start () {
        TicTacToeGrid = new char[SIZE, SIZE];
        BuildBoard ();
		CamPos ();
		char[,] dummy = new char[SIZE, SIZE];
        gameStep = (SIZE*SIZE)-1;
		dummy = TicTacToeGrid;
		isHuman = false;
		PlaceXorO (dummy);

	}

	void CamPos()
	{
		float z = (float)(SIZE - 1) / 2;
		transform.position = new Vector3 (z, z, -10);
		GetComponent<Camera>().orthographicSize = (float)SIZE/2;
	}

	void BuildBoard()
	{
		TicTacToeGrid = new char[SIZE,SIZE];
		Squares = new GameObject[SIZE, SIZE];
		int i = 1;
		//foreach (bool a in TicTacToeGrid)
		for (short y = 0; y < SIZE; y++)
			for (short x = 0; x < SIZE; x++) {
				Squares[x,y] = Instantiate (block, new Vector2 (x, y), Quaternion.identity) as GameObject;
				Squares [x, y].name = "grid" + i;
				Debug.Log("Build: "+x+" - "+y);
				TicTacToeGrid [x, y] = '0';
				i++;
			}
	}

	void PlaceXorO(char[,] myDummy)
	{
		int count = 0;
		for (int x = 0; x < 3; x++)
			for (int y = 0; y < 3; y++)
				if (myDummy [x, y] == '0')
					count++;
	}
	
	// Update is called once per frame
	void Update () {

        if(!aWinner)
            if (isHuman) { 
		        if (Input.GetMouseButtonUp (0)) {
			        bool shouldCheckWinner = false;
			        Vector3 wp = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			        foreach (GameObject go in Squares)
				        if (go.GetComponent<BoxCollider2D> ().OverlapPoint (wp)) {
					        shouldCheckWinner = BoxClick (go);
                            PrintBoard(TicTacToeGrid);
                        }
                        if (shouldCheckWinner)
                            if (DoWeHaveAWiner(isPlayerOne ? '2' : '1'))
                                aWinner = true;//Debug.Log ("Winner");
                            else
                                Debug.Log("Not yet");
                }
               
            }
            else {
				char[,] dummy = new char[SIZE, SIZE];
				dummy = TicTacToeGrid;
                LookForOptions(dummy, gameStep, 99999, isPlayerOne);

            }
    }

    int LookForOptions(char[,] myDummy, int step, bool isPlayerOne)
    {
/*01 function minimax(node, depth, maximizingPlayer)
02     if depth = 0 or node is a terminal node
03         return the heuristic value of node

04     if maximizingPlayer
05         bestValue := −∞
06         for each child of node
07             v := minimax(child, depth − 1, FALSE)
08             bestValue := max(bestValue, v)
09         return bestValue

10     else    (* minimizing player *)
11         bestValue := +∞
12         for each child of node
13             v := minimax(child, depth − 1, TRUE)
14             bestValue := min(bestValue, v)
15         return bestValue
		*/


		if(isPlayerOne)
		{
			
		}
		else
		{
		}

		/********************************************************
		if(DoWeHaveAWiner(isPlayerOne ? '2' : '1') || step == 0)
		{
			return CalculateScore(myDummy, isPlayerOne);
		}elif(isPlayerOne)
		{	
			int a = 99999;
			for(int i = 0; i < (SIZE*SIZE); i++)
			{
				if(myDummy[((int)(i-1) / SIZE)+" , "+((int)(i-1) % SIZE)] == '0')
				{
					myDummy[((int)(i-1) / SIZE)+" , "+((int)(i-1) % SIZE)] == (isPlayerOne ? '2' : '1')
					a = LookForOptions(myDummy), step-1, true);
				}
			}
		}else
		{
			int a = -99999;
			for(int i = 0; i < (SIZE*SIZE); i++)
			{
				if(myDummy[((int)(i-1) / SIZE)+" , "+((int)(i-1) % SIZE)] == '0')
				{
					myDummy[((int)(i-1) / SIZE)+" , "+((int)(i-1) % SIZE)] == (isPlayerOne ? '2' : '1')
					a = max(a, LookForOptions(myDummy), step-1, false);
				}
			}
		}
		*****************************************************/
		return a;
    }

    void PrintBoard(char[,] board)
    {
        for (int i = 0; i < SIZE; i++)
            for (int j = 0; j < SIZE; j++)
                Debug.Log( i + " , "+ j +" : "+ board[i, j]);

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

    bool DoWeHaveAWiner(char[,] board, char player)
    {
        if ((board[0, 0] == player && board[1, 1] == player && board[2, 2] == player) ||
            (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player))
        {
            Debug.Log("Diagonal: "+ player+" : "+ board[2, 0]);
            return true;

        }else
        { 

            for (int i = 0; i < 3; i++)
			    if ((board [i, 2] == player && board [i, 1] == player && board [i, 0] == player) ||
				   (board [2, i] == player && board [1, i] == player && board [0, i] == player))
                {
                    Debug.Log("Linha: " + i +" - "+ player+" - "+ board[0, i]);
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
		int i = int.Parse (piece);
		Debug.Log (((int)(i-1) / SIZE)+" , "+((int)(i-1) % SIZE));

		//verifica se a casa está vazia
		if (TicTacToeGrid [(int)(i - 1) / SIZE, (int)(i - 1) % SIZE] == '0') {
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

    void PlaceXOnBoard(char playerType, int square )
    {
        Debug.Log(playerType+ " - "+square);
        TicTacToeGrid[(int)(square - 1) / SIZE, (int)(square - 1) % SIZE] = playerType;
        Squares[(int)(square - 1) / SIZE, (int)(square - 1) % SIZE].transform.GetComponent<SpriteRenderer>().sprite = spt[(int.Parse(playerType.ToString())-1)];
        isPlayerOne = !isPlayerOne;

    }
}
