using System;
using UnityEngine;
public class PlayerShipInWorld : ShipInWorld
{
    //控制摄像机一定比例跟随
    public bool isFixedFollowing;
    public Transform main_camera;
    public float width_ratio;
    public float height_ratio;


    protected new void Start()
    {
        base.Start();

        main_camera = Camera.main.transform;
        //main_camera.position = new Vector3(test.position.x, main_camera.position.y, test.position.z);
        width_ratio = Mathf.Clamp((float)(Screen.width * 0.3), 10, 20);
        height_ratio = Mathf.Clamp((float)(Screen.height * 0.3), 10, 15);
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        //控制摄像机跟随
        if (GamePlayerLogic.Instance.m_ship != m_ship) main_camera = null;


        if (main_camera == null) return;
        if (isFixedFollowing)
        {
            main_camera.position = new Vector3(test.position.x, main_camera.position.y, test.position.z);
            return;
        }
        //LogUI.Log("main camera set");

        //float x_posi = main_camera.position.x - test.position.x;
        //float z_posi = main_camera.position.z - test.position.z;
        //x_posi = Mathf.Abs(x_differ) > width_ratio / 2 ? x_differ < 0 ? test.position.x - width_ratio / 2 : width_ratio / 2 + test.position.x : main_camera.position.x;
        //z_posi = Mathf.Abs(z_differ) > height_ratio / 2 ? z_differ < 0 ? test.position.y - height_ratio / 2 : height_ratio / 2 + test.position.y : main_camera.position.z;

        main_camera.position = new Vector3(transform.position.x + x_differ, transform.position.y + y_differ, transform.position.z + z_differ);
        main_camera.LookAt(transform);
    }
}
