// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Utils;
using osu.Framework.Logging;
using osu.Game.Graphics;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Mods
{
    public class ModEndless : Mod, IApplicableToPlayerLoader, IApplicableToPlayer
    {
        public override string Name => "Endless";
        public override string Acronym => "ES";
        public override IconUsage? Icon => OsuIcon.ModSpunout;
        public override ModType Type => ModType.Fun;
        public override string Description => @"Placeholder for Endless";
        public override double ScoreMultiplier => 1.0;
        public override bool Ranked => true;
        // public override Type[] IncompatibleMods => new[] { };

        public virtual void ApplyToPlayerLoader(PlayerLoader playerLoader)
        {
          Logger.Log("Applying Endless mod to PlayerLoader");
          return;
        }

        public void ApplyToPlayer(Player player)
        {
          Logger.Log("Applying Endless mod to Player");
          player.showResults = false;
          return;
        }
    }
}
