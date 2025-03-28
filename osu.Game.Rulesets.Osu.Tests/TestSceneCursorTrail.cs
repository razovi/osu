// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Textures;
using osu.Framework.Testing.Input;
using osu.Game.Audio;
using osu.Game.Rulesets.Osu.Skinning.Legacy;
using osu.Game.Rulesets.Osu.UI.Cursor;
using osu.Game.Skinning;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Osu.Tests
{
    public class TestSceneCursorTrail : OsuTestScene
    {
        [Resolved]
        private IRenderer renderer { get; set; }

        [Test]
        public void TestSmoothCursorTrail()
        {
            Container scalingContainer = null;

            createTest(() => scalingContainer = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Child = new CursorTrail()
            });

            AddStep("set large scale", () => scalingContainer.Scale = new Vector2(10));
        }

        [Test]
        public void TestLegacySmoothCursorTrail()
        {
            createTest(() =>
            {
                var skinContainer = new LegacySkinContainer(renderer, false);
                var legacyCursorTrail = new LegacyCursorTrail(skinContainer);

                skinContainer.Child = legacyCursorTrail;

                return skinContainer;
            });
        }

        [Test]
        public void TestLegacyDisjointCursorTrail()
        {
            createTest(() =>
            {
                var skinContainer = new LegacySkinContainer(renderer, true);
                var legacyCursorTrail = new LegacyCursorTrail(skinContainer);

                skinContainer.Child = legacyCursorTrail;

                return skinContainer;
            });
        }

        private void createTest(Func<Drawable> createContent) => AddStep("create trail", () =>
        {
            Clear();

            Add(new Container
            {
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(0.8f),
                Child = new MovingCursorInputManager { Child = createContent?.Invoke() }
            });
        });

        [Cached(typeof(ISkinSource))]
        private class LegacySkinContainer : Container, ISkinSource
        {
            private readonly IRenderer renderer;
            private readonly bool disjoint;

            public LegacySkinContainer(IRenderer renderer, bool disjoint)
            {
                this.renderer = renderer;
                this.disjoint = disjoint;

                RelativeSizeAxes = Axes.Both;
            }

            public Drawable GetDrawableComponent(ISkinComponent component) => null;

            public Texture GetTexture(string componentName, WrapMode wrapModeS, WrapMode wrapModeT)
            {
                switch (componentName)
                {
                    case "cursortrail":
                        var tex = new Texture(renderer.WhitePixel);

                        if (disjoint)
                            tex.ScaleAdjust = 1 / 25f;
                        return tex;

                    case "cursormiddle":
                        return disjoint ? null : renderer.WhitePixel;
                }

                return null;
            }

            public ISample GetSample(ISampleInfo sampleInfo) => null;

            public IBindable<TValue> GetConfig<TLookup, TValue>(TLookup lookup) => null;

            public ISkin FindProvider(Func<ISkin, bool> lookupFunction) => lookupFunction(this) ? this : null;

            public IEnumerable<ISkin> AllSources => new[] { this };

            public event Action SourceChanged
            {
                add { }
                remove { }
            }
        }

        private class MovingCursorInputManager : ManualInputManager
        {
            public MovingCursorInputManager()
            {
                UseParentInput = false;
            }

            protected override void Update()
            {
                base.Update();

                const double spin_duration = 1000;
                double currentTime = Time.Current;

                double angle = (currentTime % spin_duration) / spin_duration * 2 * Math.PI;
                Vector2 rPos = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

                MoveMouseTo(ToScreenSpace(DrawSize / 2 + DrawSize / 3 * rPos));
            }
        }
    }
}
