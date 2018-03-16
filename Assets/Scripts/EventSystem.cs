using UnityEngine;

public class EventSystem : MonoBehaviour {
    public GameObject eventPrefab;
    private void Start() {
        if(GameObject.Find("EventSystem") == null){
         Instantiate(eventPrefab);
        }
    }
}