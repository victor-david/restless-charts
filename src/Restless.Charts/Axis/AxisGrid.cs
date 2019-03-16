using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Restless.Controls.Chart
{
    public class AxisGrid : Panel
    {
        #region Private
        private readonly ChartContainer owner;
        private readonly Path horzPath;
        private readonly Path vertPath;
        private GeometryGroup vertGeometryGroup;
        private GeometryGroup horzGeometryGroup;
        #endregion

        /************************************************************************/

        #region Public fields
        /// <summary>
        /// Gets the default brush for the grid lines.
        /// </summary>
        public static readonly Brush DefaultGridBrush = Axis.DefaultMinorTickBrush;

        /// <summary>
        /// Gets the default brush for the border of the grid axis.
        /// </summary>
        public static readonly Brush DefaultBorderBrush = Axis.DefaultMinorTickBrush;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes new instance of the <see cref="AxisGrid"/> class.
        /// </summary>
        internal AxisGrid(ChartContainer owner)
        {
            this.owner = owner ?? throw new ArgumentNullException(nameof(owner));

            owner.XAxis.MajorTickGeometryCreated += XAxisMajorTickGeometryCreated;
            owner.YAxis.MajorTickGeometryCreated += YAxisMajorTickGeometryCreated;

            horzPath = new Path()
            {
                Stroke = DefaultGridBrush,
                StrokeThickness = 1.0
            };

            vertPath = new Path()
            {
                Stroke = DefaultGridBrush,
                StrokeThickness = 1.0
            };

            Children.Add(horzPath);
            Children.Add(vertPath);
        }
        #endregion

        /************************************************************************/

        #region Brushes
        /// <summary>
        /// Gets or sets the brush for the axis grid lines.
        /// </summary>
        public Brush GridBrush
        {
            get => horzPath.Stroke;
            internal set
            {
                horzPath.Stroke = value;
                vertPath.Stroke = value;
            }
        }
        #endregion

        /************************************************************************/

        #region Protected Methods
        /// <summary>
        /// Positions child elements and determines a size for a AxisGrid
        /// </summary>
        /// <param name="finalSize">The final area within the parent that AxisGrid should use to arrange itself and its children</param>
        /// <returns>The actual size used</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in Children)
            {
                child.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            }
            return finalSize;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            if (sizeInfo.WidthChanged)
            {
                CreateHorizontalGeometry(horzGeometryGroup);
            }

            if (sizeInfo.HeightChanged)
            {
                CreateVerticalGeometry(vertGeometryGroup);
            }
            base.OnRenderSizeChanged(sizeInfo);
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void XAxisMajorTickGeometryCreated(object sender, GeometryGroup group)
        {
            vertGeometryGroup = group;
            CreateVerticalGeometry(group);
        }

        private void YAxisMajorTickGeometryCreated(object sender, GeometryGroup group)
        {
            horzGeometryGroup = group;
            CreateHorizontalGeometry(group);
        }

        private void CreateVerticalGeometry(GeometryGroup group)
        {
            if (group != null)
            {
                GeometryGroup vertGroup = new GeometryGroup();

                foreach (var item in group.Children.OfType<LineGeometry>())
                {
                    double x = item.StartPoint.X - owner.GridBorderSize;

                    LineGeometry line = new LineGeometry
                    {
                        StartPoint = new Point(x, 0),
                        EndPoint = new Point(x, ActualHeight)
                    };
                    vertGroup.Children.Add(line);
                }
                vertPath.Data = vertGroup;
            }
        }

        private void CreateHorizontalGeometry(GeometryGroup group)
        {
            if (group != null)
            {
                GeometryGroup horzGroup = new GeometryGroup();

                foreach (var item in group.Children.OfType<LineGeometry>())
                {
                    double y = item.StartPoint.Y - owner.GridBorderSize;

                    LineGeometry line = new LineGeometry
                    {
                        StartPoint = new Point(0, y),
                        EndPoint = new Point(ActualWidth, y)
                    };
                    horzGroup.Children.Add(line);
                }
                horzPath.Data = horzGroup;
            }
        }
        #endregion
    }
}
