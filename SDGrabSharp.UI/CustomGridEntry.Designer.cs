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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbControl = new System.Windows.Forms.GroupBox();
            this.gbCustomEntry = new System.Windows.Forms.GroupBox();
            this.dgCustomEntry = new System.Windows.Forms.DataGridView();
            this.CustomEntryLineup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomEntryStationID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChanNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LogicChanNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomEntryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomEntryCustomName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbControl.SuspendLayout();
            this.gbCustomEntry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgCustomEntry)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(711, 14);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(630, 14);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gbControl
            // 
            this.gbControl.Controls.Add(this.btnOK);
            this.gbControl.Controls.Add(this.btnCancel);
            this.gbControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbControl.Location = new System.Drawing.Point(0, 354);
            this.gbControl.Name = "gbControl";
            this.gbControl.Size = new System.Drawing.Size(792, 43);
            this.gbControl.TabIndex = 3;
            this.gbControl.TabStop = false;
            // 
            // gbCustomEntry
            // 
            this.gbCustomEntry.Controls.Add(this.dgCustomEntry);
            this.gbCustomEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbCustomEntry.Location = new System.Drawing.Point(0, 0);
            this.gbCustomEntry.Name = "gbCustomEntry";
            this.gbCustomEntry.Size = new System.Drawing.Size(792, 354);
            this.gbCustomEntry.TabIndex = 4;
            this.gbCustomEntry.TabStop = false;
            this.gbCustomEntry.Text = "Custom Channel Entry";
            // 
            // dgCustomEntry
            // 
            this.dgCustomEntry.AllowUserToAddRows = false;
            this.dgCustomEntry.AllowUserToDeleteRows = false;
            this.dgCustomEntry.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgCustomEntry.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCustomEntry.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CustomEntryLineup,
            this.CustomEntryStationID,
            this.ChanNum,
            this.LogicChanNum,
            this.CustomEntryName,
            this.CustomEntryCustomName});
            this.dgCustomEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgCustomEntry.Location = new System.Drawing.Point(3, 16);
            this.dgCustomEntry.Name = "dgCustomEntry";
            this.dgCustomEntry.Size = new System.Drawing.Size(786, 335);
            this.dgCustomEntry.TabIndex = 1;
            this.dgCustomEntry.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgCustomEntry_CellEnter);
            // 
            // CustomEntryLineup
            // 
            this.CustomEntryLineup.FillWeight = 10F;
            this.CustomEntryLineup.HeaderText = "Lineup";
            this.CustomEntryLineup.Name = "CustomEntryLineup";
            this.CustomEntryLineup.ReadOnly = true;
            // 
            // CustomEntryStationID
            // 
            this.CustomEntryStationID.FillWeight = 5F;
            this.CustomEntryStationID.HeaderText = "Station ID";
            this.CustomEntryStationID.Name = "CustomEntryStationID";
            this.CustomEntryStationID.ReadOnly = true;
            // 
            // ChanNum
            // 
            this.ChanNum.FillWeight = 5F;
            this.ChanNum.HeaderText = "Channel No";
            this.ChanNum.Name = "ChanNum";
            this.ChanNum.ReadOnly = true;
            // 
            // LogicChanNum
            // 
            this.LogicChanNum.FillWeight = 5F;
            this.LogicChanNum.HeaderText = "Logical Chan";
            this.LogicChanNum.Name = "LogicChanNum";
            this.LogicChanNum.ReadOnly = true;
            // 
            // CustomEntryName
            // 
            this.CustomEntryName.FillWeight = 35F;
            this.CustomEntryName.HeaderText = "Name";
            this.CustomEntryName.Name = "CustomEntryName";
            this.CustomEntryName.ReadOnly = true;
            // 
            // CustomEntryCustomName
            // 
            this.CustomEntryCustomName.FillWeight = 40F;
            this.CustomEntryCustomName.HeaderText = "Custom Name";
            this.CustomEntryCustomName.Name = "CustomEntryCustomName";
            // 
            // CustomGridEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 397);
            this.Controls.Add(this.gbCustomEntry);
            this.Controls.Add(this.gbControl);
            this.Name = "CustomGridEntry";
            this.Text = "Entry Custom Channel names";
            this.gbControl.ResumeLayout(false);
            this.gbCustomEntry.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgCustomEntry)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gbControl;
        private System.Windows.Forms.GroupBox gbCustomEntry;
        private System.Windows.Forms.DataGridView dgCustomEntry;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomEntryLineup;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomEntryStationID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChanNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn LogicChanNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomEntryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomEntryCustomName;
    }
}