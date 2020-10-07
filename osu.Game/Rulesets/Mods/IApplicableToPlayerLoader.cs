// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Mods
{
    /// <summary>
    /// An interface for a mod which can temporarily override the <see cref="PlayerLoader"/> settings.
    /// </summary>
    public interface IApplicableToPlayerLoader : IApplicableMod
    {
        void ApplyToPlayerLoader(PlayerLoader playerLoader);
    }
}
