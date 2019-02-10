using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hra
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        enum Stav { MENU, VYBERPOCETHRACU, VYBERLEVEL, HRA, VYHRA, PROHRA , PAUZA};
        Stav stav;

        void NastavStav (Stav jaky)
        {
            switch (jaky)
            {
                case Stav.MENU:
                    break;
                case Stav.VYBERPOCETHRACU:
                    break;
                case Stav.VYBERLEVEL:
                    break;
                case Stav.HRA:
                    break;
                case Stav.VYHRA:
                    break;
                case Stav.PROHRA:
                    break;
                case Stav.PAUZA:
                    break;
                default:
                    break;
            }
        }

    }
}
