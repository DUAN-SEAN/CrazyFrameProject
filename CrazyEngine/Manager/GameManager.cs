using System.Collections;
using System.Collections.Generic;
public class GameManager
{
    private Engine engine;

    private Spaceship spaceship1;
    private Spaceship spaceship2;



    void Start()
    {
        if (engine == null)
            engine = new Engine();

        spaceship1 = new Spaceship();
        spaceship2 = new Spaceship();

        spaceship1.Collider = new Rectangle(new Vector2(0,0), new Vector2(1,1));
        spaceship2.Collider = new Circle(new Vector2(5,5), 1);


        World.Instanse.Bodies.Add(spaceship1);
        World.Instanse.Bodies.Add(spaceship2);
        //spaceship1.Collider = new Point(0, 0);
        //spaceship1.Collider = new Point(1, 1);
    }


    void Update()
    {
        foreach (IAnimatable animatable in TimerManager.timerList)
        {
            animatable.AdvanceTime();
        }


    }

    public static void printS(string s)
    {
        //print(s);
    }

}
