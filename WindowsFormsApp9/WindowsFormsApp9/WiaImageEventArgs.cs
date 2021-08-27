using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp9
{
    class WiaImageEventArgs:EventArgs
    {
        public WiaImageEventArgs(Image img)
        {
            ScannedImage = img;
        }
        public Image ScannedImage { get; private set; }
    }
}
