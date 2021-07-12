// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osuTK;

namespace osu.Framework.Platform
{
    public abstract class OsuTKGameHost : GameHost
    {
        private readonly Toolkit toolkit;

        protected OsuTKGameHost()
        {
            toolkit = Toolkit.Init();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            toolkit?.Dispose();
        }
    }
}
