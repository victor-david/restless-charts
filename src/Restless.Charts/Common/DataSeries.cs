using System;
using System.Collections.Generic;

namespace Restless.Controls.Chart
{
    public class DataSeries
    {
        private readonly List<DataPoint> storage;

        public DataSeries()
        {
            storage = new List<DataPoint>();
            DataRange = DataRange.EmptyDataRange();
            Clear();
        }

        internal DataRange DataRange
        {
            get;
        }

        public void Add(DataPoint item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            DataRange.Surround(item.XValue, item.YValue);
            storage.Add(item);
        }

        public void Clear()
        {
            storage.Clear();
            DataRange.X = Range.EmptyRange();
            DataRange.Y = Range.EmptyRange();
        }

        public IEnumerable<DataPoint> EnumerateData()
        {
            foreach (DataPoint dataPoint in storage)
            {
                yield return dataPoint;
            }
        }

        public IEnumerable<double> EnumerateX()
        {
            foreach (DataPoint dataPoint in storage)
            {
                yield return dataPoint.XValue;
            }
        }

        public IEnumerable<double> EnumerateY()
        {
            foreach (DataPoint dataPoint in storage)
            {
                yield return dataPoint.YValue;
            }
        }
    }
}
