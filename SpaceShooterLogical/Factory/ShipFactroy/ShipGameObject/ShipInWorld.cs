using UnityEngine;

//用于控制自己的飞船
abstract public class ShipInWorld : MonoBehaviour
{
    public Transform test;
    public Body m_ship;

   
    public GameObject death_explosion;

    public float x_differ;
    public float z_differ;
    public float y_differ;

    protected Vector2 forward_old = default;
    // Start is called before the first frame update
    protected void Start()
    {
      
        //unity世界生成
        test = transform;
        test.position = new Vector3(10f, 0f, 10f);

        //摄像机初始化
        x_differ = -10;
        y_differ = 60;
    }


   

    protected void FixedUpdate()
    {
     

        //与物理世界同步位置
        Vector2 posi = GetPosition();
        //test.position = Vector3.Lerp(test.position, new Vector3(posi.x,0,posi.y), Time.deltaTime * 5f);
        test.position = new Vector3(posi.x, 0, posi.y);
        //与物理世界同步朝向
       
        Vector2 forward = GetForward();

        
            
            if(((ShipBase)m_ship).isRight)//往右转
            {
                transform.rotation = Quaternion.Euler(0, 0, -30);
                print("往右转");
            }

           if (((ShipBase)m_ship).isLeft)
            {
            transform.rotation = Quaternion.Euler(0, 0, 30);
                print("往左转");
            }

            //GetComponent<Rigidbody>().rotation = Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);

           
        
        //forward_old = forward;

        test.forward = new Vector3(forward.x,0,forward.y);

       

    }

    public Vector2 GetPosition()
    {
        if (m_ship == null) return new Vector2();
        return new Vector2(m_ship.Position.x, m_ship.Position.y);

    }

    public Vector2 GetForward()
    {
        if (m_ship == null) return new Vector2();
        return m_ship.Forward;
    }

    public void Destroy()
    {
        LogUI.Log("destory");
        Instantiate(death_explosion,transform.position,transform.rotation);
        LogUI.Log("explosion");
        Destroy(gameObject);
        LogUI.Log("gameover");
    }
}
