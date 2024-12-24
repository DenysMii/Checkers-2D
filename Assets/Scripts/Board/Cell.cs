using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerDownHandler
{
    public int[] boardPosition { get; set; }

    [SerializeField] private Sprite greenHighlight;
    [SerializeField] private Sprite yellowHighlight;
    [SerializeField] private Sprite redHighlight;
    public string status { private get; set; }

    public BoardManager boardManager { private get; set; }
    private SpriteRenderer spriteRenderer;
    public Piece attachedPieceObject { get; set; }
    
    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (status)
        {
            case "move":
                boardManager.currentPieceObject.MoveToNewCell(this);
                break;
            case "capture":
                boardManager.currentPieceObject.CaptureOpponentPiece(this);
                break;
            case "move into king":
                boardManager.currentPieceObject.MoveToNewCell(this, turnIntoKing: true);
                break;
            case "capture into king":
                boardManager.currentPieceObject.CaptureOpponentPiece(this, turnIntoKing: true);
                break;
        }   
    }

    public void HighlightCell()
    {
        switch (status)
        {
            case "move":
                spriteRenderer.sprite = greenHighlight;
                break;
            case "capture":
                spriteRenderer.sprite = redHighlight;
                break;
            case "move into king":
                spriteRenderer.sprite = yellowHighlight;
                break;
            case "capture into king":
                spriteRenderer.sprite = yellowHighlight;
                break;
            default:
                spriteRenderer.sprite = null;
                break;
        }
    }
}
