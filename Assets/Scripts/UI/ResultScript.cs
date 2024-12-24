using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultScript : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;

    [SerializeField] private Image resultImage;
    [SerializeField] private Sprite whiteWonImage;
    [SerializeField] private Sprite blackWonImage;
    [SerializeField] private Sprite drawImage;

    [SerializeField] private AudioSource resultSound;
    [SerializeField] private AudioClip winAudio;
    [SerializeField] private AudioClip drawAudio;

    [SerializeField] private TextMeshProUGUI resultText;

    public void SetResult(bool isDraw)
    {
        if (isDraw)
        {
            resultImage.sprite = drawImage;
            resultText.text = "DRAW!!!";
            resultSound.clip = drawAudio;
        }
        else
        {
            resultImage.sprite = boardManager.isWhiteTurn ? blackWonImage : whiteWonImage;
            resultText.text = boardManager.isWhiteTurn ? "BLACK WON!!!!" : "WHITE WON!!!";
            resultSound.clip = winAudio;
        }
        resultSound.Play();
    }
}
