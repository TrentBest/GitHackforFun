using TheSingularityWorkshop.FSM_API;

namespace FSM_WPF_MinesweeperFun
{
    public class MinesweeperStateContext : IStateContext
    {
        public bool IsValid { get; set; } = true;
        public string Name { get; set; } = "MinesweeperContext";
    }
}