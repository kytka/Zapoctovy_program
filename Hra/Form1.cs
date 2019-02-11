using System;
using System.IO;
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
using System.Reflection;

namespace Hra
{
    public enum SmerPohybu { DOPRAVA, DOLEVA, NAHORU, DOLU};
    
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
            //TODO vymyslet jak uplne vypnout cursor nad formularem
            //Cursor.Hide();
            //Cursor.Dispose();

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
        Thread VlaknoNaZobrazeniHowToPlay;
        Point LocationBeforeHittingFullscreen;
        Size SizeBeforeHittingFullscreen;
        StreamReader mapa;
        List<PictureBox> SeznamPictureBoxuNaMape;
        List<PohyblivyPrvek> SeznamPohyblivychPrvkuNaMape;
        List<NepohyblivyPrvek> SeznamNepohyblivychPrvkuNaMape;
        static public List<Hranice> seznamHranice;
        public Random zivotJeJenNahoda = new Random();
        Bitmap pozadiHraciPlochy;
        Hrdina player1;
        Hrdina player2;
        //TODO nevim jestli bude fungovat
        PictureBox blikani;
        //==================================================

        enum Stav { MENU, VYBERPOCETHRACU, VYBERLEVEL, HRA, VYHRA, PROHRA , PAUZA, NASTAVENI, HOWTOPLAY};
        Stav stav;
        enum PocetHracu { JEDEN, DVA};
        PocetHracu pocetHracu;
        enum CisloLevelu { RPVNI, DRUHY, TRETI };
        CisloLevelu cisloLevelu;
        //==================================================

        void None()
        {

        }

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
                vec.Enter += new EventHandler(VkroceniNaTlacitko);
                vec.Leave += new EventHandler(VystoupeniZTlacitka);
                //TODO vycentrovani vsech tlacitek vec.Left = (this.Width-vec.Width) / 2;
                //TODO napada me, ze bych mohl chytre vymyslet tabindexy (jsou desetinne) tak, aby to na vsech obrazovkach fungovalo hezky a prepinalo se to poporade
            }
            ZneviditelniVsechnyControls();
        }

        void NastavStav(Stav jaky)
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
                    if (stav == Stav.PAUZA)
                    {
                        this.BackgroundImage = pozadiHraciPlochy;
                        stinitko.Visible = false;
                        blikani = new PictureBox();
                        blikani.Size = this.ClientSize;
                        blikani.Image = pozadiHraciPlochy;
                        this.Controls.Add(blikani);
                    }
                    else
                    {
                        ZneviditelniVsechnyControls();
                    }
                    stav = Stav.HRA;
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
                case Stav.HOWTOPLAY:
                    ZneviditelniVsechnyControls();
                    bMainMenu.Visible = true;
                    lNavodKeHre.Visible = true;
                    stav = Stav.HOWTOPLAY;
                    break;
                case Stav.PAUZA:
                    pozadiHraciPlochy = (Bitmap)this.BackgroundImage;
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
                    break;
                default:
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    player1.PohniSe(player1, SmerPohybu.DOPRAVA);
                    break;
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
                        case Stav.HOWTOPLAY:
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
            NastavStav(Stav.HOWTOPLAY);
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
            //TODO kod z https://stackoverflow.com/questions/3314140/how-to-read-embedded-resource-text-file/3314203
            //potreboval jsem, aby se soubor cetl z Resources a ne z cesty
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Hra.Resources.map11.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (mapa = new StreamReader(stream))
            {
                NactiLevel(mapa);
            }
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

        private void bHowToPlay_MouseHover(object sender, EventArgs e)
        {

        }

        private void VkroceniNaTlacitko(Object sender, EventArgs e)
        {
            //TODO na tohle jsem hrdy, Object nema Font a kdyz jsem dal do deklarace Control, tak to hlasilo chyby
            Control JinySender;
            JinySender = (Control)sender;
            Font temp = new Font(fonts.Families[0], 13F);
            JinySender.Font = temp;
        }

        private void VystoupeniZTlacitka(Object sender, EventArgs e)
        {
            Control JinySender;
            JinySender = (Control)sender;
            JinySender.Font = myFont;
        }

        public Point DejMiLevyHorniRohMapy(int PocetRadku,int PocetSloupcu)
        {
            Point souradnice = new Point();
            int moznaSirka = this.ClientSize.Width / PocetSloupcu;
            int moznaVyska = this.ClientSize.Height / PocetRadku;
            if (moznaVyska < moznaSirka)
            {
                souradnice.X = Math.Abs(this.ClientSize.Width - this.ClientSize.Height) / 2;
                souradnice.Y = 0;
            }
            else
            {
                souradnice.X = 0;
                souradnice.Y = Math.Abs(this.ClientSize.Width - this.ClientSize.Height) / 2;
            }
            return souradnice;
        }

        public int DejMiStranuCtverceNaMape(int PocetRadku, int PocetSloupcu)
        {
            int moznaSirka = this.ClientSize.Width / PocetSloupcu;
            int moznaVyska = this.ClientSize.Height / PocetRadku;
            if (moznaVyska < moznaSirka)
            {
                return moznaVyska;
            }
            else
            {
                return moznaSirka;
            }
        }

        public void NactiLevel(StreamReader soubor)
        {
            //====================================================
            Rectangle screenClientRect = base.RectangleToScreen(base.ClientRectangle);
            int leftBorderWidth = screenClientRect.Left - base.Left;
            int rightBorderWidth = base.Right - screenClientRect.Right;
            int topBorderHeight = screenClientRect.Top - base.Top;
            int bottomBorderHeight = base.Bottom - screenClientRect.Bottom;
            //====================================================
            int pocetRadku = int.Parse(soubor.ReadLine());
            int pocetSloupcu = int.Parse(soubor.ReadLine());
            Point levyHorniRohMapy = DejMiLevyHorniRohMapy(pocetRadku, pocetSloupcu);
            int stranaCtverceNaMape = DejMiStranuCtverceNaMape(pocetRadku, pocetSloupcu);
            SeznamPictureBoxuNaMape = new List<PictureBox>();
            SeznamPohyblivychPrvkuNaMape = new List<PohyblivyPrvek>();
            SeznamNepohyblivychPrvkuNaMape = new List<NepohyblivyPrvek>();
            seznamHranice = new List<Hranice>();
            pozadiHraciPlochy = new Bitmap(this.Width - leftBorderWidth - rightBorderWidth, this.Height - topBorderHeight - bottomBorderHeight);
            Graphics g = Graphics.FromImage(pozadiHraciPlochy);

            for (int i = 0; i < pocetSloupcu; i++)
            {
                string radekSouboru = soubor.ReadLine();
                for (int j = 0; j < pocetRadku; j++)
                {
                    char znak = radekSouboru[j];
                    Point pomocnyBod = new Point();
                    pomocnyBod.X = levyHorniRohMapy.X + j * stranaCtverceNaMape;
                    pomocnyBod.Y = levyHorniRohMapy.Y + i * stranaCtverceNaMape;
                    switch (znak)
                    {
                        case '0'://podlaha
                            g.DrawImage(DejObrazekPodlahy(zivotJeJenNahoda.Next(18)),pomocnyBod.X,pomocnyBod.Y,stranaCtverceNaMape,stranaCtverceNaMape);
                            break;
                        case '1'://Player1
                            player1 = new Hrdina(pomocnyBod, stranaCtverceNaMape);
                            player1.Image = Properties.Resources.eyebrowman_idle;
                            SeznamPictureBoxuNaMape.Add(player1);
                            SeznamPohyblivychPrvkuNaMape.Add(player1);
                            this.Controls.Add(player1);

                            g.DrawImage(DejObrazekPodlahy(zivotJeJenNahoda.Next(18)), pomocnyBod.X, pomocnyBod.Y, stranaCtverceNaMape, stranaCtverceNaMape);
                            break;
                        case '2'://Player2
                            if (pocetHracu == PocetHracu.DVA)
                            {
                                player2 = new Hrdina(pomocnyBod, stranaCtverceNaMape);
                                player2.Image = Properties.Resources.eyebrowman_left;
                                SeznamPictureBoxuNaMape.Add(player2);
                                SeznamPohyblivychPrvkuNaMape.Add(player2);
                                this.Controls.Add(player2);

                            }
                            g.DrawImage(DejObrazekPodlahy(zivotJeJenNahoda.Next(18)), pomocnyBod.X, pomocnyBod.Y, stranaCtverceNaMape, stranaCtverceNaMape);
                            break;
                        case 'D'://Demon
                            break;
                        case 'Z'://Zombie
                            break;
                        case 'R'://oRc
                            break;
                        case 'H'://HardBlock
                            HardBlock hardblock = new HardBlock(pomocnyBod, stranaCtverceNaMape);
                            SeznamNepohyblivychPrvkuNaMape.Add(hardblock);
                            SeznamPictureBoxuNaMape.Add(hardblock);
                            this.Controls.Add(hardblock);
                            seznamHranice.Add(new Hranice(pomocnyBod.Y, pomocnyBod.Y + stranaCtverceNaMape, pomocnyBod.X + stranaCtverceNaMape, pomocnyBod.X, false));
                            break;
                        case '|':
                            g.DrawImage(Properties.Resources.okraj64x64, pomocnyBod.X, pomocnyBod.Y, stranaCtverceNaMape, stranaCtverceNaMape);
                            seznamHranice.Add(new Hranice(pomocnyBod.Y, pomocnyBod.Y + stranaCtverceNaMape, pomocnyBod.X + stranaCtverceNaMape, pomocnyBod.X, false));
                            break;
                        case '-':
                            g.DrawImage(Properties.Resources.hardblock64x64, pomocnyBod.X, pomocnyBod.Y, stranaCtverceNaMape, stranaCtverceNaMape);
                            seznamHranice.Add(new Hranice(pomocnyBod.Y, pomocnyBod.Y + stranaCtverceNaMape, pomocnyBod.X + stranaCtverceNaMape, pomocnyBod.X, false));
                            break;
                        case 'S'://SoftBlock
                            break;
                            //TODO dalsi bloky
                        default:
                            break;
                    }
                }
            }
            //to tu mam, aby se nedelo problikavani pri pohybu transparentnich gifu
            blikani = new PictureBox();
            blikani.Size = this.ClientSize;
            blikani.Image = pozadiHraciPlochy;
            this.Controls.Add(blikani);
            this.BackgroundImage = pozadiHraciPlochy;
        }

        Image DejObrazekPodlahy(int nahodneCislo)
        {
            Image navratovyObrazek;
            switch (nahodneCislo)
            {
                case 0:
                    navratovyObrazek = Properties.Resources.podlaha00;
                    break;
                case 2:
                    navratovyObrazek = Properties.Resources.podlaha02;
                    break;
                case 3:
                    navratovyObrazek = Properties.Resources.podlaha03;
                    break;
                case 4:
                    navratovyObrazek = Properties.Resources.podlaha04;
                    break;
                case 5:
                    navratovyObrazek = Properties.Resources.podlaha05;
                    break;
                case 6:
                    navratovyObrazek = Properties.Resources.podlaha06;
                    break;
                case 7:
                    navratovyObrazek = Properties.Resources.podlaha07;
                    break;
                case 1:
                    navratovyObrazek = Properties.Resources.podlaha01;
                    break;
                default:
                    //bylo to divny, chci vic obycejne podlahy
                    navratovyObrazek = Properties.Resources.podlaha01;
                    break;
            }
            return navratovyObrazek;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            
        }
    
        
    }


    public abstract class Prvek : PictureBox
    {

    }

    public abstract class PohyblivyPrvek : Prvek
    {
        public void PohniSe(PohyblivyPrvek sender, SmerPohybu smer)
        { //TODO co by mi vubec melo rikat, kam se smim pohnout?
            const int rychlostPohybu = 2;
            int novaLeva = sender.Location.X;
            int novaPrava = sender.Location.X + sender.Width;
            int novaHorni = sender.Location.Y;
            int novaDolni = sender.Location.Y + sender.Height;
            switch (smer)
            {
                case SmerPohybu.DOPRAVA:
                    novaPrava += rychlostPohybu;
                    novaLeva += rychlostPohybu;
                    break;
                case SmerPohybu.DOLEVA:
                    novaPrava -= rychlostPohybu;
                    novaLeva -= rychlostPohybu;
                    break;
                case SmerPohybu.NAHORU:
                    //TODO
                    break;
                case SmerPohybu.DOLU:
                    //TODO
                    break;
                default:
                    break;
            }
            foreach (Hranice hrana in Form1.seznamHranice)
            {
                if ((novaHorni > hrana.dolni) || (novaDolni < hrana.horni) || (novaLeva > hrana.prava) || (novaPrava < hrana.leva))
                {
                    sender.Location = new Point(novaLeva, novaHorni);
                }
            }
        }
    }

    public abstract class NepohyblivyPrvek : Prvek
    {

    }

    public class Hrdina : PohyblivyPrvek
    {
        public Hrdina(Point souradnice, int velikostctverecku)
        {
            Location = souradnice;
            this.Height = velikostctverecku;
            this.Width = velikostctverecku;
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.BackColor = Color.Transparent;
        }
    }

    public class HardBlock : NepohyblivyPrvek
    {
        public HardBlock(Point souradnice, int velikostctverecku)
        {
            Location = souradnice;
            this.Image = Properties.Resources.hardblock64x64;
            this.Height = velikostctverecku;
            this.Width = velikostctverecku;
            this.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }

    public class Hranice
    {
        public int horni;
        public int dolni;
        public int prava;
        public int leva;
        public bool softblock = false;
        public Hranice(int Horni,int Dolni,int Prava,int Leva,bool Softblock)
        {
            horni = Horni;
            dolni = Dolni;
            prava = Prava;
            leva = Leva;
            softblock = Softblock;
        }
    }


}
