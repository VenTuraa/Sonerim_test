using System;
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
        public void DrawGraph(List<SharePointDataLoader.SensorDataPoint> dataPoints)
        {
     
            if (dataPoints == null || dataPoints.Count == 0 || _lineChart == null)
            {
                Debug.LogWarning("No data or LineRenderer not assigned");
                return;
            }
            _lineChart.gameObject.SetActive(true);
            var sorted = dataPoints.OrderBy(p => p.measuredAt).ToList();

            for (int i = 0; i < sorted.Count; i++)
            {
                var y = sorted[i].value ;
                _lineChart.AddXAxisData(sorted[i].measuredAt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture));
                _lineChart.AddYAxisData(y.ToString());
                _lineChart.AddData(0,y);
            }
        }
    }
}