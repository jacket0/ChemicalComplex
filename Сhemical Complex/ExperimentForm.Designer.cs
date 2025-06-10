namespace Сhemical_Complex
{
    partial class ExperimentForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonCalculate = new System.Windows.Forms.Button();
            this.textBoxStep = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxMaxBoarder = new System.Windows.Forms.TextBox();
            this.textBoxMinBoarder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxParameter = new System.Windows.Forms.ComboBox();
            this.labelLength = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridViewParameters = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonBuildModel = new System.Windows.Forms.Button();
            this.comboBoxModelType = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.buttonSaveReport = new System.Windows.Forms.Button();
            this.buttonVisualize = new System.Windows.Forms.Button();
            this.textBoxDeterminationCoef = new System.Windows.Forms.TextBox();
            this.textBoxFisherCriterion = new System.Windows.Forms.TextBox();
            this.textBoxRelativeAccuracy = new System.Windows.Forms.TextBox();
            this.textBoxAbsAccuracy = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            this.plotView1 = new OxyPlot.WindowsForms.PlotView();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewParameters)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox5, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 21.682F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 78.318F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1284, 761);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonCalculate);
            this.groupBox1.Controls.Add(this.textBoxStep);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxMaxBoarder);
            this.groupBox1.Controls.Add(this.textBoxMinBoarder);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBoxParameter);
            this.groupBox1.Controls.Add(this.labelLength);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(507, 159);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Входные данные";
            // 
            // buttonCalculate
            // 
            this.buttonCalculate.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCalculate.Location = new System.Drawing.Point(276, 123);
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Size = new System.Drawing.Size(225, 30);
            this.buttonCalculate.TabIndex = 68;
            this.buttonCalculate.Text = "Рассчитать данные";
            this.buttonCalculate.UseVisualStyleBackColor = true;
            this.buttonCalculate.Click += new System.EventHandler(this.buttonCalculate_Click);
            // 
            // textBoxStep
            // 
            this.textBoxStep.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxStep.Location = new System.Drawing.Point(276, 91);
            this.textBoxStep.Name = "textBoxStep";
            this.textBoxStep.Size = new System.Drawing.Size(80, 20);
            this.textBoxStep.TabIndex = 59;
            this.textBoxStep.TabStop = false;
            this.textBoxStep.Text = "0,1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 13);
            this.label4.TabIndex = 58;
            this.label4.Text = "Шаг варьирования, м/с";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(365, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 57;
            this.label3.Text = "max:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(243, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 56;
            this.label2.Text = "min:";
            // 
            // textBoxMaxBoarder
            // 
            this.textBoxMaxBoarder.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxMaxBoarder.Location = new System.Drawing.Point(400, 56);
            this.textBoxMaxBoarder.Name = "textBoxMaxBoarder";
            this.textBoxMaxBoarder.Size = new System.Drawing.Size(80, 20);
            this.textBoxMaxBoarder.TabIndex = 55;
            this.textBoxMaxBoarder.TabStop = false;
            this.textBoxMaxBoarder.Text = "3,0";
            // 
            // textBoxMinBoarder
            // 
            this.textBoxMinBoarder.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxMinBoarder.Location = new System.Drawing.Point(276, 59);
            this.textBoxMinBoarder.Name = "textBoxMinBoarder";
            this.textBoxMinBoarder.Size = new System.Drawing.Size(80, 20);
            this.textBoxMinBoarder.TabIndex = 54;
            this.textBoxMinBoarder.TabStop = false;
            this.textBoxMinBoarder.Text = "2,0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 26);
            this.label1.TabIndex = 53;
            this.label1.Text = "Диапазон варьирования \r\nскорости крышки, м/с";
            // 
            // comboBoxParameter
            // 
            this.comboBoxParameter.FormattingEnabled = true;
            this.comboBoxParameter.Location = new System.Drawing.Point(276, 19);
            this.comboBoxParameter.Name = "comboBoxParameter";
            this.comboBoxParameter.Size = new System.Drawing.Size(226, 21);
            this.comboBoxParameter.TabIndex = 52;
            // 
            // labelLength
            // 
            this.labelLength.AutoSize = true;
            this.labelLength.Location = new System.Drawing.Point(9, 22);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(149, 13);
            this.labelLength.TabIndex = 7;
            this.labelLength.Text = "Критериальный показатель\r\n";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 168);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(507, 590);
            this.splitContainer1.SplitterDistance = 495;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridViewParameters);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(507, 495);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Выходные параметры";
            // 
            // dataGridViewParameters
            // 
            this.dataGridViewParameters.AllowUserToAddRows = false;
            this.dataGridViewParameters.AllowUserToDeleteRows = false;
            this.dataGridViewParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewParameters.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewParameters.Name = "dataGridViewParameters";
            this.dataGridViewParameters.ReadOnly = true;
            this.dataGridViewParameters.Size = new System.Drawing.Size(501, 476);
            this.dataGridViewParameters.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonBuildModel);
            this.groupBox3.Controls.Add(this.comboBoxModelType);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(507, 91);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Данные полиномиальной модельки";
            // 
            // buttonBuildModel
            // 
            this.buttonBuildModel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonBuildModel.Location = new System.Drawing.Point(275, 46);
            this.buttonBuildModel.Name = "buttonBuildModel";
            this.buttonBuildModel.Size = new System.Drawing.Size(178, 30);
            this.buttonBuildModel.TabIndex = 69;
            this.buttonBuildModel.Text = "Построить модель";
            this.buttonBuildModel.UseVisualStyleBackColor = true;
            this.buttonBuildModel.Click += new System.EventHandler(this.buttonBuildModel_Click);
            // 
            // comboBoxModelType
            // 
            this.comboBoxModelType.FormattingEnabled = true;
            this.comboBoxModelType.Location = new System.Drawing.Point(275, 19);
            this.comboBoxModelType.Name = "comboBoxModelType";
            this.comboBoxModelType.Size = new System.Drawing.Size(226, 21);
            this.comboBoxModelType.TabIndex = 67;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(136, 13);
            this.label8.TabIndex = 60;
            this.label8.Text = "Полиномиальная модель";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.buttonSaveReport);
            this.groupBox4.Controls.Add(this.buttonVisualize);
            this.groupBox4.Controls.Add(this.textBoxDeterminationCoef);
            this.groupBox4.Controls.Add(this.textBoxFisherCriterion);
            this.groupBox4.Controls.Add(this.textBoxRelativeAccuracy);
            this.groupBox4.Controls.Add(this.textBoxAbsAccuracy);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(516, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(765, 159);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Количественные оценки";
            // 
            // buttonSaveReport
            // 
            this.buttonSaveReport.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSaveReport.Location = new System.Drawing.Point(524, 123);
            this.buttonSaveReport.Name = "buttonSaveReport";
            this.buttonSaveReport.Size = new System.Drawing.Size(232, 30);
            this.buttonSaveReport.TabIndex = 70;
            this.buttonSaveReport.Text = "Сохранить отчет";
            this.buttonSaveReport.UseVisualStyleBackColor = true;
            this.buttonSaveReport.Click += new System.EventHandler(this.buttonSaveReport_Click);
            // 
            // buttonVisualize
            // 
            this.buttonVisualize.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonVisualize.Location = new System.Drawing.Point(524, 87);
            this.buttonVisualize.Name = "buttonVisualize";
            this.buttonVisualize.Size = new System.Drawing.Size(232, 30);
            this.buttonVisualize.TabIndex = 69;
            this.buttonVisualize.Text = "Визуализировать результаты";
            this.buttonVisualize.UseVisualStyleBackColor = true;
            this.buttonVisualize.Click += new System.EventHandler(this.buttonVisualize_Click);
            // 
            // textBoxDeterminationCoef
            // 
            this.textBoxDeterminationCoef.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxDeterminationCoef.Location = new System.Drawing.Point(234, 123);
            this.textBoxDeterminationCoef.Name = "textBoxDeterminationCoef";
            this.textBoxDeterminationCoef.ReadOnly = true;
            this.textBoxDeterminationCoef.Size = new System.Drawing.Size(150, 20);
            this.textBoxDeterminationCoef.TabIndex = 67;
            this.textBoxDeterminationCoef.TabStop = false;
            // 
            // textBoxFisherCriterion
            // 
            this.textBoxFisherCriterion.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxFisherCriterion.Location = new System.Drawing.Point(234, 91);
            this.textBoxFisherCriterion.Name = "textBoxFisherCriterion";
            this.textBoxFisherCriterion.ReadOnly = true;
            this.textBoxFisherCriterion.Size = new System.Drawing.Size(150, 20);
            this.textBoxFisherCriterion.TabIndex = 66;
            this.textBoxFisherCriterion.TabStop = false;
            // 
            // textBoxRelativeAccuracy
            // 
            this.textBoxRelativeAccuracy.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxRelativeAccuracy.Location = new System.Drawing.Point(234, 57);
            this.textBoxRelativeAccuracy.Name = "textBoxRelativeAccuracy";
            this.textBoxRelativeAccuracy.ReadOnly = true;
            this.textBoxRelativeAccuracy.Size = new System.Drawing.Size(150, 20);
            this.textBoxRelativeAccuracy.TabIndex = 65;
            this.textBoxRelativeAccuracy.TabStop = false;
            // 
            // textBoxAbsAccuracy
            // 
            this.textBoxAbsAccuracy.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxAbsAccuracy.Location = new System.Drawing.Point(234, 25);
            this.textBoxAbsAccuracy.Name = "textBoxAbsAccuracy";
            this.textBoxAbsAccuracy.ReadOnly = true;
            this.textBoxAbsAccuracy.Size = new System.Drawing.Size(150, 20);
            this.textBoxAbsAccuracy.TabIndex = 64;
            this.textBoxAbsAccuracy.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 126);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(153, 13);
            this.label9.TabIndex = 63;
            this.label9.Text = "Коэффициент детерминации";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 13);
            this.label5.TabIndex = 61;
            this.label5.Text = "Критерий Фишера";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 59);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(168, 13);
            this.label6.TabIndex = 60;
            this.label6.Text = "Относительная погрешность, %\r\n";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(167, 13);
            this.label7.TabIndex = 59;
            this.label7.Text = "Абсолютная погрешность, Па·с";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.splitContainer2);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(516, 168);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(765, 590);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Результаты эксперимента";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 16);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dataGridViewResults);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.plotView1);
            this.splitContainer2.Size = new System.Drawing.Size(759, 571);
            this.splitContainer2.SplitterDistance = 170;
            this.splitContainer2.TabIndex = 0;
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewResults.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.Size = new System.Drawing.Size(759, 170);
            this.dataGridViewResults.TabIndex = 0;
            // 
            // plotView1
            // 
            this.plotView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotView1.Location = new System.Drawing.Point(0, 0);
            this.plotView1.Name = "plotView1";
            this.plotView1.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plotView1.Size = new System.Drawing.Size(759, 397);
            this.plotView1.TabIndex = 0;
            this.plotView1.Text = "plotView1";
            this.plotView1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotView1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotView1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // ExperimentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightBlue;
            this.ClientSize = new System.Drawing.Size(1284, 761);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ExperimentForm";
            this.Text = "Интерфейс эксперимента";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewParameters)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelLength;
        private System.Windows.Forms.ComboBox comboBoxParameter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxMaxBoarder;
        private System.Windows.Forms.TextBox textBoxMinBoarder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxStep;
        private System.Windows.Forms.Button buttonBuildModel;
        private System.Windows.Forms.Button buttonCalculate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBoxModelType;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridViewParameters;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonSaveReport;
        private System.Windows.Forms.Button buttonVisualize;
        private System.Windows.Forms.TextBox textBoxDeterminationCoef;
        private System.Windows.Forms.TextBox textBoxFisherCriterion;
        private System.Windows.Forms.TextBox textBoxRelativeAccuracy;
        private System.Windows.Forms.TextBox textBoxAbsAccuracy;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dataGridViewResults;
        private OxyPlot.WindowsForms.PlotView plotView1;
    }
}