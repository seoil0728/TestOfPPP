using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform iceGround;
    void Start()
    {
        Transform[] ices = iceGround.GetComponentsInChildren<Transform>();
        foreach (Transform t in ices)
        {
            t.transform.AddComponent<IceAction_KSW>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
