using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.Button;

namespace Bookkeeping.Components
{
    public partial class Navbar : UserControl
    {

        public Navbar()
        {
            InitializeComponent();
            this.SizeChanged += Navbar_SizeChanged;
            var Viewstypes = Assembly.GetExecutingAssembly().DefinedTypes;
            var Viewslist = Viewstypes.Where(x => x.BaseType == typeof(Form))
                .Where(y => y.Namespace == "Bookkeeping.Views").ToList();
            foreach (var view in Viewslist)
            {
                Button button = new Button();
                button.Text = view.Name;
                button.Click += ChoosePage;
                button.Margin = new Padding(0);
                flowLayoutPanel1.Controls.Add(button);
            }
        }

        private void Navbar_SizeChanged(object sender, EventArgs e)
        {
            foreach (Button button in flowLayoutPanel1.Controls)
            {
                button.Width = (flowLayoutPanel1.Width / flowLayoutPanel1.Controls.Count);
                button.Height = flowLayoutPanel1.Height;
            }
        }

        public void ChoosePage(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Form form = SignletonForm.GetForm(button.Text);
            form.Show();

        }


    }
}
