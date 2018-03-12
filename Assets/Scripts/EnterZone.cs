using UnityEngine;
using System.Linq;

public class EnterZone : MonoBehaviour {
    public GameObject cameraMain;
    
    private void OnTriggerEnter2D(Collider2D other) {
        Track cameraTrack = cameraMain.GetComponent<Track>();
        cameraTrack.isCameraFix = true;
        cameraTrack.idCurrentScreen++;
        string name = "screen" + cameraTrack.idCurrentScreen;
        foreach(GameObject ennemy in cameraTrack.allScreen.FirstOrDefault(x => x.name == name).ennemies)
        {
            ennemy.SetActive(true);
        }

        this.gameObject.SetActive(false);
    }
}