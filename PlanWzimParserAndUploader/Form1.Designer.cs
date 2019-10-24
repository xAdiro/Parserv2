namespace PlanWzimParserAndUploader
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
            this.lbOldFiles = new System.Windows.Forms.ListBox();
            this.lbNewFiles = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bFilesLoad = new System.Windows.Forms.Button();
            this.bUpload = new System.Windows.Forms.Button();
            this.bDownloadJson = new System.Windows.Forms.Button();
            this.bLoadJson = new System.Windows.Forms.Button();
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.bParse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.bCheckUpdate = new System.Windows.Forms.Button();
            this.ttCheckUpdate = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lbOldFiles
            // 
            this.lbOldFiles.FormattingEnabled = true;
            this.lbOldFiles.Location = new System.Drawing.Point(12, 42);
            this.lbOldFiles.Name = "lbOldFiles";
            this.lbOldFiles.Size = new System.Drawing.Size(454, 212);
            this.lbOldFiles.TabIndex = 0;
            this.lbOldFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LbOldFiles_KeyDown);
            // 
            // lbNewFiles
            // 
            this.lbNewFiles.FormattingEnabled = true;
            this.lbNewFiles.Location = new System.Drawing.Point(12, 273);
            this.lbNewFiles.Name = "lbNewFiles";
            this.lbNewFiles.Size = new System.Drawing.Size(454, 95);
            this.lbNewFiles.TabIndex = 0;
            this.lbNewFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LbNewFiles_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "version";
            // 
            // bFilesLoad
            // 
            this.bFilesLoad.Location = new System.Drawing.Point(9, 381);
            this.bFilesLoad.Name = "bFilesLoad";
            this.bFilesLoad.Size = new System.Drawing.Size(292, 51);
            this.bFilesLoad.TabIndex = 3;
            this.bFilesLoad.Text = "Ładowanie plików";
            this.bFilesLoad.UseVisualStyleBackColor = true;
            this.bFilesLoad.Click += new System.EventHandler(this.BFilesLoad_Click);
            // 
            // bUpload
            // 
            this.bUpload.Enabled = false;
            this.bUpload.Location = new System.Drawing.Point(694, 344);
            this.bUpload.Name = "bUpload";
            this.bUpload.Size = new System.Drawing.Size(94, 88);
            this.bUpload.TabIndex = 4;
            this.bUpload.Text = "UPLOAD";
            this.bUpload.UseVisualStyleBackColor = true;
            this.bUpload.Click += new System.EventHandler(this.BUpload_Click);
            // 
            // bDownloadJson
            // 
            this.bDownloadJson.Location = new System.Drawing.Point(581, 344);
            this.bDownloadJson.Name = "bDownloadJson";
            this.bDownloadJson.Size = new System.Drawing.Size(107, 88);
            this.bDownloadJson.TabIndex = 5;
            this.bDownloadJson.Text = "Pobierz aktualny json z serwera";
            this.bDownloadJson.UseVisualStyleBackColor = true;
            this.bDownloadJson.Click += new System.EventHandler(this.BDownloadJson_Click);
            // 
            // bLoadJson
            // 
            this.bLoadJson.Location = new System.Drawing.Point(472, 344);
            this.bLoadJson.Name = "bLoadJson";
            this.bLoadJson.Size = new System.Drawing.Size(103, 88);
            this.bLoadJson.TabIndex = 6;
            this.bLoadJson.Text = "wgraj jsona bezpośrednio";
            this.bLoadJson.UseVisualStyleBackColor = true;
            this.bLoadJson.Click += new System.EventHandler(this.BLoadJson_Click);
            // 
            // rtbOutput
            // 
            this.rtbOutput.Location = new System.Drawing.Point(472, 29);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.ReadOnly = true;
            this.rtbOutput.Size = new System.Drawing.Size(316, 309);
            this.rtbOutput.TabIndex = 7;
            this.rtbOutput.Text = "";
            // 
            // bParse
            // 
            this.bParse.Location = new System.Drawing.Point(307, 403);
            this.bParse.Name = "bParse";
            this.bParse.Size = new System.Drawing.Size(159, 29);
            this.bParse.TabIndex = 8;
            this.bParse.Text = "Parsuj";
            this.bParse.UseVisualStyleBackColor = true;
            this.bParse.Click += new System.EventHandler(this.BParse_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(472, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "json pochodzi z... ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(153, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(176, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "aktualna wersja planu na serwerze: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "pliki o starym typie: (txt)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 257);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "pliki o nowym typie: (pwzim)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(430, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(431, 257);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "0";
            // 
            // bCheckUpdate
            // 
            this.bCheckUpdate.AccessibleDescription = "";
            this.bCheckUpdate.AccessibleName = "";
            this.bCheckUpdate.BackgroundImage = global::PlanWzimParserAndUploader.Properties.Resources.refresh_double_rounded_flat;
            this.bCheckUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bCheckUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCheckUpdate.ForeColor = System.Drawing.SystemColors.Control;
            this.bCheckUpdate.Location = new System.Drawing.Point(9, 5);
            this.bCheckUpdate.Name = "bCheckUpdate";
            this.bCheckUpdate.Size = new System.Drawing.Size(25, 25);
            this.bCheckUpdate.TabIndex = 15;
            this.ttCheckUpdate.SetToolTip(this.bCheckUpdate, "Sprawdź aktualizacje");
            this.bCheckUpdate.UseVisualStyleBackColor = true;
            this.bCheckUpdate.Click += new System.EventHandler(this.BCheckUpdate_Click);
            // 
            // ttCheckUpdate
            // 
            this.ttCheckUpdate.Tag = "";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox1.Location = new System.Drawing.Point(307, 381);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(159, 17);
            this.checkBox1.TabIndex = 17;
            this.checkBox1.Text = "nadpisz date na teraźniejszą";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Click += new System.EventHandler(this.CheckBox1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 446);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.bCheckUpdate);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bParse);
            this.Controls.Add(this.rtbOutput);
            this.Controls.Add(this.bLoadJson);
            this.Controls.Add(this.bDownloadJson);
            this.Controls.Add(this.bUpload);
            this.Controls.Add(this.bFilesLoad);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbNewFiles);
            this.Controls.Add(this.lbOldFiles);
            this.Name = "Form1";
            this.Text = "Plan WZIM Parser i Uploader";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbOldFiles;
        private System.Windows.Forms.ListBox lbNewFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bFilesLoad;
        private System.Windows.Forms.Button bUpload;
        private System.Windows.Forms.Button bDownloadJson;
        private System.Windows.Forms.Button bLoadJson;
        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.Button bParse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button bCheckUpdate;
        private System.Windows.Forms.ToolTip ttCheckUpdate;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

