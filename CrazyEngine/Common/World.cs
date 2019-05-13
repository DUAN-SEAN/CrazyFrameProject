using System;
using System.Collections.Generic;

public class World
{
    /// <summary>
    /// 世界里所有的物体
    /// </summary>
    public List<Body> Bodies;

    /// <summary>
    /// 世界的长
    /// </summary>
    public float WorldX = 1000f;
    /// <summary>
    /// 世界的宽
    /// </summary>
    public float WorldY = 1000f;

    private static World _world;


    public static World Instanse
    {
        get
        {
            if (_world == null)
                _world = new World();
            return _world;
        }
    }

    public World()
    {
        Bodies = new List<Body>();
    }

}
