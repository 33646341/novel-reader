namespace OnlineSearchAndRead
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Lv_HomePage = new System.Windows.Forms.ListView();
            this.col_fiction_type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_fiction_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_fiction_author = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_update_chapter = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_update_time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_fiction_stata = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(680, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 33);
            this.button1.TabIndex = 0;
            this.button1.Text = "查找";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(87, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "请输入要查询的关键词";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(273, 33);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(286, 25);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Lv_HomePage
            // 
            this.Lv_HomePage.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_fiction_type,
            this.col_fiction_name,
            this.col_fiction_author,
            this.col_update_chapter,
            this.col_update_time,
            this.col_fiction_stata});
            this.Lv_HomePage.FullRowSelect = true;
            this.Lv_HomePage.HideSelection = false;
            this.Lv_HomePage.Location = new System.Drawing.Point(72, 76);
            this.Lv_HomePage.Name = "Lv_HomePage";
            this.Lv_HomePage.Size = new System.Drawing.Size(675, 371);
            this.Lv_HomePage.TabIndex = 3;
            this.Lv_HomePage.UseCompatibleStateImageBehavior = false;
            this.Lv_HomePage.View = System.Windows.Forms.View.Details;
            this.Lv_HomePage.SelectedIndexChanged += new System.EventHandler(this.Lv_HomePage_SelectedIndexChanged);
            // 
            // col_fiction_type
            // 
            this.col_fiction_type.Text = "作品分类";
            this.col_fiction_type.Width = 100;
            // 
            // col_fiction_name
            // 
            this.col_fiction_name.Text = "作品名称";
            this.col_fiction_name.Width = 150;
            // 
            // col_fiction_author
            // 
            this.col_fiction_author.Text = "作者";
            this.col_fiction_author.Width = 70;
            // 
            // col_update_chapter
            // 
            this.col_update_chapter.Text = "最新章节";
            this.col_update_chapter.Width = 150;
            // 
            // col_update_time
            // 
            this.col_update_time.Text = "更新时间";
            this.col_update_time.Width = 100;
            // 
            // col_fiction_stata
            // 
            this.col_fiction_stata.Text = "状态";
            this.col_fiction_stata.Width = 100;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 553);
            this.Controls.Add(this.Lv_HomePage);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListView Lv_HomePage;
        private System.Windows.Forms.ColumnHeader col_fiction_type;
        private System.Windows.Forms.ColumnHeader col_fiction_name;
        private System.Windows.Forms.ColumnHeader col_fiction_author;
        private System.Windows.Forms.ColumnHeader col_update_chapter;
        private System.Windows.Forms.ColumnHeader col_update_time;
        private System.Windows.Forms.ColumnHeader col_fiction_stata;
    }
}

