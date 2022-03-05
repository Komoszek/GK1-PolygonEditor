
namespace GK1_PolygonEditor
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.selectFigureButton = new System.Windows.Forms.Button();
            this.selectElementButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.deleteFigureButton = new System.Windows.Forms.Button();
            this.deleteElementButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.addPolygonButton = new System.Windows.Forms.Button();
            this.addCircleButton = new System.Windows.Forms.Button();
            this.splitEdgeButton = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.circleTangentButton = new System.Windows.Forms.Button();
            this.parallelEdgesButton = new System.Windows.Forms.Button();
            this.removeConstraintButton = new System.Windows.Forms.Button();
            this.equalEdgesButton = new System.Windows.Forms.Button();
            this.blockCenterButton = new System.Windows.Forms.Button();
            this.constantRadiusButton = new System.Windows.Forms.Button();
            this.ConstantEdgeButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoCircleTangentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Size = new System.Drawing.Size(800, 426);
            this.splitContainer1.SplitterDistance = 250;
            this.splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 128F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(250, 426);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.selectFigureButton);
            this.groupBox2.Controls.Add(this.selectElementButton);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(244, 64);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Select Tools";
            // 
            // selectFigureButton
            // 
            this.selectFigureButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.selectFigureButton.AutoSize = true;
            this.selectFigureButton.Location = new System.Drawing.Point(143, 22);
            this.selectFigureButton.Name = "selectFigureButton";
            this.selectFigureButton.Size = new System.Drawing.Size(84, 25);
            this.selectFigureButton.TabIndex = 1;
            this.selectFigureButton.Text = "Select Figure";
            this.selectFigureButton.UseVisualStyleBackColor = true;
            this.selectFigureButton.Click += new System.EventHandler(this.selectFigureButton_Click);
            // 
            // selectElementButton
            // 
            this.selectElementButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.selectElementButton.AutoSize = true;
            this.selectElementButton.Location = new System.Drawing.Point(11, 22);
            this.selectElementButton.Name = "selectElementButton";
            this.selectElementButton.Size = new System.Drawing.Size(94, 25);
            this.selectElementButton.TabIndex = 0;
            this.selectElementButton.Text = "Select element";
            this.selectElementButton.UseVisualStyleBackColor = true;
            this.selectElementButton.Click += new System.EventHandler(this.selectElementButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.deleteFigureButton);
            this.groupBox3.Controls.Add(this.deleteElementButton);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 168);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(244, 60);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "DeleteTools";
            // 
            // deleteFigureButton
            // 
            this.deleteFigureButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.deleteFigureButton.AutoSize = true;
            this.deleteFigureButton.Location = new System.Drawing.Point(143, 22);
            this.deleteFigureButton.Name = "deleteFigureButton";
            this.deleteFigureButton.Size = new System.Drawing.Size(86, 25);
            this.deleteFigureButton.TabIndex = 6;
            this.deleteFigureButton.Text = "Delete Figure";
            this.deleteFigureButton.UseVisualStyleBackColor = true;
            this.deleteFigureButton.Click += new System.EventHandler(this.deleteFigureButton_Click);
            // 
            // deleteElementButton
            // 
            this.deleteElementButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.deleteElementButton.AutoSize = true;
            this.deleteElementButton.Location = new System.Drawing.Point(9, 22);
            this.deleteElementButton.Name = "deleteElementButton";
            this.deleteElementButton.Size = new System.Drawing.Size(96, 25);
            this.deleteElementButton.TabIndex = 5;
            this.deleteElementButton.Text = "Delete Element";
            this.deleteElementButton.UseVisualStyleBackColor = true;
            this.deleteElementButton.Click += new System.EventHandler(this.deleteElementButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.addPolygonButton);
            this.groupBox1.Controls.Add(this.addCircleButton);
            this.groupBox1.Controls.Add(this.splitEdgeButton);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(244, 89);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add Tools";
            // 
            // addPolygonButton
            // 
            this.addPolygonButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.addPolygonButton.AutoSize = true;
            this.addPolygonButton.Location = new System.Drawing.Point(9, 22);
            this.addPolygonButton.Name = "addPolygonButton";
            this.addPolygonButton.Size = new System.Drawing.Size(86, 25);
            this.addPolygonButton.TabIndex = 2;
            this.addPolygonButton.Text = "Add Polygon";
            this.addPolygonButton.UseVisualStyleBackColor = true;
            this.addPolygonButton.Click += new System.EventHandler(this.addPolygonButton_Click);
            // 
            // addCircleButton
            // 
            this.addCircleButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.addCircleButton.AutoSize = true;
            this.addCircleButton.Location = new System.Drawing.Point(152, 22);
            this.addCircleButton.Name = "addCircleButton";
            this.addCircleButton.Size = new System.Drawing.Size(75, 25);
            this.addCircleButton.TabIndex = 3;
            this.addCircleButton.Text = "Add Circle";
            this.addCircleButton.UseVisualStyleBackColor = true;
            this.addCircleButton.Click += new System.EventHandler(this.addCircleButton_Click);
            // 
            // splitEdgeButton
            // 
            this.splitEdgeButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.splitEdgeButton.AutoSize = true;
            this.splitEdgeButton.Location = new System.Drawing.Point(9, 53);
            this.splitEdgeButton.Name = "splitEdgeButton";
            this.splitEdgeButton.Size = new System.Drawing.Size(69, 25);
            this.splitEdgeButton.TabIndex = 4;
            this.splitEdgeButton.Text = "Split Edge";
            this.splitEdgeButton.UseVisualStyleBackColor = true;
            this.splitEdgeButton.Click += new System.EventHandler(this.splitEdgeButton_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.circleTangentButton);
            this.groupBox4.Controls.Add(this.parallelEdgesButton);
            this.groupBox4.Controls.Add(this.removeConstraintButton);
            this.groupBox4.Controls.Add(this.equalEdgesButton);
            this.groupBox4.Controls.Add(this.blockCenterButton);
            this.groupBox4.Controls.Add(this.constantRadiusButton);
            this.groupBox4.Controls.Add(this.ConstantEdgeButton);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 234);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(244, 189);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Constraint Tools";
            // 
            // circleTangentButton
            // 
            this.circleTangentButton.AutoSize = true;
            this.circleTangentButton.Location = new System.Drawing.Point(137, 84);
            this.circleTangentButton.Name = "circleTangentButton";
            this.circleTangentButton.Size = new System.Drawing.Size(92, 25);
            this.circleTangentButton.TabIndex = 12;
            this.circleTangentButton.Text = "Circle Tangent";
            this.circleTangentButton.UseVisualStyleBackColor = true;
            this.circleTangentButton.Click += new System.EventHandler(this.circleTangentButton_Click);
            // 
            // parallelEdgesButton
            // 
            this.parallelEdgesButton.AutoSize = true;
            this.parallelEdgesButton.Location = new System.Drawing.Point(140, 53);
            this.parallelEdgesButton.Name = "parallelEdgesButton";
            this.parallelEdgesButton.Size = new System.Drawing.Size(89, 25);
            this.parallelEdgesButton.TabIndex = 10;
            this.parallelEdgesButton.Text = "Parallel Edges";
            this.parallelEdgesButton.UseVisualStyleBackColor = true;
            this.parallelEdgesButton.Click += new System.EventHandler(this.parallelEdgesButton_Click);
            // 
            // removeConstraintButton
            // 
            this.removeConstraintButton.AutoSize = true;
            this.removeConstraintButton.Location = new System.Drawing.Point(62, 115);
            this.removeConstraintButton.Name = "removeConstraintButton";
            this.removeConstraintButton.Size = new System.Drawing.Size(118, 25);
            this.removeConstraintButton.TabIndex = 13;
            this.removeConstraintButton.Text = "Remove Constraint";
            this.removeConstraintButton.UseVisualStyleBackColor = true;
            this.removeConstraintButton.Click += new System.EventHandler(this.removeConstraintButton_Click);
            // 
            // equalEdgesButton
            // 
            this.equalEdgesButton.AutoSize = true;
            this.equalEdgesButton.Location = new System.Drawing.Point(145, 22);
            this.equalEdgesButton.Name = "equalEdgesButton";
            this.equalEdgesButton.Size = new System.Drawing.Size(80, 25);
            this.equalEdgesButton.TabIndex = 8;
            this.equalEdgesButton.Text = "Equal Edges";
            this.equalEdgesButton.UseVisualStyleBackColor = true;
            this.equalEdgesButton.Click += new System.EventHandler(this.equalEdgesButton_Click);
            // 
            // blockCenterButton
            // 
            this.blockCenterButton.AutoSize = true;
            this.blockCenterButton.Location = new System.Drawing.Point(8, 53);
            this.blockCenterButton.Name = "blockCenterButton";
            this.blockCenterButton.Size = new System.Drawing.Size(82, 25);
            this.blockCenterButton.TabIndex = 9;
            this.blockCenterButton.Text = "Block center";
            this.blockCenterButton.UseVisualStyleBackColor = true;
            this.blockCenterButton.Click += new System.EventHandler(this.blockCenterButton_Click);
            // 
            // constantRadiusButton
            // 
            this.constantRadiusButton.AutoSize = true;
            this.constantRadiusButton.Location = new System.Drawing.Point(8, 84);
            this.constantRadiusButton.Name = "constantRadiusButton";
            this.constantRadiusButton.Size = new System.Drawing.Size(103, 25);
            this.constantRadiusButton.TabIndex = 11;
            this.constantRadiusButton.Text = "Constant Radius";
            this.constantRadiusButton.UseVisualStyleBackColor = true;
            this.constantRadiusButton.Click += new System.EventHandler(this.constantRadiusButton_Click);
            // 
            // ConstantEdgeButton
            // 
            this.ConstantEdgeButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConstantEdgeButton.AutoSize = true;
            this.ConstantEdgeButton.Location = new System.Drawing.Point(8, 22);
            this.ConstantEdgeButton.Name = "ConstantEdgeButton";
            this.ConstantEdgeButton.Size = new System.Drawing.Size(94, 25);
            this.ConstantEdgeButton.TabIndex = 7;
            this.ConstantEdgeButton.Text = "Constant Edge";
            this.ConstantEdgeButton.UseVisualStyleBackColor = true;
            this.ConstantEdgeButton.Click += new System.EventHandler(this.ConstantEdgeButton_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(546, 426);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.LoadCompleted += new System.ComponentModel.AsyncCompletedEventHandler(this.pictureBox1_LoadCompleted);
            this.pictureBox1.SizeChanged += new System.EventHandler(this.pictureBox1_SizeChanged);
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.DoubleClick += new System.EventHandler(this.pictureBox1_DoubleClick);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(800, 24);
            this.menuStrip2.TabIndex = 1;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(95, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoCircleTangentToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // autoCircleTangentToolStripMenuItem
            // 
            this.autoCircleTangentToolStripMenuItem.CheckOnClick = true;
            this.autoCircleTangentToolStripMenuItem.Name = "autoCircleTangentToolStripMenuItem";
            this.autoCircleTangentToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.autoCircleTangentToolStripMenuItem.Text = "Auto Circle Tangent";
            this.autoCircleTangentToolStripMenuItem.Click += new System.EventHandler(this.autoCircleTangentToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip2);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "GK1-PolygonEditor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button selectElementButton;
        private System.Windows.Forms.Button deleteElementButton;
        private System.Windows.Forms.Button splitEdgeButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button selectFigureButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button deleteFigureButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button addPolygonButton;
        private System.Windows.Forms.Button addCircleButton;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button parallelEdgesButton;
        private System.Windows.Forms.Button equalEdgesButton;
        private System.Windows.Forms.Button removeConstraintButton;
        private System.Windows.Forms.Button blockCenterButton;
        private System.Windows.Forms.Button constantRadiusButton;
        private System.Windows.Forms.Button ConstantEdgeButton;
        private System.Windows.Forms.Button circleTangentButton;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoCircleTangentToolStripMenuItem;
    }
}

