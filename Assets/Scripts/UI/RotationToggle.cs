using UnityEngine;
using UnityEngine.UI;

public class RotationToggle : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Sprite notPressed;
    [SerializeField] private Sprite pressed;

    private bool isRotationOn = true;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }
    public void ToggleAnimation()
    {
        isRotationOn = !isRotationOn;
        gameManager.isRotationOn = isRotationOn;
        image.sprite = isRotationOn ? notPressed : pressed;
    }
}
