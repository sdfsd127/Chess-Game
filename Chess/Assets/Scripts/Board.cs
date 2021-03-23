using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Board : MonoBehaviour
{
    // Parent gameObject for objects
    GameObject rootParent;

    // Board meshes
    private Mesh meshA;
    private Mesh meshB;

    // Board mesh components
    [SerializeField] private MeshRenderer meshRendererA;
    [SerializeField] private MeshRenderer meshRendererB;
    [SerializeField] private MeshFilter meshFilterA;
    [SerializeField] private MeshFilter meshFilterB;

    // Board variables
    private const int BOARD_WIDTH = 8;
    public int GetBoardWidth() { return BOARD_WIDTH; }

    private const int BOARD_HEIGHT = 8;
    public int GetBoardHeight() { return BOARD_HEIGHT; }

    private Square[,] board;

    public void InitialSetup()
    {
        rootParent = new GameObject("Squares");
        board = new Square[BOARD_WIDTH, BOARD_HEIGHT];

        for (int x = 0; x < BOARD_WIDTH; x++)
        {
            for (int y = 0; y < BOARD_HEIGHT; y++)
            {
                board[x, y] = new Square(new BoardPosition(x, y), new Vector2(0.5f - (BOARD_WIDTH / 2), 0.5f - (BOARD_HEIGHT / 2)), rootParent.transform);
            }
        }
    }

    public void SetupBoardMesh()
    {
        // Create meshes of correct board size
        Mesh[] meshes = MeshCreator.GetBoardMesh(new BoardPosition(BOARD_WIDTH, BOARD_HEIGHT));

        meshA = meshes[0];
        meshB = meshes[1];

        meshFilterA.mesh = meshA;
        meshFilterB.mesh = meshB;

        // Create and apply materials
        Material meshMaterialA = new Material(Shader.Find("Specular"));
        meshMaterialA.color = StylingData.Instance.colourA;
        meshRendererA.material = meshMaterialA;

        Material meshMaterialB = new Material(Shader.Find("Specular"));
        meshMaterialB.color = StylingData.Instance.colourB;
        meshRendererB.material = meshMaterialB;
    }

    public void SetupPieces(string[] pieceLayout, bool flipLayout = false)
    {
        // Potentially flip layout to write to board as seen top left to assumed board top left -> Else it writes to left to bottom left first
        if (flipLayout)
            Array.Reverse(pieceLayout);

        // Check all layout spaces and map to board
        for (int i = 0; i < pieceLayout.Length; i++)
        {
            // If current cell is not empty
            if (pieceLayout[i] != "  ")
            {
                // Find the piece it is
                PIECE_TYPE pieceTypeFound = PieceMapping.GetPieceInfo(pieceLayout[i][1]).type;
                TEAM pieceTeamFound = (pieceLayout[i][0] == 'w') ? TEAM.WHITE : TEAM.BLACK;

                Piece newPiece = new Piece(pieceTypeFound, pieceTeamFound);

                BoardPosition newPiecePosition = Tools.OneDtoTwoD(i, BOARD_WIDTH);
                board[newPiecePosition.x, newPiecePosition.y].SetPiece(newPiece);
            }
        }
    }

    public Square GetSquare(BoardPosition squarePosition) { return board[squarePosition.x, squarePosition.y]; }
}

public class Square
{
    public BoardPosition position;
    public Vector2 realPos;
    
    private GameObject squareWorldObject;
    private SpriteRenderer squareWorldObjectRenderer;

    public bool occupied; // False = No piece on this square
    public Piece piece;

    public Square(BoardPosition position_, Vector2 realPos_, Transform parentObj)
    {
        position = position_;
        realPos = realPos_;

        squareWorldObject = new GameObject("SQ OBJ (" + position.x + ", " + position.y + ")");
        squareWorldObject.transform.parent = parentObj;
        squareWorldObject.transform.position = new Vector3(realPos.x + position.x, realPos.y + position.y, 0);

        squareWorldObjectRenderer = squareWorldObject.AddComponent<SpriteRenderer>();
    }

    public bool IsOccupied() { return occupied; }

    public void SetPiece(Piece newPiece) 
    { 
        piece = newPiece;
        occupied = true;

        UpdatePieceSprite();
    }

    public void RemovePiece()
    {
        piece = null;
        occupied = false;

        ResetPieceSprite();
    }

    public bool HasPiece() 
    { 
        if (piece != null) 
            return true; 
        else 
            return false; 
    }

    public Piece GetPiece() { return piece; }

    private void UpdatePieceSprite()
    {
        position.Print();
        int wORbIndex = (piece.GetTeam() == TEAM.WHITE) ? 0 : 1;
        Sprite newSprite = PieceMapping.GetPieceInfo(piece.GetType()).sprites[wORbIndex];
        squareWorldObjectRenderer.sprite = newSprite;
    }

    private void ResetPieceSprite()
    {
        squareWorldObjectRenderer.sprite = null;
    }
}