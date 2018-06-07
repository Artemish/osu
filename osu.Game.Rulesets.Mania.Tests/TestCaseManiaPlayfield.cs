﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Timing;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Configuration;
using osu.Game.Rulesets.Mania.Beatmaps;
using osu.Game.Rulesets.Mania.Configuration;
using osu.Game.Rulesets.Mania.Judgements;
using osu.Game.Rulesets.Mania.Objects;
using osu.Game.Rulesets.Mania.Objects.Drawables;
using osu.Game.Rulesets.Mania.UI;
using osu.Game.Rulesets.Scoring;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Mania.Tests
{
    [TestFixture]
    public class TestCaseManiaPlayfield : OsuTestCase
    {
        private const double start_time = 500;
        private const double duration = 500;

        protected override double TimePerAction => 200;

        private RulesetInfo maniaRuleset;

        public TestCaseManiaPlayfield()
        {
            var rng = new Random(1337);

            AddStep("1 column", () => createPlayfield(1));
            AddStep("4 columns", () => createPlayfield(4));
            AddStep("5 columns", () => createPlayfield(5));
            AddStep("8 columns", () => createPlayfield(8));
            AddStep("4 + 4 columns", () =>
            {
                var stages = new List<StageDefinition>
                {
                    new StageDefinition { Columns = 4 },
                    new StageDefinition { Columns = 4 },
                };
                createPlayfield(stages);
            });

            AddStep("2 + 4 + 2 columns", () =>
            {
                var stages = new List<StageDefinition>
                {
                    new StageDefinition { Columns = 2 },
                    new StageDefinition { Columns = 4 },
                    new StageDefinition { Columns = 2 },
                };
                createPlayfield(stages);
            });

            AddStep("1 + 8 + 1 columns", () =>
            {
                var stages = new List<StageDefinition>
                {
                    new StageDefinition { Columns = 1 },
                    new StageDefinition { Columns = 8 },
                    new StageDefinition { Columns = 1 },
                };
                createPlayfield(stages);
            });

            AddStep("Reversed", () => createPlayfield(4, true));

            AddStep("Notes with input", () => createPlayfieldWithNotes());
            AddStep("Notes with input (reversed)", () => createPlayfieldWithNotes(true));
            AddStep("Notes with gravity", () => createPlayfieldWithNotes());
            AddStep("Notes with gravity (reversed)", () => createPlayfieldWithNotes(true));

            AddStep("Hit explosion", () =>
            {
                var playfield = createPlayfield(4);

                int col = rng.Next(0, 4);

                var note = new Note { Column = col };
                note.ApplyDefaults(new ControlPointInfo(), new BeatmapDifficulty());

                var drawableNote = new DrawableNote(note, ManiaAction.Key1)
                {
                    AccentColour = playfield.Columns.ElementAt(col).AccentColour
                };

                playfield.OnJudgement(drawableNote, new ManiaJudgement { Result = HitResult.Perfect });
                playfield.Columns[col].OnJudgement(drawableNote, new ManiaJudgement { Result = HitResult.Perfect });
            });
        }

        [BackgroundDependencyLoader]
        private void load(RulesetStore rulesets, SettingsStore settings)
        {
            maniaRuleset = rulesets.GetRuleset(3);

            Dependencies.Cache(new ManiaConfigManager(settings, maniaRuleset, 4));
        }

        private ManiaPlayfield createPlayfield(int cols, bool inverted = false)
        {
            var stages = new List<StageDefinition>
            {
                new StageDefinition { Columns = cols },
            };

            return createPlayfield(stages, inverted);
        }

        private ManiaPlayfield createPlayfield(List<StageDefinition> stages, bool inverted = false)
        {
            Clear();

            var inputManager = new ManiaInputManager(maniaRuleset, stages.Sum(g => g.Columns)) { RelativeSizeAxes = Axes.Both };
            Add(inputManager);

            ManiaPlayfield playfield;

            inputManager.Add(playfield = new ManiaPlayfield(stages)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            });

            return playfield;
        }

        private void createPlayfieldWithNotes(bool inverted = false)
        {
            Clear();

            var rateAdjustClock = new StopwatchClock(true) { Rate = 1 };

            var inputManager = new ManiaInputManager(maniaRuleset, 4) { RelativeSizeAxes = Axes.Both };
            Add(inputManager);

            ManiaPlayfield playfield;
            var stages = new List<StageDefinition>
            {
                new StageDefinition { Columns = 4 },
            };

            inputManager.Add(playfield = new ManiaPlayfield(stages)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Clock = new FramedClock(rateAdjustClock)
            });

            for (double t = start_time; t <= start_time + duration; t += 100)
            {
                var note1 = new Note { StartTime = t, Column = 0 };
                var note2 = new Note { StartTime = t, Column = 3 };

                note1.ApplyDefaults(new ControlPointInfo(), new BeatmapDifficulty());
                note2.ApplyDefaults(new ControlPointInfo(), new BeatmapDifficulty());

                playfield.Add(new DrawableNote(note1, ManiaAction.Key1));
                playfield.Add(new DrawableNote(note2, ManiaAction.Key4));
            }

            var holdNote1 = new HoldNote { StartTime = start_time, Duration = duration, Column = 1 };
            var holdNote2 = new HoldNote { StartTime = start_time, Duration = duration, Column = 2 };

            holdNote1.ApplyDefaults(new ControlPointInfo(), new BeatmapDifficulty());
            holdNote2.ApplyDefaults(new ControlPointInfo(), new BeatmapDifficulty());

            playfield.Add(new DrawableHoldNote(holdNote1, ManiaAction.Key2));
            playfield.Add(new DrawableHoldNote(holdNote2, ManiaAction.Key3));
        }
    }
}
