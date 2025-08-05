using WinFormium;

namespace MinimalExampleApp;

internal class MinimalApp : AppStartup
{
    protected override void OnApplicationLaunched(string[] commandArgs)
    {
        //Application.EnableVisualStyles();
        //Application.SetCompatibleTextRenderingDefault(false);
    }

    protected override void OnApplicationTerminated()
    {
        //Console.WriteLine("Goodbye World!");
    }

    protected override void OnApplicationException(Exception? exception = null)
    {
        //Console.WriteLine($"Error:\r\n{exception}");
    }

    protected override AppCreationAction? OnApplicationStartup(StartupSettings settings)
    {
        //#if !DEBUG
        //        var startup = new StartupWindow();
        //        startup.ShowDialog();
        //#endif
        var startup = new StartupWindow();
        startup.ShowDialog();
        return settings.UseMainWindow(new MainWindow());
    }

    protected override void ConfigureSubprocessAppSettings(SubprocessAppSettings subprocessAppSettings)
    {
#if !DEBUG
        if (File.Exists("BrowserSubprocess.exe"))
        {
            subprocessAppSettings.SubprocessFilePath = "BrowserSubprocess.exe";
        }
#endif
    }

    //protected override void ConfigureCefRedistributableSettings(ChromiumEmbeddedRedistributableSettings cefRedistributableSettings)
    //{
    //    if (!cefRedistributableSettings.IsCefRedistributableValid)
    //    {
    //        MessageBox.Show("CEF distribution is not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

    //        cefRedistributableSettings.ThrowExceptionIfCefRedistributableIsNotValid = false;
    //    }
    //}
}