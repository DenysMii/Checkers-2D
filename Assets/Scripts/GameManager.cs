using System.Collections;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Range(0, 3)]
    [SerializeField] private float hideUIDuringRotationTime;

    [SerializeField] private Animator boardAnimator;

    [SerializeField] private CanvasGroup buttonsElement;
    [SerializeField] private TextMeshProUGUI turnMessage;

    public bool isRotationOn { private get; set; }

    private void Start()
    {
        isRotationOn = true;
    }
    
    public void SwitchTurn(bool isWhiteTurn)
    {
        boardAnimator.SetBool("IsWhiteTurn", isWhiteTurn);
        RotateBoard();

        DisableTurnMessage();
        
        if (isRotationOn)
        {
            HideButtons();
            StartCoroutine(ShowButtonsAfterTime(hideUIDuringRotationTime));
            StartCoroutine(EnableTurnMessageAfterTime(hideUIDuringRotationTime, isWhiteTurn));
        }
        else
        {
            StartCoroutine(EnableTurnMessageAfterTime(0, isWhiteTurn));
        }
    }

    public void RotateBoard()
    {
        boardAnimator.enabled = isRotationOn;
        if (isRotationOn)
            boardAnimator.SetBool("Rotate", true);
        else
            boardAnimator.gameObject.transform.Rotate(0, 0, 180);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void HideButtons()
    {
        buttonsElement.interactable = false;
        buttonsElement.blocksRaycasts = false;
        buttonsElement.alpha = 0;

    }

    private IEnumerator ShowButtonsAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        buttonsElement.interactable = true;
        buttonsElement.blocksRaycasts = true;
        buttonsElement.alpha = 1;
    }

    private void DisableTurnMessage()
    {
        turnMessage.gameObject.SetActive(false);
    }

    private IEnumerator EnableTurnMessageAfterTime(float delay, bool isWhite)
    {
        yield return new WaitForSeconds(delay);
        turnMessage.color = isWhite ? Color.white : Color.black;
        turnMessage.text = (isWhite ? "WHITE" : "BLACK") + " TURN";
        turnMessage.gameObject.SetActive(true);
    }

}
