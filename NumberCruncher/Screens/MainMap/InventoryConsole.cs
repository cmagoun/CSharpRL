using NumberCruncher.Components;
using SadSharp.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberCruncher.Screens.MainMap
{
    public class InventoryConsole : GameConsole
    {
        private IGameData _gameData;

        public override string MyKey => "INVENTORY_CONSOLE";

        public InventoryConsole(IGameData gameData) : base(18, 30, 0, 0) 
        {
            _gameData = gameData;
        }

        public override void Draw(TimeSpan timeElapsed)
        {
            Children.Clear();

            var inventory = _gameData.Ecs.Get<InventoryComponent>(Program.Player);
            var items = inventory.Items.Values.OrderBy(item => item.Key);

            var y = 0;

            foreach(var item in items)
            {
                Children.Add(new Label(item.Display, 0, y));
                y = y + 2;
            }

            base.Draw(timeElapsed);
        }
    }
}
