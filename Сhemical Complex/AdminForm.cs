using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Сhemical_Complex
{
    public partial class AdminForm : Form
    {
        private readonly string _connectionString = @"Data Source=" + Application.StartupPath + @"\ComplexDB.db";
        private readonly string _dbFilePath = Path.Combine(Application.StartupPath, "ComplexDB.db");

        public AdminForm()
        {
            InitializeComponent();
            RefreshAllData();
            LoadComboBoxes();
        }

        private void RefreshAllData()
        {
            LoadData("Users", dataGridViewUsers, "UserId");
            LoadData("Materials", dataGridViewMaterials, "MaterialId");
            LoadData("Units", dataGridViewUnits, "UnitId");
            LoadData("Properties", dataGridViewProperties, "PropertyId");
            LoadData("MaterialPropertyBinds", dataGridViewPropertyBinds, "BindId");
            LoadData("EmpiricCoefficients", dataGridViewEmpiric, "CoefficientId");
            LoadData("MaterialEmpiricBinds", dataGridViewEmpiricBinds, "BindId");
            LoadComboBoxes();
        }

        private void ConfigureDataGridView(DataGridView dgv)
        {
            dgv.AutoGenerateColumns = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.MultiSelect = false;
            dgv.AllowUserToAddRows = true;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;

            var translations = new Dictionary<string, string>
            {
                { "Name", "Название" },
                { "Password", "Пароль" },
                { "RoleId", "Роль" },
                { "Chars", "Обозначение" },
                { "UnitsId", "Единица измерения" },
                { "MaterialId", "Материал" },
                { "PropertyId", "Свойство" },
                { "EmpiricId", "Коэффициент" },
                { "Value", "Значение" }
            };

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Name == "Id")
                {
                    col.HeaderText = col.Name;
                    continue;
                }
                if (translations.TryGetValue(col.Name, out var ru))
                {
                    col.HeaderText = ru;
                }
                else
                {
                    col.HeaderText = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(col.HeaderText.Replace("_", " "));
                }
            }
        }

        private void LoadData(string tableName, DataGridView dgv, string primaryKey)
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"SELECT * FROM {tableName}";

                    var dt = new DataTable();
                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }

                    if (dt.Columns.Contains(primaryKey))
                        dt.PrimaryKey = new[] { dt.Columns[primaryKey] };

                    dgv.DataSource = dt;
                    dgv.Tag = tableName;
                    ConfigureDataGridView(dgv);
                }
            }
        }

        private void LoadComboBoxes()
        {
            var units = GetLookup("Units", "Id", "Name");
            comboBoxUnitsToProperty.DataSource = units;
            comboBoxUnitsToProperty.DisplayMember = "Name";
            comboBoxUnitsToProperty.ValueMember = "Id";
            comboBoxUnitToPropertyEdit.DataSource = units.Copy();
            comboBoxUnitToPropertyEdit.DisplayMember = "Name";
            comboBoxUnitToPropertyEdit.ValueMember = "Id";

            var materials = GetLookup("Materials", "Id", "Name");
            comboBoxMaterialToPropertyBinds.DataSource = materials;
            comboBoxMaterialToPropertyBinds.DisplayMember = "Name";
            comboBoxMaterialToPropertyBinds.ValueMember = "Id";

            var properties = GetLookup("Properties", "Id", "Name");
            comboBoxIdToPropertyBinds.DataSource = properties;
            comboBoxIdToPropertyBinds.DisplayMember = "Name";
            comboBoxIdToPropertyBinds.ValueMember = "Id";

            comboBoxMaterialToEmpiricAdd.DataSource = materials.Copy();
            comboBoxMaterialToEmpiricAdd.DisplayMember = "Name";
            comboBoxMaterialToEmpiricAdd.ValueMember = "Id";

            var empirics = GetLookup("EmpiricCoefficients", "Id", "Name");
            comboBoxIdToEmpiricBindsAdd.DataSource = empirics;
            comboBoxIdToEmpiricBindsAdd.DisplayMember = "Name";
            comboBoxIdToEmpiricBindsAdd.ValueMember = "Id";

            //comboBoxIdEmpiricBindsEdit.DataSource = empirics.Copy();
            //comboBoxIdEmpiricBindsEdit.DisplayMember = "Name";
            //comboBoxIdEmpiricBindsEdit.ValueMember = "CoefficientId";
        }

        private DataTable GetLookup(string tableName, string idField, string nameField)
        {
            var dt = new DataTable();
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"SELECT {idField}, {nameField} FROM {tableName}";
                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }

        private int GetRoleId(RadioButton adminRadio, RadioButton userRadio)
        {
            try
            {
                if (adminRadio.Checked) return 1;
                else if (userRadio.Checked) return 2;
                else throw new Exception("Роль не выбрана");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Проверьте правильность ввода данных!");
                return 0;
            }
        }

        private void Delete(string textId, string table)
        {
            try
            {
                if (String.IsNullOrEmpty(textId))
                    throw new Exception();

                int id = Convert.ToInt32(textId);
                DatabaseManage.DeleteRecord(table, id);
            }
            catch (Exception)
            {
                MessageBox.Show($"Проверьте правильность ввода данных!");
                return;
            }
        }

        private void AddRecord(string table, Dictionary<string, object> fields)
        {
            try
            {
                foreach (var field in fields)
                {
                    if (string.IsNullOrEmpty(field.Value.ToString()))
                        throw new Exception($"Необхобимо заполнить все поля!");
                }

                DatabaseManage.InsertRecord(table, fields);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
                return;
            }
        }

        private void buttonAddUser_Click(object sender, EventArgs e)
        {
            int roleId = GetRoleId(radioButtonAdminAdd, radioButtonResearchAdd);

            var fields = new Dictionary<string, object>
            {
                {"Name", textBoxUserName.Text},
                {"Password", PasswordManage.GetHash(textBoxPassword.Text)},
                {"RoleId", roleId}
            };

            AddRecord("Users", fields);
            RefreshAllData();
        }

        private void buttonDeleteUser_Click(object sender, EventArgs e)
        {
            Delete(textBoxUserIdDelete.Text, "Users");
            RefreshAllData();
        }

        private void buttonUserEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int roleId = GetRoleId(radioButtonAdminAdd, radioButtonResearchAdd);

                if (String.IsNullOrEmpty(textBoxUserIdEdit.Text) && String.IsNullOrEmpty(textBoxUserNameEdit.Text) && String.IsNullOrEmpty(textBoxPasswordEdit.Text) && (roleId != 2 || roleId != 1))
                    throw new Exception($"Необхобимо заполнить все поля!");

                string hash = PasswordManage.GetHash(textBoxPasswordEdit.Text);
                int id = Convert.ToInt32(textBoxUserIdEdit.Text);

                var fields = new Dictionary<string, object>
                {
                    {"Name", textBoxUserNameEdit.Text},
                    {"Password", hash},
                    {"RoleId", roleId}
                };

                bool success = DatabaseManage.UpdateRecord("Users", id, fields);

                RefreshAllData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
                return;
            }
        }

        private void buttonAddMaterial_Click(object sender, EventArgs e)
        {
            var fields = new Dictionary<string, object>
            {
                { "Name", textBoxMaterialName.Text },
            };

            AddRecord("Materials", fields);
            RefreshAllData();
        }

        private void buttonDeleteMaterial_Click(object sender, EventArgs e)
        {
            Delete(textBoxDeleteMaterial.Text, "Materials");
            RefreshAllData();
        }

        private void buttonEditMaterial_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(textBoxEditMaterialId.Text) && String.IsNullOrEmpty(textBoxEditMaterialName.Text))
                    throw new Exception($"Необхобимо заполнить все поля!");

                int id = Convert.ToInt32(textBoxEditMaterialId.Text);

                var fields = new Dictionary<string, object>
                {
                    {"Id", textBoxEditMaterialId.Text},
                    {"Name", textBoxEditMaterialName.Text}
                };

                DatabaseManage.UpdateRecord("Materials", id, fields);
                RefreshAllData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
                return;
            }
        }

        private void buttonUnitAdd_Click(object sender, EventArgs e)
        {
            var fields = new Dictionary<string, object>
            {
                { "Name", textBoxUnitAdd.Text },
            };

            AddRecord("Units", fields);
            RefreshAllData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Delete(textBoxUnitIdDelete.Text, "Units");
            RefreshAllData();
        }

        private void buttonUnitEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(textBoxUnitIdEdit.Text) && String.IsNullOrEmpty(textBoxUnitEdit.Text))
                    throw new Exception($"Необхобимо заполнить все поля!");

                int id = Convert.ToInt32(textBoxUnitIdEdit.Text);

                var fields = new Dictionary<string, object>
                {
                    {"Id", textBoxUnitIdEdit.Text},
                    {"Name", textBoxUnitEdit.Text}
                };

                DatabaseManage.UpdateRecord("Units", id, fields);
                RefreshAllData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
                return;
            }
        }

        private void buttonPropertyAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int unitId = Convert.ToInt32(comboBoxUnitsToProperty.SelectedValue.ToString());
                var fields = new Dictionary<string, object>
                {
                    { "Chars", textBoxCharsAdd.Text },
                    { "Name", textBoxUnitsNameAdd.Text },
                    { "UnitsId", unitId }
                };
                AddRecord("Properties", fields);
                RefreshAllData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления свойства: {ex.Message}");
            }
        }

        private void buttonPropertyDelete_Click(object sender, EventArgs e)
        {
            Delete(textBoxPropertyIdDelete.Text, "Properties");
            RefreshAllData();
        }

        private void buttonPropertyEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(textBoxPropertyIdEdit.Text);
                int unitId = Convert.ToInt32(comboBoxUnitToPropertyEdit.SelectedValue);
                var fields = new Dictionary<string, object>
                {
                    { "Chars", textBoxCharsEdit.Text },
                    { "Name", textBoxPropertyNameEdit.Text },
                    { "UnitsId", unitId }
                };
                DatabaseManage.UpdateRecord("Properties", id, fields);
                RefreshAllData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка редактирования свойства: {ex.Message}");
            }
        }

        private void buttonPropertyBindsAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int materialId = Convert.ToInt32(comboBoxMaterialToPropertyBinds.SelectedValue);
                int propertyId = Convert.ToInt32(comboBoxIdToPropertyBinds.SelectedValue);
                var fields = new Dictionary<string, object>
                {
                    { "MaterialId", materialId },
                    { "PropertyId", propertyId },
                    { "Value", textBoxValuePropertyBindsAdd.Text }
                };
                AddRecord("MaterialPropertyBinds", fields);
                RefreshAllData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления связи свойств: {ex.Message}");
            }
        }

        private void buttonPropertyBindsDelete_Click(object sender, EventArgs e)
        {
            Delete(textBoxIdPropertyBindsDelete.Text, "MaterialPropertyBinds");
            RefreshAllData();
        }

        private void buttonPropteryBindsEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(textBoxIdPropertyBindsEdit.Text);
                var fields = new Dictionary<string, object>
                {
                    { "Value", textBoxValuePropertyBindsEdit.Text }
                };
                DatabaseManage.UpdateRecord("MaterialPropertyBinds", id, fields);
                RefreshAllData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка редактирования связи свойств: {ex.Message}");
            }
        }

        private void buttonEmpiricAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int materialId = Convert.ToInt32(comboBoxMaterialToEmpiricAdd.SelectedValue);
                int coeffId = Convert.ToInt32(comboBoxIdToEmpiricBindsAdd.SelectedValue);
                var fields = new Dictionary<string, object>
                {
                    { "MaterialId", materialId },
                    { "EmpiricId", coeffId },
                    { "Value", textBoxEmpiricValueAdd.Text }
                };
                DatabaseManage.InsertRecord("MaterialEmpiricBinds", fields);
                RefreshAllData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления эмпирической связи: {ex.Message}");
            }
        }

        // Delete Empiric Bind
        private void buttonEmpiricBindsDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Delete(textBoxEmpiricBindsDelete.Text, "MaterialEmpiricBinds");
                RefreshAllData();
            }
            catch
            {
                MessageBox.Show("Проверьте корректность ввода ID для удаления эмпирической связи");
            }
        }

        // Edit Empiric Bind
        private void buttonEmpiricBindsEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(textBoxIdEmpiricBindsEdit.Text);
                int materialId = Convert.ToInt32(comboBoxMaterialToEmpiricAdd.SelectedValue);
                //int coeffId = Convert.ToInt32(comboBoxIdEmpiricBindsEdit.SelectedValue);
                var fields = new Dictionary<string, object>
                {
                    { "MaterialId", materialId },
                    //{ "CoefficientId", coeffId },
                    { "Value", textBoxValueEmpiricBindsEdit.Text }
                };
                DatabaseManage.UpdateRecord("MaterialEmpiricBinds", id, fields);
                RefreshAllData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка редактирования эмпирической связи: {ex.Message}");
            }
        }

        private void CreateDataBaseCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Выберите папку для сохранения резервной копии базы";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var dest = Path.Combine(dlg.SelectedPath, $"ModellingComplexDB_backup_{DateTime.Now:yyyyMMdd_HHmmss}.db");
                        File.Copy(_dbFilePath, dest, true);
                        MessageBox.Show($"Резервная копия сохранена:\n{dest}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при создании резервной копии: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void LoadCopyDataBaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Title = "Выберите файл резервной копии базы";
                dlg.Filter = "SQLite DB (*.db)|*.db|Все файлы (*.*)|*.*";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        File.Copy(dlg.FileName, _dbFilePath, true);
                        MessageBox.Show("База данных успешно восстановлена из копии", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshAllData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка восстановления базы: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
