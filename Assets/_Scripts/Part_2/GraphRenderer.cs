using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Part_2
{
    public class GraphRenderer : MonoBehaviour
    {
        public LineRenderer lineRenderer;
        public float timeScale = 1.0f;   // Масштаб времени по оси X
        public float valueScale = 1.0f;  // Масштаб значений по оси Y

        public void DrawGraph(List<SharePointDataLoader.SensorDataPoint> dataPoints)
        {
            if (dataPoints == null || dataPoints.Count == 0 || lineRenderer == null)
            {
                Debug.LogWarning("No data or LineRenderer not assigned");
                return;
            }
            lineRenderer.enabled = true;
            // Отсортировать точки по времени
            var sorted = dataPoints.OrderBy(p => p.measuredAt).ToList();

            lineRenderer.positionCount = sorted.Count;

            DateTime baseTime = sorted[0].measuredAt;

            for (int i = 0; i < sorted.Count; i++)
            {
                float x = (float)(sorted[i].measuredAt - baseTime).TotalSeconds * timeScale;
                float y = sorted[i].value * valueScale;
                lineRenderer.SetPosition(i, new Vector3(x, y, 0));
            }
        }
    }
}