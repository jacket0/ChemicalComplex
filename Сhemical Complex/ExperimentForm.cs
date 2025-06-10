using MathNet.Numerics;
using OfficeOpenXml;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Сhemical_Complex
{
    public partial class ExperimentForm : Form
    {
        private MathModule _mathModule;
        private List<Tuple<double, double>> _experimentData = new List<Tuple<double, double>>();
        private double[] _currentCoefficients;
        private double[] _xValues;
        private double[] _yValues;

        public ExperimentForm(MathModule mathModule)
        {
            _mathModule = mathModule;
            InitializeComponent();
            InitializeDataGridViews();
            InitializeComboBox();
            InitializePlot();
        }

        private void InitializeDataGridViews()
        {
            // Таблица с исходными параметрами
            dataGridViewParameters.Columns.Clear();
            dataGridViewParameters.Columns.Add("Speed", "Скорость крышки, м/с");
            dataGridViewParameters.Columns.Add("Viscosity", "Вязкость продукта, Па·с");
            dataGridViewParameters.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewParameters.ReadOnly = true;

            // Таблица с результатами моделирования
            dataGridViewResults.Columns.Clear();
            dataGridViewResults.Columns.Add("Speed", "Скорость крышки, м/с");
            dataGridViewResults.Columns.Add("ActualViscosity", "Вязкость, Па·с");
            dataGridViewResults.Columns.Add("ModelViscosity", "Вязкость по модели, Па·с");
            dataGridViewResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewResults.ReadOnly = true;
        }

        private void InitializeComboBox()
        {
            comboBoxModelType.Items.Add("Линейная модель");
            comboBoxModelType.Items.Add("Квадратичная модель");
            comboBoxModelType.Items.Add("Кубическая модель");
            comboBoxModelType.SelectedIndex = 1;

            comboBoxParameter.Items.Add("Вязкость продукта, Па*с");
            comboBoxParameter.SelectedIndex = 0;
        }

        private void InitializePlot()
        {
            // Настройка графика
            var plotModel = new PlotModel
            {
                Title = "Зависимость вязкости от скорости крышки",
                TitleFontSize = 12,
                DefaultFontSize = 10
            };

            var legend = new Legend
            {
                LegendPosition = LegendPosition.RightTop,
                LegendPlacement = LegendPlacement.Outside,
                LegendOrientation = LegendOrientation.Vertical
            };
            plotModel.Legends.Add(legend);

            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Скорость крышки, м/с",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });

            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Вязкость, Па·с",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });

            plotView1.Model = plotModel;
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            var validator = new ExperimentCorrectControlModule(
                textBoxMinBoarder.Text,
                textBoxMaxBoarder.Text,
                textBoxStep.Text
            );

            if (!validator.IsValid)
            {
                MessageBox.Show(string.Join("\n", validator.Errors));
                return;
            }

            double min = validator.CorrectValues[0];
            double max = validator.CorrectValues[1];
            double step = validator.CorrectValues[2];

            dataGridViewParameters.Rows.Clear();
            _experimentData.Clear();

            for (double speed = min; speed <= max; speed += step)
            {
                speed = Math.Round(speed, GetDecimalDigitsCount(step) + 1);
                _mathModule.SetCoverSpeed(speed);
                _mathModule.CalculateOutputParameters();
                double viscosity = _mathModule.LastViscosity;

                _experimentData.Add(new Tuple<double, double>(speed, viscosity));
                dataGridViewParameters.Rows.Add(speed.ToString("F2"), viscosity.ToString("F2"));
            }
        }

        static int GetDecimalDigitsCount(double number)
        {
            string[] str = number.ToString(new System.Globalization.NumberFormatInfo()
            { NumberDecimalSeparator = "." })
                            .Split('.');
            return str.Length == 2 ? str[1].Length : 0;
        }

        private void buttonBuildModel_Click(object sender, EventArgs e)
        {
            if (_experimentData.Count == 0)
            {
                MessageBox.Show("Сначала выполните расчеты!");
                return;
            }

            int degree;
            switch (comboBoxModelType.SelectedIndex)
            {
                case 0: degree = 1; break; // Линейная модель
                case 1: degree = 2; break; // Квадратичная модель
                case 2: degree = 3; break; // Кубическая модель
                default: degree = 2; break; // По умолчанию квадратичная
            }

            _xValues = _experimentData.Select(d => d.Item1).ToArray();
            _yValues = _experimentData.Select(d => d.Item2).ToArray();

            try
            {
                // Построение полиномиальной модели выбранной степени
                _currentCoefficients = Polynomial.Fit(_xValues, _yValues, degree).Coefficients;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при построении модели: {ex.Message}");
                return;
            }

            ShowCoefficients(_currentCoefficients);
            CalculateAndDisplayMetrics();
        }

        private void buttonVisualize_Click(object sender, EventArgs e)
        {
            if (_currentCoefficients == null || _experimentData.Count == 0)
            {
                MessageBox.Show("Сначала постройте модель!");
                return;
            }

            // Заполняем таблицу результатов
            dataGridViewResults.Rows.Clear();
            foreach (var dataPoint in _experimentData)
            {
                double speed = dataPoint.Item1;
                double actualViscosity = dataPoint.Item2;
                double modelViscosity = EvaluatePolynomial(_currentCoefficients, speed);

                dataGridViewResults.Rows.Add(
                    speed.ToString("F2"),
                    actualViscosity.ToString("F2"),
                    modelViscosity.ToString("F3")
                );
            }

            // Строим график
            var plotModel = new PlotModel
            {
                Title = $"Зависимость вязкости от скорости крышки ({comboBoxModelType.SelectedItem})",
                TitleFontSize = 12,
                DefaultFontSize = 10
            };

            // Создаем и настраиваем легенду
            var legend = new Legend
            {
                LegendPosition = LegendPosition.RightTop,
                LegendPlacement = LegendPlacement.Outside,
                LegendOrientation = LegendOrientation.Vertical
            };
            plotModel.Legends.Add(legend);

            // Добавляем экспериментальные точки
            var scatterSeries = new ScatterSeries
            {
                Title = "Экспериментальные данные",
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                MarkerFill = OxyColors.Blue
            };

            for (int i = 0; i < _xValues.Length; i++)
            {
                scatterSeries.Points.Add(new ScatterPoint(_xValues[i], _yValues[i]));
            }
            plotModel.Series.Add(scatterSeries);

            // Добавляем линию модели
            var lineSeries = new LineSeries
            {
                Title = "Полиномиальная модель",
                Color = OxyColors.Red,
                StrokeThickness = 2
            };

            double minX = _xValues.Min();
            double maxX = _xValues.Max();
            int pointsCount = 100;

            for (int i = 0; i <= pointsCount; i++)
            {
                double x = minX + (maxX - minX) * i / pointsCount;
                double y = EvaluatePolynomial(_currentCoefficients, x);
                lineSeries.Points.Add(new DataPoint(x, y));
            }
            plotModel.Series.Add(lineSeries);

            // Настраиваем оси
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Скорость крышки, м/с",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });

            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Вязкость, Па·с",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });

            // Обновляем график
            plotView1.Model = plotModel;
            plotView1.InvalidatePlot(true);
        }
        private double EvaluatePolynomial(double[] coefficients, double x)
        {
            double result = 0;
            for (int i = 0; i < coefficients.Length; i++)
            {
                result += coefficients[i] * Math.Pow(x, i);
            }
            return result;
        }

        private void ShowCoefficients(double[] coefficients)
        {
            string modelType = comboBoxModelType.SelectedItem.ToString();
            string coefficientsText = $"Коэффициенты модели ({modelType}):\n";

            for (int i = 0; i < coefficients.Length; i++)
            {
                coefficientsText += $"a{i} = {coefficients[i]:F3}\n";
            }

            MessageBox.Show(coefficientsText, "Результаты аппроксимации");
        }

        private void CalculateAndDisplayMetrics()
        {
            if (_xValues == null || _yValues == null || _currentCoefficients == null)
                return;

            int n = _xValues.Length;
            double[] predictedValues = new double[n];
            double ssr = 0; // Объясненная сумма квадратов
            double sse = 0; // Сумма квадратов ошибок
            double sst = 0; // Общая сумма квадратов

            double yMean = _yValues.Average();

            for (int i = 0; i < n; i++)
            {
                predictedValues[i] = EvaluatePolynomial(_currentCoefficients, _xValues[i]);
                double error = _yValues[i] - predictedValues[i];
                sse += error * error;
                sst += (_yValues[i] - yMean) * (_yValues[i] - yMean);
            }

            ssr = sst - sse; // Объясненная сумма квадратов

            // 1. Коэффициент детерминации (R²)
            double rSquared = (sst > 0) ? ssr / sst : 0;
            textBoxDeterminationCoef.Text = rSquared.ToString("F4");

            // 2. Средняя абсолютная погрешность (MAE)
            double mae = 0;
            for (int i = 0; i < n; i++)
            {
                mae += Math.Abs(_yValues[i] - predictedValues[i]);
            }
            mae /= n;
            textBoxAbsAccuracy.Text = mae.ToString("F3");

            // 3. Средняя относительная погрешность (MAPE)
            double mape = 0;
            int validCount = 0;
            for (int i = 0; i < n; i++)
            {
                if (Math.Abs(_yValues[i]) > double.Epsilon)
                {
                    mape += Math.Abs((_yValues[i] - predictedValues[i]) / _yValues[i]);
                    validCount++;
                }
            }
            mape = (validCount > 0) ? (mape / validCount) * 100 : 0;
            textBoxRelativeAccuracy.Text = $"{mape.ToString("F3")}";

            // 4. Критерий Фишера (F-статистика)
            int k = _currentCoefficients.Length; // Количество параметров модели
            double fStatistic = 0;
            if (sse > 0 && n > k)
            {
                fStatistic = (sst / (n - 1)) / (sse / (n - k));
            }
            textBoxFisherCriterion.Text = fStatistic.ToString("F1");
        }

        private void buttonSaveReport_Click(object sender, EventArgs e)
        {
            if (_experimentData.Count == 0 || _currentCoefficients == null)
            {
                MessageBox.Show("Сначала выполните расчеты и постройте модель!");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                Title = "Сохранить отчет",
                FileName = $"Отчет_эксперимента_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var package = new ExcelPackage())
                    {
                        // Лист с входными параметрами и результатами
                        var wsData = package.Workbook.Worksheets.Add("Данные эксперимента");
                        GenerateDataWorksheet(wsData);

                        // Лист с количественными оценками
                        var wsMetrics = package.Workbook.Worksheets.Add("Количественные оценки");
                        GenerateMetricsWorksheet(wsMetrics);

                        // Лист с графиком
                        var wsChart = package.Workbook.Worksheets.Add("График");
                        GenerateChartWorksheet(wsChart);

                        // Сохраняем файл
                        package.SaveAs(new FileInfo(saveFileDialog.FileName));
                        MessageBox.Show("Отчет успешно сохранен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании отчета: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void GenerateDataWorksheet(ExcelWorksheet worksheet)
        {
            // Заголовки
            worksheet.Cells[1, 1].Value = "Входные параметры эксперимента";
            worksheet.Cells[1, 1, 1, 3].Merge = true;
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            worksheet.Cells[2, 1].Value = "Минимальная скорость, м/с:";
            worksheet.Cells[2, 2].Value = textBoxMinBoarder.Text;
            worksheet.Cells[3, 1].Value = "Максимальная скорость, м/с:";
            worksheet.Cells[3, 2].Value = textBoxMaxBoarder.Text;
            worksheet.Cells[4, 1].Value = "Шаг варьирования, м/с:";
            worksheet.Cells[4, 2].Value = textBoxStep.Text;
            worksheet.Cells[5, 1].Value = "Тип модели:";
            worksheet.Cells[5, 2].Value = comboBoxModelType.Text;

            // Таблица с экспериментальными данными
            worksheet.Cells[7, 1].Value = "Экспериментальные данные";
            worksheet.Cells[7, 1, 7, 2].Merge = true;
            worksheet.Cells[7, 1].Style.Font.Bold = true;

            worksheet.Cells[8, 1].Value = "Скорость крышки, м/с";
            worksheet.Cells[8, 2].Value = "Вязкость продукта, Па·с";
            worksheet.Cells[8, 1, 8, 2].Style.Font.Bold = true;

            for (int i = 0; i < _experimentData.Count; i++)
            {
                worksheet.Cells[9 + i, 1].Value = _experimentData[i].Item1;
                worksheet.Cells[9 + i, 2].Value = _experimentData[i].Item2;
            }

            // Таблица с результатами моделирования
            int resultsStartRow = 9 + _experimentData.Count + 2;
            worksheet.Cells[resultsStartRow, 1].Value = "Результаты моделирования";
            worksheet.Cells[resultsStartRow, 1, resultsStartRow, 3].Merge = true;
            worksheet.Cells[resultsStartRow, 1].Style.Font.Bold = true;

            worksheet.Cells[resultsStartRow + 1, 1].Value = "Скорость крышки, м/с";
            worksheet.Cells[resultsStartRow + 1, 2].Value = "Фактическая вязкость, Па·с";
            worksheet.Cells[resultsStartRow + 1, 3].Value = "Вязкость по модели, Па·с";
            worksheet.Cells[resultsStartRow + 1, 1, resultsStartRow + 1, 3].Style.Font.Bold = true;

            for (int i = 0; i < dataGridViewResults.Rows.Count; i++)
            {
                if (dataGridViewResults.Rows[i].Cells[0].Value != null)
                {
                    worksheet.Cells[resultsStartRow + 2 + i, 1].Value = dataGridViewResults.Rows[i].Cells[0].Value.ToString();
                    worksheet.Cells[resultsStartRow + 2 + i, 2].Value = dataGridViewResults.Rows[i].Cells[1].Value.ToString();
                    worksheet.Cells[resultsStartRow + 2 + i, 3].Value = dataGridViewResults.Rows[i].Cells[2].Value.ToString();
                }
            }

            // Автоподбор ширины столбцов
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        }

        private void GenerateMetricsWorksheet(ExcelWorksheet worksheet)
        {
            // Заголовок
            worksheet.Cells[1, 1].Value = "Количественные оценки модели";
            worksheet.Cells[1, 1, 1, 2].Merge = true;
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            // Коэффициенты модели
            worksheet.Cells[3, 1].Value = "Коэффициенты модели:";
            worksheet.Cells[3, 1].Style.Font.Bold = true;

            for (int i = 0; i < _currentCoefficients.Length; i++)
            {
                worksheet.Cells[4 + i, 1].Value = $"a{i}";
                worksheet.Cells[4 + i, 2].Value = _currentCoefficients[i];
                worksheet.Cells[4 + i, 2].Style.Numberformat.Format = "0.000";
            }

            // Метрики качества
            int metricsStartRow = 4 + _currentCoefficients.Length + 2;
            worksheet.Cells[metricsStartRow, 1].Value = "Метрика";
            worksheet.Cells[metricsStartRow, 2].Value = "Значение";
            worksheet.Cells[metricsStartRow, 1, metricsStartRow, 2].Style.Font.Bold = true;

            worksheet.Cells[metricsStartRow + 1, 1].Value = "Коэффициент детерминации (R²)";
            worksheet.Cells[metricsStartRow + 1, 2].Value = textBoxDeterminationCoef.Text;
            worksheet.Cells[metricsStartRow + 1, 2].Style.Numberformat.Format = "0.0000";

            worksheet.Cells[metricsStartRow + 2, 1].Value = "Средняя абсолютная погрешность (MAE)";
            worksheet.Cells[metricsStartRow + 2, 2].Value = textBoxAbsAccuracy.Text;
            worksheet.Cells[metricsStartRow + 2, 2].Style.Numberformat.Format = "0.000";

            worksheet.Cells[metricsStartRow + 3, 1].Value = "Средняя относительная погрешность (MAPE)";
            worksheet.Cells[metricsStartRow + 3, 2].Value = textBoxRelativeAccuracy.Text.Replace(" %", "");
            worksheet.Cells[metricsStartRow + 3, 2].Style.Numberformat.Format = "0.00\" %\"";

            worksheet.Cells[metricsStartRow + 4, 1].Value = "Критерий Фишера (F-статистика)";
            worksheet.Cells[metricsStartRow + 4, 2].Value = textBoxFisherCriterion.Text;
            worksheet.Cells[metricsStartRow + 4, 2].Style.Numberformat.Format = "0.000";

            // Автоподбор ширины столбцов
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        }

        private void GenerateChartWorksheet(ExcelWorksheet worksheet)
        {
            // Заголовок
            worksheet.Cells[1, 1].Value = "График зависимости вязкости от скорости крышки";
            worksheet.Cells[1, 1, 1, 2].Merge = true;
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            // Сохраняем график как изображение
            var plotModel = plotView1.Model;
            if (plotModel != null)
            {
                using (var stream = new MemoryStream())
                {
                    var pngExporter = new PngExporter { Width = 800, Height = 600 };
                    pngExporter.Export(plotModel, stream);
                    stream.Position = 0;

                    // Добавляем изображение в Excel
                    var picture = worksheet.Drawings.AddPicture("График", stream);
                    picture.SetPosition(3, 0, 0, 0);
                    picture.SetSize(800, 600);
                }
            }

            // Автоподбор ширины столбцов
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        }
    }
}