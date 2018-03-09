using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class MenuGifController : MonoBehaviour {
    public Sprite[] frames;
    public Image menuBackground;
    public float frameRate = 0.1f;
 
    private int currentImage;
    // Use this for initialization
    void Start () {
        currentImage = 0;
        InvokeRepeating ("ChangeImage", 0.1f, frameRate);
    }
 
    private void ChangeImage() {
        if (currentImage == frames.Length - 1) {
            // CancelInvoke("ChangeImage");
			currentImage = 0;
            // Destroy (explosion);
        }
        currentImage += 1;
        menuBackground.sprite = frames [currentImage];
    }
}