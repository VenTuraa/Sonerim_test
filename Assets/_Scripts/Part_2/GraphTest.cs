using UnityEngine;
using System;
using System.Collections.Generic;
using Part_2;

public class GraphTest : MonoBehaviour
{
    public GraphRenderer graphRenderer; // Ссылка на ваш GraphRenderer

    void Start()
    {
        // Генерация тестовых данных
        List<SharePointDataLoader.SensorDataPoint> testData = GenerateTestData();

        // Вызов метода для отрисовки графика
        graphRenderer.DrawGraph(testData);
    }

    List<SharePointDataLoader.SensorDataPoint> GenerateTestData()
    {
        List<SharePointDataLoader.SensorDataPoint> data = new List<SharePointDataLoader.SensorDataPoint>();

        DateTime startTime = DateTime.Now;

        // Генерация данных с интервалом в 1 секунду
        for (int i = 0; i < 100; i++)
        {
            float value = Mathf.Sin(i * 0.1f) * 10f; // Пример значения (синусоидальная волна)
            data.Add(new SharePointDataLoader.SensorDataPoint(value, startTime.AddSeconds(i)));
        }

        return data;
    }
}