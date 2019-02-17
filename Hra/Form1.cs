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
using System.Windows.Input;

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
        Point LocationBeforeHittingFullscreen;
        Size SizeBeforeHittingFullscreen;
        StreamReader mapa;
        List<PictureBox> SeznamPictureBoxuNaMape;
        static public List<PohyblivyPrvek> SeznamPohyblivychPrvkuNaMape;
        List<NepohyblivyPrvek> SeznamNepohyblivychPrvkuNaMape;
        static public List<Bomba> seznamBomb;
        static public List<Hranice> seznamHranice;
        static public List<Prisera> seznamPriser;
        static public Random zivotJeJenNahoda = new Random();
        public static Bitmap pozadiHraciPlochy;
        public Hrdina player1;
        public Hrdina player2;
        public PictureBox blikani;
        public static int stranaCtverceNaMape;
        public static Point levyHorniRohMapy;
        static public Graphics g;
        //==================================================

        public enum Stav { MENU, VYBERPOCETHRACU, VYBERLEVEL, HRA, VYHRA, PROHRA , PAUZA, NASTAVENI, HOWTOPLAY};
        public Stav stav;
        public enum PocetHracu { JEDEN, DVA};
        public PocetHracu pocetHracu;
        enum CisloLevelu { RPVNI, DRUHY, TRETI };
        CisloLevelu cisloLevelu;
        public enum TypPrisery { DEMON, ZOMBIE, ORC };
        TypPrisery typPrisery;
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
                vec.Enter += new EventHandler(VkroceniNaTlacitko);
                vec.Leave += new EventHandler(VystoupeniZTlacitka);
                //TODO vycentrovani vsech tlacitek vec.Left = (this.Width-vec.Width) / 2;
                //TODO napada me, ze bych mohl chytre vymyslet tabindexy (jsou desetinne) tak, aby to na vsech obrazovkach fungovalo hezky a prepinalo se to poporade
            }
            ZneviditelniVsechnyControls();
        }

        public void NastavStav(Stav jaky)
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
                        ZneviditelniVsechnyControls();
                        foreach (PictureBox obrazek in SeznamPictureBoxuNaMape) obrazek.Visible = true;
                        this.BackgroundImage = pozadiHraciPlochy;
                        stinitko.Visible = false;
                        blikani = new PictureBox();
                        blikani.Size = this.ClientSize;
                        blikani.Image = pozadiHraciPlochy;
                        this.Controls.Add(blikani);
                        //=================================================
                        //zapnuti casovacu vsech bomb
                        foreach (Bomba bomba in seznamBomb)
                        {
                            bomba.casDoVybuchnuti.Start();
                        }
                        //=================================================
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
                    bMainMenu.Visible = true;
                    lYouDied.Visible = true;
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
                    //zastaveni casovacu vsech bomb
                    foreach (Bomba bomba in seznamBomb)
                    {
                        bomba.casDoVybuchnuti.Stop();
                    }
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

        private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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
            Form1_KeyDown(this, new System.Windows.Forms.KeyEventArgs(Keys.Escape));
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
            Form1_KeyDown(this, new System.Windows.Forms.KeyEventArgs(Keys.Escape));
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
            levyHorniRohMapy = DejMiLevyHorniRohMapy(pocetRadku, pocetSloupcu);
            stranaCtverceNaMape = DejMiStranuCtverceNaMape(pocetRadku, pocetSloupcu);
            SeznamPictureBoxuNaMape = new List<PictureBox>();
            SeznamPohyblivychPrvkuNaMape = new List<PohyblivyPrvek>();
            SeznamNepohyblivychPrvkuNaMape = new List<NepohyblivyPrvek>();
            seznamPriser = new List<Prisera>();
            seznamHranice = new List<Hranice>();
            pozadiHraciPlochy = new Bitmap(this.Width - leftBorderWidth - rightBorderWidth, this.Height - topBorderHeight - bottomBorderHeight);
            g = Graphics.FromImage(pozadiHraciPlochy);

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
                            g.DrawImage(DejObrazekPodlahy(zivotJeJenNahoda.Next(25)),pomocnyBod.X,pomocnyBod.Y,stranaCtverceNaMape,stranaCtverceNaMape);
                            break;
                        case '1'://Player1
                            player1 = new Hrdina(pomocnyBod, stranaCtverceNaMape,this);
                            player1.Image = Properties.Resources.eyebrowman_idle;
                            SeznamPictureBoxuNaMape.Add(player1);
                            SeznamPohyblivychPrvkuNaMape.Add(player1);
                            this.Controls.Add(player1);
                            seznamBomb = new List<Bomba>();

                            g.DrawImage(DejObrazekPodlahy(zivotJeJenNahoda.Next(25)), pomocnyBod.X, pomocnyBod.Y, stranaCtverceNaMape, stranaCtverceNaMape);
                            break;
                        case '2'://Player2
                            if (pocetHracu == PocetHracu.DVA)
                            {
                                player2 = new Hrdina(pomocnyBod, stranaCtverceNaMape,this);
                                player2.Image = Properties.Resources.eyebrowman_left;
                                SeznamPictureBoxuNaMape.Add(player2);
                                SeznamPohyblivychPrvkuNaMape.Add(player2);
                                this.Controls.Add(player2);
                            }
                            g.DrawImage(DejObrazekPodlahy(zivotJeJenNahoda.Next(25)), pomocnyBod.X, pomocnyBod.Y, stranaCtverceNaMape, stranaCtverceNaMape);
                            break;
                        case 'D'://Demon
                            Prisera prisera = new Prisera(pomocnyBod, stranaCtverceNaMape, zivotJeJenNahoda.Next(4),TypPrisery.DEMON);
                            SeznamPictureBoxuNaMape.Add(prisera);
                            SeznamPohyblivychPrvkuNaMape.Add(prisera);
                            this.Controls.Add(prisera);
                            seznamPriser.Add(prisera);

                            g.DrawImage(DejObrazekPodlahy(zivotJeJenNahoda.Next(25)), pomocnyBod.X, pomocnyBod.Y, stranaCtverceNaMape, stranaCtverceNaMape);
                            break;
                        case 'Z'://Zombie
                            break;
                        case 'R'://oRc
                            break;
                        case 'H'://HardBlock
                            g.DrawImage(Properties.Resources.hardblock64x64, pomocnyBod.X, pomocnyBod.Y, stranaCtverceNaMape, stranaCtverceNaMape);
                            seznamHranice.Add(new Hranice(pomocnyBod.Y, pomocnyBod.Y + stranaCtverceNaMape, pomocnyBod.X + stranaCtverceNaMape, pomocnyBod.X, false,false,null));
                            break;
                        case '|'://HardBlock okraj vertikalni
                            g.DrawImage(Properties.Resources.okraj64x64, pomocnyBod.X, pomocnyBod.Y, stranaCtverceNaMape, stranaCtverceNaMape);
                            seznamHranice.Add(new Hranice(pomocnyBod.Y, pomocnyBod.Y + stranaCtverceNaMape, pomocnyBod.X + stranaCtverceNaMape, pomocnyBod.X, false,false,null));
                            break;
                        case '-'://hardblock okraj horizontalni
                            g.DrawImage(Properties.Resources.hardblock64x64, pomocnyBod.X, pomocnyBod.Y, stranaCtverceNaMape, stranaCtverceNaMape);
                            seznamHranice.Add(new Hranice(pomocnyBod.Y, pomocnyBod.Y + stranaCtverceNaMape, pomocnyBod.X + stranaCtverceNaMape, pomocnyBod.X, false,false,null));
                            break;
                        case 'S'://SoftBlock
                            g.DrawImage(Properties.Resources.bedna64x64, pomocnyBod.X, pomocnyBod.Y, stranaCtverceNaMape, stranaCtverceNaMape);
                            seznamHranice.Add(new Hranice(pomocnyBod.Y, pomocnyBod.Y + stranaCtverceNaMape, pomocnyBod.X + stranaCtverceNaMape, pomocnyBod.X, true,false,null));
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

        static public Image DejObrazekPodlahy(int nahodneCislo)
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

        Point DejMiSouradniceLevehoHornihoRohuCtverceNaKteremSeNachaziTentoHrdina(Hrdina mujHrdina)
        {
            Point stredniBodHrdiny = new Point();
            Point vysledek = new Point();
            stredniBodHrdiny.X = mujHrdina.Location.X + mujHrdina.Width / 2;
            stredniBodHrdiny.Y = mujHrdina.Location.Y + mujHrdina.Height / 2;
            stredniBodHrdiny.X -= levyHorniRohMapy.X;
            stredniBodHrdiny.Y -= levyHorniRohMapy.Y;
            vysledek.X = stredniBodHrdiny.X / 30;
            vysledek.X *= 30;
            vysledek.X += levyHorniRohMapy.X;
            vysledek.Y = stredniBodHrdiny.Y / 30;
            vysledek.Y *= 30;
            vysledek.Y += levyHorniRohMapy.Y;
            return vysledek;
        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            int kontrola = 0;
            if (stav == Stav.HRA)
            {
                Point pomocnyBod = DejMiSouradniceLevehoHornihoRohuCtverceNaKteremSeNachaziTentoHrdina(player1);
                if (Keyboard.IsKeyDown(Key.F))
                {
                    if (seznamBomb.Count < player1.kolikBombMohuMit)
                    {
                        foreach (Bomba bomba in seznamBomb)
                        {
                            if ((bomba.Location.X == pomocnyBod.X) && (bomba.Location.Y == pomocnyBod.Y))
                            {
                                kontrola++;
                                break;
                            }
                        }
                        if (kontrola == 0)
                        {
                            Bomba bomb = new Bomba(DejMiSouradniceLevehoHornihoRohuCtverceNaKteremSeNachaziTentoHrdina(player1), stranaCtverceNaMape, player1.dosahBomb, this);
                            //bombu pridavam do seznamu v kontruktoru
                            //TODO rozmysli si, jestli nebude lepsi dat vsechna pridavani do vsech seznamu rovnou do konstruktoru, zacina v tom byt bordel
                            this.Controls.Add(bomb);
                            Controls.SetChildIndex(bomb, Controls.IndexOf(player1) + 1); //to tu je, aby panak byl nad bombou
                        }
                    }
                }
                if (Keyboard.IsKeyDown(Key.S))
                {
                    player1.PohniSe(player1, SmerPohybu.DOLU, 2); //TODO sem musim naspat nejakou rozumnou rcyhlost, aby to fungovalo i pro formulare jine velikosti
                }
                else
                {
                    if (Keyboard.IsKeyDown(Key.W)) player1.PohniSe(player1, SmerPohybu.NAHORU,2);
                }
                if (Keyboard.IsKeyDown(Key.A))
                {
                    player1.PohniSe(player1, SmerPohybu.DOLEVA,2);
                }
                else
                {
                    if (Keyboard.IsKeyDown(Key.D)) player1.PohniSe(player1, SmerPohybu.DOPRAVA,2);
                }
                
                if (pocetHracu == PocetHracu.DVA)
                {
                    if (Keyboard.IsKeyDown(Key.Left))
                    {
                        player2.PohniSe(player2, SmerPohybu.DOLEVA,2);
                    }
                    else
                    {
                        if (Keyboard.IsKeyDown(Key.Right)) player2.PohniSe(player2, SmerPohybu.DOPRAVA,2);
                    }
                    if (Keyboard.IsKeyDown(Key.Up))
                    {
                        player2.PohniSe(player2, SmerPohybu.NAHORU,2);
                    }
                    else
                    {
                        if (Keyboard.IsKeyDown(Key.Down)) player2.PohniSe(player2, SmerPohybu.DOLU,2);
                    }
                }
                foreach(Prisera prisera in seznamPriser)
                {
                    if (((prisera.Location.X - levyHorniRohMapy.X) % stranaCtverceNaMape == 0) && ((prisera.Location.Y - levyHorniRohMapy.Y) % stranaCtverceNaMape == 0))
                    {
                        int priseraJeUveznena = 0; //to je tu potreba, aby to nezkouselo vsecny ctyri smery donekonecna
                        SmerPohybu opacnySmer = DejMiOpacnySmerPohybu(prisera.smer);
                        SmerPohybu prvniVolba = DejMiNahodnySmerPohybuRuznyOd(opacnySmer);
                        SmerPohybu druhaVolba = DejMiNahodnySmerPohybuRuznyOd(opacnySmer, prvniVolba);
                        SmerPohybu tretiVolba = DejMiNahodnySmerPohybuRuznyOd(opacnySmer, prvniVolba, druhaVolba);
                        SmerPohybu volba = prvniVolba;
                        while((priseraJeUveznena < 4) && (!prisera.PohniSe(prisera,volba, 2)))
                        {
                            priseraJeUveznena++;
                            if(volba == prvniVolba)
                            {
                                volba = druhaVolba;
                            }
                            else
                            {
                                if (volba == druhaVolba)
                                {
                                    volba = tretiVolba;
                                }
                                else
                                {
                                    if (volba == tretiVolba) volba = opacnySmer;
                                }
                            }
                        }
                        prisera.smer = volba;
                    }
                    else
                    {
                        prisera.PohniSe(prisera, prisera.smer, 2);
                    }
                }
            }
        }

        public SmerPohybu DejMiNahodnySmerPohybuRuznyOd(SmerPohybu smerPohybu)
        {
            SmerPohybu zkoumanySmer = SmerPohybu.DOPRAVA;
            while (zkoumanySmer == smerPohybu)
            {
                int nahodneCislo = zivotJeJenNahoda.Next(4);
                switch (nahodneCislo)
                {
                    case 0:
                        zkoumanySmer = SmerPohybu.NAHORU;
                        break;
                    case 1:
                        zkoumanySmer = SmerPohybu.DOLU;
                        break;
                    case 2:
                        zkoumanySmer = SmerPohybu.DOLEVA;
                        break;
                    case 3:
                        zkoumanySmer = SmerPohybu.DOPRAVA;
                        break;
                }
            }
            return zkoumanySmer;
        }

        public SmerPohybu DejMiNahodnySmerPohybuRuznyOd(SmerPohybu prvniSmer, SmerPohybu druhySmer)
        {
            SmerPohybu zkoumanySmer = SmerPohybu.DOPRAVA;
            while ((zkoumanySmer == prvniSmer) || (zkoumanySmer == druhySmer))
            {
                int nahodneCislo = zivotJeJenNahoda.Next(4);
                switch (nahodneCislo)
                {
                    case 0:
                        zkoumanySmer = SmerPohybu.NAHORU;
                        break;
                    case 1:
                        zkoumanySmer = SmerPohybu.DOLU;
                        break;
                    case 2:
                        zkoumanySmer = SmerPohybu.DOLEVA;
                        break;
                    case 3:
                        zkoumanySmer = SmerPohybu.DOPRAVA;
                        break;
                }
            }
            return zkoumanySmer;
        }

        public SmerPohybu DejMiNahodnySmerPohybuRuznyOd(SmerPohybu prvniSmer,SmerPohybu druhySmer,SmerPohybu tretiSmer)
        {
            SmerPohybu zkoumanySmer = SmerPohybu.DOPRAVA;
            while ((zkoumanySmer == prvniSmer) || (zkoumanySmer == druhySmer) || (zkoumanySmer == tretiSmer))
            {
                switch (zkoumanySmer)
                {
                    case SmerPohybu.DOPRAVA:
                        zkoumanySmer = SmerPohybu.DOLU;
                        break;
                    case SmerPohybu.DOLEVA:
                        zkoumanySmer = SmerPohybu.NAHORU;
                        break;
                    case SmerPohybu.NAHORU:
                        zkoumanySmer = SmerPohybu.DOPRAVA;
                        break;
                    case SmerPohybu.DOLU:
                        zkoumanySmer = SmerPohybu.DOLEVA;
                        break;
                }
            }
            return zkoumanySmer;
        }
    
        public SmerPohybu DejMiOpacnySmerPohybu(SmerPohybu puvodniSmerPohybu)
        {
            switch (puvodniSmerPohybu)
            {
                case SmerPohybu.DOPRAVA:
                    return SmerPohybu.DOLEVA;
                case SmerPohybu.DOLEVA:
                    return SmerPohybu.DOPRAVA;
                case SmerPohybu.NAHORU:
                    return SmerPohybu.DOLU;
                case SmerPohybu.DOLU:
                    return SmerPohybu.NAHORU;
            }
            return SmerPohybu.NAHORU; //sem se to stejne nikdy nedostane
        }

        public void NastavSvePozadiAObrazekBlikaniJakoTentoObrazek(Image obrazek)
        {
            blikani = new PictureBox();
            blikani.Size = this.ClientSize;
            blikani.Image = pozadiHraciPlochy;
            this.Controls.Add(blikani);
            this.BackgroundImage = pozadiHraciPlochy;
        }
    
    }


    public abstract class Prvek : PictureBox
    {

    }

    public abstract class PohyblivyPrvek : Prvek
    {
        public enum OtoceniAminace { DOPRAVA , DOLEVA };
        OtoceniAminace otoceniAnimace = new OtoceniAminace();
        public bool PohniSe(PohyblivyPrvek sender, SmerPohybu smer,int RychlostPohybu)
        {
            int rychlostPohybu = RychlostPohybu;
            int kontrola = 0;
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
                    novaDolni -= rychlostPohybu;
                    novaHorni -= rychlostPohybu;
                    break;
                case SmerPohybu.DOLU:
                    novaDolni += rychlostPohybu;
                    novaHorni += rychlostPohybu;
                    break;
                default:
                    break;
            }
            foreach (Hranice hrana in Form1.seznamHranice)
            {
                //pripad, ze jsem na bombe a musim z ni odejit
                if ((sender.Location.Y < hrana.dolni) && (sender.Location.Y + sender.Height > hrana.horni) && (sender.Location.X < hrana.prava) && (sender.Location.X + sender.Width > hrana.leva))
                {
                    kontrola++;
                }
                if ((novaHorni >= hrana.dolni) || (novaDolni <= hrana.horni) || (novaLeva >= hrana.prava) || (novaPrava <= hrana.leva))
                {
                    kontrola++;
                }
            }
            if (kontrola >= Form1.seznamHranice.Count()) //tady je mensi nebo rovna se, protoze kdyz hrdina odejde z bomby, tak je ta kontrola vetsi kvuli necemu
            {
                sender.Location = new Point(novaLeva, novaHorni);
                return true;
            }
            else
            {
                return false;
            }
        }
        abstract public void UberSiZivot(int pocetZivotuKtereSiMaUbrat);   
    }

    public abstract class NepohyblivyPrvek
    {

    }

    public class Hrdina : PohyblivyPrvek
    {
        public int dosahBomb = 3;
        public Form1 formularNaKteremSeHrdinaNachazi;
        public int pocetZivotu = 1;
        //TODO nastav spravne
        public int kolikBombMohuMit = 3;
        public bool nesmrtelny; // bude se hodit, kdyz ho sejme prisera nebo bomba, tak musi byt na chvili nesmrtelny
        System.Windows.Forms.Timer casovac;
        System.Windows.Forms.Timer ukonceniHry;
        public Hrdina(Point souradnice, int velikostctverecku, Form1 formularSender)
        {
            nesmrtelny = false;
            Location = souradnice;
            this.Height = 4*velikostctverecku/5; //TODO jak velky bude hrdina
            this.Width = 4*velikostctverecku/5;
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.BackColor = Color.Transparent;
            this.formularNaKteremSeHrdinaNachazi = formularSender;
        }
        public void UdelejHradinuNesmrtelneho(int naKolikMilisekund)
        {
            casovac = new System.Windows.Forms.Timer();
            casovac.Interval = naKolikMilisekund;
            nesmrtelny = true;
            casovac.Tick += new EventHandler(UkonceniNesmrtelnosti);
            casovac.Start();
        }
        public void UkonceniNesmrtelnosti(Object sender, EventArgs e)
        {
            casovac.Stop();
            nesmrtelny = false;
        }
        public void PridejSiZivot(int pocetPridavanychZivotu)
        {
            this.pocetZivotu += pocetPridavanychZivotu;
        }
        public void NastavProhru(Object sender, EventArgs e)
        {
            formularNaKteremSeHrdinaNachazi.NastavStav(Form1.Stav.PROHRA);
            ukonceniHry.Stop();
        }
        public override void UberSiZivot(int pocetZivotuKtereSiMaUbrat)
        {
            if (!this.nesmrtelny)
            {
                pocetZivotu -= pocetZivotuKtereSiMaUbrat;
                UdelejHradinuNesmrtelneho(3000);
                if (pocetZivotu < 1)
                {
                    switch (formularNaKteremSeHrdinaNachazi.pocetHracu)
                    {
                        case Form1.PocetHracu.JEDEN:
                            ukonceniHry = new System.Windows.Forms.Timer();
                            ukonceniHry.Interval = 1000;
                            ukonceniHry.Tick += new EventHandler(NastavProhru);
                            ukonceniHry.Start();
                            this.Visible = false;
                            break;
                        case Form1.PocetHracu.DVA:
                            this.Dispose();
                            break;
                    }
                }        
            }
            
        }
    }


    public class HardBlock : NepohyblivyPrvek
    {
        int i; //xova souradnice
        int j;
        Hranice odkazNaHraniciKteraPatriDotycenmuHardBlocku;
        public HardBlock(Point souradnice, int velikostctverecku)
        {
            i = souradnice.X - Form1.levyHorniRohMapy.X;
            i /= velikostctverecku;
            j = souradnice.Y - Form1.levyHorniRohMapy.Y;
            j /= velikostctverecku;

        }
    }

    public class SoftBlock : NepohyblivyPrvek
    {
        int i; //xova souradnice
        int j;
        Hranice odkazNaHraniciKteraPatriDotycenmuSoftBlocku;
        public SoftBlock(Point souradnice, int velikostctverecku)
        {
            i = souradnice.X - Form1.levyHorniRohMapy.X;
            i /= velikostctverecku;
            j = souradnice.Y - Form1.levyHorniRohMapy.Y;
            j /= velikostctverecku;

        }
    }

    public class Bomba : Prvek
    {
        public int i;//x-ovy index
        public int j;//y-ovy index
        public int dosahBomby;
        public Form1 totoJeFormularNaKteremSeBombaNachazi;
        Point polohaVuciLevemuHornimuRohuDesky;
        public Hranice odkazNaHraniciKteraPatriDotycneBombe; //to tu mam abych pak mel jak odstranit bombu ze seznamu hranic
        public System.Windows.Forms.Timer casDoVybuchnuti;
        public Bomba(Point souradnice, int velikostctverecku, int dosahbomby,Form1 formular)
        {
            Location = souradnice;
            this.Image = Properties.Resources.bomba_25t_64x64;
            this.Height = velikostctverecku; 
            this.Width = velikostctverecku;
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.BackColor = Color.Transparent;
            casDoVybuchnuti = new System.Windows.Forms.Timer();
            casDoVybuchnuti.Interval = 3000;
            casDoVybuchnuti.Enabled = true;
            casDoVybuchnuti.Tick += new EventHandler(VybuchBomby);
            odkazNaHraniciKteraPatriDotycneBombe = new Hranice(souradnice.Y, souradnice.Y + velikostctverecku, souradnice.X + velikostctverecku, souradnice.X, true,true,this);
            Form1.seznamHranice.Add(odkazNaHraniciKteraPatriDotycneBombe);
            Form1.seznamBomb.Add(this);
            polohaVuciLevemuHornimuRohuDesky.X = souradnice.X - Form1.levyHorniRohMapy.X;
            polohaVuciLevemuHornimuRohuDesky.Y = souradnice.Y - Form1.levyHorniRohMapy.Y;
            i = polohaVuciLevemuHornimuRohuDesky.X / velikostctverecku;
            j = polohaVuciLevemuHornimuRohuDesky.Y / velikostctverecku;
            dosahBomby = dosahbomby;
            totoJeFormularNaKteremSeBombaNachazi = formular;
            Graphics graphics = Graphics.FromImage(Form1.pozadiHraciPlochy);
        }
        public void VybuchBomby(Object sender, EventArgs e)
        {
            int g = 0;//x-ova
            int f = 0;//y-ova
            bool uzDalNepokracuj = false;
            casDoVybuchnuti.Stop();
            //ted se chystam proves jakoby ctyri cykly do ctyr ruznych smeru. g,f jsou koneficienty, kterymi pronasobuju, abych dostal spravne ctyri smery
            Form1.seznamBomb.Remove(this);
            Form1.seznamHranice.Remove(odkazNaHraniciKteraPatriDotycneBombe);
            for (int k = 0; k < 4; k++)
            {
                switch (k)
                {
                    case 1:
                        g = 1;
                        f = 0;
                        break;
                    case 2:
                        g = -1;
                        f = 0;
                        break;
                    case 3:
                        g = 0;
                        f = 1;
                        break;
                    case 0:
                        g = 0;
                        f = -1;
                        break;
                    default:
                        break;
                }
                uzDalNepokracuj = false;
                for (int index = 1; index < dosahBomby+1; index++)
                {
                    foreach (Hranice hrana in Form1.seznamHranice)
                    {
                        if (((i + g * index) * Form1.stranaCtverceNaMape == (hrana.leva - Form1.levyHorniRohMapy.X) && ((j + f * index) * Form1.stranaCtverceNaMape == (hrana.horni - Form1.levyHorniRohMapy.Y))))
                        {//pokud jsem na teto radce, tak policko, ktere zkoumam je v seznamu hran
                            if (hrana.softblock)
                            {//pokud je zrovna hrana softblock, tak se musi premalovat
                                if (hrana.jsemBomba)//pripad, ze je to softblock a bomba zaroven
                                {
                                    hrana.odkazNaBombuKterePatri.VybuchBomby(hrana, new EventArgs());
                                }
                                Plamen plaminek = new Plamen(i + g * index, j + f * index, Form1.stranaCtverceNaMape);
                                totoJeFormularNaKteremSeBombaNachazi.Controls.Add(plaminek);
                                totoJeFormularNaKteremSeBombaNachazi.Controls.SetChildIndex(plaminek, totoJeFormularNaKteremSeBombaNachazi.Controls.GetChildIndex(totoJeFormularNaKteremSeBombaNachazi.player1) + 1);
                                //ted musim odstanit hranici, aby tam zase mohl hrdina vkrocit
                                Form1.seznamHranice.Remove(hrana);
                                //ted tam musim nakreslit podlahu
                                Form1.g.DrawImage(Form1.DejObrazekPodlahy(Form1.zivotJeJenNahoda.Next(25)), (i + g * index) * Form1.stranaCtverceNaMape + Form1.levyHorniRohMapy.X, (j + f * index) * Form1.stranaCtverceNaMape + Form1.levyHorniRohMapy.Y, Form1.stranaCtverceNaMape, Form1.stranaCtverceNaMape);
                                //a tady se to jeste musi nastavit na pozadi Formu
                                totoJeFormularNaKteremSeBombaNachazi.NastavSvePozadiAObrazekBlikaniJakoTentoObrazek(Form1.pozadiHraciPlochy);
                                uzDalNepokracuj = true;
                                break; // ten tu musi byt, protoze foreach po odstaraneni prvku, vyhazuje vyjimky
                            }
                            uzDalNepokracuj = true;
                        }
                    }
                    if (uzDalNepokracuj) break;
                    //pokud dojdu sem, tak na dotycnem policku nelezi zadna hranice
                    //tedy az tady ma cenu testovat, jestli jsou na polcku jine veci nez hranice, treba prisery nebo hrdina
                    Plamen plamen1 = new Plamen(i + g * index, j + f * index, Form1.stranaCtverceNaMape);
                    totoJeFormularNaKteremSeBombaNachazi.Controls.Add(plamen1);
                    totoJeFormularNaKteremSeBombaNachazi.Controls.SetChildIndex(plamen1, totoJeFormularNaKteremSeBombaNachazi.Controls.GetChildIndex(totoJeFormularNaKteremSeBombaNachazi.player1) + 1);
                    plamen1.ZnicVseCoJeNaTvemPoli();
                }
            }
            //toto spusti plamen na policku bomby
            Plamen plamen = new Plamen(i, j, Form1.stranaCtverceNaMape);
            totoJeFormularNaKteremSeBombaNachazi.Controls.Add(plamen);
            totoJeFormularNaKteremSeBombaNachazi.Controls.SetChildIndex(plamen, totoJeFormularNaKteremSeBombaNachazi.Controls.GetChildIndex(totoJeFormularNaKteremSeBombaNachazi.player1) + 1);
            plamen.ZnicVseCoJeNaTvemPoli();
            this.Dispose();
        }
    }

    public class Hranice
    {
        public int horni;
        public int dolni;
        public int prava;
        public int leva;
        public bool softblock;
        public bool jsemBomba;
        public Bomba odkazNaBombuKterePatri;
        public Hranice(int Horni,int Dolni,int Prava,int Leva,bool Softblock,bool JsemBomba,Bomba patrimTetoBombe)
        {
            horni = Horni;
            dolni = Dolni;
            prava = Prava;
            leva = Leva;
            softblock = Softblock;
            jsemBomba = JsemBomba;
            odkazNaBombuKterePatri = patrimTetoBombe;
        }
    }

    public class Prisera : PohyblivyPrvek
    {
        public SmerPohybu smer = SmerPohybu.DOPRAVA;
        System.Windows.Forms.Timer casovac;
        public Prisera(Point souradnice, int velikostctverecku,int nahodneCislo,Form1.TypPrisery typPrisery)
        {
            Location = souradnice;
            this.Height = velikostctverecku;
            this.Width = velikostctverecku;
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.BackColor = Color.Transparent;
            this.Image = DejMiObrazekPrisery(nahodneCislo,typPrisery);
        }

        public Image DejMiObrazekPrisery(int nahodneCislo, Form1.TypPrisery typPrisery)
        {
            //TODO dodelat
            Image vysledek;
            Image prvni;
            Image druhy;
            Image treti;
            Image ctvrty;
            switch (typPrisery)
            {
                case Form1.TypPrisery.DEMON:
                    //sem vzdy napisu Image Prvni, Image Druha,... a pak budu vybirat z promennych
                    break;
                case Form1.TypPrisery.ZOMBIE:
                    break;
                case Form1.TypPrisery.ORC:
                    break;
                default:
                    break;
            }
            switch (nahodneCislo)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                default:
                    break;
            }
            vysledek = Properties.Resources.eyebrowman_right;//TODO sem musim nahrat spravne gify
            return vysledek;
        }

        public void DestruktorPrisery()
        {
            //TODO vymysli, jak ma prisera umirat
            //TODO odebrat ze vsech tech seznamu
            this.Dispose();
        }
        public override void UberSiZivot(int pocetZivotuKtereSiMaUbrat)
        {
            DestruktorPrisery();
        }
    }

    public class Plamen : Prvek
    {
        int i;//x-ovy index
        int j;//y-ovy index
        System.Windows.Forms.Timer casovacAutoDestrukce;
        public Plamen(int I,int J,int velikostctverecku)
        {
            i = I;
            j = J;
            BackColor = Color.Transparent;
            Image = Properties.Resources.plamen_64x64;
            Location = new Point(Form1.levyHorniRohMapy.X + i * velikostctverecku,Form1.levyHorniRohMapy.Y + j * velikostctverecku);
            Height = velikostctverecku;
            Width = velikostctverecku;
            SizeMode = PictureBoxSizeMode.StretchImage;
            casovacAutoDestrukce = new System.Windows.Forms.Timer();
            casovacAutoDestrukce.Interval = 750; // takhle dlouho bezi ta animace vybuchu
            casovacAutoDestrukce.Tick += new EventHandler(destruktorPlamenu);
            casovacAutoDestrukce.Start();
        }
        public void destruktorPlamenu(Object sender, EventArgs e)
        {
            casovacAutoDestrukce.Stop();
            this.Dispose();
        }
        public void ZnicVseCoJeNaTvemPoli()
        {
            int horni = j*Form1.stranaCtverceNaMape + Form1.levyHorniRohMapy.Y; //to budou okraje policka, na kterem se deje vybuch
            int dolni = (j+1)*Form1.stranaCtverceNaMape + Form1.levyHorniRohMapy.Y;
            int leva = i * Form1.stranaCtverceNaMape + Form1.levyHorniRohMapy.X;
            int prava = (i + 1) * Form1.stranaCtverceNaMape + Form1.levyHorniRohMapy.X;
            foreach(PohyblivyPrvek vec in Form1.SeznamPohyblivychPrvkuNaMape)
            {
                if ((horni < vec.Location.Y + Size.Height) && (dolni > vec.Location.Y) && (leva < vec.Location.X + Size.Width) && (prava > vec.Location.X))
                {
                    vec.UberSiZivot(1);
                }
            }
        }
    }


}
