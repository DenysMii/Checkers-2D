using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
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
        AudioListener.volume = isSoundOn ? 1.0f : 0.0f;
        image.sprite = isSoundOn ? notPressed : pressed;
    }
}
