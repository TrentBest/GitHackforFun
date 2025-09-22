using System.Collections.Generic;

using TheSingularityWorkshop.FSM_API;

namespace FSM_WPF_MinesweeperFun
{
    public class MinesweeperFactoryFSM : IFactoryFSM
    {
        private FSM_AppContext context;
        public MinesweeperFactoryFSM(FSM_AppContext appContext)
        {
            context = appContext;
            Define();
        }

        public void Define()
        {
            if (!FSM_API.Interaction.Exists("GUIAppFSM", "GUI"))
            {
                // ToDo: Make this a real GUI FSM in the FSM_Bridge.WPF
                FSM_API.Create.CreateProcessingGroup("GUI");
                FSM_API.Create.CreateFiniteStateMachine("GUIAppFSM", 0, "GUI")
                    .State("Hidden", null, null, null)
                    .State("Visible", null, null, null)
                    .WithInitialState("Hidden")
                    .Transition("Hidden", "Visible", FSM_App_Behavior.IsVisible)
                    .Transition("Visible", "Hidden", FSM_App_Behavior.IsHidden)
                    .BuildDefinition();

                // FSM for the color picker panel's buttons
                FSM_API.Create.CreateFiniteStateMachine("GuiButtonFSM", 0, "GUI")
                    .State("Normal", null, null, null)
                    .State("Pressed", null, null, null)
                    .Transition("Normal", "Pressed", FSM_Gui_Behavior.IsPressed)
                    .Transition("Pressed", "Normal", FSM_Gui_Behavior.IsNotPressed)
                    .BuildDefinition();

                // FSM for the text input fields
                FSM_API.Create.CreateFiniteStateMachine("GuiTextInputFieldFSM", 0, "GUI")
                    .State("Inactive", null, null, null)
                    .State("Active", null, null, null)
                    .Transition("Inactive", "Active", FSM_Gui_Behavior.IsActive)
                    .Transition("Active", "Inactive", FSM_Gui_Behavior.IsInactive)
                    .BuildDefinition();
            }

            if (!FSM_API.Interaction.Exists("PlayerFSM", "Gameplay"))
            {
                FSM_API.Create.CreateProcessingGroup("Gameplay");
                FSM_API.Create.CreateFiniteStateMachine("PlayerFSM", 0, "Gameplay")
                    .State("Playing", null, null, null)
                    .State("GameOver", null, null, null)
                    .State("Win", null, null, null)
                    .BuildDefinition();
            }

            if (!FSM_API.Interaction.Exists("BoardFSM", "Gameplay"))
            {
                FSM_API.Create.CreateFiniteStateMachine("BoardFSM", 0, "Gameplay")
                    .State("Inactive", null, null, null)
                    .State("Active", null, null, null)
                    .BuildDefinition();
            }

            if (!FSM_API.Interaction.Exists("TileFSM", "Gameplay"))
            {
                FSM_API.Create.CreateFiniteStateMachine("TileFSM", 0, "Gameplay")
                    .State("Covered", null, null, null)
                    .State("Revealed", null, null, null)
                    .State("Flagged", null, null, null)
                    .State("Detonated", null, null, null )
                    .BuildDefinition();
            }

            // ToDo: This needs to be a list of these, not a single one.
            //FSM_API.FSM_API.Create.CreateInstance("GUIAppFSM", GuiAppFSM, "GUI");
            //FSM_API.FSM_API.Create.CreateInstance("GuiButtonFSM", GuiAppFSM, "GUI");
            //FSM_API.FSM_API.Create.CreateInstance("GuiTextInputFieldFSM", GuiAppFSM, "GUI");
            //FSM_API.FSM_API.Create.CreateInstance("GuiTextInputFieldFSM", GuiAppFSM, "GUI");

            FSM_API.Create.CreateInstance("PlayerFSM", context, "Gameplay");
            FSM_API.Create.CreateInstance("BoardFSM", context, "Gameplay");
            // FSM_API.FSM_API.Create.CreateInstance("TileFSM", context, "Gameplay");
        }

        private static class FSM_App_Behavior
        {
            public static bool IsVisible(IStateContext context) { return false; }
            public static bool IsHidden(IStateContext context) { return false; }
        }

        private static class FSM_Gui_Behavior
        {
            public static bool IsPressed(IStateContext context) { return false; }
            public static bool IsNotPressed(IStateContext context) { return false; }
            public static bool IsActive(IStateContext context) { return false; }
            public static bool IsInactive(IStateContext context) { return false; }
        }

        public void CreateFSMs()
        {
            Define();
        }
    }
}