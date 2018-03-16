using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class triggerStory : MonoBehaviour {
    
    public string action;
    public GameObject[] ActionOnGameObject;
    public GameObject CameraMain;

    private bool IsEnd = false;

    private void Update() {
        if(Input.GetButtonDown("Fire1")){
            if(IsEnd)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }	
            else
            {
                IsEnd = true;
            }	
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        switch(action)
        {
            case "scene1":
            if(other.tag == "Player")
                ActionOnGameObject[0].SetActive(false);
            break;
            case "scene2":

            if(other.tag != "Enemy")
            {
                ActionOnGameObject[0].GetComponent<movePlayer>().directionStory = 0;
                ActionOnGameObject[1].GetComponent<movePlayer>().directionStory = 0;
                ActionOnGameObject[2].GetComponent<TrackStory>().target = ActionOnGameObject[3].transform;
                ActionOnGameObject[3].GetComponent<movePnjBackGround>().isWaiting = false;
                ActionOnGameObject[4].GetComponent<movePnjBackGround>().isWaiting = false;
                ActionOnGameObject[5].GetComponent<movePnjBackGround>().isWaiting = false;
                ActionOnGameObject[6].GetComponent<movePnjBackGround>().isWaiting = false;
                CameraMain.GetComponents<AudioSource>().FirstOrDefault(x => x.clip.name == "tank").loop = true;
                CameraMain.GetComponents<AudioSource>().FirstOrDefault(x => x.clip.name == "tank").Play();
            }
            break;
            case "scene3":
                CameraMain.GetComponents<AudioSource>().FirstOrDefault(x => x.clip.name == "tank").Stop();
                CameraMain.GetComponents<AudioSource>().FirstOrDefault(x => x.clip.name == "accident").PlayOneShot(CameraMain.GetComponents<AudioSource>().FirstOrDefault(x => x.clip.name == "accident").clip);
                ActionOnGameObject[0].GetComponent<movePnjBackGround>().isWaiting = true;
                ActionOnGameObject[0].transform.Rotate(0,0,-5.004f, Space.Self); 
                ActionOnGameObject[1].GetComponent<movePnjBackGround>().positionY = -4f;             
                ActionOnGameObject[2].GetComponent<RotateAnimation>().isRotate = false;
                ActionOnGameObject[2].GetComponent<movePnjBackGround>().isWaiting = true;
                ActionOnGameObject[3].GetComponent<movePnjBackGround>().isWaiting = true;
                ActionOnGameObject[3].GetComponent<EnemyFollow>().enabled = true;
                ActionOnGameObject[4].GetComponent<TrackStory>().target = ActionOnGameObject[3].transform;
            break;
            case "scene4":
                if(other.tag == "Enemy")
                {
                    ActionOnGameObject[0].SetActive(false);
                    ActionOnGameObject[1].GetComponent<TrackStory>().target = ActionOnGameObject[2].transform;
                    ActionOnGameObject[2].GetComponent<movePnjBackGround>().isWaiting = false;
                    CameraMain.GetComponents<AudioSource>().FirstOrDefault(x => x.clip.name == "car").Play();
                }
            break;
            case "scene5":
            if(other.tag != "Player")
            {
                IsEnd = true;   
                CameraMain.GetComponents<AudioSource>().FirstOrDefault(x => x.clip.name == "car").Stop();
                ActionOnGameObject[0].SetActive(false);
                ActionOnGameObject[3].SetActive(true);
                ActionOnGameObject[4].SetActive(true);
                ActionOnGameObject[5].SetActive(true);                
                ActionOnGameObject[1].GetComponent<TrackStory>().target = ActionOnGameObject[2].transform;             
            }
            break;

        }
    }
}