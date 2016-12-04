namespace SDGrabSharp.UI
{
    partial class CustomGridEntry
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
            this.dgCustomEntry = new System.Windows.Forms.DataGridView();
            this.CustomEntryLineup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomEntryStationID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomEntryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomEntryCustomName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgCustomEntry)).BeginInit();
            this.SuspendLayout();
            // 
            // dgCustomEntry
            // 
            this.dgCustomEntry.AllowUserToAddRows = false;
            this.dgCustomEntry.AllowUserToDeleteRows = false;
            this.dgCustomEntry.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCustomEntry.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CustomEntryLineup,
            this.CustomEntryStationID,
            this.CustomEntryName,
            this.CustomEntryCustomName});
            this.dgCustomEntry.Location = new System.Drawing.Point(12, 12);
            this.dgCustomEntry.Name = "dgCustomEntry";
            this.dgCustomEntry.Size = new System.Drawing.Size(765, 301);
            this.dgCustomEntry.TabIndex = 0;
            this.dgCustomEntry.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgCustomEntry_CellValidated);
            // 
            // CustomEntryLineup
            // 
            this.CustomEntryLineup.HeaderText = "Lineup";
            this.CustomEntryLineup.Name = "CustomEntryLineup";
            this.CustomEntryLineup.ReadOnly = true;
            this.CustomEntryLineup.Width = 150;
            // 
            // CustomEntryStationID
            // 
            this.CustomEntryStationID.HeaderText = "Station ID";
            this.CustomEntryStationID.Name = "CustomEntryStationID";
            this.CustomEntryStationID.ReadOnly = true;
            // 
            // CustomEntryName
            // 
            this.CustomEntryName.HeaderText = "Name";
            this.CustomEntryName.Name = "CustomEntryName";
            this.CustomEntryName.ReadOnly = true;
            this.CustomEntryName.Width = 200;
            // 
            // CustomEntryCustomName
            // 
            this.CustomEntryCustomName.HeaderText = "Custom Name";
            this.CustomEntryCustomName.Name = "CustomEntryCustomName";
            this.CustomEntryCustomName.Width = 250;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(702, 323);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(621, 323);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // CustomGridEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 358);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dgCustomEntry);
            this.Name = "CustomGridEntry";
            this.Text = "Entry Custom Channel names";
            ((System.ComponentModel.ISupportInitialize)(this.dgCustomEntry)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgCustomEntry;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomEntryLineup;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomEntryStationID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomEntryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomEntryCustomName;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}