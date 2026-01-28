namespace faceted_glass_plugin
{
    partial class GUI
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxMainMenu = new System.Windows.Forms.PictureBox();
            this.groupBoxParameters = new System.Windows.Forms.GroupBox();
            this.buttonExportSTL = new System.Windows.Forms.Button();
            this.labelLimitNumberOfEdges = new System.Windows.Forms.Label();
            this.labelLimitHeightUpperEdge = new System.Windows.Forms.Label();
            this.labelLimitThicknessUpperEdge = new System.Windows.Forms.Label();
            this.labelLimitThicknessLowerEdge = new System.Windows.Forms.Label();
            this.labelLimitHeightBottom = new System.Windows.Forms.Label();
            this.labelLimitRadius = new System.Windows.Forms.Label();
            this.labelLimitHeightTotal = new System.Windows.Forms.Label();
            this.textBoxError = new System.Windows.Forms.TextBox();
            this.buttonBuildFacetedGlass = new System.Windows.Forms.Button();
            this.labelNumberOfEdge = new System.Windows.Forms.Label();
            this.labelHeightUpperEdge = new System.Windows.Forms.Label();
            this.labelThicknessUpperEdge = new System.Windows.Forms.Label();
            this.labelThicknessLowerEdge = new System.Windows.Forms.Label();
            this.labelHeightBottom = new System.Windows.Forms.Label();
            this.textBoxNumberOfEdge = new System.Windows.Forms.TextBox();
            this.textBoxHeightUpperEdge = new System.Windows.Forms.TextBox();
            this.textBoxThicknessUpperEdge = new System.Windows.Forms.TextBox();
            this.textBoxThicknessLowerEdge = new System.Windows.Forms.TextBox();
            this.textBoxHeightBottom = new System.Windows.Forms.TextBox();
            this.textBoxRadius = new System.Windows.Forms.TextBox();
            this.textBoxHeightTotal = new System.Windows.Forms.TextBox();
            this.labelRadius = new System.Windows.Forms.Label();
            this.labelHeightTotal = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMainMenu)).BeginInit();
            this.groupBoxParameters.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxMainMenu
            // 
            this.pictureBoxMainMenu.Image = global::faceted_glass_plugin.Properties.Resources.Circuit;
            this.pictureBoxMainMenu.Location = new System.Drawing.Point(631, 15);
            this.pictureBoxMainMenu.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxMainMenu.Name = "pictureBoxMainMenu";
            this.pictureBoxMainMenu.Size = new System.Drawing.Size(609, 363);
            this.pictureBoxMainMenu.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxMainMenu.TabIndex = 0;
            this.pictureBoxMainMenu.TabStop = false;
            // 
            // groupBoxParameters
            // 
            this.groupBoxParameters.Controls.Add(this.labelLimitNumberOfEdges);
            this.groupBoxParameters.Controls.Add(this.labelLimitHeightUpperEdge);
            this.groupBoxParameters.Controls.Add(this.labelLimitThicknessUpperEdge);
            this.groupBoxParameters.Controls.Add(this.labelLimitThicknessLowerEdge);
            this.groupBoxParameters.Controls.Add(this.labelLimitHeightBottom);
            this.groupBoxParameters.Controls.Add(this.labelLimitRadius);
            this.groupBoxParameters.Controls.Add(this.labelLimitHeightTotal);
            this.groupBoxParameters.Controls.Add(this.textBoxError);
            this.groupBoxParameters.Controls.Add(this.buttonBuildFacetedGlass);
            this.groupBoxParameters.Controls.Add(this.labelNumberOfEdge);
            this.groupBoxParameters.Controls.Add(this.labelHeightUpperEdge);
            this.groupBoxParameters.Controls.Add(this.labelThicknessUpperEdge);
            this.groupBoxParameters.Controls.Add(this.labelThicknessLowerEdge);
            this.groupBoxParameters.Controls.Add(this.labelHeightBottom);
            this.groupBoxParameters.Controls.Add(this.textBoxNumberOfEdge);
            this.groupBoxParameters.Controls.Add(this.textBoxHeightUpperEdge);
            this.groupBoxParameters.Controls.Add(this.textBoxThicknessUpperEdge);
            this.groupBoxParameters.Controls.Add(this.textBoxThicknessLowerEdge);
            this.groupBoxParameters.Controls.Add(this.textBoxHeightBottom);
            this.groupBoxParameters.Controls.Add(this.textBoxRadius);
            this.groupBoxParameters.Controls.Add(this.textBoxHeightTotal);
            this.groupBoxParameters.Controls.Add(this.labelRadius);
            this.groupBoxParameters.Controls.Add(this.labelHeightTotal);
            this.groupBoxParameters.Location = new System.Drawing.Point(16, 7);
            this.groupBoxParameters.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxParameters.Name = "groupBoxParameters";
            this.groupBoxParameters.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxParameters.Size = new System.Drawing.Size(607, 370);
            this.groupBoxParameters.TabIndex = 1;
            this.groupBoxParameters.TabStop = false;
            this.groupBoxParameters.Text = "Входные параметры";
            // 
            // buttonExportSTL
            // 
            this.buttonExportSTL.Location = new System.Drawing.Point(1204, 320);
            this.buttonExportSTL.Name = "buttonExportSTL";
            this.buttonExportSTL.Size = new System.Drawing.Size(27, 43);
            this.buttonExportSTL.TabIndex = 23;
            this.buttonExportSTL.Text = "Построить и сохранить в формате .stl";
            this.buttonExportSTL.UseVisualStyleBackColor = true;
            this.buttonExportSTL.Visible = false;
            this.buttonExportSTL.Click += new System.EventHandler(this.buttonExportSTL_Click);
            // 
            // labelLimitNumberOfEdges
            // 
            this.labelLimitNumberOfEdges.AutoSize = true;
            this.labelLimitNumberOfEdges.Location = new System.Drawing.Point(455, 236);
            this.labelLimitNumberOfEdges.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLimitNumberOfEdges.Name = "labelLimitNumberOfEdges";
            this.labelLimitNumberOfEdges.Size = new System.Drawing.Size(44, 16);
            this.labelLimitNumberOfEdges.TabIndex = 22;
            this.labelLimitNumberOfEdges.Text = "label1";
            // 
            // labelLimitHeightUpperEdge
            // 
            this.labelLimitHeightUpperEdge.AutoSize = true;
            this.labelLimitHeightUpperEdge.Location = new System.Drawing.Point(455, 204);
            this.labelLimitHeightUpperEdge.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLimitHeightUpperEdge.Name = "labelLimitHeightUpperEdge";
            this.labelLimitHeightUpperEdge.Size = new System.Drawing.Size(44, 16);
            this.labelLimitHeightUpperEdge.TabIndex = 21;
            this.labelLimitHeightUpperEdge.Text = "label1";
            // 
            // labelLimitThicknessUpperEdge
            // 
            this.labelLimitThicknessUpperEdge.AutoSize = true;
            this.labelLimitThicknessUpperEdge.Location = new System.Drawing.Point(455, 172);
            this.labelLimitThicknessUpperEdge.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLimitThicknessUpperEdge.Name = "labelLimitThicknessUpperEdge";
            this.labelLimitThicknessUpperEdge.Size = new System.Drawing.Size(44, 16);
            this.labelLimitThicknessUpperEdge.TabIndex = 20;
            this.labelLimitThicknessUpperEdge.Text = "label1";
            // 
            // labelLimitThicknessLowerEdge
            // 
            this.labelLimitThicknessLowerEdge.AutoSize = true;
            this.labelLimitThicknessLowerEdge.Location = new System.Drawing.Point(455, 140);
            this.labelLimitThicknessLowerEdge.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLimitThicknessLowerEdge.Name = "labelLimitThicknessLowerEdge";
            this.labelLimitThicknessLowerEdge.Size = new System.Drawing.Size(44, 16);
            this.labelLimitThicknessLowerEdge.TabIndex = 19;
            this.labelLimitThicknessLowerEdge.Text = "label1";
            // 
            // labelLimitHeightBottom
            // 
            this.labelLimitHeightBottom.AutoSize = true;
            this.labelLimitHeightBottom.Location = new System.Drawing.Point(455, 108);
            this.labelLimitHeightBottom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLimitHeightBottom.Name = "labelLimitHeightBottom";
            this.labelLimitHeightBottom.Size = new System.Drawing.Size(44, 16);
            this.labelLimitHeightBottom.TabIndex = 18;
            this.labelLimitHeightBottom.Text = "label1";
            // 
            // labelLimitRadius
            // 
            this.labelLimitRadius.AutoSize = true;
            this.labelLimitRadius.Location = new System.Drawing.Point(455, 76);
            this.labelLimitRadius.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLimitRadius.Name = "labelLimitRadius";
            this.labelLimitRadius.Size = new System.Drawing.Size(44, 16);
            this.labelLimitRadius.TabIndex = 17;
            this.labelLimitRadius.Text = "label1";
            // 
            // labelLimitHeightTotal
            // 
            this.labelLimitHeightTotal.AutoSize = true;
            this.labelLimitHeightTotal.Location = new System.Drawing.Point(455, 44);
            this.labelLimitHeightTotal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLimitHeightTotal.Name = "labelLimitHeightTotal";
            this.labelLimitHeightTotal.Size = new System.Drawing.Size(44, 16);
            this.labelLimitHeightTotal.TabIndex = 16;
            this.labelLimitHeightTotal.Text = "label1";
            // 
            // textBoxError
            // 
            this.textBoxError.Location = new System.Drawing.Point(12, 310);
            this.textBoxError.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxError.Multiline = true;
            this.textBoxError.Name = "textBoxError";
            this.textBoxError.Size = new System.Drawing.Size(585, 46);
            this.textBoxError.TabIndex = 15;
            this.textBoxError.Text = "Поле для ошибок.";
            // 
            // buttonBuildFacetedGlass
            // 
            this.buttonBuildFacetedGlass.Location = new System.Drawing.Point(12, 268);
            this.buttonBuildFacetedGlass.Margin = new System.Windows.Forms.Padding(4);
            this.buttonBuildFacetedGlass.Name = "buttonBuildFacetedGlass";
            this.buttonBuildFacetedGlass.Size = new System.Drawing.Size(585, 37);
            this.buttonBuildFacetedGlass.TabIndex = 14;
            this.buttonBuildFacetedGlass.Text = "Построить ";
            this.buttonBuildFacetedGlass.UseVisualStyleBackColor = true;
            this.buttonBuildFacetedGlass.Click += new System.EventHandler(this.buttonBuildFacetedGlass_Click);
            // 
            // labelNumberOfEdge
            // 
            this.labelNumberOfEdge.AutoSize = true;
            this.labelNumberOfEdge.Location = new System.Drawing.Point(8, 236);
            this.labelNumberOfEdge.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNumberOfEdge.Name = "labelNumberOfEdge";
            this.labelNumberOfEdge.Size = new System.Drawing.Size(134, 16);
            this.labelNumberOfEdge.TabIndex = 13;
            this.labelNumberOfEdge.Text = "Количество граней";
            // 
            // labelHeightUpperEdge
            // 
            this.labelHeightUpperEdge.AutoSize = true;
            this.labelHeightUpperEdge.Location = new System.Drawing.Point(8, 204);
            this.labelHeightUpperEdge.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelHeightUpperEdge.Name = "labelHeightUpperEdge";
            this.labelHeightUpperEdge.Size = new System.Drawing.Size(278, 16);
            this.labelHeightUpperEdge.TabIndex = 12;
            this.labelHeightUpperEdge.Text = "Высота верхней грани (height_UpperEdge)";
            // 
            // labelThicknessUpperEdge
            // 
            this.labelThicknessUpperEdge.AutoSize = true;
            this.labelThicknessUpperEdge.Location = new System.Drawing.Point(8, 172);
            this.labelThicknessUpperEdge.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelThicknessUpperEdge.Name = "labelThicknessUpperEdge";
            this.labelThicknessUpperEdge.Size = new System.Drawing.Size(315, 16);
            this.labelThicknessUpperEdge.TabIndex = 11;
            this.labelThicknessUpperEdge.Text = "Толщина верхней стенки (thickness_UpperEdge)";
            // 
            // labelThicknessLowerEdge
            // 
            this.labelThicknessLowerEdge.AutoSize = true;
            this.labelThicknessLowerEdge.Location = new System.Drawing.Point(8, 140);
            this.labelThicknessLowerEdge.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelThicknessLowerEdge.Name = "labelThicknessLowerEdge";
            this.labelThicknessLowerEdge.Size = new System.Drawing.Size(308, 16);
            this.labelThicknessLowerEdge.TabIndex = 10;
            this.labelThicknessLowerEdge.Text = "Толщина нижней стенки (thickness_LowerEdge)\r\n";
            // 
            // labelHeightBottom
            // 
            this.labelHeightBottom.AutoSize = true;
            this.labelHeightBottom.Location = new System.Drawing.Point(8, 108);
            this.labelHeightBottom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelHeightBottom.Name = "labelHeightBottom";
            this.labelHeightBottom.Size = new System.Drawing.Size(178, 16);
            this.labelHeightBottom.TabIndex = 9;
            this.labelHeightBottom.Text = "Высота дна (height_Bottom)";
            // 
            // textBoxNumberOfEdge
            // 
            this.textBoxNumberOfEdge.Location = new System.Drawing.Point(349, 233);
            this.textBoxNumberOfEdge.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxNumberOfEdge.Name = "textBoxNumberOfEdge";
            this.textBoxNumberOfEdge.Size = new System.Drawing.Size(96, 22);
            this.textBoxNumberOfEdge.TabIndex = 8;
            this.textBoxNumberOfEdge.TextChanged += new System.EventHandler(this.textBoxNumberOfEdge_Leave);
            // 
            // textBoxHeightUpperEdge
            // 
            this.textBoxHeightUpperEdge.Location = new System.Drawing.Point(349, 201);
            this.textBoxHeightUpperEdge.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxHeightUpperEdge.Name = "textBoxHeightUpperEdge";
            this.textBoxHeightUpperEdge.Size = new System.Drawing.Size(96, 22);
            this.textBoxHeightUpperEdge.TabIndex = 7;
            this.textBoxHeightUpperEdge.TextChanged += new System.EventHandler(this.textBoxHeightUpperEdge_Leave);
            // 
            // textBoxThicknessUpperEdge
            // 
            this.textBoxThicknessUpperEdge.Location = new System.Drawing.Point(349, 169);
            this.textBoxThicknessUpperEdge.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxThicknessUpperEdge.Name = "textBoxThicknessUpperEdge";
            this.textBoxThicknessUpperEdge.Size = new System.Drawing.Size(96, 22);
            this.textBoxThicknessUpperEdge.TabIndex = 6;
            this.textBoxThicknessUpperEdge.TextChanged += new System.EventHandler(this.textBoxThicknessUpperEdge_Leave);
            // 
            // textBoxThicknessLowerEdge
            // 
            this.textBoxThicknessLowerEdge.Location = new System.Drawing.Point(349, 137);
            this.textBoxThicknessLowerEdge.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxThicknessLowerEdge.Name = "textBoxThicknessLowerEdge";
            this.textBoxThicknessLowerEdge.Size = new System.Drawing.Size(96, 22);
            this.textBoxThicknessLowerEdge.TabIndex = 5;
            this.textBoxThicknessLowerEdge.TextChanged += new System.EventHandler(this.textBoxThicknessLowerEdge_Leave);
            // 
            // textBoxHeightBottom
            // 
            this.textBoxHeightBottom.Location = new System.Drawing.Point(349, 105);
            this.textBoxHeightBottom.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxHeightBottom.Name = "textBoxHeightBottom";
            this.textBoxHeightBottom.Size = new System.Drawing.Size(96, 22);
            this.textBoxHeightBottom.TabIndex = 4;
            this.textBoxHeightBottom.TextChanged += new System.EventHandler(this.textBoxHeightBottom_Leave);
            // 
            // textBoxRadius
            // 
            this.textBoxRadius.Location = new System.Drawing.Point(349, 73);
            this.textBoxRadius.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxRadius.Name = "textBoxRadius";
            this.textBoxRadius.Size = new System.Drawing.Size(96, 22);
            this.textBoxRadius.TabIndex = 3;
            this.textBoxRadius.TextChanged += new System.EventHandler(this.textBoxRadius_Leave);
            // 
            // textBoxHeightTotal
            // 
            this.textBoxHeightTotal.Location = new System.Drawing.Point(349, 41);
            this.textBoxHeightTotal.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxHeightTotal.Name = "textBoxHeightTotal";
            this.textBoxHeightTotal.Size = new System.Drawing.Size(96, 22);
            this.textBoxHeightTotal.TabIndex = 2;
            this.textBoxHeightTotal.TextChanged += new System.EventHandler(this.textBoxHeightTotal_Leave);
            // 
            // labelRadius
            // 
            this.labelRadius.AutoSize = true;
            this.labelRadius.Location = new System.Drawing.Point(8, 76);
            this.labelRadius.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRadius.Name = "labelRadius";
            this.labelRadius.Size = new System.Drawing.Size(244, 16);
            this.labelRadius.TabIndex = 1;
            this.labelRadius.Text = "Радиус внешней окружности (radius)\r\n";
            // 
            // labelHeightTotal
            // 
            this.labelHeightTotal.AutoSize = true;
            this.labelHeightTotal.Location = new System.Drawing.Point(8, 44);
            this.labelHeightTotal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelHeightTotal.Name = "labelHeightTotal";
            this.labelHeightTotal.Size = new System.Drawing.Size(196, 16);
            this.labelHeightTotal.TabIndex = 0;
            this.labelHeightTotal.Text = "Высота стакана (height_Total)";
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1243, 373);
            this.Controls.Add(this.buttonExportSTL);
            this.Controls.Add(this.groupBoxParameters);
            this.Controls.Add(this.pictureBoxMainMenu);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(1261, 420);
            this.MinimumSize = new System.Drawing.Size(1261, 420);
            this.Name = "GUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Построитель гранёного стакана";
            this.Load += new System.EventHandler(this.GUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMainMenu)).EndInit();
            this.groupBoxParameters.ResumeLayout(false);
            this.groupBoxParameters.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxMainMenu;
        private System.Windows.Forms.GroupBox groupBoxParameters;
        private System.Windows.Forms.TextBox textBoxThicknessLowerEdge;
        private System.Windows.Forms.TextBox textBoxHeightBottom;
        private System.Windows.Forms.TextBox textBoxRadius;
        private System.Windows.Forms.TextBox textBoxHeightTotal;
        private System.Windows.Forms.Label labelRadius;
        private System.Windows.Forms.Label labelHeightTotal;
        private System.Windows.Forms.Label labelThicknessLowerEdge;
        private System.Windows.Forms.Label labelHeightBottom;
        private System.Windows.Forms.TextBox textBoxNumberOfEdge;
        private System.Windows.Forms.TextBox textBoxHeightUpperEdge;
        private System.Windows.Forms.TextBox textBoxThicknessUpperEdge;
        private System.Windows.Forms.Button buttonBuildFacetedGlass;
        private System.Windows.Forms.Label labelNumberOfEdge;
        private System.Windows.Forms.Label labelHeightUpperEdge;
        private System.Windows.Forms.Label labelThicknessUpperEdge;
        private System.Windows.Forms.TextBox textBoxError;
        private System.Windows.Forms.Label labelLimitNumberOfEdges;
        private System.Windows.Forms.Label labelLimitHeightUpperEdge;
        private System.Windows.Forms.Label labelLimitThicknessUpperEdge;
        private System.Windows.Forms.Label labelLimitThicknessLowerEdge;
        private System.Windows.Forms.Label labelLimitHeightBottom;
        private System.Windows.Forms.Label labelLimitRadius;
        private System.Windows.Forms.Label labelLimitHeightTotal;
        private System.Windows.Forms.Button buttonExportSTL;
    }
}

