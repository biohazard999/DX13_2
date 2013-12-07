namespace WinTaskbarDemo
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
            DevExpress.Utils.Taskbar.JumpListCategory jumpListCategory1 = new DevExpress.Utils.Taskbar.JumpListCategory();
            DevExpress.Utils.Taskbar.JumpListItemTask jumpListItemTask1 = new DevExpress.Utils.Taskbar.JumpListItemTask();
            DevExpress.Utils.Taskbar.JumpListItemTask jumpListItemTask2 = new DevExpress.Utils.Taskbar.JumpListItemTask();
            DevExpress.Utils.Taskbar.JumpListItemTask jumpListItemTask3 = new DevExpress.Utils.Taskbar.JumpListItemTask();
            this.taskbarAssistant1 = new DevExpress.Utils.Taskbar.TaskbarAssistant();
            this.whatsNewTaskbarButton = new DevExpress.Utils.Taskbar.ThumbnailButton();
            this.helpTaskbarButton = new DevExpress.Utils.Taskbar.ThumbnailButton();
            this.busyCheckEdit = new DevExpress.XtraEditors.CheckEdit();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.busyCheckEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // taskbarAssistant1
            // 
            this.taskbarAssistant1.IconsAssembly = "WinTaskbarDemo.exe";
            jumpListCategory1.Caption = "DevExpress";
            jumpListItemTask1.Caption = "13.2 Rocks!";
            jumpListItemTask1.IconIndex = 2;
            jumpListItemTask1.Path = "https://www.devexpress.com/Subscriptions/New-2013.xml";
            jumpListCategory1.JumpItems.Add(jumpListItemTask1);
            this.taskbarAssistant1.JumpListCustomCategories.Add(jumpListCategory1);
            jumpListItemTask2.Caption = "Notepad";
            jumpListItemTask2.IconIndex = 0;
            jumpListItemTask2.Path = "notepad";
            jumpListItemTask3.Caption = "Calculator";
            jumpListItemTask3.IconIndex = 1;
            jumpListItemTask3.Path = "calc";
            this.taskbarAssistant1.JumpListTasksCategory.Add(jumpListItemTask2);
            this.taskbarAssistant1.JumpListTasksCategory.Add(jumpListItemTask3);
            this.taskbarAssistant1.ParentControl = this;
            this.taskbarAssistant1.ThumbnailButtons.Add(this.whatsNewTaskbarButton);
            this.taskbarAssistant1.ThumbnailButtons.Add(this.helpTaskbarButton);
            // 
            // whatsNewTaskbarButton
            // 
            this.whatsNewTaskbarButton.Image = global::WinTaskbarDemo.Properties.Resources.newspaper_new;
            this.whatsNewTaskbarButton.Tooltip = "Shows the what\'s new Page of DevExpress 13.2";
            this.whatsNewTaskbarButton.Click += new System.EventHandler<DevExpress.Utils.Taskbar.ThumbButtonClickEventArgs>(this.whatsNewTaskbarButton_Click);
            // 
            // helpTaskbarButton
            // 
            this.helpTaskbarButton.Image = global::WinTaskbarDemo.Properties.Resources.help;
            this.helpTaskbarButton.Tooltip = "Shows the DevExpress Documentation";
            this.helpTaskbarButton.Click += new System.EventHandler<DevExpress.Utils.Taskbar.ThumbButtonClickEventArgs>(this.helpTaskbarButton_Click);
            // 
            // busyCheckEdit
            // 
            this.busyCheckEdit.Location = new System.Drawing.Point(12, 12);
            this.busyCheckEdit.Name = "busyCheckEdit";
            this.busyCheckEdit.Properties.Caption = "Busy";
            this.busyCheckEdit.Size = new System.Drawing.Size(75, 20);
            this.busyCheckEdit.TabIndex = 0;
            this.busyCheckEdit.CheckedChanged += new System.EventHandler(this.busyCheckEdit_CheckedChanged);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(12, 38);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(838, 493);
            this.webBrowser1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 543);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.busyCheckEdit);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.busyCheckEdit.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.Taskbar.TaskbarAssistant taskbarAssistant1;
        private DevExpress.XtraEditors.CheckEdit busyCheckEdit;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private DevExpress.Utils.Taskbar.ThumbnailButton whatsNewTaskbarButton;
        private DevExpress.Utils.Taskbar.ThumbnailButton helpTaskbarButton;

    }
}

