using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Сhemical_Complex
{
    public partial class MainForm : Form
    {
        private CorrectControlModule _controlModule;
        private double _calculationStep;
        private MathModule _mathModule;
        private VisualizationModule _visualizationModule;
        private readonly string _connectionString = @"Data Source=" + Application.StartupPath + @"\ComplexDB.db";

        public MainForm()
        {
            InitializeComponent();

            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT Name FROM Materials";

                    using (var cmd = new SqliteCommand(query, connection))
                    {
                        using (SqliteDataReader reader = cmd.ExecuteReader())
                        {
                            comboBoxType.Items.Clear();

                            while (reader.Read())
                            {
                                string materialName = reader.GetString(0);
                                comboBoxType.Items.Add(materialName);
                            }

                            if (comboBoxType.Items.Count > 0)
                            {
                                comboBoxType.SelectedIndex = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки материалов: {ex.Message}");
                return;
            }
            this.CenterToScreen();
        }

        private void buttonBuild_Click(object sender, EventArgs e)
        {
            EfficiencyMetricsModule.PrepareMemoryMeasurement();

            string[] inputs = { comboBoxType.Text, textBoxStep.Text, textBoxLength.Text, textBoxWidth.Text, textBoxHeight.Text,
                textBoxSpeedCover.Text, textBoxCoverT.Text,
            textBoxDensity.Text, textBoxAverageHeatCapacity.Text, textBoxMeltingT.Text, textBoxGlassTransitionT.Text,
            textBoxConsistencyCoef.Text, textBoxC1g.Text, textBoxC2g.Text, textBoxCastingT.Text, textBoxHeatTransferTCoef.Text, textBoxFlowIndex.Text};

            _controlModule = new CorrectControlModule(inputs);

            if (!_controlModule.IsValid)
            {
                MessageBox.Show("Ошибки:\n" + string.Join("\n", _controlModule.Errors));
                return;
            }

            double[] calculationParams = _controlModule.CorrectValues;
            _calculationStep = calculationParams[0];

            _mathModule = new MathModule(calculationParams);
            _mathModule.CalculateOutputParameters();

            _visualizationModule = new VisualizationModule(_calculationStep, Convert.ToInt32(textBoxSkipStep.Text));
            _visualizationModule.FillData(_mathModule);

            plotViewT.Model = _visualizationModule.TemperaturePlotModel;
            plotViewViscosity.Model = _visualizationModule.ViscosityPlotModel;
            tableResult.DataSource = _visualizationModule.ResultsTable;

            textBoxEfficiency.Text = _visualizationModule.Efficiency.ToString();
            textBoxProductT.Text = _visualizationModule.Temperature.ToString();
            textBoxProductViscosity.Text = _visualizationModule.Viscosity.ToString();

            ConfigureDataGridView();
            textBoxCalculationTime.Text = _mathModule.CalculationTime.TotalMilliseconds.ToString();
            textBoxCalculateCount.Text = _mathModule.OperationCount.ToString();
            EfficiencyMetricsModule.FinalizeMemoryMeasurement();
            textBoxSpendMemory.Text = EfficiencyMetricsModule.MemoryUsedBytes.ToString();
        }

        private void ConfigureDataGridView()
        {
            tableResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tableResult.ReadOnly = true;
            tableResult.AllowUserToAddRows = false;

            tableResult.Columns[0].DefaultCellStyle.Format = "F2";
            tableResult.Columns[1].DefaultCellStyle.Format = "F2";
            tableResult.Columns[2].DefaultCellStyle.Format = "F1";
        }

        private void LoadMaterialProperties(string materialName)
        {
            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();

                    int materialId;
                    string query = "SELECT Id FROM Materials WHERE Name = @MaterialName";

                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaterialName", materialName);
                        materialId = Convert.ToInt32(command.ExecuteScalar());
                    }

                    var propertyMapping = new Dictionary<string, TextBox>
                    {
                        {"Density", textBoxDensity },
                        {"AverageHeatCapacity", textBoxAverageHeatCapacity },
                        {"MeltingTemperature", textBoxMeltingT},
                        {"GlassTransitionTemperature", textBoxGlassTransitionT }
                    };

                    query = @"SELECT p.Chars, mpb.Value FROM MaterialPropertyBinds mpb JOIN Properties p ON mpb.PropertyId = p.Id WHERE mpb.MaterialId = @MaterialId";

                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaterialId", materialId);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string propertyName = reader["Chars"].ToString();
                                string value = reader["Value"].ToString();

                                if (propertyMapping.TryGetValue(propertyName, out TextBox textBox))
                                {
                                    textBox.Text = value;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки свойств: {ex.Message}");
            }
        }

        private void LoadEmpiricCoefficients(string materialName)
        {
            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();

                    int materialId;
                    string getMaterialIdQuery = "SELECT Id FROM Materials WHERE Name = @MaterialName";

                    using (var cmd = new SqliteCommand(getMaterialIdQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaterialName", materialName);
                        materialId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    var coefficientMapping = new Dictionary<string, TextBox>
                    {
                        {"ConsistencyCoef", textBoxConsistencyCoef},
                        {"C1g", textBoxC1g},
                        {"C2g", textBoxC2g},
                        {"CastingT", textBoxCastingT},
                        {"flowIndex", textBoxFlowIndex},
                        {"HeatTransferTCoef", textBoxHeatTransferTCoef}
                    };

                    string query = @"SELECT ec.Chars, meb.Value FROM MaterialEmpiricBinds meb JOIN EmpiricCoefficients ec ON meb.EmpiricId = ec.Id WHERE meb.MaterialId = @MaterialId";

                    using (var cmd = new SqliteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaterialId", materialId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string coefficientName = reader["Chars"].ToString();
                                string value = reader["Value"].ToString();

                                if (coefficientMapping.TryGetValue(coefficientName, out TextBox textBox))
                                {
                                    textBox.Text = value;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки коэффициентов: {ex.Message}");
            }
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxType.SelectedItem != null)
            {
                string selectedMaterial = comboBoxType.SelectedItem.ToString();
                LoadMaterialProperties(selectedMaterial);
                LoadEmpiricCoefficients(selectedMaterial);
            }
        }

        private void buttonSaveReport_Click(object sender, EventArgs e)
        {
            if (_visualizationModule == null)
            {
                MessageBox.Show("Сначала нужно рассчитать и отобразить результаты.",
                                "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = "Excel Workbook|*.xlsx";
                dlg.DefaultExt = "xlsx";
                dlg.FileName = "Report.xlsx";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var inputData = new Dictionary<string, string>
                {
                    {"Тип материала", comboBoxType.Text},
                    {"Шаг расчета", textBoxStep.Text},
                    {"Длина канала", textBoxLength.Text},
                    {"Ширина канала", textBoxWidth.Text},
                    {"Высота канала", textBoxHeight.Text},
                    {"Скорость крышки", textBoxSpeedCover.Text},
                    {"Температура крышки", textBoxCoverT.Text},
                    {"Плотность", textBoxDensity.Text},
                    {"Средняя теплоемкость", textBoxAverageHeatCapacity.Text},
                    {"Темп. плавления", textBoxMeltingT.Text},
                    {"Темп. стеклования", textBoxGlassTransitionT.Text},
                    {"Коэф. консистенции", textBoxConsistencyCoef.Text},
                    {"C1g", textBoxC1g.Text},
                    {"C2g", textBoxC2g.Text},
                    {"Температура литья", textBoxCastingT.Text},
                    {"Коэф. теплопередачи", textBoxHeatTransferTCoef.Text},
                    {"Индекс течения", textBoxFlowIndex.Text}
                };

                        _visualizationModule.ExportToExcel(dlg.FileName, inputData);
                        MessageBox.Show("Отчет успешно сохранён.",
                                        "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении отчета: {ex.Message}",
                                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void buttonExperiment_Click(object sender, EventArgs e)
        {
            ExperimentForm dlg = new ExperimentForm(_mathModule);
            dlg.ShowDialog();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
