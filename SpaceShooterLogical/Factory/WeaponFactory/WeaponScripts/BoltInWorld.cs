using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltInWorld : BodyInWorld
{
    public GameObject explosion;
    public GameObject playerExplosion;
    public IAliveable aliveable; 
    public new void Update()
    {
        base.Update();
        if(aliveable == null) aliveable = m_body as IAliveable;
        if(!aliveable.GetAliveState())
        {

            Instantiate(explosion, this.gameObject.transform.position, this.gameObject.transform.rotation);
            //(playerExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }

    }
}
