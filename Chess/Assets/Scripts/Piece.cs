using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece
{
    PIECE_TYPE type;
    TEAM team;

    public Piece(PIECE_TYPE type_, TEAM team_)
    {
        type = type_;
        team = team_;
    }

    public void SetType(PIECE_TYPE type_) { type = type_; }
    public void SetTeam(TEAM team_) { team = team_; }
    public new PIECE_TYPE GetType() { return type; }
    public TEAM GetTeam() { return team; }
}

public class King : Piece
{
    King(TEAM team_) : base(PIECE_TYPE.KING, team_)
    {

    }
}

public class Queen : Piece
{
    Queen(TEAM team_) : base(PIECE_TYPE.QUEEN, team_)
    {

    }
}

public class Bishop : Piece
{
    Bishop(TEAM team_) : base(PIECE_TYPE.BISHOP, team_)
    {

    }
}

public class Knight : Piece
{
    Knight(TEAM team_) : base(PIECE_TYPE.KNIGHT, team_)
    {

    }
}

public class Rook : Piece
{
    Rook(TEAM team_) : base(PIECE_TYPE.ROOK, team_)
    {

    }
}

public class Pawn : Piece
{
    Pawn(TEAM team_) : base(PIECE_TYPE.PAWN, team_)
    {

    }
}