// See https://aka.ms/new-console-template for more information


using RobotGame;

Manager manager = new Manager();

while (true)
{
    manager.DoTurn();
}

public class Manager
{
    private List<List<MapTile>> _gameBoard;
    private List<Bot> _bots = new List<Bot>();
    private readonly Int32 _gameHeight;
    private readonly Int32 _gameWidth;
    

    public Manager()
    {
        _gameHeight = 10;
        _gameWidth = 10;

        _gameBoard = Enumerable.Repeat(Enumerable.Repeat(MapTile.Empty, _gameHeight).ToList(), _gameWidth).ToList();
    }

    public Manager(Int32 gameWidth, Int32 gameHeight)
    {
        _gameHeight = gameHeight;
        _gameWidth = gameWidth;

        _gameBoard = Enumerable.Repeat(Enumerable.Repeat(MapTile.Empty, _gameHeight).ToList(), _gameWidth).ToList();
    }


    public void DoTurn()
    {
        CalculateMap(ref _gameBoard);

        foreach (Bot bot in _bots)
        {
            bot.CalculateAction(_gameBoard);
        }

        // Move
        foreach (Bot bot in _bots.Where(bot => bot.NextAction == Bot.Action.Move))
        {
            Move(bot);
        }

        List<Bot> revertBotLocation = new List<Bot>();
        foreach (Bot bot1 in _bots)
        {
            foreach (Bot bot2 in _bots.Where(bot2 => !bot1.Equals(bot2)))
            {
                if (bot1.X != bot2.X || bot1.Y != bot2.Y) continue;

                bot1.TakeDamage(1);

                revertBotLocation.Add(bot1);
            }
        }

        foreach (Bot bot in revertBotLocation)
        {
            bot.RevertMove();
        }

        // Attack
        foreach (Bot bot in _bots)
        {
            Bot? bot2;
            switch (bot.NextDirection)
            {
                case Bot.Direction.Left:
                    bot2 = _bots.Find(x => x.X == bot.X - 1 && x.Y == bot.Y);
                    break;
                case Bot.Direction.Up:
                    bot2 = _bots.Find(x => x.X == bot.X && x.Y == bot.Y - 1);
                    break;
                case Bot.Direction.Right:
                    bot2 = _bots.Find(x => x.X == bot.X + 1 && x.Y == bot.Y);
                    break;
                case Bot.Direction.Down:
                    bot2 = _bots.Find(x => x.X == bot.X - 1 && x.Y == bot.Y + 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            bot2?.TakeDamage(bot.ShotDamage);
        }

        // Self Destruct
        foreach (Bot bot in _bots)
        {
            if(bot.Health <= 0) continue;

            if (bot2 == null) continue;

            bot2.TakeDamage(bot.ShotDamage);
        }
    }

    private void CalculateMap(ref List<List<MapTile>> map)
    {
        foreach (Bot bot in _bots)
        {
            map[bot.X][bot.Y] = MapTile.Player1;
        }
    }

    private void Gaurd(Bot bot)
    {
        bot.NextAction = Bot.Action.Guard;
    }

    private void Move(Bot bot)
    {
        bot.LastX = bot.X;
        bot.LastY = bot.Y;

        switch (bot.NextDirection)
        {

            case Bot.Direction.Left:
                if (bot.X == 0)
                {
                    bot.X = _gameWidth;
                }

                bot.X -= 1;
                break;

            case Bot.Direction.Up:
                if (bot.Y == 0)
                {
                    bot.Y = _gameHeight;
                }
                bot.Y -= 1;
                break;

            case Bot.Direction.Right:
                if (bot.X == 0)
                {
                    bot.X = _gameWidth;
                }
                bot.X -= 1;
                break;

            case Bot.Direction.Down:
                if (bot.Y == 0)
                {
                    bot.Y = _gameHeight;
                }
                bot.Y -= 1;
                break;
        }
    }

    private void Attack(Bot bot)
    {

    }

    private void SelfDestruct()
    {

    }

    public enum Direction
    {
        Left,
        Up,
        Right,
        Down
    }

    public enum MapTile
    {
        Player1,
        Player2,
        Empty
    }
}
