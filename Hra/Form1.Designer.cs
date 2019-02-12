namespace Hra
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.bPlay = new System.Windows.Forms.Button();
            this.bHowToPlay = new System.Windows.Forms.Button();
            this.bQuit = new System.Windows.Forms.Button();
            this.b1Player = new System.Windows.Forms.Button();
            this.b2Players = new System.Windows.Forms.Button();
            this.bStartGame = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.bNextLevel = new System.Windows.Forms.Button();
            this.bMainMenu = new System.Windows.Forms.Button();
            this.bRetry = new System.Windows.Forms.Button();
            this.bResume = new System.Windows.Forms.Button();
            this.pLevel = new System.Windows.Forms.Panel();
            this.rbLvl3 = new System.Windows.Forms.RadioButton();
            this.rbLvl2 = new System.Windows.Forms.RadioButton();
            this.rbLvl1 = new System.Windows.Forms.RadioButton();
            this.lAdjustSize = new System.Windows.Forms.Label();
            this.bSettings = new System.Windows.Forms.Button();
            this.pWindowMode = new System.Windows.Forms.Panel();
            this.rbFullscreen = new System.Windows.Forms.RadioButton();
            this.rbWindowed = new System.Windows.Forms.RadioButton();
            this.bBack = new System.Windows.Forms.Button();
            this.lNavodKeHre = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.stinitko = new System.Windows.Forms.PictureBox();
            this.obstaravaKlavesovyVstup = new System.Windows.Forms.Timer(this.components);
            this.pLevel.SuspendLayout();
            this.pWindowMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stinitko)).BeginInit();
            this.SuspendLayout();
            // 
            // bPlay
            // 
            this.bPlay.Location = new System.Drawing.Point(74, 33);
            this.bPlay.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.bPlay.Name = "bPlay";
            this.bPlay.Size = new System.Drawing.Size(138, 19);
            this.bPlay.TabIndex = 0;
            this.bPlay.Text = "Play";
            this.bPlay.UseVisualStyleBackColor = true;
            this.bPlay.Click += new System.EventHandler(this.bPlay_Click);
            // 
            // bHowToPlay
            // 
            this.bHowToPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bHowToPlay.AutoSize = true;
            this.bHowToPlay.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bHowToPlay.BackColor = System.Drawing.Color.Transparent;
            this.bHowToPlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.bHowToPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bHowToPlay.ForeColor = System.Drawing.Color.Black;
            this.bHowToPlay.Location = new System.Drawing.Point(74, 74);
            this.bHowToPlay.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.bHowToPlay.Name = "bHowToPlay";
            this.bHowToPlay.Size = new System.Drawing.Size(138, 23);
            this.bHowToPlay.TabIndex = 2;
            this.bHowToPlay.Text = "How To Play";
            this.bHowToPlay.UseVisualStyleBackColor = false;
            this.bHowToPlay.Click += new System.EventHandler(this.bHowToPlay_Click);
            this.bHowToPlay.MouseHover += new System.EventHandler(this.bHowToPlay_MouseHover);
            // 
            // bQuit
            // 
            this.bQuit.Location = new System.Drawing.Point(209, 91);
            this.bQuit.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.bQuit.Name = "bQuit";
            this.bQuit.Size = new System.Drawing.Size(138, 30);
            this.bQuit.TabIndex = 3;
            this.bQuit.Text = "Quit";
            this.bQuit.UseVisualStyleBackColor = true;
            this.bQuit.Click += new System.EventHandler(this.bQuit_Click);
            // 
            // b1Player
            // 
            this.b1Player.Location = new System.Drawing.Point(101, 195);
            this.b1Player.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.b1Player.Name = "b1Player";
            this.b1Player.Size = new System.Drawing.Size(160, 29);
            this.b1Player.TabIndex = 4;
            this.b1Player.Text = "1 Player";
            this.b1Player.UseVisualStyleBackColor = true;
            this.b1Player.Click += new System.EventHandler(this.b1Player_Click);
            // 
            // b2Players
            // 
            this.b2Players.Location = new System.Drawing.Point(114, 247);
            this.b2Players.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.b2Players.Name = "b2Players";
            this.b2Players.Size = new System.Drawing.Size(145, 33);
            this.b2Players.TabIndex = 5;
            this.b2Players.Text = "2 Players";
            this.b2Players.UseVisualStyleBackColor = true;
            this.b2Players.Click += new System.EventHandler(this.b2Players_Click);
            // 
            // bStartGame
            // 
            this.bStartGame.Location = new System.Drawing.Point(129, 349);
            this.bStartGame.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.bStartGame.Name = "bStartGame";
            this.bStartGame.Size = new System.Drawing.Size(130, 17);
            this.bStartGame.TabIndex = 7;
            this.bStartGame.Text = "Start Game";
            this.bStartGame.UseVisualStyleBackColor = true;
            this.bStartGame.Click += new System.EventHandler(this.bStartGame_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(74, 127);
            this.button8.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(156, 30);
            this.button8.TabIndex = 8;
            this.button8.Text = "Choose Level";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // bNextLevel
            // 
            this.bNextLevel.Location = new System.Drawing.Point(91, 170);
            this.bNextLevel.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.bNextLevel.Name = "bNextLevel";
            this.bNextLevel.Size = new System.Drawing.Size(131, 19);
            this.bNextLevel.TabIndex = 9;
            this.bNextLevel.Text = "Next Level";
            this.bNextLevel.UseVisualStyleBackColor = true;
            this.bNextLevel.Click += new System.EventHandler(this.bNextLevel_Click);
            // 
            // bMainMenu
            // 
            this.bMainMenu.Location = new System.Drawing.Point(101, 286);
            this.bMainMenu.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.bMainMenu.Name = "bMainMenu";
            this.bMainMenu.Size = new System.Drawing.Size(129, 23);
            this.bMainMenu.TabIndex = 10;
            this.bMainMenu.Text = "Main Menu";
            this.bMainMenu.UseVisualStyleBackColor = true;
            this.bMainMenu.Click += new System.EventHandler(this.bMainMenu_Click);
            // 
            // bRetry
            // 
            this.bRetry.Location = new System.Drawing.Point(118, 381);
            this.bRetry.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.bRetry.Name = "bRetry";
            this.bRetry.Size = new System.Drawing.Size(141, 20);
            this.bRetry.TabIndex = 11;
            this.bRetry.Text = "Retry";
            this.bRetry.UseVisualStyleBackColor = true;
            this.bRetry.Click += new System.EventHandler(this.bRetry_Click);
            // 
            // bResume
            // 
            this.bResume.Location = new System.Drawing.Point(114, 311);
            this.bResume.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.bResume.Name = "bResume";
            this.bResume.Size = new System.Drawing.Size(108, 32);
            this.bResume.TabIndex = 12;
            this.bResume.Text = "Resume";
            this.bResume.UseVisualStyleBackColor = true;
            this.bResume.Click += new System.EventHandler(this.bResume_Click);
            // 
            // pLevel
            // 
            this.pLevel.Controls.Add(this.rbLvl3);
            this.pLevel.Controls.Add(this.rbLvl2);
            this.pLevel.Controls.Add(this.rbLvl1);
            this.pLevel.Location = new System.Drawing.Point(740, 313);
            this.pLevel.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.pLevel.Name = "pLevel";
            this.pLevel.Size = new System.Drawing.Size(270, 88);
            this.pLevel.TabIndex = 16;
            // 
            // rbLvl3
            // 
            this.rbLvl3.AutoSize = true;
            this.rbLvl3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbLvl3.Location = new System.Drawing.Point(72, 53);
            this.rbLvl3.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.rbLvl3.Name = "rbLvl3";
            this.rbLvl3.Size = new System.Drawing.Size(99, 15);
            this.rbLvl3.TabIndex = 2;
            this.rbLvl3.Text = "level 3";
            this.rbLvl3.UseVisualStyleBackColor = true;
            // 
            // rbLvl2
            // 
            this.rbLvl2.AutoSize = true;
            this.rbLvl2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbLvl2.Location = new System.Drawing.Point(73, 34);
            this.rbLvl2.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.rbLvl2.Name = "rbLvl2";
            this.rbLvl2.Size = new System.Drawing.Size(99, 15);
            this.rbLvl2.TabIndex = 1;
            this.rbLvl2.Text = "level 2";
            this.rbLvl2.UseVisualStyleBackColor = true;
            // 
            // rbLvl1
            // 
            this.rbLvl1.AutoSize = true;
            this.rbLvl1.Checked = true;
            this.rbLvl1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbLvl1.Location = new System.Drawing.Point(73, 15);
            this.rbLvl1.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.rbLvl1.Name = "rbLvl1";
            this.rbLvl1.Size = new System.Drawing.Size(99, 15);
            this.rbLvl1.TabIndex = 0;
            this.rbLvl1.TabStop = true;
            this.rbLvl1.Text = "level 1";
            this.rbLvl1.UseVisualStyleBackColor = true;
            // 
            // lAdjustSize
            // 
            this.lAdjustSize.AutoSize = true;
            this.lAdjustSize.Location = new System.Drawing.Point(284, 441);
            this.lAdjustSize.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lAdjustSize.Name = "lAdjustSize";
            this.lAdjustSize.Size = new System.Drawing.Size(467, 11);
            this.lAdjustSize.TabIndex = 17;
            this.lAdjustSize.Text = "You can adjust the size of the window now.";
            // 
            // bSettings
            // 
            this.bSettings.Location = new System.Drawing.Point(209, 39);
            this.bSettings.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.bSettings.Name = "bSettings";
            this.bSettings.Size = new System.Drawing.Size(172, 23);
            this.bSettings.TabIndex = 1;
            this.bSettings.Text = "Settings";
            this.bSettings.UseVisualStyleBackColor = true;
            this.bSettings.Click += new System.EventHandler(this.bSettings_Click);
            // 
            // pWindowMode
            // 
            this.pWindowMode.Controls.Add(this.rbFullscreen);
            this.pWindowMode.Controls.Add(this.rbWindowed);
            this.pWindowMode.Location = new System.Drawing.Point(740, 212);
            this.pWindowMode.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.pWindowMode.Name = "pWindowMode";
            this.pWindowMode.Size = new System.Drawing.Size(273, 95);
            this.pWindowMode.TabIndex = 19;
            // 
            // rbFullscreen
            // 
            this.rbFullscreen.AutoSize = true;
            this.rbFullscreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbFullscreen.Location = new System.Drawing.Point(66, 29);
            this.rbFullscreen.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.rbFullscreen.Name = "rbFullscreen";
            this.rbFullscreen.Size = new System.Drawing.Size(132, 15);
            this.rbFullscreen.TabIndex = 15;
            this.rbFullscreen.Text = "Fullscreen";
            this.rbFullscreen.UseVisualStyleBackColor = true;
            this.rbFullscreen.CheckedChanged += new System.EventHandler(this.rbFullscreen_CheckedChanged);
            // 
            // rbWindowed
            // 
            this.rbWindowed.AutoSize = true;
            this.rbWindowed.Checked = true;
            this.rbWindowed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbWindowed.Location = new System.Drawing.Point(66, 8);
            this.rbWindowed.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.rbWindowed.Name = "rbWindowed";
            this.rbWindowed.Size = new System.Drawing.Size(110, 15);
            this.rbWindowed.TabIndex = 14;
            this.rbWindowed.TabStop = true;
            this.rbWindowed.Text = "Windowed";
            this.rbWindowed.UseVisualStyleBackColor = true;
            this.rbWindowed.CheckedChanged += new System.EventHandler(this.rbWindowed_CheckedChanged);
            // 
            // bBack
            // 
            this.bBack.Location = new System.Drawing.Point(65, 230);
            this.bBack.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.bBack.Name = "bBack";
            this.bBack.Size = new System.Drawing.Size(57, 21);
            this.bBack.TabIndex = 6;
            this.bBack.Text = "Back";
            this.bBack.UseVisualStyleBackColor = true;
            this.bBack.Click += new System.EventHandler(this.bBack_Click);
            // 
            // lNavodKeHre
            // 
            this.lNavodKeHre.AutoEllipsis = true;
            this.lNavodKeHre.AutoSize = true;
            this.lNavodKeHre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lNavodKeHre.Location = new System.Drawing.Point(667, 39);
            this.lNavodKeHre.Name = "lNavodKeHre";
            this.lNavodKeHre.Size = new System.Drawing.Size(7817, 13);
            this.lNavodKeHre.TabIndex = 20;
            this.lNavodKeHre.Text = resources.GetString("lNavodKeHre.Text");
            this.lNavodKeHre.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(733, 77);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(205, 68);
            this.textBox1.TabIndex = 21;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // stinitko
            // 
            this.stinitko.BackColor = System.Drawing.Color.Transparent;
            this.stinitko.ErrorImage = null;
            this.stinitko.Image = global::Hra.Properties.Resources.transparent;
            this.stinitko.Location = new System.Drawing.Point(15, 407);
            this.stinitko.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.stinitko.Name = "stinitko";
            this.stinitko.Size = new System.Drawing.Size(92, 42);
            this.stinitko.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.stinitko.TabIndex = 0;
            this.stinitko.TabStop = false;
            this.stinitko.Visible = false;
            // 
            // obstaravaKlavesovyVstup
            // 
            this.obstaravaKlavesovyVstup.Enabled = true;
            this.obstaravaKlavesovyVstup.Interval = 25;
            this.obstaravaKlavesovyVstup.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 11F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 461);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lNavodKeHre);
            this.Controls.Add(this.pWindowMode);
            this.Controls.Add(this.bSettings);
            this.Controls.Add(this.lAdjustSize);
            this.Controls.Add(this.pLevel);
            this.Controls.Add(this.bResume);
            this.Controls.Add(this.bRetry);
            this.Controls.Add(this.bMainMenu);
            this.Controls.Add(this.bNextLevel);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.bStartGame);
            this.Controls.Add(this.bBack);
            this.Controls.Add(this.b2Players);
            this.Controls.Add(this.b1Player);
            this.Controls.Add(this.bQuit);
            this.Controls.Add(this.bHowToPlay);
            this.Controls.Add(this.bPlay);
            this.Controls.Add(this.stinitko);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Font = new System.Drawing.Font("PC Senior", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "bumbumbum";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.pLevel.ResumeLayout(false);
            this.pLevel.PerformLayout();
            this.pWindowMode.ResumeLayout(false);
            this.pWindowMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stinitko)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox stinitko;
        private System.Windows.Forms.Button bPlay;
        private System.Windows.Forms.Button bHowToPlay;
        private System.Windows.Forms.Button bQuit;
        private System.Windows.Forms.Button b1Player;
        private System.Windows.Forms.Button b2Players;
        private System.Windows.Forms.Button bStartGame;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button bNextLevel;
        private System.Windows.Forms.Button bMainMenu;
        private System.Windows.Forms.Button bRetry;
        private System.Windows.Forms.Button bResume;
        private System.Windows.Forms.Panel pLevel;
        private System.Windows.Forms.RadioButton rbLvl2;
        private System.Windows.Forms.RadioButton rbLvl1;
        private System.Windows.Forms.RadioButton rbLvl3;
        private System.Windows.Forms.Label lAdjustSize;
        private System.Windows.Forms.Button bSettings;
        private System.Windows.Forms.Panel pWindowMode;
        private System.Windows.Forms.RadioButton rbFullscreen;
        private System.Windows.Forms.RadioButton rbWindowed;
        private System.Windows.Forms.Button bBack;
        private System.Windows.Forms.Label lNavodKeHre;
        private System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.Timer obstaravaKlavesovyVstup;
    }
}

