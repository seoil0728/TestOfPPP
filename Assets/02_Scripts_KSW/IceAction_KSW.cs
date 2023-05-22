using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IceAction_KSW : MonoBehaviourPun
{
    int life;
    public Material[] mats;
    MeshCollider mesh;
    MeshRenderer render;
    bool isBreakable;

    // Start is called before the first frame update
    void Start()
    {
        life = 3;
        mesh = GetComponent<MeshCollider>();
        render = GetComponent<MeshRenderer>();
        isBreakable = false;
    }

    // Update is called once per frame
    void Update()
    {
        LifeCheck();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Picker") && isBreakable)
        {
            other.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(CoolSet(other));
            life--;
            print("맞아부러써");
        }
    }
    void LifeCheck()
    {
        if (life == 0)
        {
            mesh.enabled = false;
            render.enabled = false;
        }
    }

    public void Reset()
    {
        life = 3;
        mesh.enabled = true;
        render.enabled = true;
    }

    public void LifeZero()
    {
        life = 0;
    }
    public void NotBreak()
    {
        isBreakable = false;
    }
    public void CanBreak()
    {
        isBreakable = true;
    }

    public int GetLife()
    {
        return life;
    }

    IEnumerator CoolSet(Collider other)
    {
        yield return new WaitForSeconds(0.73f);
        other.GetComponent<BoxCollider>().enabled = true;
    }
}
