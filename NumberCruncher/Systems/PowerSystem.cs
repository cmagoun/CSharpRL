using NumberCruncher.Screens.MainMap;
using NumberCruncher.Systems.Powerups;
using SadSharp.Powers;
using System;
using System.Collections.Generic;

namespace NumberCruncher.Systems
{
    public static class PowerSystem
    {
        public static MoveResult ActivatePower(string entityId, Powerup power, IGameData data)
        {
            return MoveResult.Done();
        }

        public static Dictionary<string, Powerup> AllItems = new Dictionary<string, Powerup>
        {
            {"B", new BlinkPowerup() }
        };
    }
    public abstract class Powerup
    {
        public string Name { get; }
        public string Display { get; }
        public string Key { get; } //what to press to use
        public abstract MoveResult Activate(string activator, IGameData gameData);
        public abstract Func<IGameData, ITargeter> Targeter { get; }


        public Powerup(string key, string name, string display)
        {
            Key = key;
            Name = name;
            Display = display;
        }

        //when pressed, we fire teh keyed power?
    }
}
