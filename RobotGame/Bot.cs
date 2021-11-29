using System.Drawing;

namespace RobotGame;

public class Bot
{
    public Int32 X;
    public Int32 Y;

    public Int32 LastX;
    public Int32 LastY;

    public Int32 ShotDamage;
    public Int32 SelfDestructDmg;

    public Action NextAction;
    public Direction NextDirection;

    public Int32 Health;

    public Bot(Int32 x, Int32 y, Int32 health = 10, Int32 shotDmg = 2, Int32 selfDestructDmg = 3)
    {
        X = x;
        Y = y;
        Health = health;
        ShotDamage = shotDmg;
        SelfDestructDmg = selfDestructDmg;
    }

    public void CalculateAction(List<List<Manager.MapTile>> map)
    {
        // Do something


    }

    public Alive TakeDamage(Int32 dmgToInflict)
    {
        if (NextAction == Action.Guard)
        {
            Health -= (Int32)Math.Floor(new Decimal(dmgToInflict / 2));
            return Health <= 0 ? Alive.Dead : Alive.Alive;
        }

        Health -= dmgToInflict;
        return Health <= 0 ? Alive.Dead : Alive.Alive;

    }

    public void RevertMove()
    {
        X = LastX;
        Y = LastY;
    }
    public enum Action
    {
        Move,
        Guard,
        Attack,
        SelfDestruct
    }

    public enum Direction
    {
        Left,
        Up,
        Right,
        Down,
    }

    public enum Alive
    {
        Alive,
        Dead,
    }
}