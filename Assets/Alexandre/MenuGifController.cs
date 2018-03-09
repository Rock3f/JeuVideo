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
		// We comment the 2 lines above to have a loop on our GIF
		// If we wants to play it once do the opposite of what is comment in the if condition
        if (currentImage == frames.Length - 1) {
            // CancelInvoke("ChangeImage");
			currentImage = 0;
            // Destroy (explosion);
        }
        currentImage += 1;
        menuBackground.sprite = frames [currentImage];
    }
}