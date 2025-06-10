using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Table;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Сhemical_Complex
{
    public class ReportExporter
    {
        public void SaveReportToExcel(VisualizationModule viz, MathModule mathModule, string filePath, Dictionary<string, string> inputParams)
        {
            var fi = new FileInfo(filePath);
            if (fi.Exists) fi.Delete();

            using (var package = new ExcelPackage(fi))
            {
                // Лист 1: Результаты
                var wsData = package.Workbook.Worksheets.Add("Результаты");
                DataTable table = viz.ResultsTable;
                wsData.Cells["A1"].LoadFromDataTable(table, true, TableStyles.Medium9);
                wsData.Cells[wsData.Dimension.Address].AutoFitColumns();

                int rows = table.Rows.Count;

                // Считаем min/max температуры и вязкости
                var tempValues = table.AsEnumerable()
                                      .Select(r => r.Field<double>("Температура материала, °C"))
                                      .ToList();
                var viscValues = table.AsEnumerable()
                                      .Select(r => r.Field<double>("Вязкость материала, Па·с"))
                                      .ToList();
                double minTemp = tempValues.Min(), maxTemp = tempValues.Max();
                double minVisc = viscValues.Min(), maxVisc = viscValues.Max();

                var wsChart = package.Workbook.Worksheets.Add("Графики");

                // Температура
                var chartTemp = wsChart.Drawings
                    .AddChart("chartTemp", eChartType.XYScatterLines) as ExcelScatterChart;
                chartTemp.Title.Text = "Распределение температуры";
                chartTemp.SetPosition(0, 0, 0, 0);
                chartTemp.SetSize(600, 300);
                // Диапазоны X и Y из листа данных
                rows = table.Rows.Count;
                chartTemp.Series.Add(
                    wsData.Cells[2, 2, rows + 1, 2],   // Y = столбец B (2) или C (3) в зависимости от индекса
                    wsData.Cells[2, 1, rows + 1, 1]    // X = столбец A
                ).Header = "Темп., °C";
                chartTemp.YAxis.Title.Text = "Температура, °C";
                chartTemp.XAxis.Title.Text = "Координата, м"; chartTemp.YAxis.MinValue = minTemp;
                chartTemp.YAxis.MaxValue = maxTemp;

                // Вязкость
                var chartVisc = wsChart.Drawings
                    .AddChart("chartVisc", eChartType.XYScatterLines) as ExcelScatterChart;
                chartVisc.Title.Text = "Распределение вязкости";
                chartVisc.SetPosition(16, 0, 0, 0);
                chartVisc.SetSize(600, 300);
                chartVisc.Series.Add(
                    wsData.Cells[2, 3, rows + 1, 3],   // Y = столбец D (вязкость)
                    wsData.Cells[2, 1, rows + 1, 1]    // X = столбец A
                ).Header = "Вязкость, Па·с";
                chartVisc.YAxis.Title.Text = "Вязкость, Па·с";
                chartVisc.XAxis.Title.Text = "Координата, м"; chartVisc.YAxis.MinValue = minVisc;
                chartVisc.YAxis.MaxValue = maxVisc;



                // 3. Лист «Сводка» с ключевыми метриками
                var wsSummary = package.Workbook.Worksheets.Add("Сводка");
                wsSummary.Cells[1, 1].Value = "Показатель";
                wsSummary.Cells[1, 2].Value = "Значение";

                wsSummary.Cells[2, 1].Value = "Производительность канала (Q)";
                wsSummary.Cells[2, 2].Value = viz.Efficiency;

                wsSummary.Cells[3, 1].Value = "Температура продукта, °C";
                wsSummary.Cells[3, 2].Value = viz.Temperature;

                wsSummary.Cells[4, 1].Value = "Вязкость продукта, Па·с";
                wsSummary.Cells[4, 2].Value = viz.Viscosity;

                wsSummary.Cells[5, 1].Value = "Время расчёта, мс";
                wsSummary.Cells[5, 2].Value = mathModule.CalculationTime.TotalMilliseconds;

                wsSummary.Cells[6, 1].Value = "Затраченная память, байт";
                wsSummary.Cells[6, 2].Value = EfficiencyMetricsModule.MemoryUsedBytes.ToString();

                wsSummary.Cells[7, 1].Value = "Арифм. операций";
                wsSummary.Cells[7, 2].Value = mathModule.OperationCount;

                wsSummary.Cells[wsSummary.Dimension.Address].AutoFitColumns();

                var wsInputs = package.Workbook.Worksheets.Add("Входные данные");
                wsInputs.Cells[1, 1].Value = "Параметр";
                wsInputs.Cells[1, 2].Value = "Значение";

                int row = 2;
                foreach (var kvp in inputParams)
                {
                    wsInputs.Cells[row, 1].Value = kvp.Key;
                    wsInputs.Cells[row, 2].Value = kvp.Value;
                    row++;
                }
                wsInputs.Cells[wsInputs.Dimension.Address].AutoFitColumns();

                package.Save();
            }
        }

    }
}
