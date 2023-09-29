using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Cube.CubeBuilder;
using Sandbox.ModAPI;
using VRage.Game;
using VRageMath;

namespace BallTurretHelper
{
    public static class AttachLoop
    {
        private static int _ticks = 0;

        public static MyMechanicalConnectionBlockBase MotorBase;

        private static readonly MethodInfo CallAttachMethod = typeof(MyMechanicalConnectionBlockBase).GetMethod("CallAttach",
            BindingFlags.NonPublic | BindingFlags.Instance);


        public static void CallAttach(MyMechanicalConnectionBlockBase block)
        {
            if (CallAttachMethod != null)
            {
                CallAttachMethod.Invoke(block, new object[] { });
            }
                
        }

        public static void Update()
        {
            if (MotorBase == null)
                return;

            if (MotorBase.TopBlock != null)
            {
                MotorBase = null;
                _ticks = 0;
                return;
            }

            if (_ticks < 4)
            {
                _ticks++;
                CallAttach(MotorBase);
                return;
            }

            
            _ticks = 0;
            MotorBase = null;
        }



    }
}
