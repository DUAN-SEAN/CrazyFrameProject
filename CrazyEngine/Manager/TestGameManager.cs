using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    private Engine engine;

    private Spaceship spaceship1;
    private Spaceship spaceship2;

    public GameObject cube;

    void Start()
    {
        if (engine == null)
            engine = new Engine();

        spaceship1 = new Spaceship();
        spaceship2 = new Spaceship();

        spaceship1.Collider = new Collider(new Vector2(1, 1));
        spaceship2.Collider = new Collider(new Vector2(1, 1), 1);

        World.Instanse.Bodies.Add(spaceship1);
        World.Instanse.Bodies.Add(spaceship2);
    }


    void Update()
    {



        if (Input.GetKey(KeyCode.Space))
        {
            spaceship1.AddForce(new Vector2(0, 1));
        }

        cube.transform.position = Vector3.Lerp(cube.transform.position, new Vector3(spaceship1.Position.x / 10, 0, spaceship1.Position.y / 10), Time.deltaTime * 20f);

    }

    public static void Printf(string s)
    {
        print(s);
    }

}
