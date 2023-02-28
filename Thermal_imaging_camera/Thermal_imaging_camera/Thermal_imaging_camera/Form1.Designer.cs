namespace Thermal_imaging_camera
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.button1 = new System.Windows.Forms.Button();
            this.tbDebug = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.tbDebug_Send = new System.Windows.Forms.TextBox();
            this.ChRecv = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.GUI_Timer = new System.Windows.Forms.Timer(this.components);
            this.tbDisplay = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ChRecv)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(492, 121);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 88);
            this.button1.TabIndex = 0;
            this.button1.Text = "Pause/Initialize";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbDebug
            // 
            this.tbDebug.Location = new System.Drawing.Point(12, 14);
            this.tbDebug.Multiline = true;
            this.tbDebug.Name = "tbDebug";
            this.tbDebug.Size = new System.Drawing.Size(432, 88);
            this.tbDebug.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(492, 14);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 88);
            this.button2.TabIndex = 3;
            this.button2.Text = "Auto Connect";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(492, 224);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(88, 59);
            this.button3.TabIndex = 4;
            this.button3.Text = "Send Data";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // tbDebug_Send
            // 
            this.tbDebug_Send.Location = new System.Drawing.Point(12, 195);
            this.tbDebug_Send.Multiline = true;
            this.tbDebug_Send.Name = "tbDebug_Send";
            this.tbDebug_Send.Size = new System.Drawing.Size(432, 88);
            this.tbDebug_Send.TabIndex = 5;
            // 
            // ChRecv
            // 
            chartArea6.Name = "ChartArea1";
            this.ChRecv.ChartAreas.Add(chartArea6);
            legend6.Name = "Legend1";
            this.ChRecv.Legends.Add(legend6);
            this.ChRecv.Location = new System.Drawing.Point(12, 289);
            this.ChRecv.Name = "ChRecv";
            series6.ChartArea = "ChartArea1";
            series6.Legend = "Legend1";
            series6.Name = "Series1";
            this.ChRecv.Series.Add(series6);
            this.ChRecv.Size = new System.Drawing.Size(568, 300);
            this.ChRecv.TabIndex = 6;
            this.ChRecv.Text = "chart1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(467, 353);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(467, 380);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "label1";
            // 
            // GUI_Timer
            // 
            this.GUI_Timer.Tick += new System.EventHandler(this.GUI_Timer_Tick);
            // 
            // tbDisplay
            // 
            this.tbDisplay.Location = new System.Drawing.Point(586, 14);
            this.tbDisplay.Multiline = true;
            this.tbDisplay.Name = "tbDisplay";
            this.tbDisplay.Size = new System.Drawing.Size(766, 415);
            this.tbDisplay.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1578, 802);
            this.Controls.Add(this.tbDisplay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ChRecv);
            this.Controls.Add(this.tbDebug_Send);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tbDebug);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.ChRecv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbDebug;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox tbDebug_Send;
        private System.Windows.Forms.DataVisualization.Charting.Chart ChRecv;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer GUI_Timer;
        private System.Windows.Forms.TextBox tbDisplay;
    }
}

