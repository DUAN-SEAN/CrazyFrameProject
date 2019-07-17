using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentInWorld : BodyInWorld
{
    public GameObject death_explosion;



    public void Destroy()
    {
        LogUI.Log("destory");
        Instantiate(death_explosion, transform.position, transform.rotation);
        LogUI.Log("explosion");
        Destroy(gameObject);
        LogUI.Log("gameover");
    }
   
}
