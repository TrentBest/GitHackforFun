using TheSingularityWorkshop.FSM_API;

namespace FSM_WPF_MinesweeperFun
{
    internal class MineFieldStateContext : IStateContext
    {
        public int Width { get; internal set; } = 10;
        public int Height { get; internal set; } = 10;
        public bool IsValid { get; set; } = false;
        public string Name { get; set; } = "MineFieldContext";

        public MineFieldStateContext(int width, int height)
        {
            Width = width;
            Height = height;
            IsValid = true;
        }
    }
}