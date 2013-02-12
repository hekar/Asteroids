using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Asteroids_Xbox.Manager
{
    class EntityManager
    {
        private List<Entity> entities;

        public EntityManager()
        {
            entities = new List<Entity>();
        }

        public void Add(Entity entity)
        {
            entities.Add(entity);
        }

        public void Remove(Entity entity)
        {
            entities.Remove(entity);
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
            foreach (var entity in entities)
            {
                entity.Update(inputManager, gameTime);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (var entity in entities)
            {
                entity.Draw(batch);
            }
        }        
    }
}
