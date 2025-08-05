using WinFormium;
using WinFormium.WebResource;

namespace MinimalExampleApp;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        

        var builder = WinFormiumApp.CreateBuilder();

        var app = builder
            //.UseEmbeddedBrowser()
            .UseDevTools()
            //.UseCulture("en-US")
            //.UseCacheCleanup()
            .UseSingleRunningInstance()
            .UseGlobalVirtualHostNameForAssemblyEmbeddedFileMapping(new EmbeddedFileMappingOptions {
                DomainName = "localresources.app",
                Scheme = "https",
                EmbeddedResourceDirectoryName = "wwwroot",
                ResourceAssembly = typeof(MainWindow).Assembly,
            })
            //.UseGlobalVirtualHostNameForFolderMapping(new FolderMappingOptions { 
            //    DomainName ="folderresources.app",
            //    Scheme= "https",
            //    FolderPath = "C:\\Temp\\"
            //})
            .UseGlobalHttpProxyMapping("https","www.google.com","https://www.bing.com")
            .AddWindowBindingScripts(() => { 
                
            })
            .UseWinFormiumApp<MinimalApp>()
            .Build();

        app.Run();
    }
}
