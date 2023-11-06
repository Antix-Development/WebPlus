
# WebPlus

### Web Apps go Native





## exit

```
exit()
```

exits the app immediately.







Web Apps are fantastic, but for security reasons (yes thats a thing) they have no access to the local file system. This issue can be overcome by using one of the numerous available frameworks that transform web applications into native applications and allow them access to said file system.

I've previously used one sucj framework (Electron) and whilst I was always pleased with the resulting applications I created using it, I really didn't like the large distribution size. This lead me to evaluate other frameworks that produce a smaller overall application footprint whist still providing access to the local file system.

I searched about the internet and found a bunch of *web to native* frameworks and did some basic tests to see what kind of distribution sizes they produced for a basic "Hello world" type application:

| Framework | Installing | Build Process | Distro Size |
| - | - | - | - | - |
| Electron | Easy | Easy | 247Mb+ |
| Tauri | Easy | Easy | 6Mb+ |
| NW.js | Stupid | frustrating | 90Mb+ |
| CefSharp | Easy | Easy | 791Mb+ |
| Neutralino | Easy | Easy | 2.8Mb+ |

**Electron:**

- Electron packages it's entire version of Chrome with every distribution and it didn't pull any punches, delivering a large distribution.

**Tauri:**

- Tauri doesn't package an entire browser, instead it uses a common runtime. Whilst I was able to compile a distribution of just on 6MB, I faced other issues that I could not resolve.

**NW.js:**

- NW.js packages an entire browser like Electron. I was unable to get it to work at all. I instead downloaded a small application created with NW.js and guestimated the distribution size to be around 90MB.

**CefSharp:**

- I don't know what the heck this thing does but my distribution was around 790MB when I compiled it. That's just ridiculous, and it even uses a common runtime! What The Total Heck!

**Neutralino:**

- Neutralino uses a common runtime so I expected a small distribution. I tried this framework a long time ago and it was pretty basic and buggy. It has really matured over time and I was happy with how easy it was to use and that the distribution size ended up being less than 3MB.

If I had to choose one of the frameworks that I tested (mostly) then I would choose Neutralino. Even though it isn't as flashy as Tauri, or mature as others, it was so easy to get it working and I was actually coding my application quickly and not having to install all manner of crap and troubleshoot issues that really shouldn't even exist in the year 2023. It also produces a very small distribution so I'm pretty much sold on Neutralino.

Unfortunately by the time I compiled my results I was already elbows deep in creating my own framework, and in it's partially functional state it compiled to a stupidly small distribution size of 0.98Mb.

Whilst this is quite amazing, it's not a true indication of the applications footprint because it requires some DotNET frameworks and WebView2 runtimes to be installed on the users system as well. These requirements aren't that bad in hindsight because most users will already have DotNET frameworks installed, and WebView2 (since Windows11) ships with the Windows OS.

