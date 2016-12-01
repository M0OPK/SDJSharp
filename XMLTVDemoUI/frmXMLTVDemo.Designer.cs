namespace XMLTVDemoUI
{
    partial class frmXMLTVDemo
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
            this.btnXmlLoad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnXmlLoad
            // 
            this.btnXmlLoad.Location = new System.Drawing.Point(40, 28);
            this.btnXmlLoad.Name = "btnXmlLoad";
            this.btnXmlLoad.Size = new System.Drawing.Size(94, 28);
            this.btnXmlLoad.TabIndex = 0;
            this.btnXmlLoad.Text = "Load XML";
            this.btnXmlLoad.UseVisualStyleBackColor = true;
            this.btnXmlLoad.Click += new System.EventHandler(this.btnXmlLoad_Click);
            // 
            // frmXMLTVDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 318);
            this.Controls.Add(this.btnXmlLoad);
            this.Name = "frmXMLTVDemo";
            this.Text = "XMLTV Demo";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnXmlLoad;
    }
}

