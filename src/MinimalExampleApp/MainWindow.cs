using System.Text.Json;

using WinFormium;
using WinFormium.JavaScript;
using WinFormium.WebResource;

namespace MinimalExampleApp;

public class MainWindow : Formium
{
//#if DEBUG
//    const string DEMO_HOST_ADDR = "http://127.0.0.1:8080";
//#else
//    const string DEMO_HOST_ADDR = "https://localresources.app";
//#endif

    const string DEMO_HOST_ADDR = "https://localresources.app";


    static Icon DVAIcon = new Icon(new MemoryStream(Properties.Resources.DVAIcon));
    static Icon BrowserIcon = new Icon(new MemoryStream(Properties.Resources.BrowserIcon));
    static Icon ColorsIcon = new Icon(new MemoryStream(Properties.Resources.ColorsIcon));
    static Icon ExampleIcon = new Icon(new MemoryStream(Properties.Resources.Example));
    static Icon MatrixIcon = new Icon(new MemoryStream(Properties.Resources.MatrixIcon));


    private class MainWindowJavaScriptObject : JavaScriptNativeObject
    {
        private MainWindow _mainWindow;

        private LayeredDemoWindow? _layeredDemoWindow;
        private KioskDemoWindow? _kioskDemoWindow;
        private FilterWebResponseDemoWindow? _filterWebResponseDemoWindow;




        List<Formium> _demoWindows { get; } = new();

        public MainWindowJavaScriptObject(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            DefineSynchronousFunction("createDemoWindow", CreateDemoWindow);
            DefineSynchronousFunction("closeAllDemoWindows", CloseAllDemoWindows);
            DefineSynchronousFunction("createLayeredWindow", CreateLayeredWindow);
            DefineSynchronousFunction("createKioskWindow", CreateKioskWindow);
            DefineSynchronousFunction("openFilterWebResponseDemoWindow", OpenFilterWebResponseDemoWindow);


        }

        private string? OpenFilterWebResponseDemoWindow(string? args)
        {
            _mainWindow.InvokeIfRequired(() =>
            {
                if (_filterWebResponseDemoWindow == null || _filterWebResponseDemoWindow.IsDisposed)
                {
                    _filterWebResponseDemoWindow = new FilterWebResponseDemoWindow
                    {
                        WindowTitle = "Filter Web Response Demo",
                        Icon = BrowserIcon,
                    };
                    _filterWebResponseDemoWindow.Show();
                }
                else
                {
                    _filterWebResponseDemoWindow.Activate();
                }

            });
            return null;
        }

        private string? CreateKioskWindow(string? arg)
        {
            _mainWindow.InvokeIfRequired(() =>
            {
                if (_kioskDemoWindow == null || _kioskDemoWindow.IsDisposed)
                {

                    MessageBox.Show(_mainWindow, "Kiosk 模式下窗口将无法被最小化、最大化或关闭，请使用 ALT + F4 关闭示例窗口。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    _kioskDemoWindow = new KioskDemoWindow
                    {
                        WindowTitle = "Kiosk Window Demo"
                    };
                    _kioskDemoWindow.Show();
                }
            });

            return null;
        }

        private string? CreateLayeredWindow(string? arg)
        {
            _mainWindow.InvokeIfRequired(() =>
            {
                if (_layeredDemoWindow == null || _layeredDemoWindow.IsDisposed)
                {
                    _layeredDemoWindow = new LayeredDemoWindow
                    {
                        WindowTitle = "Layered Window Demo",
                        Icon = BrowserIcon
                    };
                    _layeredDemoWindow.Show();
                }
                else
                {
                    _layeredDemoWindow.Close();
                    _layeredDemoWindow.Dispose();
                }
            });

            return null;
        }

        private string? CloseAllDemoWindows(string? json)
        {
            _mainWindow.InvokeIfRequired(async () =>
            {
                // 循环关闭并移除所有演示窗体
                while (_demoWindows.Count > 0)
                {
                    var window = _demoWindows[_demoWindows.Count - 1];
                    _demoWindows.RemoveAt(_demoWindows.Count - 1);
                    if (window is null || window.IsDisposed) continue;
                    window.Close();
                    window.Dispose();

                    await Task.Delay(100);
                }

                GC.Collect();
            });

            return null;
        }

        private string? CreateDemoWindow(string? json)
        {
            if (string.IsNullOrEmpty(json)) throw new ArgumentNullException(nameof(json), "JSON string cannot be null or empty.");

            var jsonElement = JsonDocument.Parse(json).RootElement; // Validate JSON format

            if (jsonElement.ValueKind == JsonValueKind.Array && jsonElement.GetArrayLength() == 1 && jsonElement[0].ValueKind == JsonValueKind.Object)
            {
                var config = jsonElement[0];

                var style = config.GetProperty("style").GetString() ?? default;
                var backdrop = config.GetProperty("backdrop").GetString() ?? default;
                var page = config.GetProperty("page").GetString() ?? default;

                _mainWindow.InvokeIfRequired(() =>
                {

                    if (style == "NonDecorated" && !OperatingSystem.IsWindowsVersionAtLeast(8, 0))
                    {
                        MessageBox.Show(_mainWindow, "非装饰窗口样式仅在 Windows 8 及以上版本支持。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }


                    if (!OperatingSystem.IsWindowsVersionAtLeast(8, 0) && backdrop != "Auto")
                    {
                        MessageBox.Show(_mainWindow, "Windows 7 无法使用背景类型选项。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string[] win11Supports = ["Mica", "MicaAlt", "Transient"];

                    if (!OperatingSystem.IsWindowsVersionAtLeast(10, 0,22000) && win11Supports.Contains(backdrop))
                    {
                        MessageBox.Show(_mainWindow, "选中的背景类型选项需要 Windows 11 版本。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }



                    if (page == "ShapedTransparentPage" && _demoWindows.Any(x => x.WindowTitle.StartsWith("Shaped")))
                    {
                        // If a shaped transparent window already exists, do not create another one
                        MessageBox.Show(_mainWindow, "鉴于性能问题，每次仅能创建一个异性窗体。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }






                    var demoWin = new DemoWindow(_mainWindow, style, backdrop, page);

                    demoWin.FormClosed += (_, _) =>
                    {
                        _demoWindows.Remove(demoWin);
                    };

                    _demoWindows.Add(demoWin);

                    demoWin.Show(_mainWindow);
                });
            }

            // Logic to create a demo window
            return null; // Return a JSON string if needed
        }

        private class LayeredDemoWindow : Formium
        {
            public LayeredDemoWindow()
            {
                Size = new Size(320, 260);
                Url = $"{DEMO_HOST_ADDR}/home/window-styles/layered/";
                TopMost = true;
                BackColor = Color.Transparent; // Set a transparent background for the layered window
                ShowInTaskbar = false;
                Load += LayeredDemoWindow_Loaded;
            }

            private void LayeredDemoWindow_Loaded(object? sender, BrowserEventArgs e)
            {
                var screen = Screen.FromHandle(Handle);

                var screenBounds = screen.WorkingArea;

                Left = screenBounds.Right - Width;
                Top = screenBounds.Bottom - Height;

            }

            protected override WindowSettings ConfigureWindowSettings(HostWindowBuilder opts)
            {
                var settings = opts.UseLayeredWindow();

                settings.Resizable = false;

                settings.DisableAppRegionMenu = true;

                settings.ClickThrough = true;

                return settings;
            }
        }

        private class KioskDemoWindow : Formium
        {
            public KioskDemoWindow()
            {
                Url = $"{DEMO_HOST_ADDR}/home/window-styles/kiosk/";
                BackColor = Color.Black;
                Icon = MatrixIcon;
            }
            protected override WindowSettings ConfigureWindowSettings(HostWindowBuilder opts)
            {
                var settings = opts.UseKisokWindow();
                settings.Resizable = false;
                settings.DisableAppRegionMenu = true;
                return settings;
            }
        }


        private class DemoWindow : Formium
        {
            private enum WindowStyle
            {
                ExtendsContentIntoTitlebar,
                NonDecorated,
                Default
            }

            private WindowStyle _style;
            private SystemBackdropType _backdropType;
            private bool _isResizable = true;
            private readonly MainWindow _mainWindow;

            public DemoWindow(MainWindow mainWindow, string? style, string? backdrop, string? page)
            {
                _mainWindow = mainWindow;


                if (Enum.TryParse<SystemBackdropType>(backdrop, true, out var backdropType))
                {
                    _backdropType = backdropType;
                    // Set a transparent background for the backdrop
                }
                else
                {
                    _backdropType = SystemBackdropType.Auto;
                }

                WindowTitle = "Window Style Demo";


                switch (style)
                {
                    case "ExtendsContentIntoTitlebar":
                        _style = WindowStyle.ExtendsContentIntoTitlebar;

                        break;

                    case "NonDecorated":
                        _style = WindowStyle.NonDecorated;
                        break;

                    default:
                        _style = WindowStyle.Default;
                        break;
                }

                var url = "https://www.bing.com/";



                switch (page)
                {
                    case "BorderlessPage":
                        url = $"{DEMO_HOST_ADDR}/home/window-styles/default/borderless.html";

                        Size = new Size(1280, 960);
                        MinimumSize = new Size(1024, 640);
                        Icon = DVAIcon;

                        break;

                    case "TransparentPage":
                        url = $"{DEMO_HOST_ADDR}/home/window-styles/default/transparent.html";

                        WindowTitle = "Transparent Window";

                        MinimumSize = Size = new Size(720, 640);

                        Icon = ColorsIcon;


                        break;

                    case "ShapedTransparentPage":
                        url = $"{DEMO_HOST_ADDR}/home/window-styles/default/shaped.html";

                        WindowTitle = "Shaped Transparent Window";

                        Size = new Size(564, 564);

                        StartPosition = FormStartPosition.CenterParent;

                        _isResizable = false;
                        Icon = BrowserIcon;


                        break;

                    default:

                        Icon = BrowserIcon;

                        ShowDocumentTitle = true;

                        Size = new Size(1280, 800);

                        break;
                }

                if (_backdropType != SystemBackdropType.Auto)
                {
                    BackColor = Color.Transparent;
                }

                Url = url;
            }

            protected override WindowSettings ConfigureWindowSettings(HostWindowBuilder opts)
            {
                var settings = opts.UseDefaultWindow();
                settings.ExtendsContentIntoTitleBar = _style == WindowStyle.ExtendsContentIntoTitlebar || _style == WindowStyle.NonDecorated;
                settings.ShowWindowDecorators = _style != WindowStyle.NonDecorated;
                settings.SystemBackdropType = _backdropType;
                settings.Resizable = _isResizable;

                if (!settings.ShowWindowDecorators && _isResizable)
                {
                    settings.WindowEdgeOffsets = new Padding { Top = 15, Right = 20, Bottom = 25, Left = 20 };
                }

                return settings;
            }
        }
    }

    public MainWindow()
    {
        Url = $"{DEMO_HOST_ADDR}/home/";

        StartPosition = FormStartPosition.CenterScreen;
        Icon = ExampleIcon;


        AllowFullscreen = true;

        Load += (sender, e) =>
        {
            RegisterNativeObject("mainWindow", new MainWindowJavaScriptObject(this));
        };


        var sseController = SetVirtualHostNameToServerSentEvents("https://localresources.data/sse");

        sseController.ClientConnected += Controller_ClientConnected;
    }

    private const string FAKE_PROMPT = "我说道，“爸爸，你走吧。”他望车外看了看，说，“我买几个橘子去。你就在此地，不要走动。”我看那边月台的栅栏处有几个卖东西的等着顾客。走到那边月台，须穿过铁道，须跳下去又爬上去。父亲是一个胖子，走过去自然要费事些。我本来要去的，他不肯，只好让他去。我看见他戴着黑布小帽，穿着黑布大马褂，深青布棉袍，蹒跚地走到铁道边，慢慢探身下去，尚不大难。可是他穿过铁道，要爬上那边月台，就不容易了。他用两手攀着上面，两脚再向上缩;他肥胖的身子向左微倾，显出努力的样子。这时我看见他的背影，我的泪很快地流下来了。我赶紧拭干了泪，怕他看见，也怕别人看见。我再向外看时，他已抱了朱红的橘子望回走了。过铁道时，他先将橘子散放在地上，自己慢慢爬下，再抱起橘子走。到这边时，我赶紧去搀他。他和我走到车上，将橘子一股脑儿放在我的皮大衣上。于是扑扑衣上的泥土，心里很轻松似的，过一会说，“我走了;到那边来信!”我望着他走出去。他走了几步，回过头看见我，说，“进去吧，里边没人。”等他的背影混入来来往往的人里，再找不着了，我便进来坐下，我的眼泪又来了。";

    private async void Controller_ClientConnected(object? sender, SSEClientConnectedEventArgs e)
    {
        for (var i = 0; i < FAKE_PROMPT.Length; i++)
        {
            await Task.Delay(Random.Shared.Next(200));
            e.SendEvent(new WinFormium.WebResource.ServerSentEvent
            {
                Data = $"{FAKE_PROMPT[i]}",
            });
        }

        e.SendEvent(new WinFormium.WebResource.ServerSentEvent
        {
            Event = "end",
        });
    }

    protected override WindowSettings ConfigureWindowSettings(HostWindowBuilder opts)
    {
        var win = opts.UseDefaultWindow();

        MinimumSize = new Size(1024, 640);
        Size = new Size(1280, 800);
        win.ExtendsContentIntoTitleBar = true;

        return win;
    }
}