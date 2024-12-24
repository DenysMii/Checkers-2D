using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Piece : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] public bool isWhite;
    [SerializeField] public AudioClip moveAudio;
    [SerializeField] public AudioClip captureAudio;

    protected int moveDirection;
    public List<int[]> highlightedCellsBPos { get; protected set; }

    public BoardManager boardManager { protected get; set; }
    public Cell attachedCellObject { protected get; set; }
    protected AudioSource audioSource;

    protected List<int[]> cellsDir = new List<int[]>
    {
        new int[]{ 1, 1 },
        new int[]{ 1, -1 },
        new int[]{ -1, 1 },
        new int[]{ -1, -1 },
    };

    protected virtual void Start()
    {
        moveDirection = isWhite ? 1 : -1;
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isWhite == boardManager.isWhiteTurn)
            HighlightPossibleCells();
    }

    protected void HighlightPossibleCells()
    {
        string highlightStatus;
        if (boardManager.isObligatedToCapture)
        {
            highlightedCellsBPos = GetCaptureCellsBPos();
            highlightStatus = "capture";
        }
        else
        {
            highlightedCellsBPos = GetMoveCellsBPos();
            highlightStatus = "move";
        }
        boardManager.HighlightCells(this, highlightedCellsBPos, highlightStatus);
    }

    public abstract List<int[]> GetCaptureCellsBPos();
    public abstract List<int[]> GetMoveCellsBPos();

    public virtual void ClearPossibleCells()
    {
        boardManager.ClearCells(highlightedCellsBPos);
    }

    public virtual void MoveToNewCell(Cell newCellObject, bool isCapturing = false, bool turnIntoKing = false)
    {
        audioSource.clip = isCapturing ? captureAudio : moveAudio;
        audioSource.Play();

        ClearPossibleCells();

        attachedCellObject.attachedPieceObject = null;
        attachedCellObject = newCellObject;
        attachedCellObject.attachedPieceObject = this;

        Vector3 piecePos = attachedCellObject.gameObject.transform.localPosition;
        Quaternion pieceRotation = gameObject.transform.localRotation;
        gameObject.transform.SetLocalPositionAndRotation(piecePos, pieceRotation);

    }

    public virtual void CaptureOpponentPiece(Cell newCellObject, bool turnIntoKing = false)
    {
        int[] opponentPieceBPos = new int[]
        {
            newCellObject.boardPosition[0] - Math.Sign(newCellObject.boardPosition[0] - attachedCellObject.boardPosition[0]),
            newCellObject.boardPosition[1] - Math.Sign(newCellObject.boardPosition[1] - attachedCellObject.boardPosition[1])
        };

        boardManager.DestroyPiece(opponentPieceBPos);
        boardManager.isObligatedToCapture = false;

        MoveToNewCell(newCellObject, true, turnIntoKing);
    }

    protected int[] CoordsSum(int[] a,  int[] b)
    {
        int[] sum = new int[]
        {
            a[0] + b[0],
            a[1] + b[1]
        };
        return sum;
    }
}
