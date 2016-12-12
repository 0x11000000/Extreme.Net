# Extreme.Net

[![NuGet version](https://badge.fury.io/nu/Extreme.Net.svg)](https://badge.fury.io/nu/Extreme.Net)
[![Build status](https://ci.appveyor.com/api/projects/status/7mwsovabbtwq6i65?svg=true)](https://ci.appveyor.com/project/extremecodetv/extreme-net)

**Extreme.Net** - http class library for C# which includes:
 * Classes for work with proxy servers: HTTP, Socks4(a), Socks5, Chain.
 * Classes for work with HTTP 1.0/1.1 protocol: keep-alive, gzip, deflate, chunked, SSL, proxies and more.

**Extreme.Net** it's a fork of [xNet](https://github.com/X-rus/xNet)


# Installation
 
Install via NuGet
 
```
PM > Install-Package Extreme.Net
```
 
# Examples
 
```csharp
HttpRequest request = new HttpRequest();
HttpResponse response;

response = await request.GetAsync("http://site.com");

``` 

### Another one
```csharp 
using (var request = new HttpRequest("http://site.com/"))
{
    request.UserAgent = Http.ChromeUserAgent();
    request.Proxy = Socks5ProxyClient.Parse("127.0.0.1:1080");

    request
        // Parameters URL-address.
        .AddUrlParam("data1", "value1")
        .AddUrlParam("data2", "value2")

        // Parameters 'x-www-form-urlencoded'.
        .AddParam("data1", "value1")
        .AddParam("data2", "value2")
        .AddParam("data2", "value2")

        // Multipart data.
        .AddField("data1", "value1")
        .AddFile("game_code", @"C:\orion.zip")

        // HTTP-header.
        .AddHeader("X-Apocalypse", "21.12.12");
        
    // These parameters are sent in this request.
    request.Post("/").None();

    // But in this request they will be gone.
    request.Post("/").None();
}
```

#Developer

Artem Dontsov

![VK](https://vk.com/images/faviconnew.ico?3) [VK](https://vk.com/extremecodetv)

![YouTube](https://s.ytimg.com/yts/img/favicon-vflz7uhzw.ico) [YouTube](https://www.youtube.com/channel/UCBNlINWfd08qgDkUTaUY4_w)
