using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PieceMapping
{
    private static PieceDefinition[] piecesArray;

    static PieceMapping()
    {
        List<PieceDefinition> piecesList = new List<PieceDefinition>();

        piecesList.Add(new PieceDefinition('K', new Sprite[] { GameManager.Instance.GetSprite(0), GameManager.Instance.GetSprite(6) }, PIECE_TYPE.KING));
        piecesList.Add(new PieceDefinition('Q', new Sprite[] { GameManager.Instance.GetSprite(1), GameManager.Instance.GetSprite(7) }, PIECE_TYPE.QUEEN));
        piecesList.Add(new PieceDefinition('B', new Sprite[] { GameManager.Instance.GetSprite(2), GameManager.Instance.GetSprite(8) }, PIECE_TYPE.BISHOP));
        piecesList.Add(new PieceDefinition('N', new Sprite[] { GameManager.Instance.GetSprite(3), GameManager.Instance.GetSprite(9) }, PIECE_TYPE.KNIGHT));
        piecesList.Add(new PieceDefinition('R', new Sprite[] { GameManager.Instance.GetSprite(4), GameManager.Instance.GetSprite(10) }, PIECE_TYPE.ROOK));
        piecesList.Add(new PieceDefinition('P', new Sprite[] { GameManager.Instance.GetSprite(5), GameManager.Instance.GetSprite(11) }, PIECE_TYPE.PAWN));

        piecesArray = piecesList.ToArray();
    }

    public static PieceDefinition GetPieceInfo(char key)
    {
        for (int i = 0; i < piecesArray.Length; i++)
            if (piecesArray[i].key.Equals(key))
                return piecesArray[i];

        return null;
    }

    public static PieceDefinition GetPieceInfo(PIECE_TYPE type)
    {
        for (int i = 0; i < piecesArray.Length; i++)
            if (piecesArray[i].type.Equals(type))
                return piecesArray[i];

        return null;
    }
}

public class PieceDefinition
{
    public char key;
    public Sprite[] sprites;
    public PIECE_TYPE type;

    public PieceDefinition(char key_, Sprite[] sprites_, PIECE_TYPE type_)
    {
        key = key_;
        sprites = sprites_;
        type = type_;
    }
}

public static class Tools
{
    public static bool Inbounds(BoardPosition position, int boundX, int boundY)
    {
        if (position.x >= 0 && position.x < boundX && position.y >= 0 && position.y < boundY)
            return true;
        else
            return false;
    }

    public static BoardPosition OneDtoTwoD(int singleD, int xBound)
    {
        return new BoardPosition(singleD % xBound, Mathf.Floor(singleD / xBound));
    }
}

public enum PIECE_TYPE
{
    KING = 0,
    QUEEN = 1,
    BISHOP = 2,
    KNIGHT = 3,
    ROOK = 4,
    PAWN = 5,
    UNASSIGNED = 6
}

public enum TEAM
{
    WHITE = 0,
    BLACK = 1,
    UNASSIGNED = 2
}