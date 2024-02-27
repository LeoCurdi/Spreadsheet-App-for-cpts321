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
            cellGrid = new DataGridView();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)cellGrid).BeginInit();
            SuspendLayout();
            // 
            // cellGrid
            // 
            cellGrid.AllowUserToAddRows = false;
            cellGrid.AllowUserToDeleteRows = false;
            cellGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            cellGrid.Location = new Point(12, 12);
            cellGrid.Name = "cellGrid";
            cellGrid.RowHeadersWidth = 51;
            cellGrid.Size = new Size(1070, 577);
            cellGrid.TabIndex = 0;
            cellGrid.CellValueChanged += cellGrid_CellValueChanged;
            // 
            // button1
            // 
            button1.Location = new Point(12, 595);
            button1.Name = "button1";
            button1.Size = new Size(1070, 29);
            button1.TabIndex = 1;
            button1.Text = "Demo";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1094, 636);
            Controls.Add(button1);
            Controls.Add(cellGrid);
            Name = "Form1";
            Text = "Spreadsheet Cpts 321";
            ((System.ComponentModel.ISupportInitialize)cellGrid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView cellGrid;
        private Button button1;
    }
}
