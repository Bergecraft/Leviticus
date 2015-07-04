using Assets.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Entities.Modules
{
    public class SpriteBehaviour<T> : DefinitionBehaviour<T> where T : SpriteDef
    {
        public void Start()
        {                
            if (def != null)
            {
                BuildSprite();
            }
        }
        protected void BuildSprite()
        {
            if (gameObject.GetComponent<SpriteRenderer>() == null)
            {
                gameObject.AddComponent<SpriteRenderer>();
            }
            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>(def.spritePath);
            spriteRenderer.color = new Color(def.spriteColor.r, def.spriteColor.g, def.spriteColor.b, def.spriteColor.a);
        }
    }
}
