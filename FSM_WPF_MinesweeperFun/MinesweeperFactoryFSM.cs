using HelixToolkit.Wpf.SharpDX;

using TheSingularityWorkshop.FSM_API;
using TheSingularityWorkshop.FSM_Bridge.Helix;



namespace FSM_WPF_MinesweeperFun
{
    public class MinesweeperFactoryFSM : IFactoryFSM
    {
        private FSM_AppContext _appContext;

        public FSMHandle MinefieldGridHandle { get; private set; }

        public MinesweeperFactoryFSM(FSM_AppContext appContext)
        {
            _appContext = appContext;
        }

        public void CreateFSMs()
        {
            // You are creating the FSM definition and the instance in one step
            if (!FSM_API.Interaction.Exists("MineField", "Update"))
            {
                FSM_API.Create.CreateFiniteStateMachine("MineFIeld", -1, "Update")
                    .State("Init", OnEnterInit, null, null)
                    .State("GenerateBoard", OnEnterGenerateBoard, null, null)
                    .Transition("Init", "GenerateBoard", WhenShouldGenerateBoard)
                    .Transition("GenerateBoard", "Init", WhenBoardIsGenerated)
                    .BuildDefinition();


            }
            if (!FSM_API.Interaction.Exists("Player", "Update"))
            {
                FSM_API.Create.CreateFiniteStateMachine("Player", -1, "Update")
                    .State("", null, null, null)
                    .BuildDefinition();
            }
            if (!FSM_API.Interaction.Exists("MineFieldCell", "Update"))
            {
                FSM_API.Create.CreateFiniteStateMachine("MineFieldCell", 0, "Update")
                    .State("Unrevealed", OnEnterUnrevealed, null, null)
                    .State("Flagged", OnEnterFlagged, null, null)
                    .State("Revealed", OnEnterRevealed, null, null)
                    .BuildDefinition();
            }
        }

        private void OnEnterRevealed(IStateContext context)
        {
            throw new NotImplementedException();
        }

        private void OnEnterFlagged(IStateContext context)
        {
            throw new NotImplementedException();
        }

        private void OnEnterUnrevealed(IStateContext context)
        {
            throw new NotImplementedException();
        }

        private void OnEnterInit(IStateContext context) { }

        private void OnEnterGenerateBoard(IStateContext context)
        {
            if (context is MinesweeperStateContext minesweeperContext)
            {
                //Our initial context is still Helix... 
                LineGeometryModel3D gridLines = new LineGeometryModel3D
                {
                    Color = System.Windows.Media.Colors.LightGray,
                    Thickness = 1,
                    IsHitTestVisible = false,
                    Geometry = minesweeperContext.GenerateGridLines()
                };
                LineGeometryModel3D_FSM gridFSM = new LineGeometryModel3D_FSM(gridLines, "Grid");
                MinefieldGridHandle = FSM_API.Create.CreateInstance("MineFieldGrid", gridFSM, "Update");

            }
        }

        private bool WhenShouldGenerateBoard(IStateContext context)
        {
            if (context is MinesweeperStateContext minesweeper)
            {
                return minesweeper.MineField.CurrentState == "";//ToDo fill in string
            }
            return true;
        }

        private bool WhenBoardIsGenerated(IStateContext context)
        {
            if (context is MinesweeperStateContext mc)
            {
                return mc.MineField.CurrentState == "";
            }
            return false;
        }
    }
}