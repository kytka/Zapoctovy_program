using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Text;

namespace Hra
{
    public partial class Form1 : Form
    {
        //==================================================
        //valcim s fonty, kod je z https://stackoverflow.com/questions/556147/how-to-quickly-and-easily-embed-fonts-in-winforms-app-in-c-sharp
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private PrivateFontCollection fonts = new PrivateFontCollection();

        Font myFont;
        //==================================================
        public Form1()
        {
            InitializeComponent();
            UsporadejFormular();
            //TODO tohle se musi vymazat
            NastavStav(Stav.MENU);
            //==================================================
            //valcim s fonty podruhe
            byte[] fontData = Properties.Resources.pcsenior;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Properties.Resources.pcsenior.Length);
            AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.pcsenior.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);
            
            myFont = new Font(fonts.Families[0], 8.25F);
            this.Font = myFont;
            //==================================================
        }
        Thread vlaknoNaZobrazeniHowToPlay;
        Point LocationBeforeHittingFullscreen;
        Size SizeBeforeHittingFullscreen;

        //==================================================
        enum Stav { MENU, VYBERPOCETHRACU, VYBERLEVEL, HRA, VYHRA, PROHRA , PAUZA, NASTAVENI};
        Stav stav;
        enum PocetHracu { JEDEN, DVA};
        PocetHracu pocetHracu;
        enum CisloLevelu { RPVNI, DRUHY, TRETI };
        CisloLevelu cisloLevelu;
        //==================================================

        void ZneviditelniVsechnyControls()
        {
            foreach (Control vec in this.Controls.OfType<Control>())
            {
                vec.Visible = false;
            }
        }

        void UsporadejFormular()
        {
            foreach (Button tlacitko in this.Controls.OfType<Button>())
            {
                tlacitko.Anchor = (AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left);
                tlacitko.AutoSize = true;
                tlacitko.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                tlacitko.Visible = false;
                tlacitko.FlatStyle = FlatStyle.Flat;
            }
            foreach (Control vec in this.Controls)
            {
                
            }
            ZneviditelniVsechnyControls();
        }

        void NastavStav (Stav jaky)
        {
            switch (jaky)
            {
                case Stav.MENU:
                    ZneviditelniVsechnyControls();
                    bPlay.Visible = true;
                    bHowToPlay.Visible = true;
                    bSettings.Visible = true;
                    bQuit.Visible = true;
                    stav = Stav.MENU;
                    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                    break;
                case Stav.VYBERPOCETHRACU:
                    ZneviditelniVsechnyControls();
                    b1Player.Visible = true;
                    b2Players.Visible = true;
                    bBack.Visible = true;
                    stav = Stav.VYBERPOCETHRACU;
                    break;
                case Stav.VYBERLEVEL:
                    ZneviditelniVsechnyControls();
                    pLevel.Visible = true;
                    bBack.Visible = true;
                    bStartGame.Visible = true;
                    stav = Stav.VYBERLEVEL;
                    break;
                case Stav.HRA:
                    //TODO NactiLevel();
                    if (stav == Stav.PAUZA)
                    {
                        this.BackgroundImage = null;
                        stinitko.Visible = false;
                    }
                    stav = Stav.HRA;
                    ZneviditelniVsechnyControls();
                    break;
                case Stav.VYHRA:
                    ZneviditelniVsechnyControls();
                    stav = Stav.VYHRA;
                    break;
                case Stav.NASTAVENI:
                    ZneviditelniVsechnyControls();
                    pWindowMode.Visible = true;
                    lAdjustSize.Visible = true;
                    bMainMenu.Visible = true;
                    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                    stav = Stav.NASTAVENI;
                    break;
                case Stav.PROHRA:
                    ZneviditelniVsechnyControls();
                    stav = Stav.PROHRA;
                    break;
                case Stav.PAUZA:
                    ZneviditelniVsechnyControls();
                    bHowToPlay.Visible = true;
                    bResume.Visible = true;
                    bMainMenu.Visible = true;
                    //=================================================
                    //TODO pujceny kod https://arstechnica.com/civis/viewtopic.php?t=125326
                    Rectangle screenClientRect = base.RectangleToScreen(base.ClientRectangle);
                    int leftBorderWidth = screenClientRect.Left - base.Left;
                    int rightBorderWidth = base.Right - screenClientRect.Right;
                    int topBorderHeight = screenClientRect.Top - base.Top;
                    int bottomBorderHeight = base.Bottom - screenClientRect.Bottom;

                    //==================================================

                    Bitmap bitmap = new Bitmap(this.Width - leftBorderWidth - rightBorderWidth, this.Height - topBorderHeight - bottomBorderHeight);

                    Graphics g = Graphics.FromImage(bitmap);

                    g.CopyFromScreen(this.Left + leftBorderWidth, this.Top + topBorderHeight, 0, 0, bitmap.Size);
                    this.BackgroundImage = bitmap;
                    //TODO musim snaviditelnit vsechny objekty, ktere reaguji na mys
                    //ty ktere nereaguji na mys, tam si to musim vyzkouset
                    //stinitko.BackColor = Color.Transparent; //dulezite
                    stinitko.Location = Point.Empty;
                    stinitko.Size = this.Size;
                    stinitko.Height += 5; //to tu musi byt, aby stinitko prekryvalo cely prostor, jinak je tam bila cara
                    stinitko.Width += 5;
                    stinitko.Visible = true;
                    stav = Stav.PAUZA;
                    //==================================================
                    
                    //==================================================
                    stav = Stav.PAUZA;
                    break;
                default:
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    switch (stav)
                    {
                        case Stav.MENU:
                            break;
                        case Stav.VYBERPOCETHRACU:
                            NastavStav(Stav.MENU);
                            break;
                        case Stav.VYBERLEVEL:
                            NastavStav(Stav.VYBERPOCETHRACU);
                            break;
                        case Stav.HRA:
                            NastavStav(Stav.PAUZA);
                            break;
                        case Stav.VYHRA:
                            break;
                        case Stav.PROHRA:
                            break;
                        case Stav.PAUZA:
                            NastavStav(Stav.HRA);
                            break;
                        case Stav.NASTAVENI:
                            NastavStav(Stav.MENU);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        private void bPlay_Click(object sender, EventArgs e)
        {
            NastavStav(Stav.VYBERPOCETHRACU);
        }

        private void bHowToPlay_Click(object sender, EventArgs e)
        {
            vlaknoNaZobrazeniHowToPlay = new Thread(new ThreadStart(delegate
            {
                MessageBox.Show
                (
                //TODO dopsat navod v anglictine
                  "Tady bude navod, jak hrat hru i s tim, jak ovladat panaky",
                  "How to Play",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Information
                );
            }));
            vlaknoNaZobrazeniHowToPlay.Start();
        }

        private void bSettings_Click(object sender, EventArgs e)
        {
            NastavStav(Stav.NASTAVENI);
        }

        private void bQuit_Click(object sender, EventArgs e)
        {
            //vlaknoNaZobrazeniHowToPlay.Abort();
            Application.Exit();
        }

        private void b1Player_Click(object sender, EventArgs e)
        {
            pocetHracu = PocetHracu.JEDEN;
            NastavStav(Stav.VYBERLEVEL);
        }

        private void b2Players_Click(object sender, EventArgs e)
        {
            pocetHracu = PocetHracu.DVA;
            NastavStav(Stav.VYBERLEVEL);
        }

        private void bBack_Click(object sender, EventArgs e)
        {
            //tady jsem nechcetel psat novou funkci, tak volam jiz exitujici pripad pro stisknuti escape
            Form1_KeyDown(this, new KeyEventArgs(Keys.Escape));
        }

        private void bStartGame_Click(object sender, EventArgs e)
        {
            NastavStav(Stav.HRA);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            NastavStav(Stav.VYBERLEVEL);
        }

        private void bNextLevel_Click(object sender, EventArgs e)
        {
            //TODO NactiLevel(Dalsi);
        }

        private void bMainMenu_Click(object sender, EventArgs e)
        {
            NastavStav(Stav.MENU);
        }

        private void bRetry_Click(object sender, EventArgs e)
        {
            //TODO NactiLevel(Stejny);
        }

        private void bResume_Click(object sender, EventArgs e)
        {
            //tady jsem nechcetel psat novou funkci, tak volam jiz exitujici pripad pro stisknuti escape
            Form1_KeyDown(this, new KeyEventArgs(Keys.Escape));
        }

        private void rbFullscreen_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFullscreen.Checked == true)
            {
                LocationBeforeHittingFullscreen = this.Location;
                SizeBeforeHittingFullscreen = this.Size;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.Location = Point.Empty;
                this.Size = Screen.PrimaryScreen.Bounds.Size;
                stinitko.Size = this.Size;
                stinitko.Height += 5; //to tu musi byt, aby stinitko prekryvalo cely prostor, jinak je tam bila cara
                stinitko.Width += 5;
            }
            else
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                this.Location = LocationBeforeHittingFullscreen;
                this.Size = SizeBeforeHittingFullscreen; ;
                stinitko.Size = this.Size;
                stinitko.Height += 5; //to tu musi byt, aby stinitko prekryvalo cely prostor, jinak je tam bila cara
                stinitko.Width += 5;
            }
        }

        private void rbWindowed_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}
