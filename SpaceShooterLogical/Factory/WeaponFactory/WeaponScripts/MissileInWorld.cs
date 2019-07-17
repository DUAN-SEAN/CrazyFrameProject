using UnityEngine;
using UnityCollider = UnityEngine.Collider;
public class MissileInWorld : BodyInWorld
{
    public GameObject explosion;
    public IAliveable aliveable;
    public float SearchRadius;
    public new void Update()
    {
        base.Update();
        if (aliveable == null)  aliveable = m_body as IAliveable;
        if (!aliveable.GetAliveState())
        {
            LogUI.Log("missile destoried");
            UnityCollider[] colliders = Physics.OverlapSphere(m_transform.position, SearchRadius);

            if (colliders.Length <= 0)
                return;
            Instantiate(explosion, transform.position, transform.rotation);
            for (int i = 0; i < colliders.Length; i++)
            {
                //print(colliders[i].gameObject.name);
                //Destroy(colliders[i].gameObject);
            }
            Destroy(this.gameObject);
        }


    }
}
