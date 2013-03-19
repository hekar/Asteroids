using System.Collections.Generic;
using System.Linq;
using Asteroids_Xbox.Entities;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Manager
{
    /// <summary>
    /// Container for all the in game entities
    /// </summary>
    class EntityManager
    {
        private readonly ContentManager contentManager;
        private readonly GraphicsDevice graphicsDevice;

        /// <summary>
        /// All the entities that are currently in the game
        /// </summary>
        private readonly List<Entity> entities = new List<Entity>();

        /// <summary>
        /// All the animated entities that are currently in the game
        /// </summary>
        private readonly List<AnimatedEntity> animatedEntities = new List<AnimatedEntity>();

        public EntityManager(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            this.contentManager = contentManager;
            this.graphicsDevice = graphicsDevice;
        }

        /// <summary>
        /// Add an entity
        /// </summary>
        /// <param name="entity"></param>
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

        /// <summary>
        /// Remove an entity
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(Entity entity)
        {
            entities.Remove(entity);

            if (entity is AnimatedEntity)
            {
                animatedEntities.Remove((AnimatedEntity)entity);
            }
        }

        /// <summary>
        /// Remove all entities from the entity manager
        /// </summary>
        public void Clear()
        {
            entities.Clear();
            animatedEntities.Clear();
        }

        /// <summary>
        /// Intialize all entities
        /// </summary>
        /// <param name="content"></param>
        /// <param name="graphicsDevice"></param>
        public void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
        {
            foreach (var entity in entities)
            {
                entity.Initialize(content, graphicsDevice);
            }
        }

        /// <summary>
        /// Handle updates in the gameloop for all entities
        /// </summary>
        /// <param name="inputManager"></param>
        /// <param name="gameTime"></param>
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

        /// <summary>
        /// Draw all entities
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(SpriteBatch batch)
        {
            foreach (var entity in entities)
            {
                entity.Draw(batch);
            }
        }

        /// <summary>
        /// Check collisions between entities
        /// </summary>
        /// <param name="entities"></param>
        private void CheckCollisions(List<AnimatedEntity> entities)
        {
            var copy = entities.ToList();
            foreach (var entity in copy)
            {
                foreach (var other in copy)
                {
                    if (entity is Bullet && other is Asteroid ||
                        entity is Bullet && other is EnemyShip ||
                        entity is Asteroid && other is Player ||
                        entity is EnemyBullet && other is Asteroid ||
                        entity is EnemyBullet && other is Player)
                    {
                        var collision = entity.CheckCollision(other);
                        if (collision)
                        {
                            entity.Touch(other);
                        }    
                    }
                }
            }
        }

        public List<Entity> List()
        {
            return entities;
        }
    }
}
