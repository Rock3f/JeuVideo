using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class triggerStory : MonoBehaviour {
    
    public string action;
    public GameObject[] ActionOnGameObject;

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
            }
            break;
            case "scene3":
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
                }
            break;
            case "scene5":
            if(other.tag != "Player")
            {
                ActionOnGameObject[0].SetActive(false);
                ActionOnGameObject[1].transform.position = new Vector3(ActionOnGameObject[2].transform.position.x, ActionOnGameObject[1].transform.position.y, ActionOnGameObject[1].transform.position.z);
                ActionOnGameObject[3].SetActive(true);
                ActionOnGameObject[4].SetActive(true);
                ActionOnGameObject[5].SetActive(true);
                wait();
                
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            break;

        }
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(5f);
    }
}