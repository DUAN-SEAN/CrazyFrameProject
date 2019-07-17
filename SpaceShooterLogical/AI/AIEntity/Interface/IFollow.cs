using System;
namespace SpaceShip.AI
{
    public interface IFollow
    {
        Body GetBody();
        Vector2 GetFollowPoint();
        bool TickResult();
    }
}

