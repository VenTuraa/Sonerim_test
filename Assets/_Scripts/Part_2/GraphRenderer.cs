using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using XCharts.Runtime;

namespace Part_2
{
    public class GraphRenderer : MonoBehaviour
    {
        [SerializeField] private LineChart _lineChart;

        public void DrawGraph(List<SensorDataPoint> dataPoints)
        {
            if (dataPoints == null || dataPoints.Count == 0 || _lineChart == null)
            {
                Debug.LogWarning("No data or LineChart not assigned");
                return;
            }

            _lineChart.gameObject.SetActive(true);
            var sorted = dataPoints.OrderBy(p => p.measuredAt).ToList();
            _lineChart.EnsureChartComponent<Title>().text = "Boiler Chart";
            foreach (SensorDataPoint t in sorted)
            {
                float y = t.value;
                _lineChart.AddXAxisData(t.measuredAt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture));
                _lineChart.AddYAxisData(y.ToString());
                _lineChart.AddData(0, y);
            }
        }
    }
}