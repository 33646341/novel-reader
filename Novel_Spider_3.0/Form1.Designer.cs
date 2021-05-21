
namespace Novel_Spider
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
            this.Url_Txt = new System.Windows.Forms.TextBox();
            this.title = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.download = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(300, 193);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 41);
            this.button1.TabIndex = 0;
            this.button1.Text = "下载";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Url_Txt
            // 
            this.Url_Txt.Location = new System.Drawing.Point(231, 147);
            this.Url_Txt.Name = "Url_Txt";
            this.Url_Txt.Size = new System.Drawing.Size(344, 25);
            this.Url_Txt.TabIndex = 1;
            this.Url_Txt.Text = "https://www.biquzhh.com/29719_29719087/";
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.title.Location = new System.Drawing.Point(252, 124);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(309, 20);
            this.title.TabIndex = 2;
            this.title.Text = "请输入小说网址（仅限笔趣阁）：";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(340, 271);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(244, 23);
            this.progressBar1.TabIndex = 3;
            // 
            // download
            // 
            this.download.AutoSize = true;
            this.download.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.download.Location = new System.Drawing.Point(210, 271);
            this.download.Name = "download";
            this.download.Size = new System.Drawing.Size(109, 20);
            this.download.TabIndex = 4;
            this.download.Text = "下载进度：";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("宋体", 12F);
            this.button2.Location = new System.Drawing.Point(445, 194);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 41);
            this.button2.TabIndex = 5;
            this.button2.Text = "暂停";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 477);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.download);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.title);
            this.Controls.Add(this.Url_Txt);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox Url_Txt;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label download;
        private System.Windows.Forms.Button button2;
    }
}

