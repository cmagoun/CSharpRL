using CsEcs;
using NumberCruncher.Components;
using RogueSharp;
using SadSharp.MapCreators;
using System;
using System.Collections.Generic;
using System.Text;

namespace NumberCruncher.Systems
{
    public static class FovSystem
    {
        public static FieldOfView<RogueCell> UpdatePlayerFov(Ecs ecs, Map<RogueCell>terrain)
        {
            var ppos = ecs.Get<SadWrapperComponent>(Program.Player);
            var fov = new FieldOfView<RogueCell>(terrain);
            fov.ComputeFov(ppos.X, ppos.Y, 20, false);
            return fov;
        }
    }
}
