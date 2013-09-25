namespace IntegrationAccountMergeUtility
{
    partial class AccountDataReassignment
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
            this.lblStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.txtAccountChanges = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbDelete = new System.Windows.Forms.CheckBox();
            this.cbNewContact = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblStatus.Location = new System.Drawing.Point(12, 120);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(40, 13);
            this.lblStatus.TabIndex = 13;
            this.lblStatus.Text = "Status:";
            // 
            // label3
            // 
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(269, 50);
            this.label3.TabIndex = 12;
            this.label3.Text = "Re-assignes all activity, history, and attachments from existing SalesLogix dupli" +
    "cate account to imported integration account.";
            // 
            // btnUpdate
            // 
            this.btnUpdate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnUpdate.Location = new System.Drawing.Point(180, 214);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 24);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.Text = "Re-Assign";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // txtAccountChanges
            // 
            this.txtAccountChanges.Location = new System.Drawing.Point(155, 151);
            this.txtAccountChanges.Name = "txtAccountChanges";
            this.txtAccountChanges.Size = new System.Drawing.Size(100, 20);
            this.txtAccountChanges.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(12, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Account records processed";
            // 
            // cbDelete
            // 
            this.cbDelete.AutoSize = true;
            this.cbDelete.Checked = true;
            this.cbDelete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDelete.Location = new System.Drawing.Point(15, 58);
            this.cbDelete.Name = "cbDelete";
            this.cbDelete.Size = new System.Drawing.Size(145, 17);
            this.cbDelete.TabIndex = 14;
            this.cbDelete.Text = "Delete duplicate account";
            this.cbDelete.UseVisualStyleBackColor = true;
            // 
            // cbNewContact
            // 
            this.cbNewContact.AutoSize = true;
            this.cbNewContact.Checked = true;
            this.cbNewContact.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbNewContact.Location = new System.Drawing.Point(15, 81);
            this.cbNewContact.Name = "cbNewContact";
            this.cbNewContact.Size = new System.Drawing.Size(120, 17);
            this.cbNewContact.TabIndex = 15;
            this.cbNewContact.Text = "Move new contacts";
            this.cbNewContact.UseVisualStyleBackColor = true;
            // 
            // AccountDataReassignment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 263);
            this.Controls.Add(this.cbNewContact);
            this.Controls.Add(this.cbDelete);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.txtAccountChanges);
            this.Controls.Add(this.label1);
            this.Name = "AccountDataReassignment";
            this.Text = "Account Data Re-Assignment";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.TextBox txtAccountChanges;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbDelete;
        private System.Windows.Forms.CheckBox cbNewContact;
    }
}

