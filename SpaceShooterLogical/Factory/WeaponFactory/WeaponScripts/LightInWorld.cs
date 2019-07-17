using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightInWorld : BodyInWorld
{
    private IAliveable m_body_alive;
    private Line Line;
    public new void Update()
    {
        if (m_body == null) return;
        if (m_transform == null) m_transform = transform;
        if (m_body_alive == null) m_body_alive = m_body as IAliveable;
        
        //与物理世界同步位置
        Vector2 posi = GetPosition();
        //test.position = Vector3.Lerp(test.position, new Vector3(posi.x,0,posi.y), Time.deltaTime * 5f);
        Vector2 forward = GetForward().normalized;
        Line = (Line)m_body.Collider.collider;
        m_transform.position = new Vector3(posi.x - forward.x * Line.Length/2, 0, posi.y - forward.y * Line.Length/2);
        //与物理世界同步朝向
        m_transform.forward = new Vector3(forward.x, 0, forward.y);

        if (!m_body_alive.GetAliveState()) 
        {
            //LogUI.Log("dead");
            Destroy(this.gameObject);
        }
         if (isAwaked) return;
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        if (!m_body.Enable) m_body.Enable = true;
        isAwaked = true;

    }
}
