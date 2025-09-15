using System.Text.Json.Serialization.Metadata;

namespace WinFormium.JavaScript;

class TestJsBindingObjectChild : NativeProxyObject
{
    //public override string Name => "testInnerObject";
    public override void OnFunctionApply(NativeProxyObjectFunctionApplyEventArgs args)
    {
        
    }
    public override void OnGettingProperty(NativeProxyObjectGetPropertyEventArgs args)
    {
    }
    public override bool OnSettingProperty(NativeProxyObjectSetPropertyEventArgs args)
    {
        return false;
    }
}

class TestJsBindingObject : NativeProxyObject
{

    public TestJsBindingObjectChild AnotherObject { get; } = new TestJsBindingObjectChild();


    public string GetOrSetString { get; set; } = "GetOrSetString";

    public int[] GetOnlyArray => [1,2,3];

    public DateTime DateTimeProperty { get; set; } = DateTime.Now;

    public string SyncMethodTest(string a, string b, string c)
    {
        Console.WriteLine($"WriteConsoleTest: {a}, {b}, {c}");
        return $"WriteConsoleTest: {a}, {b}, {c}";
    }

    public async Task<string> AsyncMethodTest(string a, string b, string c)
    {
        await Task.Delay(5000);
        Console.WriteLine($"WriteConsoleTest: {a}, {b}, {c}");
        return $"WriteConsoleTest: {a}, {b}, {c}";
    }
    //public override string Name => "testNativeObject";

    public override void OnFunctionApply(NativeProxyObjectFunctionApplyEventArgs args)
    {
        switch (args.PropertyName)
        {
            case "syncFunction":
                {
                    args.ReturnJson(JsonSerializer.Serialize(new { a = 1, b = 2, c = 3 }));
                }
                return;
            case "asyncFunction":
                {
                    var promise = args.ReturnPromise();

                    TestAsyncFunc(promise);
                }
                return;
         
        }

        args.Cancel();
    }

    private async void TestAsyncFunc(JavaScriptPromise promise)
    {
        await Task.Delay(new Random().Next(1000,5000));

        var rnd = new Random();
        var a = rnd.Next(0, 5);

        if(a == 0)
        {
            promise.Resolve(JsonSerializer.Serialize(new { Succeed = true, Message="Hello World", Date = DateTime.Now }));
        }
        else
        {
            promise.Reject("Reject");
        }

    }

    public override void OnGettingProperty(NativeProxyObjectGetPropertyEventArgs args)
    {
        switch (args.PropertyName)
        {
            case "datetime":
                args.ReturnJson(JsonSerializer.Serialize(DateTime.Now));
                break;
            case "jsonobject":
                args.ReturnJson(JsonSerializer.Serialize(JsonSerializer.Serialize(new { a = 1, b = 2, c = 3, d="test" })));
                break;
            case "jsonarray":
                args.ReturnJson(JsonSerializer.Serialize(JsonSerializer.Serialize(new []{1,2,3,4,5 })));
                break;

            case "jsonstring":
                args.ReturnJson(JsonSerializer.Serialize("json string hello world"));
                break;
            case "object":
                args.ReturnJson(JsonSerializer.Serialize(new { a = 1, b = 2, c = 3 }));
                break;
            case "array":
                args.ReturnJson(JsonSerializer.Serialize(new[] { 1, 2, 3 }));
                break;
            case "string":
                args.ReturnJson(GetOrSetString);
                break;
            case "int":
                args.ReturnJson("123");
                break;
            case "number":
                args.ReturnJson("123.456");
                break;
            case "bool":
                args.ReturnJson("true");
                break;
            case "innerObject":
                args.AsNativeObject(AnotherObject);
                break;
            case "asyncFunction":
                args.AsFunction();
                break;
            case "syncFunction":
                args.AsFunction();
                break;


        }
    }

    public override bool OnSettingProperty(NativeProxyObjectSetPropertyEventArgs args)
    {
        switch (args.PropertyName)
        {
            case "string":
                GetOrSetString = args.ValueAsJson?? string.Empty;
                return true;
        }

        return false;
    }
}