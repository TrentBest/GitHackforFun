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
using System.Windows.Threading;

using TheSingularityWorkshop.FSM_API;
using TheSingularityWorkshop.FSM_Bridge.Helix;

using HelixToolkit.Wpf.SharpDX;

namespace FSM_WPF_MinesweeperFun
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public FSMHandle Status { get; private set; }
        private DispatcherTimer _fsmUpdateTimer;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            InitializeAppFSM();
         
            InitializeFSMUpdateLoop();
            
        }

        private void InitializeFSMUpdateLoop()
        {
            _fsmUpdateTimer = new DispatcherTimer();
            _fsmUpdateTimer.Interval = TimeSpan.FromMilliseconds(16); // Approximately 60 ticks per second
            _fsmUpdateTimer.Tick += FsmUpdateLoop;
            _fsmUpdateTimer.Start();
        }

        private void FsmUpdateLoop(object sender, EventArgs e)
        {
            // The FSM update logic
            FSM_API.Interaction.Update("Update");

            // Check for shutdown condition
            if (Status != null && !Status.IsValid)
            {
                _fsmUpdateTimer.Stop();
            }
        }

        private void InitializeAppFSM()
        {
            if (!FSM_API.Interaction.Exists("Minesweeper", "MainUpdate"))
            {
                FSM_API.Create.CreateFiniteStateMachine("Minesweeper", -1, "MainUpdate")
                    .State("Initialization", OnEnterInitialization, OnUpdateInitialization, OnExitInitialization)
                    .State("Running", OnEnterRunning, OnUpdateRunning, OnExitRunning)
                    .State("Shutdown", OnEnterShutdown, OnUpdateShutdown, OnExitShutdown)
                    .Transition("Initialization", "Running", WhenInitComplete)
                    .Transition("Running", "Shutdown", WhenShutdown)
                    .BuildDefinition();
            }
            var fsmAppContext = new FSM_AppContext() { IsValid = false, Name = "AppContext" };
            Status = FSM_API.Create.CreateInstance("Minesweeper", fsmAppContext, "Update");
            fsmAppContext.IsValid = true;
        }

        /// <summary>
        /// Initialize the minesweeper application
        ///     Create the visual grid lines
        ///     Create the MineField
        /// </summary>
        /// <param name="context"></param>
        private void OnEnterInitialization(IStateContext context)
        {
            if (context is FSM_AppContext appContext)
            {
                new MinesweeperFactoryFSM(appContext).CreateFSMs();
                appContext.MineField = FSM_API.Create.CreateInstance("MineField", 
                    new MineFieldStateContext(appContext.MineFieldWidth, 
                    appContext.MineFieldHeight), "Update");
            }
        }

        private void OnUpdateInitialization(IStateContext context)
        {
            if (context is FSM_AppContext appContext)
            {
               
            }
        }

        private void OnExitInitialization(IStateContext context)
        {
            if (context is FSM_AppContext appContext)
            {
               
            }
        }

        private void OnEnterRunning(IStateContext context)
        {
            if (context is FSM_AppContext appContext)
            {

            }
        }

        private void OnUpdateRunning(IStateContext context)
        {
            if (context is FSM_AppContext appContext)
            {
                FSM_API.Interaction.Update("Update");
            }
        }

        private void OnExitRunning(IStateContext context)
        {
            if (context is FSM_AppContext appContext)
            {
                foreach (var operationGroup in appContext.OperationSequence)
                {
                    FSM_API.Interaction.RemoveProcessingGroup(operationGroup);
                }
            }
        }

        private void OnEnterShutdown(IStateContext context)
        {
            if (context is FSM_AppContext appContext)
            {
                foreach (var shutdownGroup in appContext.ShutdownSequence)
                {
                    FSM_API.Create.CreateProcessingGroup(shutdownGroup);
                }
            }
        }

        private void OnUpdateShutdown(IStateContext context)
        {
            if (context is FSM_AppContext appContext)
            {
                foreach (var shutdownGroup in appContext.ShutdownSequence)
                {
                    FSM_API.Interaction.Update(shutdownGroup);
                }
            }
        }

        private void OnExitShutdown(IStateContext context)
        {
            if (context is FSM_AppContext appContext)
            {
                foreach (var shutdownGroup in appContext.ShutdownSequence)
                {
                    FSM_API.Interaction.RemoveProcessingGroup(shutdownGroup);
                }
            }
        }

        private bool WhenInitComplete(IStateContext context)
        {
            if (context is FSM_AppContext appContext)
            {
                foreach (var initGroup in appContext.InitializationSequence)
                {
                    if (FSM_API.Internal.GetFSMHandleCountInGroup(initGroup) > 0)
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
            if (context is FSM_AppContext appContext)
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
        public int MineFieldWidth { get; internal set; }
        public int MineFieldHeight { get; internal set; }
        public FSMHandle MineField { get; internal set; }

        public void Shutdown() { ShouldShutdown = true; }
    }
}