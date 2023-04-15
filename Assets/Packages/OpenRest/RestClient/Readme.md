<!-- A full readme to cover all details about this repo -->

# OpenRest - REST Client

A simple REST client for Unity3D with fluent interface.
Unlike other clients, this client works both in the editor and in the build.
and it requires EditorCoroutine package to work in the editor.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Examples](#examples)
- [Contributing](#contributing)
- [License](#license)

## Installation

The easiest way to install OpenRest is via the Unity Package Manager.

- Install [Newtonsoft.Json](https://github.com/jilleJr/Newtonsoft.Json-for-Unity/wiki/Install-official-via-UPM) package from the Package Manager window -> Add package by name:

```
com.unity.nuget.newtonsoft-json
```

- Install [EditorCoroutine](https://docs.unity3d.com/Packages/com.unity.editorcoroutines@1.0/manual/index.html) package from the Package Manager window -> Add package from git URL:

```
https://github.com/omid3098/OpenRest.git
```

## Usage

### Basic Usage

```csharp

// Add required namespaces
using OpenRest;

// Create a rest client
var client =  RestClient.Get()
            .Url("https://jsonplaceholder.typicode.com/posts/1")
            .OnSuccess((response) => {
                Debug.Log(response);
            }).Send();
```

### Complete Usage

```csharp
        JObject jsonObject = new JObject(){
            {"title", "foo"},
            {"body", "bar"},
            {"userId", 1}
        };

        RestClient.Post()
            .Url("https://jsonplaceholder.typicode.com/posts/1")
            .Headers(new Dictionary<string, string> {
                { "Content-Type", "application/json" },
                { "Authorization", "Bearer " + API_KEY}
            }).JsonData(
                jsonObject.ToString(Formatting.None)
            ).OnSuccess((response) =>
            {
                Debug.Log(response);
            }).OnProgress((progress) =>
            {
                Debug.Log(progress);
            }).OnError((error) =>
            {
                Debug.Log(error);
            }).Send();
```

## License

[MIT](LICENSE)

```

```
