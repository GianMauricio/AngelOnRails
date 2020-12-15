using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //PlayerMovement Variables
    private float SPEED = 5;
    [SerializeField] Camera MainView;
    [SerializeField] GameObject Player;

    private int Armor = 100;
    private float Armor_Rank = 3;
    private int Health = 100;

    Vector3 CameraRotation;
    // Start is called before the first frame update
    void Start()
    {
    }

    void TakeDamage(float Damage, float GunLevel)
    {
    }

    // Update is called once per frame

    void Update()
    {
        //Get perspective rotation of camera
        CameraRotation = MainView.GetComponent<Transform>().localRotation.eulerAngles;

        Player.GetComponent<Transform>().Rotate(new Vector3(0, CameraRotation.y));

        if (Input.GetKey(KeyCode.W))
        {
            Player.transform.position += (Player.transform.rotation * new Vector3(0, 0, SPEED * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.A))
        {
            Player.transform.position += (Player.transform.rotation * new Vector3(-SPEED * Time.deltaTime, 0, 0));
        }

        if (Input.GetKey(KeyCode.S))
        {
            Player.transform.position += (Player.transform.rotation * new Vector3(0, 0, -SPEED * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.D))
        {
            Player.transform.position += (Player.transform.rotation * new Vector3(SPEED * Time.deltaTime, 0, 0));
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
        {
            MainView.transform.position -= (new Vector3(0, 0.5f, 0));
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
        {
            MainView.transform.position += (new Vector3(0, 0.5f, 0));
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SPEED = 7.5f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            SPEED = 5;
        }
    }
}
