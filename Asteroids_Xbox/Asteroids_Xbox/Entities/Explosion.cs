using Asteroids_Xbox.Manager;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Entities
{
    class Explosion : AnimatedEntity
    {
        private readonly EntityManager entityManager;
        private readonly string explosionTextureName;
        private Texture2D explosionTexture;
        public SoundEffect explodeSound;

        public Explosion(EntityManager entityManager, string explosionTextureName)
        {
            this.entityManager = entityManager;
            this.explosionTextureName = explosionTextureName;
        }

        public override void Load(ContentManager content)
        {
            explodeSound = content.Load<SoundEffect>("sound/explosion");
            explosionTexture = content.Load<Texture2D>(explosionTextureName + "_Animated_Trans1");

            Animation.Initialize(explosionTexture, Vector2.Zero,
                explosionTexture.Width / 16, explosionTexture.Height, 16, 60, Color.White, 1f, false);

            WrapScreen = true;
        }

        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            Rotate(5.0f);
            if (!Animation.ShouldDraw)
            {
                entityManager.Remove(this);
            }

            base.Update(inputManager, gameTime);
        }

        public void PlayExplosionSound()
        {
            explodeSound.Play();
        }
    }
}
