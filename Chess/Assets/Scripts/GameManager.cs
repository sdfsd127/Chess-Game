using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // SINGLETON
    public static GameManager Instance;

    // References to scripts
    private Board board;

    // References to Sizes on Board script
    private int board_width;
    private int board_height;

    // Piece definitions
    string[] defaultSetup =
    {
        "bR", "bN", "bB", "bK", "bQ", "bB", "bN", "bR",
        "bP", "bP", "bP", "bP", "bP", "bP", "bP", "bP",
        "  ", "  ", "  ", "  ", "  ", "  ", "  ", "  ",
        "  ", "  ", "  ", "  ", "  ", "  ", "  ", "  ",
        "  ", "  ", "  ", "  ", "  ", "  ", "  ", "  ",
        "  ", "  ", "  ", "  ", "  ", "  ", "  ", "  ",
        "wP", "wP", "wP", "wP", "wP", "wP", "wP", "wP",
        "wR", "wN", "wB", "wK", "wQ", "wB", "wN", "wR"
    };
    [SerializeField] private Sprite[] pieceSprites;

    // Game state vars
    private enum GAME_STATE
    {
        LOADING = 0,
        P1_UNSELECTED = 1,
        P1_SELECTED = 2,
        P2_UNSELECTED = 3,
        P2_SELECTED = 4
    }
    private GAME_STATE gameState;

    private const bool FLIP_BOARD_LAYOUT = false;

    TEAM currentTurn;

    BoardPosition pieceCurrentlySelected;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        // Begin loading
        gameState = GAME_STATE.LOADING;

        // Setup beginning rules such as N = Knight = {SPRITE} etc

        // Find board object and create a mesh on it
        board = GameObject.Find("Board").GetComponent<Board>();
        board.InitialSetup();
        board.SetupBoardMesh();

        // Put board size into memory
        board_width = board.GetBoardWidth();
        board_height = board.GetBoardHeight();

        // Put board into screen centre
        board.gameObject.transform.position = new Vector3(0.5f - (board.GetBoardWidth() / 2), 0.5f - (board.GetBoardHeight() / 2), 0);

        // Setup Pieces on board
        board.SetupPieces(defaultSetup, FLIP_BOARD_LAYOUT);

        // Begin the game logic
        BeginGame();
    }

    private void Update()
    {
        // If player left clicks
        if (Input.GetMouseButtonDown(0))
        {
            // Find mouse position
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = Camera.main.nearClipPlane;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

            // Convert to board position
            BoardPosition boardPositionClicked = MousePosToBoardPos(mouseWorldPos.x, mouseWorldPos.y);
            boardPositionClicked.Print();

            // Check clicking within board
            if (Tools.Inbounds(boardPositionClicked, board_width, board_height))
            {
                // Check if a piece is on the board position
                if (board.GetSquare(boardPositionClicked).IsOccupied())
                {
                    // If so player is either trying to move onto opponent OR choose a different piece
                    // Differentiate by comparing current player turn to piece owner
                    Piece pieceSelected = new Piece(PIECE_TYPE.UNASSIGNED, TEAM.UNASSIGNED);
                    if (board.GetSquare(boardPositionClicked).HasPiece())
                        pieceSelected = board.GetSquare(boardPositionClicked).piece;

                    // Piece has same colour as players turn -> Switch piece selected
                    if (pieceSelected.GetTeam() == currentTurn)
                    {
                        pieceCurrentlySelected = boardPositionClicked;
                        SetGameState("SELECTED");
                    }
                    else if (pieceCurrentlySelected != null) // Piece has different colour -> Try to move
                    {
                        Move(pieceCurrentlySelected, boardPositionClicked);
                    }
                }
                else
                {
                    // Else check if they are trying to make a move
                    if (IsSelected())
                    {
                        // TEMP => Move to that square, no questions
                        Move(pieceCurrentlySelected, boardPositionClicked);
                    }
                }
            }
        }
    }

    private void BeginGame()
    {
        gameState = GAME_STATE.P1_UNSELECTED;
        currentTurn = TEAM.WHITE;
    }

    private void Move(BoardPosition a, BoardPosition b)
    {
        Piece pieceMoved = board.GetSquare(a).piece;
        board.GetSquare(a).RemovePiece();
        board.GetSquare(b).SetPiece(pieceMoved);

        AfterMove();
    }

    private void AfterMove()
    {
        // Deselect the piece
        DeselectPiece();

        // Move made -> Change turns
        ChangeTurn();
    }

    private BoardPosition MousePosToBoardPos(float xPos, float yPos)
    {
        int boundX = board_width / 2;
        int boundY = board_height / 2;
        int x = (int)Mathf.Floor(xPos) + boundX;
        int y = (int)Mathf.Floor(yPos) + boundY;

        return new BoardPosition(x, y);
    }

    public Sprite GetSprite(int index) { return pieceSprites[index]; }

    private bool IsSelected()
    {
        if (gameState == GAME_STATE.P1_SELECTED || gameState == GAME_STATE.P2_SELECTED)
            return true;
        else
            return false;
    }

    private void SetGameState(string state)
    {
        switch (state)
        {
            case "SELECTED":
                if (currentTurn == TEAM.WHITE)
                    gameState = GAME_STATE.P1_SELECTED;
                else
                    gameState = GAME_STATE.P2_SELECTED;
                break;
            case "UNSELECTED":
                if (currentTurn == TEAM.WHITE)
                    gameState = GAME_STATE.P1_UNSELECTED;
                else
                    gameState = GAME_STATE.P2_UNSELECTED;
                break;
        }
    }

    private void DeselectPiece()
    {
        pieceCurrentlySelected = null;
        SetGameState("UNSELECTED");
    }

    private void ChangeTurn()
    {
        // Flip the turns
        if (currentTurn == TEAM.WHITE)
            currentTurn = TEAM.BLACK;
        else
            currentTurn = TEAM.WHITE;

        UIManager.Instance.SetPlayerTurn(currentTurn);
    }
}

public class BoardPosition
{
    public int x;
    public int y;

    public BoardPosition(int x_, int y_)
    {
        x = x_;
        y = y_;
    }

    public BoardPosition(float x_, float y_)
    {
        x = (int)x_;
        y = (int)y_;
    }

    public void Print()
    {
        Debug.Log("X: " + x + " and Y: " + y);
    }

    public void Print(string msg)
    {
        Debug.Log(msg + ": X: " + x + " and Y: " + y);
    }
}

public class PotentialMove
{
    BoardPosition moveTo;
    BoardPosition moveFrom;

    public PotentialMove(BoardPosition moveTo_, BoardPosition moveFrom_)
    {
        moveTo = moveTo_;
        moveFrom = moveFrom_;
    }
}