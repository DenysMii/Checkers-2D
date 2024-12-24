using System.Drawing;
using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject resultWindow;

    private const int piecesCounter = 24;
    private int drawCounter = 10;
    private Cell[,] cellObjectsMatrix;

    public CellsPiecesGenerator cellsCheckersGenerator { get; private set; }
    public Piece currentPieceObject { get; private set; }
    private ResultScript resultScript;

    public bool isWhiteTurn { get; private set; }
    public bool isObligatedToCapture {  get; set; }
    
    private void Start()
    {
        isWhiteTurn = true;
        cellsCheckersGenerator = gameObject.GetComponent<CellsPiecesGenerator>();
        cellsCheckersGenerator.boardManager = this;
        cellObjectsMatrix = cellsCheckersGenerator.GenerateCellsAndCheckers();

        resultScript = resultWindow.GetComponent<ResultScript>();
    }

    public int GetHighlightedCellsCount(List<int[]> cellsBPos)
    {
        int highlightedCells = 0;
        foreach (var cellBPos in cellsBPos)
        {
            if (IsCellFree(cellBPos))
                highlightedCells++;
        }
        return highlightedCells;
    }

    public void HighlightCells(Piece newPieceObject, List<int[]> cellsBPos, string status)
    {
        if(currentPieceObject != null)
            currentPieceObject.ClearPossibleCells();
        currentPieceObject = newPieceObject;

        foreach (var cellBPos in cellsBPos)
        {
            if(IsCellOnEdge(cellBPos) && IsCellFree(cellBPos) && newPieceObject is Checker)
            {
                cellObjectsMatrix[cellBPos[0], cellBPos[1]].status = status + " into king";
                cellObjectsMatrix[cellBPos[0], cellBPos[1]].HighlightCell();
            }
            else if (IsCellFree(cellBPos))
            {
                cellObjectsMatrix[cellBPos[0], cellBPos[1]].status = status;
                cellObjectsMatrix[cellBPos[0], cellBPos[1]].HighlightCell();
            }
        }
    }

    public void ClearCells(List<int[]> cellsBPos)
    {
        foreach(var cellBPos in cellsBPos)
        {
            if (IsInsideBoard(cellBPos))
            {
                cellObjectsMatrix[cellBPos[0], cellBPos[1]].status = "";
                cellObjectsMatrix[cellBPos[0], cellBPos[1]].HighlightCell();
            }
                
        }
    }

    public void SwitchTurn()
    {
        isWhiteTurn = !isWhiteTurn;

        CheckForCapture();
        CheckForEndGame();

        RotateKings();
        gameManager.SwitchTurn(isWhiteTurn);
    }

    public void CheckForCapture()
    {
        foreach (Cell cell in cellObjectsMatrix)
        {
            if (cell != null && !IsCellFree(cell.boardPosition))
                if (cell.attachedPieceObject.isWhite == isWhiteTurn && GetHighlightedCellsCount(cell.attachedPieceObject.GetCaptureCellsBPos()) > 0)
                {
                    isObligatedToCapture = true;
                    drawCounter = 40;
                    return;
                }

        }
        isObligatedToCapture = false;
        drawCounter--;
    }

    private void CheckForEndGame()
    {
        if (!isObligatedToCapture)
        {
            if ((!IsMovePossible(isWhiteTurn) && !IsMovePossible(!isWhiteTurn)) || drawCounter <= 0)
            {
                resultWindow.transform.parent.gameObject.SetActive(true);
                resultWindow.SetActive(true);
                resultScript.SetResult(true);
            }
            else if (!IsMovePossible(isWhiteTurn))
            {
                resultWindow.transform.parent.gameObject.SetActive(true);
                resultWindow.SetActive(true);
                resultScript.SetResult(false);
            }
        }
    }

    private void RotateKings()
    {
        foreach (Cell cell in cellObjectsMatrix)
        {
            if (cell != null && !IsCellFree(cell.boardPosition) && cell.attachedPieceObject is King)
                cell.attachedPieceObject.gameObject.transform.Rotate(0, 0, 180);
        }
    }

    public void DestroyPiece(int[] cellBPos)
    {
        Destroy(cellObjectsMatrix[cellBPos[0], cellBPos[1]].attachedPieceObject.gameObject);
        cellObjectsMatrix[cellBPos[0], cellBPos[1]].attachedPieceObject = null;
    }

    public bool IsCellFree(int[] cellBPos)
    {
        if(!IsInsideBoard(cellBPos))
            return false;
        return cellObjectsMatrix[cellBPos[0], cellBPos[1]].attachedPieceObject == null;
    }

    public bool IsCellOnEdge(int[] cellBPos)
    {
        return (cellBPos[0] == 0 && !currentPieceObject.isWhite) || (cellBPos[0] == 7 && currentPieceObject.isWhite);
    }

    public bool IsCellOccupiedByOpponent(int[] cellBPos, bool isPieceWhite)
    {
        if (!IsInsideBoard(cellBPos))
            return false;
        if(IsCellFree(cellBPos))
            return false;
        return cellObjectsMatrix[cellBPos[0], cellBPos[1]].attachedPieceObject.isWhite != isPieceWhite;
    }

    private bool IsInsideBoard(int[] cellBPos)
    {
        return (cellBPos[0] >= 0 && cellBPos[0] < 8 && cellBPos[1] >= 0 && cellBPos[1] < 8);
    }

    public bool IsMovePossible(bool isWhite)
    {
        foreach (Cell cell in cellObjectsMatrix)
        {
            if (cell != null && !IsCellFree(cell.boardPosition))
                if (cell.attachedPieceObject.isWhite == isWhite && GetHighlightedCellsCount(cell.attachedPieceObject.GetMoveCellsBPos()) > 0)
                    return true;
        }
        return false;
    }
}

