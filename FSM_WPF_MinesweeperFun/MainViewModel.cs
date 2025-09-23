using CommunityToolkit.Mvvm.ComponentModel;

using HelixToolkit.Maths;
using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX;

using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

using Camera = HelixToolkit.Wpf.SharpDX.Camera;
using PerspectiveCamera = HelixToolkit.Wpf.SharpDX.PerspectiveCamera;
using Vector3 = System.Numerics.Vector3;

namespace FSM_WPF_MinesweeperFun
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private EffectsManager effectsManager;

        [ObservableProperty]
        private Camera camera;

        [ObservableProperty]
        private LineGeometry3D grid;

        [ObservableProperty]
        private Transform3D gridTransform;

        [ObservableProperty]
        private Color ambientLightColor = Colors.DimGray;

        [ObservableProperty]
        private Color directionalLightColor = Colors.White;

        [ObservableProperty]
        private Vector3D directionalLightDirection = new(-2, -5, -2);

        public MainViewModel()
        {
            EffectsManager = new DefaultEffectsManager();

            Camera = new PerspectiveCamera
            {
                Position = new Point3D(10, 10, 10),
                LookDirection = new Vector3D(-10, -10, -10),
                FieldOfView = 60
            };

            // This creates the grid geometry
            var builder = new LineBuilder();
            builder.AddGrid(Vector3.Zero, Vector3.UnitX, Vector3.UnitZ, 20, 20, 1, 1);
            Grid = builder.ToLineGeometry3D();

            // This places the grid at y=0, which is your "virtual ground"
            GridTransform = new TranslateTransform3D(0, 0, 0);
        }
    }
}