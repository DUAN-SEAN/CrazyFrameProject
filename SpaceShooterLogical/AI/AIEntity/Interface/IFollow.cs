using System;
public interface IFollow
{
    Body GetBody();
    Vector2 GetFollowPoint();
    bool TickResult();
}
