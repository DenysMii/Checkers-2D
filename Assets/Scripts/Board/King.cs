using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{

    protected override void Start()
    {
        base.Start();
        boardManager.CheckForCapture();
        if (!boardManager.isObligatedToCapture)
            boardManager.SwitchTurn();
    }
    public override List<int[]> GetCaptureCellsBPos()
    {
        List<int[]> captureCellsBPos = new List<int[]>();

        foreach(var cellDir in cellsDir)
        {
            List<int[]> dirMoveCells = GetDirectionalMoveCells(cellDir);
            int[] cellCoords = CoordsSum(dirMoveCells.Count > 0 ? dirMoveCells[^1] : attachedCellObject.boardPosition, cellDir);
            if (boardManager.IsCellOccupiedByOpponent(cellCoords, isWhite))
                captureCellsBPos.Add(CoordsSum(cellCoords, cellDir));
        }

        return captureCellsBPos;
    }

    public override List<int[]> GetMoveCellsBPos()
    {
        List<int[]> moveCellsBPos = new List<int[]>();

        foreach(var cellDir in cellsDir)
            moveCellsBPos.AddRange(GetDirectionalMoveCells(cellDir));

        return moveCellsBPos;

    }

    private List<int[]> GetDirectionalMoveCells(int[] cellDir)
    {
        List<int[]> directionalMoveCells = new List<int[]>();

        int[] cellDiff = cellDir;

        while (boardManager.IsCellFree(CoordsSum(attachedCellObject.boardPosition, cellDiff)))
        {
            directionalMoveCells.Add(CoordsSum(attachedCellObject.boardPosition, cellDiff));
            cellDiff = CoordsSum(cellDiff, cellDir);
        }

        return directionalMoveCells;
    }

    public override void MoveToNewCell(Cell newCellObject, bool checkForBeating = false, bool turnIntoKing = false)
    {
        base.MoveToNewCell(newCellObject, checkForBeating, turnIntoKing);

        if (checkForBeating)
            boardManager.CheckForCapture();

        if (!boardManager.isObligatedToCapture)
            boardManager.SwitchTurn();
    }
}
