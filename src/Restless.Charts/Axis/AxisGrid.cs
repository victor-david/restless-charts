using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Restless.Controls.Chart
{
    /// <summary>
    /// Represents a panel that displays horizontal and vertical grid lines
    /// inside the chart area.
    /// </summary>
    public class AxisGrid : Panel
    {
        #region Private
        private readonly ChartContainer owner;
        private readonly Path horzPath;
        private readonly Path vertPath;
        private double minEdgeDistance;
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

            minEdgeDistance = 3.0;
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

        /// <summary>
        /// Gets or sets the minimum distance that a grid line can be from the edge.
        /// This value is clamped between 1.0 and 8.0. The default is 3.0
        /// </summary>
        public double MinEdgeDistance
        {
            get => minEdgeDistance;
            set
            {
                value = value.Clamp(1.0, 8.0);
                if (value != minEdgeDistance)
                {
                    minEdgeDistance = value;
                    InvalidateMeasure();
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Protected Methods
        /// <summary>
        /// Measures the size in layout required for child elements and determines a size this element.
        /// </summary>
        /// <param name="availableSize">
        /// The available size that this element can give to child elements.
        /// Infinity can be specified as a value to indicate that the element will size to whatever content is available.
        /// </param>
        /// <returns>The size that this element determines it needs during layout, based on its calculations of child element sizes.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            Size desiredSize = new Size
                (
                    availableSize.Width.IsFinite() ? availableSize.Width : 128,
                    availableSize.Height.IsFinite() ? availableSize.Height : 128
                );

            if (owner.Orientation == Orientation.Vertical)
            {
                CreateVerticalGeometry(owner.XAxis.MajorTickCoordinates, desiredSize);
                CreateHorizontalGeometry(owner.YAxis.MajorTickCoordinates, desiredSize);
            }
            else
            {
                CreateVerticalGeometry(owner.YAxis.MajorTickCoordinates, desiredSize);
                CreateHorizontalGeometry(owner.XAxis.MajorTickCoordinates, desiredSize);
            }

            foreach (UIElement child in Children)
            {
                child.Measure(desiredSize);
            }

            return desiredSize;
        }

        /// <summary>
        /// Positions children of this element.
        /// </summary>
        /// <param name="finalSize">The final area within the parent this element should use to arrange itself and its children.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in Children)
            {
                child.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            }
            return finalSize;
        }
        #endregion

        /************************************************************************/

        #region Private methods

        private void CreateVerticalGeometry(TickCoordinateCollection tickCoordinates, Size size)
        {
            GeometryGroup vertGroup = new GeometryGroup();

            foreach (double tickCoordinate in tickCoordinates)
            {
                double x = tickCoordinate - owner.GridBorderSize;

                // don't make grid lines too close to the edge
                if (x > MinEdgeDistance && x < size.Width - MinEdgeDistance)
                {
                    LineGeometry line = new LineGeometry
                    {
                        StartPoint = new Point(x, 0),
                        EndPoint = new Point(x, size.Height)
                    };
                    vertGroup.Children.Add(line);
                }
            }
            vertPath.Data = vertGroup;
        }

        private void CreateHorizontalGeometry(TickCoordinateCollection tickCoordinates, Size size)
        {
            GeometryGroup horzGroup = new GeometryGroup();

            foreach (double tickCoordinate in tickCoordinates)
            {
                double y = tickCoordinate - owner.GridBorderSize;

                // don't make grid lines too close to the edge
                if (y > MinEdgeDistance && y < size.Height - MinEdgeDistance)
                {
                    LineGeometry line = new LineGeometry
                    {
                        StartPoint = new Point(0, y),
                        EndPoint = new Point(size.Width, y)
                    };

                    horzGroup.Children.Add(line);
                }
            }
            horzPath.Data = horzGroup;
        }
        #endregion
    }
}