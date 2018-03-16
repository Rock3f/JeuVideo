using UnityEngine;

public class RotateAnimation : MonoBehaviour {
    public bool isRotate = true;
    private void Update() {
        if(isRotate)
        {
            float rotationZ = this.gameObject.transform.rotation.z - 5f;
            this.gameObject.transform.Rotate(0,0,rotationZ, Space.Self);
        }
    }
}