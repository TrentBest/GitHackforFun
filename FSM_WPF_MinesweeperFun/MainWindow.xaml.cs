using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TheSingularityWorkshop.FSM_API;

namespace FSM_WPF_MinesweeperFun
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public FSMHandle Status { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            InitializeAppFSM();
            var fsmAppContext = new FSM_AppContext() { IsValid = false, Name = "AppContext" };
            Status = FSM_API.Create.CreateInstance("Peer2Peer:CustomArchitecture", fsmAppContext, "Update");
            fsmAppContext.IsValid = true;

            do
            {
                FSM_API.Interaction.Update("Update");//Main driver of the FSM ticks
            } while (Status != null && Status.IsValid);
        }


        private void InitializeAppFSM()
        {
            if (!FSM_API.Interaction.Exists("Peer2Peer:CustomArchitecture", "Update"))
            {
                FSM_API.Create.CreateFiniteStateMachine("Peer2Peer:CustomArchitecture", -1, "Update")
                    .State("Initialization", OnEnterInitialization, OnUpdateInitialization, OnExitInitialization)
                    .State("Running", OnEnterRunning, OnUpdateRunning, OnExitRunning)
                    .State("Shutdown", OnEnterShutdown, OnUpdateShutdown, OnExitShutdown)
                    .Transition("Initialization", "Running", WhenInitComplete)
                    .Transition("Running", "Shutdown", WhenShutdown)
                    .BuildDefinition();
            }
            
        }

        private void OnEnterInitialization(IStateContext context)
        {
            if (context is FSM_AppContext appContext)
            { 
                IFactoryFSM factory = new MinesweeperFactoryFSM(appContext);
                factory.CreateFSMs();
                foreach(var initGroup in appContext.InitializationSequence)
                {
                    FSM_API.Create.CreateProcessingGroup(initGroup);
                }        
            }
        }

        private void OnUpdateInitialization(IStateContext context)
        {
            if(context is FSM_AppContext appContext)
            {
                foreach(var initGroup in appContext.InitializationSequence)
                {
                    FSM_API.Interaction.Update(initGroup);
                }
            }
        }

        private void OnExitInitialization(IStateContext context)
        {
            if(context is FSM_AppContext appContext)
            {
                foreach(var initGroup in appContext.InitializationSequence)
                {
                    FSM_API.Interaction.RemoveProcessingGroup(initGroup);
                }
            }
        }

        private void OnEnterRunning(IStateContext context)
        {
            if(context is FSM_AppContext appContext)
            {
               
                foreach (var operationGroup in appContext.OperationSequence)
                {
                    FSM_API.Create.CreateProcessingGroup(operationGroup);
                }
            }
        }

        private void OnUpdateRunning(IStateContext context)
        {
            if(context is FSM_AppContext appContext)
            {
                foreach(var operationGroup in appContext.OperationSequence)
                {
                    FSM_API.Interaction.Update(operationGroup);
                }
            }
        }

        private void OnExitRunning(IStateContext context)
        {
            if(context is FSM_AppContext appContext)
            {
                foreach (var operationGroup in appContext.OperationSequence)
                {
                    FSM_API.Interaction.RemoveProcessingGroup(operationGroup);
                }
            }
        }

        private void OnEnterShutdown(IStateContext context)
        {
            if(context is FSM_AppContext appContext)
            {
                foreach(var shutdownGroup in appContext.ShutdownSequence)
                {
                    FSM_API.Create.CreateProcessingGroup(shutdownGroup);
                }
            }
        }

        private void OnUpdateShutdown(IStateContext context)
        {
            if(context is FSM_AppContext appContext)
            {
                foreach(var shutdownGroup in appContext.ShutdownSequence)
                {
                    FSM_API.Interaction.Update(shutdownGroup);
                }
            }
        }

        private void OnExitShutdown(IStateContext context)
        {
            if(context is FSM_AppContext appContext)
            {
                foreach(var shutdownGroup in appContext.ShutdownSequence)
                {
                    FSM_API.Interaction.RemoveProcessingGroup(shutdownGroup);
                }
            }
        }

        private bool WhenInitComplete(IStateContext context)
        {
            if(context is FSM_AppContext appContext)
            {
                foreach(var initGroup in appContext.InitializationSequence)
                {
                    if(FSM_API.Internal.GetFSMHandleCountInGroup(initGroup) > 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private bool WhenShutdown(IStateContext context)
        {
            if(context is FSM_AppContext appContext)
            {
                return appContext.ShouldShutdown;
            }
            return false;

        }
    }

    public class FSM_AppContext : IStateContext
    {
        public bool IsValid { get; set; } = false;
        public string Name { get; set; }
        public List<string> InitializationSequence { get; internal set; } = new List<string>() { "Init" };
        public List<string> ShutdownSequence { get; internal set; } = new List<string>() { "Shutdown" };
        public List<string> OperationSequence { get; internal set; } = new List<string>() { "MainUpdate" };
        public bool ShouldShutdown { get; internal set; } = false;

        public void Shutdown() { ShouldShutdown = true; }
    }
}