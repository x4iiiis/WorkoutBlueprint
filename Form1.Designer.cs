using System;

namespace WorkoutBlueprint
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnGo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listboxWorkoutType = new System.Windows.Forms.ListBox();
            this.radioStrength = new System.Windows.Forms.RadioButton();
            this.radioHypertrophy = new System.Windows.Forms.RadioButton();
            this.PopulationProgressBar = new System.Windows.Forms.ProgressBar();
            this.GenerationProgressBar = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ProgramDisplay = new System.Windows.Forms.DataGridView();
            this.exerciseDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.muscleGroupDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.specificTargetDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exercisesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.blueprintDataSet = new WorkoutBlueprint.BlueprintDataSet();
            this.exercisesTableAdapter = new WorkoutBlueprint.BlueprintDataSetTableAdapters.ExercisesTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgramDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exercisesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueprintDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGo
            // 
            this.btnGo.Enabled = false;
            this.btnGo.Location = new System.Drawing.Point(330, 94);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 0;
            this.btnGo.Text = "DO IT";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(121, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Workout Type:";
            // 
            // listboxWorkoutType
            // 
            this.listboxWorkoutType.FormattingEnabled = true;
            this.listboxWorkoutType.Items.AddRange(new object[] {
            "Push",
            "Pull",
            "Legs",
            "Chest",
            "Back",
            "Arms",
            "Shoulders",
            "Accessories"});
            this.listboxWorkoutType.Location = new System.Drawing.Point(124, 24);
            this.listboxWorkoutType.Name = "listboxWorkoutType";
            this.listboxWorkoutType.Size = new System.Drawing.Size(120, 108);
            this.listboxWorkoutType.TabIndex = 2;
            // 
            // radioStrength
            // 
            this.radioStrength.AutoSize = true;
            this.radioStrength.Location = new System.Drawing.Point(299, 48);
            this.radioStrength.Name = "radioStrength";
            this.radioStrength.Size = new System.Drawing.Size(106, 17);
            this.radioStrength.TabIndex = 3;
            this.radioStrength.TabStop = true;
            this.radioStrength.Text = "Strength Training";
            this.radioStrength.UseVisualStyleBackColor = true;
            this.radioStrength.CheckedChanged += new System.EventHandler(this.radioStrength_CheckedChanged);
            // 
            // radioHypertrophy
            // 
            this.radioHypertrophy.AutoSize = true;
            this.radioHypertrophy.Location = new System.Drawing.Point(299, 71);
            this.radioHypertrophy.Name = "radioHypertrophy";
            this.radioHypertrophy.Size = new System.Drawing.Size(123, 17);
            this.radioHypertrophy.TabIndex = 4;
            this.radioHypertrophy.TabStop = true;
            this.radioHypertrophy.Text = "Hypertrophy Training";
            this.radioHypertrophy.UseVisualStyleBackColor = true;
            this.radioHypertrophy.CheckedChanged += new System.EventHandler(this.radioHypertrophy_CheckedChanged);
            // 
            // PopulationProgressBar
            // 
            this.PopulationProgressBar.Location = new System.Drawing.Point(124, 186);
            this.PopulationProgressBar.Name = "PopulationProgressBar";
            this.PopulationProgressBar.Size = new System.Drawing.Size(298, 23);
            this.PopulationProgressBar.TabIndex = 5;
            this.PopulationProgressBar.Tag = "";
            // 
            // GenerationProgressBar
            // 
            this.GenerationProgressBar.Location = new System.Drawing.Point(124, 246);
            this.GenerationProgressBar.Name = "GenerationProgressBar";
            this.GenerationProgressBar.Size = new System.Drawing.Size(298, 23);
            this.GenerationProgressBar.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(121, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Database Population:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(121, 230);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Workout Generation:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(559, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(208, 331);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // ProgramDisplay
            // 
            this.ProgramDisplay.AllowUserToAddRows = false;
            this.ProgramDisplay.AllowUserToDeleteRows = false;
            this.ProgramDisplay.AllowUserToResizeColumns = false;
            this.ProgramDisplay.AllowUserToResizeRows = false;
            this.ProgramDisplay.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ProgramDisplay.AutoGenerateColumns = false;
            this.ProgramDisplay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.ProgramDisplay.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ProgramDisplay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProgramDisplay.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.exerciseDataGridViewTextBoxColumn,
            this.muscleGroupDataGridViewTextBoxColumn,
            this.specificTargetDataGridViewTextBoxColumn});
            this.ProgramDisplay.DataSource = this.exercisesBindingSource;
            this.ProgramDisplay.Location = new System.Drawing.Point(3, 375);
            this.ProgramDisplay.Name = "ProgramDisplay";
            this.ProgramDisplay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ProgramDisplay.Size = new System.Drawing.Size(543, 385);
            this.ProgramDisplay.TabIndex = 10;
            this.ProgramDisplay.Visible = false;
            // 
            // exerciseDataGridViewTextBoxColumn
            // 
            this.exerciseDataGridViewTextBoxColumn.DataPropertyName = "Exercise";
            this.exerciseDataGridViewTextBoxColumn.HeaderText = "Exercise";
            this.exerciseDataGridViewTextBoxColumn.Name = "exerciseDataGridViewTextBoxColumn";
            this.exerciseDataGridViewTextBoxColumn.Width = 72;
            // 
            // muscleGroupDataGridViewTextBoxColumn
            // 
            this.muscleGroupDataGridViewTextBoxColumn.DataPropertyName = "MuscleGroup";
            this.muscleGroupDataGridViewTextBoxColumn.HeaderText = "MuscleGroup";
            this.muscleGroupDataGridViewTextBoxColumn.Name = "muscleGroupDataGridViewTextBoxColumn";
            this.muscleGroupDataGridViewTextBoxColumn.Width = 95;
            // 
            // specificTargetDataGridViewTextBoxColumn
            // 
            this.specificTargetDataGridViewTextBoxColumn.DataPropertyName = "SpecificTarget";
            this.specificTargetDataGridViewTextBoxColumn.HeaderText = "SpecificTarget";
            this.specificTargetDataGridViewTextBoxColumn.Name = "specificTargetDataGridViewTextBoxColumn";
            this.specificTargetDataGridViewTextBoxColumn.Width = 101;
            // 
            // exercisesBindingSource
            // 
            this.exercisesBindingSource.DataMember = "Exercises";
            this.exercisesBindingSource.DataSource = this.blueprintDataSet;
            // 
            // blueprintDataSet
            // 
            this.blueprintDataSet.DataSetName = "BlueprintDataSet";
            this.blueprintDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // exercisesTableAdapter
            // 
            this.exercisesTableAdapter.ClearBeforeFill = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 313);
            this.Controls.Add(this.ProgramDisplay);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.GenerationProgressBar);
            this.Controls.Add(this.PopulationProgressBar);
            this.Controls.Add(this.radioHypertrophy);
            this.Controls.Add(this.radioStrength);
            this.Controls.Add(this.listboxWorkoutType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Workout Blueprint";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgramDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exercisesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueprintDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        

        #endregion

        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listboxWorkoutType;
        private System.Windows.Forms.RadioButton radioStrength;
        private System.Windows.Forms.RadioButton radioHypertrophy;
        private System.Windows.Forms.ProgressBar PopulationProgressBar;
        private System.Windows.Forms.ProgressBar GenerationProgressBar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private BlueprintDataSet blueprintDataSet;
        private System.Windows.Forms.BindingSource exercisesBindingSource;
        private BlueprintDataSetTableAdapters.ExercisesTableAdapter exercisesTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn exerciseDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn muscleGroupDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn specificTargetDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridView ProgramDisplay;
    }
}

