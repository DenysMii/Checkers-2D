using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private Sprite notPressed;
    [SerializeField] private Sprite pressed;

    private bool isSoundOn = true;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }
    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        audioListener.enabled = isSoundOn;
        image.sprite = isSoundOn ? notPressed : pressed;
    }
}
