﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.Replays.Legacy;
using osu.Game.Rulesets.Replays.Types;
using osu.Game.Rulesets.Scoring;
using OpenTK;

namespace osu.Game.Rulesets.Osu.Replays
{
    public class OsuReplayFrame : ReplayFrame, IConvertibleReplayFrame
    {
        public Vector2 Position;
        public List<OsuAction> Actions = new List<OsuAction>();

        public OsuReplayFrame()
            : base(0)
        {
        }

        public OsuReplayFrame(double time, Vector2 position, params OsuAction[] actions)
            : base(time)
        {
            Position = position;
            Actions.AddRange(actions);
        }

        public void ConvertFrom(LegacyReplayFrame legacyFrame, Score score, Beatmap beatmap)
        {
            Position = legacyFrame.Position;
            if (legacyFrame.MouseLeft) Actions.Add(OsuAction.LeftButton);
            if (legacyFrame.MouseRight) Actions.Add(OsuAction.RightButton);
        }
    }
}
