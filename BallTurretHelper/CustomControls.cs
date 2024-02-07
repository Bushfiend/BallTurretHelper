using Entities.Blocks;
using Sandbox.ModAPI.Interfaces.Terminal;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities;
using System.Reflection;
using HarmonyLib;
using Sandbox.Game.Entities.Cube.CubeBuilder;
using VRageMath;
using Sandbox.Game.Gui;
using System.Reflection.Emit;
using Sandbox.Common.ObjectBuilders.Definitions;

namespace BallTurretHelper
{

    public class CustomControls
    {
       
     
        private static void Action(MyMotorStator block)
        {
            AddSmallPart(block);
        }

         private static readonly FieldInfo CubeBuilderState =
             typeof(MyCubeBuilder).GetField("m_cubeBuilderState", BindingFlags.Instance | BindingFlags.NonPublic);
         public static void ClearBuilder()
         {
             var cubeBuilderState = (MyCubeBuilderState)CubeBuilderState.GetValue(MyCubeBuilder.Static);
             cubeBuilderState.CurrentBlockDefinition = null;
         }
         public static void AddSmallPart(IMyTerminalBlock block)
         {

            var motorBase = (MyMotorBase)block;

            ClearBuilder();

            var def = MyDefinitionManager.Static.GetCubeBlockDefinition(MyDefinitionId.Parse("MyObjectBuilder_MotorAdvancedRotor/SmallHingeHead"));

            if (def == null)
                return;

            var matrix = MatrixD.CreateWorld(Vector3D.Transform(motorBase.DummyPosition, motorBase.CubeGrid.WorldMatrix), motorBase.WorldMatrix.Forward, motorBase.WorldMatrix.Up);

            double offsetAmount = 0.4;
            var offsetMatrix = MatrixD.CreateTranslation(new Vector3D(0, offsetAmount, 0));
            matrix = MatrixD.Multiply(offsetMatrix, matrix);


            MyCubeBuilder.Static.AddBlocksToBuildQueueOrSpawn(def, ref matrix, Vector3I.Zero, Vector3I.Zero,
                Vector3I.Zero, Quaternion.Zero);

            AttachLoop.MotorBase = motorBase;
         }


        [HarmonyPatch(typeof(MyMotorStator), "CreateTerminalControls")]
        class CreateTerminalControlsPatchPrefix
        {
            static void Prefix()
            {
                if (!MyTerminalControlFactory.AreControlsCreated<MyMotorStator>())
                    _controlsCreated = false;           
            }
        }

        [HarmonyPatch(typeof(MyMotorStator), "CreateTerminalControls")]
         class CreateTerminalControlsPatch
        {
             static void Postfix()
             {
                if (!MyTerminalControlFactory.AreControlsCreated<MyMotorStator>())
                    _controlsCreated = false;

                if (MyTerminalControlFactory.AreControlsCreated<MyMotorStator>() && _controlsCreated)
                    return;

                var title = MyStringId.GetOrCompute("Add Very Small Head");
                var tooltip = MyStringId.GetOrCompute("Add Very Small Head.");
                MyTerminalControlButton<MyMotorStator> controlButton = new MyTerminalControlButton<MyMotorStator>("AddVerySmallHingeTopPart", title, tooltip, Action);
                controlButton.Enabled = (MyMotorStator b) => b.TopBlock == null;
                controlButton.Visible = (MyMotorStator b) => b.MotorDefinition.RotorType == MyRotorType.Hinge;
                MyTerminalControlFactory.AddControl<MyMotorStator>(15, controlButton);
                _controlsCreated = true;


            }
         }
        private static bool _controlsCreated;
    }
}
