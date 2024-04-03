namespace NavSpatialDataWorkerGUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnSyncAirports = new Button();
            btnSyncWaypoints = new Button();
            progressBarSync = new ProgressBar();
            SuspendLayout();
            // 
            // btnSyncAirports
            // 
            btnSyncAirports.Location = new Point(179, 186);
            btnSyncAirports.Name = "btnSyncAirports";
            btnSyncAirports.Size = new Size(234, 29);
            btnSyncAirports.TabIndex = 0;
            btnSyncAirports.Text = "Synchronize Airports";
            btnSyncAirports.UseVisualStyleBackColor = true;
            btnSyncAirports.Click += btnSyncAirports_Click;
            // 
            // btnSyncWaypoints
            // 
            btnSyncWaypoints.Location = new Point(455, 186);
            btnSyncWaypoints.Name = "btnSyncWaypoints";
            btnSyncWaypoints.Size = new Size(236, 29);
            btnSyncWaypoints.TabIndex = 1;
            btnSyncWaypoints.Text = "Synchronize Waypoints";
            btnSyncWaypoints.UseVisualStyleBackColor = true;
            btnSyncWaypoints.Click += btnSyncWaypoints_Click;
            // 
            // progressBarSync
            // 
            progressBarSync.Location = new Point(288, 277);
            progressBarSync.Name = "progressBarSync";
            progressBarSync.Size = new Size(125, 29);
            progressBarSync.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(progressBarSync);
            Controls.Add(btnSyncWaypoints);
            Controls.Add(btnSyncAirports);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button btnSyncAirports;
        private Button btnSyncWaypoints;
        private ProgressBar progressBarSync;
    }
}