using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coindeleter : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.name == "Ethan") {
            this.gameObject.SetActive(false);
        }
        
    }
}
