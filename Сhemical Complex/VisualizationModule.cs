using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Сhemical_Complex
{
    public class VisualizationModule
    {
        private DataTable _resultsTable;
        private double _calculationStep;
        private int _skipStep = 0;
        private List<double> _lengths = new List<double>();
        private List<double> _temperatures = new List<double>();
        private List<double> _viscosities = new List<double>();
        private MathModule _mathModule;

        public double Viscosity { get; private set; }
        public double Temperature { get; private set; }
        public double Efficiency { get; private set; }
        public DataTable ResultsTable => _resultsTable;
        public PlotModel TemperaturePlotModel { get; private set; }
        public PlotModel ViscosityPlotModel { get; private set; }

        public VisualizationModule(double calculationStep, int skipStep)
        {
            _skipStep = skipStep;
            _calculationStep = calculationStep;
            InitializeDataTable();
            InitializePlotModels();
        }

        public void ExportToExcel(string filePath, Dictionary<string, string> inputParameters)
        {
            var exporter = new ReportExporter();
            exporter.SaveReportToExcel(this, _mathModule, filePath, inputParameters);
        }

        private void InitializeDataTable()
        {
            _resultsTable = new DataTable("Results");
            _resultsTable.Columns.Add(new DataColumn("Координата по длине, м", typeof(double)));
            _resultsTable.Columns.Add(new DataColumn("Температура материала, °C", typeof(double)));
            _resultsTable.Columns.Add(new DataColumn("Вязкость материала, Па·с", typeof(double)));
        }

        private void InitializePlotModels()
        {
            TemperaturePlotModel = CreatePlotModel("Распределение температуры", "Температура материала, °C");
            ViscosityPlotModel = CreatePlotModel("Распределение вязкости", "Вязкость материала, Па·с");
        }

        public void FillData(MathModule mathModel)
        {
            if (mathModel == null)
                throw new ArgumentNullException("Объект математической модели не может быть null");

            if (mathModel.ProductT.Count != mathModel.ProductViscosity.Count)
                throw new ArgumentException("Количество элементов температур и вязкостей не совпадает");

            ClearData();
            int count = mathModel.ProductT.Count;

            if (count == 0)
                return;

            _mathModule = mathModel;

            List<int> selectedIndixes = new List<int>();
            selectedIndixes.Add(0);

            if (_skipStep <= 0)
            {
                for (int i = 1; i < count; i++)
                    selectedIndixes.Add(i);
            }
            else
            {
                int lastAdded = 0;

                for (int i = 1; i < count - 1; i++)
                {
                    if (i - lastAdded >= _skipStep + 1)
                    {
                        selectedIndixes.Add(i);
                        lastAdded = i;
                    }
                }

                int lastIndex = count - 1;
                if (lastIndex > selectedIndixes.Last())
                    selectedIndixes.Add(lastIndex);
            }

            foreach (int index in selectedIndixes)
            {
                double currentLength = index * _calculationStep;
                AddDataRow(currentLength, mathModel.ProductT[index], mathModel.ProductViscosity[index]);
            }

            UpdatePlots();

            Viscosity = mathModel.LastViscosity;
            Temperature = mathModel.LastT;
            Efficiency = mathModel.Q;
        }

        private void AddDataRow(double length, double temperature, double viscosity)
        {
            _lengths.Add(length);
            _temperatures.Add(temperature);
            _viscosities.Add(viscosity);

            DataRow newRow = _resultsTable.NewRow();
            newRow[0] = length;
            newRow[1] = temperature;
            newRow[2] = viscosity;
            _resultsTable.Rows.Add(newRow);
        }

        private void ClearData()
        {
            _lengths.Clear();
            _temperatures.Clear();
            _viscosities.Clear();
            _resultsTable.Rows.Clear();
        }

        private void UpdatePlots()
        {
            UpdatePlot(TemperaturePlotModel, _temperatures);
            UpdatePlot(ViscosityPlotModel, _viscosities);
        }

        private PlotModel CreatePlotModel(string title, string yAxisTitle)
        {
            var plotModel = new PlotModel { Title = title };

            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Координата по длине канала, м",
                Key = "XAxis",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });

            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = yAxisTitle,
                Key = "YAxis",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });

            return plotModel;
        }

        private void UpdatePlot(PlotModel plotModel, List<double> values)
        {
            plotModel.Series.Clear();

            var lineSeries = new LineSeries
            {
                Color = OxyColors.Blue,
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.Red
            };

            for (int i = 0; i < _lengths.Count; i++)
            {
                lineSeries.Points.Add(new DataPoint(_lengths[i], values[i]));
            }

            plotModel.Series.Add(lineSeries);
            plotModel.InvalidatePlot(true);
        }
    }
}
