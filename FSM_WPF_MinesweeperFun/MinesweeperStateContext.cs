using HelixToolkit.Wpf.SharpDX;

using TheSingularityWorkshop.FSM_API;

namespace FSM_WPF_MinesweeperFun
{
    public class MinesweeperStateContext : IStateContext
    {
        public bool IsValid { get; set; } = true;
        public string Name { get; set; } = "MinesweeperContext";
        public FSMHandle MineField { get; private set; }
        public FSMHandle Status { get; private set; }

        public MinesweeperStateContext(FSMHandle mineFieldHandle)
        {
            Status = FSM_API.Create.CreateInstance("Minesweeper", this, "Update");
        }

        internal Geometry3D GenerateGridLines()
        {
            var geometry = new LineGeometry3D();
            MineFieldStateContext mineFieldContext = (MineFieldStateContext)MineField.Context;
            int width = mineFieldContext.Width;
            int height = mineFieldContext.Height;

            // Draw horizontal lines
            for (int y = 0; y <= height; y++)
            {
                geometry.Positions.Add(new global::SharpDX.Vector3(0, y, 0));
                geometry.Positions.Add(new global::SharpDX.Vector3(width, y, 0));
            }
            // Draw vertical lines
            for (int x = 0; x <= width; x++)
            {
                geometry.Positions.Add(new global::SharpDX.Vector3(x, 0, 0));
                geometry.Positions.Add(new global::SharpDX.Vector3(x, height, 0));
            }

            return geometry;
        }

        public void SetMineField(FSMHandle mineFieldHandle)
        {
            MineField = mineFieldHandle;
        }
    }
}