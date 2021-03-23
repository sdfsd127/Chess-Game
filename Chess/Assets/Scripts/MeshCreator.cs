using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshCreator
{
    public static Mesh[] GetBoardMesh(BoardPosition sizeOfBoard)
    {
        // A is Highlight squares, B is Shadow squares
        Mesh meshA = CreateColouredCheckerboardPattern(false, sizeOfBoard);
        Mesh meshB = CreateColouredCheckerboardPattern(true, sizeOfBoard);

        return new Mesh[] { meshA, meshB };
    }

    private static Mesh CreateColouredCheckerboardPattern(bool offset, BoardPosition sizeOfBoard)
    {
        // Create new mesh
        Mesh newMesh = new Mesh();

        // Create lists to store vertices and triangles of squares to be instantiated
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        int count = 0;

        for (int y = 0; y < sizeOfBoard.y; y++)
        {
            for (int x = ((offset && y % 2 == 0) ||(!offset && y % 2 != 0)) ? 1: 0; x < sizeOfBoard.x; x += 2)
            {
                // Add 4 vertices of the square
                vertices.Add(new Vector3(x - 0.5f, y + 0.5f, 0)); // Top Left
                vertices.Add(new Vector3(x + 0.5f, y + 0.5f, 0)); // Top Right
                vertices.Add(new Vector3(x + 0.5f, y - 0.5f, 0)); // Bottom Right
                vertices.Add(new Vector3(x - 0.5f, y - 0.5f, 0)); // Bottom Left

                // TRI 1
                triangles.Add(count + 0);
                triangles.Add(count + 1);
                triangles.Add(count + 2);

                // TRI 2
                triangles.Add(count + 2);
                triangles.Add(count + 3);
                triangles.Add(count + 0);

                count += 4;
            }
        }

        // Write to mesh with new created data
        newMesh.vertices = vertices.ToArray();
        newMesh.triangles = triangles.ToArray();

        return newMesh;
    }
}
