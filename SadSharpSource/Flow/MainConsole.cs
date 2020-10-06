namespace SadSharp.Game
{
    //Instantiated in ConsoleManager to act as parent for all other consoles
    public class MainConsole:GameConsole
    {
        public override string MyKey => "MAIN";
        public MainConsole(int width, int height) : base(width, height, 0, 0)
        {
        }

    }
}
