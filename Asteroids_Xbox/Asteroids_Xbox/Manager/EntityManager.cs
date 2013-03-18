using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using Asteroids_Xbox.Entities;

namespace Asteroids_Xbox.Manager
{
    class EntityManager
    {
        private readonly List<Entity> entities = new List<Entity>();
        private readonly List<AnimatedEntity> animatedEntities = new List<AnimatedEntity>();
        private readonly ContentManager contentManager;
        private readonly GraphicsDevice graphicsDevice;

        public EntityManager(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            this.contentManager = contentManager;
            this.graphicsDevice = graphicsDevice;
        }

        public void Add(Entity entity)
        {
            if (!entity.Initialized)
            {
                entity.Initialize(contentManager, graphicsDevice);
            }

            entities.Add(entity);

            if (entity is AnimatedEntity)
            {
                animatedEntities.Add((AnimatedEntity)entity);
            }
        }

        public void Remove(Entity entity)
        {
            entities.Remove(entity);

            if (entity is AnimatedEntity)
            {
                animatedEntities.Remove((AnimatedEntity)entity);
            }
        }

        public void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
        {
            foreach (var entity in entities)
            {
                entity.Initialize(content, graphicsDevice);
            }
        }

        public void Update(InputManager inputManager, GameTime gameTime)
        {
            // We need a copy, because the list may change while iterating
            // over the entities
            var copy = entities.ToList();
            foreach (var entity in copy)
            {
                entity.Update(inputManager, gameTime);
            }

            CheckCollisions(animatedEntities);
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (var entity in entities)
            {
                entity.Draw(batch);
            }
        }

        private void CheckCollisions(List<AnimatedEntity> entities)
        {
            var copy = entities.ToList();
            foreach (var entity in copy)
            {
                foreach (var other in copy)
                {// comment
                    if (entity is Bullet && other is Asteroid)
                    {
                        var collision = entity.CheckCollision(other);
                        if (collision)
                        {
                            entity.Touch(other);
                            // TODO: Handle collision
                            Debug.WriteLine("Collision: " + collision.ToString() + " - " + other.ToString());
                        }    
                    }
                }
            }
        }
    }
}
