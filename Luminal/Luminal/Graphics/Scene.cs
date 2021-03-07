﻿using Luminal.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luminal.Graphics
{
    public class Scene
    {
        public virtual void Update(Engine main, float deltaTime)
        {
            // It's an update function.
        }

        public virtual void Draw(Engine main)
        {
            // Do things here. This is going to get overridden.
        }

        public virtual void OnEnter()
        {
            // Called when scene entered.
        }

        public virtual void OnExit()
        {
            // Called when scene exited.
        }

        public virtual void OnKeyDown(Engine main)
        {
            // Called when key is pressed.
        }

        public virtual void OnKeyUp(Engine main)
        {
            // Called when key is released.
        }
    }
}
