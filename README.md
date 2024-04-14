<img src="Readme/nothing_logo.jpg" alt="nothing_logo" width=200 height=auto/>

## NothingOS for Unity
This SDK provides native APIs for all of the essential Glyph features on Nothing Phone.

## Features
- glyph-based progress bars
- custom glyph animations
- supports all range of distributed devices (Phone 1, Phone 2, Phone 2a)

## What's inside?
- .NET wrapper for Unity
- native [Glyph Developer Kit](https://github.com/Nothing-Developer-Programme/Glyph-Developer-Kit) `.jar` library

## How to use
 - First of all, you need to obtain `API Key` from Nothing Company by filling out the [request form](https://docs.google.com/forms/d/e/1FAIpQLScHZF5_1gZQABugJvJrWGTNuN2lhKlDWWP-B62ie29PtSB1uw/viewform)
- In addition, you have to create `NothingSettings` scriptable object in any folder into `Assets` folder and place this `API key` in it.
- The last step is to write your own code and make Nothing Phone light!

##### Example:
```csharp
using NothingOS;
using NothingOS.Glyphs;

public class Example : MonoBehaviour
{
    void Start()
    {
        Nothing.Initialize();
    }

    void OnApplicationQuit()
    {
        Nothing.Shutdown();
    }

    void Update()
    {
        using (var builder = Nothing.glyphs.Builder())
        {
            builder
            .SetPeriod(TimeSpan.FromSeconds(0.5f))
            .SetInterval(TimeSpan.FromSeconds(0.25f))
            .SetCycles(3)
            .SetChannel(GlyphBuilder.Channel.ChannelA)
            .SetChannelB();

            using (var frame = builder.Build())
            {
                Nothing.glyphs.Animate(frame);
            }
        }
    }
}
```

#### Unity Plugin
##### via Unity Package Manager
The latest version can be installed via [package manager](https://docs.unity3d.com/Manual/upm-ui-giturl.html) using following git URL: \
`https://github.com/am1goo/NothingOS.Unity.git#1.0.1`

## Requirements
- Minimal SDK 19 (Android 4.4, KitKat)
  
## Limitations
This plugin works only on SDK 34 or newer (Android 14, Upside Down Cake), on other devices it just do nothing.

## Tested in
- Unity 2019.4.x
- Unity 2020.3.x

## Contribute
Contribution in any form is very welcome. Bugs, feature requests or feedback can be reported in form of Issues.
