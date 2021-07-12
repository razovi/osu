// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Audio;
using osu.Framework.Testing;
using osu.Framework.Tests.Visual;
using osu.Framework.Utils;

namespace osu.Framework.Tests.Audio
{
    [HeadlessTest]
    public class TestSceneDrawableTrack : FrameworkTestScene
    {
        [Resolved]
        private ITrackStore trackStore { get; set; }

        private DrawableTrack track;

        [SetUp]
        public void Setup() => Schedule(() =>
        {
            Child = track = new DrawableTrack(trackStore.Get("sample-track"));
        });

        [Test]
        public void TestVolumeResetWhenReset()
        {
            AddStep("set volume to 0", () => track.Volume.Value = 0);
            AddAssert("track volume is 0", () => Precision.AlmostEquals(0, track.AggregateVolume.Value));

            AddStep("reset", () => track.Reset());
            AddAssert("track volume is 1", () => Precision.AlmostEquals(1, track.AggregateVolume.Value));
        }

        [Test]
        public void TestAdjustmentsRemovedWhenReset()
        {
            // ReSharper disable once NotAccessedVariable
            // Bindable - need to store the reference.
            BindableDouble volumeAdjustment = null;

            AddStep("add adjustment", () => track.AddAdjustment(AdjustableProperty.Volume, volumeAdjustment = new BindableDouble { Value = 0.5 }));
            AddAssert("track volume is 0.5", () => Precision.AlmostEquals(0.5, track.AggregateVolume.Value));

            AddStep("reset", () => track.Reset());
            AddAssert("track volume is 1", () => Precision.AlmostEquals(1, track.Volume.Value));
        }
    }
}
