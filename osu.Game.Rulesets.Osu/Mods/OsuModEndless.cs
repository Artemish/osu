// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Logging;
using osu.Framework.Utils;
using osu.Framework.Input.StateChanges;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Osu.Replays;
using osu.Game.Rulesets.UI;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Osu.Mods
{
    public class OsuModEndless : ModEndless
    {
      private BeatmapInfo selectRandomBeatmap(BeatmapManager beatmaps) {
        Logger.Log($"Calling selectRandomBeatmap PlayerLoader");
        // TODO properly filter by ruleset
        var results = beatmaps.QueryBeatmaps(b => b.Ruleset.ID == 0).OrderBy(info => info.OnlineBeatmapID).Skip(1).ToList();
        return results.ElementAt(RNG.Next(results.Count));
      }

      public override void ApplyToPlayerLoader(PlayerLoader playerLoader) {
        Logger.Log("Applying Endless mod to PlayerLoader");
        playerLoader.selectNextBeatmap = selectRandomBeatmap;
      }
    }
}
