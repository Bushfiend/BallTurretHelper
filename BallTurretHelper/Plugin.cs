using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using VRage.Plugins;

namespace BallTurretHelper
{
    public class Plugin : IPlugin, IDisposable
    {
        public static Harmony harmony;
        public void Init(object gameInstance)
        {
            harmony = new Harmony("Helper");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
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
