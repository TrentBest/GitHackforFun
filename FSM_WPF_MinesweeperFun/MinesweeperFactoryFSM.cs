using TheSingularityWorkshop.FSM_API;

namespace FSM_WPF_MinesweeperFun
{
    internal class MinesweeperFactoryFSM : IFactoryFSM
    {
        private FSM_AppContext appContext;

        public MinesweeperFactoryFSM(FSM_AppContext appContext)
        {
            this.appContext = appContext;
        }

        public void CreateFSMs()
        {
            FSM_API.Create.CreateProcessingGroup("PlayerGroup");
            FSM_API.Create.CreateProcessingGroup("TilesGroup");
            FSM_API.Create.CreateProcessingGroup("BoardGroup");

            FSM_API.Create.CreateFiniteStateMachine("PlayerFSM", -1, "PlayerGroup")
                .State("AwaitingInput", PlayerFSM.OnEnterAwaitingInput, PlayerFSM.OnUpdateAwaitingInput, PlayerFSM.OnExitAwaitingInput)
                .State("SelectingCell", PlayerFSM.OnEnterSelectingCell, PlayerFSM.OnUpdateSelectingCell, PlayerFSM.OnExitSelectingCell)
                .State("PlacingFlag", PlayerFSM.OnEnterPlacingFlag, PlayerFSM.OnUpdatePlacingFlag, PlayerFSM.OnExitPlacingFlag)
                .State("Moving", PlayerFSM.OnEnterMoving, PlayerFSM.OnUpdateMoving, PlayerFSM.OnExitMoving)
                .Transition("AwaitingInput", "SelectingCell", PlayerFSM.WhenSelectCell)
                .Transition("SelectingCell", "AwaitingInput", PlayerFSM.WhenNotSelectCell)
                .Transition("AwaitingInput", "PlacingFlag", PlayerFSM.WhenPlaceFlag)
                .Transition("PlacingFlag", "AwaitingInput", PlayerFSM.WhenNotPlaceFlag)
                .Transition("AwaitingInput", "Moving", PlayerFSM.WhenMove)
                .Transition("Moving", "AwaitingInput", PlayerFSM.WhenNotMove)
                .BuildDefinition();

            FSM_API.Create.CreateFiniteStateMachine("TileFSM", 0, "TilesGroup")
                .State("Unrevealed", TileFSM.OnEnterUnrevealed, TileFSM.OnUpdateUnrevealed, TileFSM.OnExitUnrevealed)
                .State("Flagged", TileFSM.OnEnterFlagged, TileFSM.OnUpdateFlagged, TileFSM.OnExitFlagged)
                .State("Revealed", TileFSM.OnEnterRevealed, TileFSM.OnUpdateRevealed, TileFSM.OnExitRevealed)
                .BuildDefinition();

            FSM_API.Create.CreateFiniteStateMachine("BoardFSM", -1, "BoardGroup")
                .State("Idle", BoardFSM.OnEnterIdle, BoardFSM.OnUpdateIdle, BoardFSM.OnExitIdle)
                .State("Flagging", BoardFSM.OnEnterFlagging, BoardFSM.OnUpdateFlagging, BoardFSM.OnExitFlagging)
                .State("Revealing", BoardFSM.OnEnterRevealing, BoardFSM.OnUpdateRevealing, BoardFSM.OnExitRevealing)
                .BuildDefinition();

            FSM_API.Create.CreateFiniteStateMachine("UserInterfaceFSM", -1, "UIGroup")
                .State("Normal", GuiAppFSM.OnEnterNormal, GUIAppFSM.OnUpdateNormal, GUIAppFSM.OnExitNormal)
                .State("MouseOver", GuiAppFSM.OnEnterMouseOver, GUIAppFSM.OnUpdateMouseOver, GUIAppFSM.OnExitMouseOver)
                .State("Disabled", GuiAppFSM.OnEnterDisabled, GUIAppFSM.OnUpdateDisabled, GUIAppFSM.OnExitDisabled)
                .State("ReadOnly", GuiAppFSM.OnEnterReadOnly, GUIAppFSM.OnUpdateReadOnly, GUIAppFSM.OnExitReadOnly)

                .BuildDefinition();

            FSM_API.Create.CreateFiniteStateMachine("TextInputFieldFSM", 0, "UIGroup")
                .State("Idle", GuiTextInputFieldFSM.OnEnterIdle, GuiTextInputFieldFSM.OnUpdateIdle, GuiTextInputFieldFSM.OnExitIdle)
                .State("Focused", GuiTextInputFieldFSM.OnEnterFocused, GuiTextInputFieldFSM.OnUpdateFocused, GuiTextInputFieldFSM.OnExitFocused)
                .State("Modified", GuiTextInputFieldFSM.OnEnterModified, GuiTextInputFieldFSM.OnUpdateModified, GuiTextInputFieldFSM.OnExitModified)
                .BuildDefinition();

            FSM_API.Create.CreateFiniteStateMachine("ButtonFSM", 0, "UIGroup")
                .State("Idle", GuiButtonFSM.OnEnterIdle, GuiButtonFSM.OnUpdateIdle, GuiButtonFSM.OnExitIdle)
                .State("MouseOver", GuiButtonFSM.OnEnterMouseOver, GuiButtonFSM.OnUpdateMouseOver, GuiButtonFSM.OnExitMouseOver)
                .State("Pressed", GuiButtonFSM.OnEnterPressed, GuiButtonFSM.OnUpdatePressed, GuiButtonFSM.OnExitPressed)
                .BuildDefinition();

        }
    }
}