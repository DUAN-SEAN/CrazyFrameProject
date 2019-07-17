using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCollider = UnityEngine.Collider;
public class MineInWorld : BodyInWorld
{
    public GameObject explosion;
    public IAliveable aliveable;
    public float SearchRadius;
    private int flag = 0;      //控制时间只检测一次
    public new void Update()
    {
        base.Update();
        if(aliveable == null) aliveable = m_body as IAliveable;

        if (!aliveable.GetAliveState())
        {
             if (Time.time > mineTime + 2 && flag == 0)
            {
                UnityCollider[] colliders = Physics.OverlapSphere(m_transform.position, SearchRadius);
               
                if (colliders.Length <= 0)
                    return;
                Instantiate(explosion, transform.position, transform.rotation);
                for (int i = 0; i < colliders.Length; i++)  //摧毁范围内所有的物体
                {
                    //print(colliders[i].gameObject.name);
                    //Destroy(colliders[i].gameObject);     
                }
                Destroy(this.gameObject);
                flag = 1;
            }
        }
    }
}
