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
using Sandbox.Game.Entities.Cube.CubeBuilder;
using VRageMath;

namespace BallTurretHelper
{
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]

    public class CustomControls : MySessionComponentBase
    {
        public override void LoadData()
        {
            MyAPIGateway.TerminalControls.CustomControlGetter -= InsertControl;
            MyAPIGateway.TerminalControls.CustomControlGetter += InsertControl;
        }
        protected override void UnloadData()
        {
            MyAPIGateway.TerminalControls.CustomControlGetter -= InsertControl;
        }

        public static void InsertControl(IMyTerminalBlock block, List<IMyTerminalControl> controls)
        {

            if (!(block is IMyMechanicalConnectionBlock))
                return;

            var myTerminalControlButton = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, IMyMotorAdvancedStator>("ExtraSmallHinge");
            myTerminalControlButton.Title = MyStringId.GetOrCompute("Add (Very) Small Part");
            myTerminalControlButton.Tooltip = MyStringId.GetOrCompute("Adds The Smallest Hinge Part, Perfect For Ball Turrets!.");
            myTerminalControlButton.SupportsMultipleBlocks = false;
            myTerminalControlButton.Action = Action;
            myTerminalControlButton.Enabled = Enable;
            myTerminalControlButton.Visible = Visible;
            controls.Insert(13, myTerminalControlButton);
        }

        private static bool Visible(IMyTerminalBlock block)
        {
            MyDefinitionId hingeId;
            MyDefinitionId.TryParse("MyObjectBuilder_MotorAdvancedStator/LargeHinge", out hingeId);
            return (MyDefinitionId)block.BlockDefinition == hingeId;
        }


        private static bool Enable(IMyTerminalBlock block)
        {
            var motorBase = block as IMyMotorBase;

            return motorBase.Top == null;
        }

        private static void Action(IMyTerminalBlock block)
        {
            if (block is MyMotorBase)
            {
                var motorBase = block as MyMotorBase;

                if (motorBase.TopBlock != null)
                    return;

                ClearBuilder();

                var def = MyDefinitionManager.Static.GetCubeBlockDefinition(MyDefinitionId.Parse("MyObjectBuilder_MotorAdvancedRotor/SmallHingeHead"));

                var matrix = MatrixD.CreateWorld(Vector3D.Transform(motorBase.DummyPosition, motorBase.CubeGrid.WorldMatrix), motorBase.WorldMatrix.Forward, motorBase.WorldMatrix.Up);

                double offsetAmount = 0.4;
                var offsetMatrix = MatrixD.CreateTranslation(new Vector3D(0, offsetAmount, 0));
                matrix = MatrixD.Multiply(offsetMatrix, matrix);


                MyCubeBuilder.Static.AddBlocksToBuildQueueOrSpawn(def, ref matrix, Vector3I.Zero, Vector3I.Zero,
                    Vector3I.Zero, Quaternion.Zero);

                AttachLoop.MotorBase = motorBase;
            }

        }

        private static readonly FieldInfo CubeBuilderState =
            typeof(MyCubeBuilder).GetField("m_cubeBuilderState", BindingFlags.Instance | BindingFlags.NonPublic);
        public static void ClearBuilder()
        {
            var cubeBuilderState = (MyCubeBuilderState)CubeBuilderState.GetValue(MyCubeBuilder.Static);
            cubeBuilderState.CurrentBlockDefinition = null;
        }


    }
}
