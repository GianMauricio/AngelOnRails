using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    //Variables for movement
    public float sensitivity = 100;

    private float yRot = 0;
    [SerializeField] Transform playerTransform;
    void Start()
    {
        //Make mouse invisible
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        yRot -= mouseY;
        yRot = Mathf.Clamp(yRot, -90f, 90f);

        //Left-right
        playerTransform.Rotate(Vector3.up, mouseX);

        //Up-down
        transform.localRotation = Quaternion.Euler(yRot, 0f, 0f);
    }
}