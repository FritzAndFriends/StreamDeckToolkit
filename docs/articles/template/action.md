## Action

When you use the template there are a number of parameters which can set things in the following `Action` class:

    --uuid com.yourcompany.pluginname.actionname
    --plugin-name FirstPlugin

You will also have a model created `CounterSettingsModel` which will be set.

### Attribute

    [StreamDeckActionAttribute(Uuid="com.yourcompany.pluginname.actionname")]

### Class Name

The `--plugin-name` will be a prefix for the `Action` class that is created:

    public class FirstPluginAction : BaseStreamDeckActionWithSettingsModel<Models.CounterSettingsModel>

## BaseStreamDeckAction

```csharp
public ILogger Logger { get; }
public string ActionUuid { get; }
protected internal ConnectionManager Manager { get; set; }
```

There are a number of methods you can then override and implement with your own logic: 

```csharp
public virtual Task OnApplicationDidLaunch(StreamDeckEventPayload args);
public virtual Task OnApplicationDidTerminate(StreamDeckEventPayload args);
public virtual Task OnDeviceDidConnect(StreamDeckEventPayload args);
public virtual Task OnDeviceDidDisconnect(StreamDeckEventPayload args);
public virtual Task OnDidReceiveGlobalSettings(StreamDeckEventPayload args);
public virtual Task OnDidReceiveSettings(StreamDeckEventPayload args);
public virtual Task OnKeyDown(StreamDeckEventPayload args);
public virtual Task OnKeyUp(StreamDeckEventPayload args);
public virtual Task OnPropertyInspectorDidAppear(StreamDeckEventPayload args);
public virtual Task OnPropertyInspectorDidDisappear(StreamDeckEventPayload args);
public virtual Task OnTitleParametersDidChange(StreamDeckEventPayload args);
public virtual Task OnWillAppear(StreamDeckEventPayload args);
public virtual Task OnWillDisappear(StreamDeckEventPayload args);
```

Just call the base method then implement the functionality you wish to achieve.

For example:

```csharp
public override async Task OnDidReceiveSettings(StreamDeckEventPayload args)
{
    await base.OnDidReceiveSettings(args);
    await Manager.SetTitleAsync(args.context, SettingsModel.Counter.ToString());
}
```

```csharp
public override async Task OnWillAppear(StreamDeckEventPayload args)
{
    await base.OnWillAppear(args);
    await Manager.SetTitleAsync(args.context, SettingsModel.Counter.ToString());
}
```

## BaseStreamDeckActionWithSettingsModel

```csharp
public T SettingsModel { get; }
```

```csharp
public override Task OnDidReceiveSettings(StreamDeckEventPayload args);
public override Task OnWillAppear(StreamDeckEventPayload args);
protected void SetModelProperties(StreamDeckEventPayload args);
```