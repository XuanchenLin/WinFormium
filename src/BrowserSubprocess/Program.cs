using WinFormium;

namespace BrowserSubprocess;

internal class Program
{
    [STAThread]
    static void Main(string[] args)
    {


        ApplicationConfiguration.Initialize();


        if(args.Length == 0)
        {
            //MessageBox.Show("This application is a subprocess for WinFormim Project example apps and should not be run directly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }


        var builder = WinFormiumApp.CreateBuilder();

        var app = builder.Build();

        app.RunAsSubprocess();

    }
}
