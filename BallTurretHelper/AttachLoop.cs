using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Game.Entities.Blocks;

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

            if (_ticks < 5)
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
