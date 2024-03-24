namespace Spreadsheet_Leonardo_Curdi {
    partial class Form1 {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            CellGrid = new DataGridView();
            DemoButton = new Button();
            ((System.ComponentModel.ISupportInitialize)CellGrid).BeginInit();
            SuspendLayout();
            // 
            // CellGrid
            // 
            CellGrid.AllowUserToAddRows = false;
            CellGrid.AllowUserToDeleteRows = false;
            CellGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            CellGrid.Location = new Point(12, 12);
            CellGrid.Name = "CellGrid";
            CellGrid.RowHeadersWidth = 51;
            CellGrid.Size = new Size(1070, 577);
            CellGrid.TabIndex = 0;
            CellGrid.CellBeginEdit += CellGrid_CellBeginEdit;
            CellGrid.CellEndEdit += CellGrid_CellEndEdit;
            CellGrid.CellValueChanged += CellGrid_CellValueChanged;
            // 
            // DemoButton
            // 
            DemoButton.Location = new Point(12, 595);
            DemoButton.Name = "DemoButton";
            DemoButton.Size = new Size(1070, 29);
            DemoButton.TabIndex = 1;
            DemoButton.Text = "Demo";
            DemoButton.UseVisualStyleBackColor = true;
            DemoButton.Click += DemoButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1094, 636);
            Controls.Add(DemoButton);
            Controls.Add(CellGrid);
            Name = "Form1";
            Text = "Spreadsheet Cpts 321";
            ((System.ComponentModel.ISupportInitialize)CellGrid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView CellGrid;
        private Button DemoButton;
    }
}
