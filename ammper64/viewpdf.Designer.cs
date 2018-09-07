namespace ammper64
{
    partial class viewpdf
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
            this.namepdf = new System.Windows.Forms.TextBox();
            this.radPdfViewer1 = new Telerik.WinControls.UI.RadPdfViewer();
            ((System.ComponentModel.ISupportInitialize)(this.radPdfViewer1)).BeginInit();
            this.SuspendLayout();
            // 
            // namepdf
            // 
            this.namepdf.Location = new System.Drawing.Point(0, 0);
            this.namepdf.Name = "namepdf";
            this.namepdf.Size = new System.Drawing.Size(100, 20);
            this.namepdf.TabIndex = 1;
            this.namepdf.Visible = false;
            // 
            // radPdfViewer1
            // 
            this.radPdfViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPdfViewer1.Location = new System.Drawing.Point(0, 0);
            this.radPdfViewer1.Name = "radPdfViewer1";
            this.radPdfViewer1.Size = new System.Drawing.Size(800, 450);
            this.radPdfViewer1.TabIndex = 2;
            this.radPdfViewer1.ThumbnailsScaleFactor = 0.15F;
            // 
            // viewpdf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.radPdfViewer1);
            this.Controls.Add(this.namepdf);
            this.Name = "viewpdf";
            this.Text = "Visor PDF";
            this.Load += new System.EventHandler(this.viewpdf_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radPdfViewer1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox namepdf;
        private Telerik.WinControls.UI.RadPdfViewer radPdfViewer1;
    }
}