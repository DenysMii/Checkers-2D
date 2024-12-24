using System.Collections.Generic;
using UnityEngine;

public class Checker : Piece
{
    public override List<int[]> GetCaptureCellsBPos()
    {
        List<int[]> beatCellsBPos = new List<int[]>();

        foreach (var cellDir in cellsDir)
        {
            int[] cellCoords = CoordsSum(attachedCellObject.boardPosition, cellDir);
            if (boardManager.IsCellOccupiedByOpponent(cellCoords, isWhite))
                beatCellsBPos.Add(CoordsSum(cellCoords, cellDir));
        }

        return beatCellsBPos;
    }

    public override List<int[]> GetMoveCellsBPos()
    {
        int cellsDirIndex = isWhite ? 0 : 2;
        List<int[]> moveCellsBPos = new List<int[]>
        {
            CoordsSum(attachedCellObject.boardPosition, cellsDir[cellsDirIndex]),
            CoordsSum(attachedCellObject.boardPosition, cellsDir[cellsDirIndex + 1])
        };

        return moveCellsBPos;
    }

    public override void MoveToNewCell(Cell newCellObject, bool checkForBeating = false, bool turnIntoKing = false)
    {
        base.MoveToNewCell(newCellObject, checkForBeating, turnIntoKing);

        if (turnIntoKing)
        {
            boardManager.cellsCheckersGenerator.GenerateKing(this, attachedCellObject);
            return;
        }
            
        if (checkForBeating)
            boardManager.CheckForCapture();

        if(!boardManager.isObligatedToCapture)
            boardManager.SwitchTurn();

    }
    
}
