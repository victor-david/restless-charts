using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restless.Controls.Chart
{
    public interface IDataConnector
    {
        void OnDataSeriesChanged(ChartBase chart);
    }
}
