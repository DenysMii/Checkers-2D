using System;
using UnityEngine;

public class CellsPiecesGenerator : MonoBehaviour
{
    [SerializeField] private float localCellStartPos = -2.755f;
    [SerializeField] private float cellCoordDiff = 0.787f;

    private Cell[,] cellObjectsMatrix = new Cell[8, 8];
    private int piecesCounter = 0;

    [SerializeField] private GameObject cellsHolder;
    [SerializeField] private GameObject piecesHolder;

    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject whiteCheckerPrefab;
    [SerializeField] private GameObject blackCheckerPrefab;

    [SerializeField] private GameObject whiteKingPrefab;
    [SerializeField] private GameObject blackKingPrefab;

    public BoardManager boardManager { private get; set; }

    private void Start()
    {

    }
    public Cell[,] GenerateCellsAndCheckers()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = i % 2; j < 8; j += 2)
            {
                GameObject newCell = Instantiate(cellPrefab, cellsHolder.transform);
                Cell newCellObject = newCell.GetComponent<Cell>();

                newCell.name = "Cell " + (i + 1) + " " + (j + 1);
                newCellObject.boardPosition = new int[]{ i, j };
                newCellObject.boardManager = boardManager;

                Vector2 pos = new Vector2(localCellStartPos + j * cellCoordDiff, localCellStartPos + i * cellCoordDiff);
                newCell.transform.SetLocalPositionAndRotation(pos, Quaternion.identity);
                if (i + 1 < 4 || i + 1 > 5)
                {
                    newCellObject.attachedPieceObject = GenerateChecker(newCellObject, pos);
                }
                cellObjectsMatrix[i, j] = newCellObject;
            }
        }
        return cellObjectsMatrix;
    }
    private Checker GenerateChecker(Cell cellObject, Vector2 cellPos)
    {
        GameObject newChecker;

        if (++piecesCounter <= 12)
        {
            newChecker = Instantiate(whiteCheckerPrefab, piecesHolder.transform);
            newChecker.name = "WhiteChecker " + piecesCounter;
        }
        else
        {
            newChecker = Instantiate(blackCheckerPrefab, piecesHolder.transform);
            newChecker.name = "BlackChecker " + (piecesCounter - 12);
        }
        Checker newCheckerObject = newChecker.GetComponent<Checker>();
        newCheckerObject.boardManager = boardManager;
        newCheckerObject.attachedCellObject = cellObject;

        newChecker.transform.SetLocalPositionAndRotation(cellPos, Quaternion.identity);
        return newChecker.GetComponent<Checker>();
    }

    public void GenerateKing(Checker checkerObject, Cell cellObject)
    {
        GameObject prefab = checkerObject.isWhite ? whiteKingPrefab : blackKingPrefab;
        Vector3 kingPos = checkerObject.gameObject.transform.localPosition;
        Quaternion kingRotation = prefab.transform.rotation;

        GameObject newKing = Instantiate(prefab, piecesHolder.transform);
        newKing.transform.SetLocalPositionAndRotation(kingPos, kingRotation);
        newKing.name = checkerObject.gameObject.name.Replace("Checker", "King");

        boardManager.DestroyPiece(cellObject.boardPosition);

        King newKingObject = newKing.GetComponent<King>();
        cellObject.attachedPieceObject = newKingObject;
        newKingObject.boardManager = boardManager;
        newKingObject.attachedCellObject = cellObject;
        
    }
}
