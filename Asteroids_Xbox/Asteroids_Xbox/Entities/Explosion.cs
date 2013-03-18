using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Asteroids_Xbox.Entities
{
    class Explosion : AnimatedEntity
    {
        private Texture2D explosionTexture;
        private string explosionTextureName;
        public SoundEffect explodeSound;

        public Explosion(string explosionTextureName)
        {
            this.explosionTextureName = explosionTextureName;
        }

        public override void Load(ContentManager content)
        {
            explodeSound = content.Load<SoundEffect>("sound/explosion");
            explosionTexture = content.Load<Texture2D>(explosionTextureName + "_Animated_Trans1");
            Animation.Initialize(explosionTexture, Vector2.Zero,
                explosionTexture.Width / 16, explosionTexture.Height, 16, 60, Color.White, 1f, true);

            WrapScreen = true;
        }

        public override void Update(Manager.InputManager inputManager, GameTime gameTime)
        {
            Rotate(5.0f);
            base.Update(inputManager, gameTime);
        }
    }
}
