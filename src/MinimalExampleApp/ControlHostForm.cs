using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinimalExampleApp;
public partial class ControlHostForm : Form
{
    public ControlHostForm()
    {
        InitializeComponent();
        ShowInTaskbar = false;
    }

    private void button1_Click(object sender, EventArgs e)
    {
        Application.Exit(new CancelEventArgs() {  });

        
    }
}
