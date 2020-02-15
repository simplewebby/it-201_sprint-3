using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDect : MonoBehaviour
{


    // Update is called once per frame
    void OnMouseDown()
    {
        Debug.Log(name + " was clicked and returned position is " + transform.position);

    }
}
