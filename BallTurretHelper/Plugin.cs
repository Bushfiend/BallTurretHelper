using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Game.World;
using VRage.Plugins;

namespace BallTurretHelper
{
    public class Plugin : IPlugin, IDisposable
    {

        public void Init(object gameInstance)
        {

        }


        public void Update()
        {
            if (MySession.Static == null)
                return;

            AttachLoop.Update();
        }


        public void Dispose()
        {
           

        }

    }
}
