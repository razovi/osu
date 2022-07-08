﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using NUnit.Framework;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Osu.Tests.Editor
{
    [TestFixture]
    public class TestSceneOsuEditor : EditorTestScene
    {
        protected override Ruleset CreateEditorRuleset() => new OsuRuleset();
    }
}
