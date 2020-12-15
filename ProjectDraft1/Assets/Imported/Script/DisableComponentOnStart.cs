using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableComponentOnStart : MonoBehaviour
{
    void Start()
    {
        this.gameObject.SetActive(false);
    }
}
