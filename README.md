﻿
![alt text](images/logo.svg)

An easy to use framework that adds super powers to your web apps, transforming them into lithe desktop applications, with no compilation required.

### Features

&emsp;:zap: Hot reloading for instant visual feedback during development.

&emsp;:zap: Setting window icon and title.

&emsp;:zap: Window resizing and repositioning.

&emsp;:zap: Window minification to system tray.

&emsp;:zap: Fullscreen and frameless window toggling.

&emsp;:zap: True window resize event pumping.

&emsp;:zap: Dialogs to select files and folders.

&emsp;:zap: Loading and saving text files.

&emsp;:zap: Saving PNG images.

&emsp;:zap: Renaming, moving, and deleting files.

&emsp;:zap: Creating, renaming, moving, and deleting directories.

&emsp;:zap: Persistent state.

&emsp;:zap: Base distribution size of 790KB.

## What's New?

v1.0.0 (8 Nov 2023)

&emsp;:bulb: Initial release.

## Why WebPlus?

I really enjoy creating web applications using HTML, CSS, and JavaScript.

Sadly *"web apps"* have no access to the local file system, because of security. This issue can be solved by using currently available frameworks, so I tried a bunch of them, [ElectronJS](https://www.electronjs.org/), [NWjs](https://nwjs.io/), [CEFSharp](https://cefsharp.github.io/), [Tauri](https://tauri.app/), and [Neutralino](https://neutralino.js.org/).

Some of them created gigantic distributions (90Mb+) and others had unresolvable issues, so I just decided to write my own.. WebPlus.

WebPlus was created for my own personal use and is publicly availabe in case anyone finds it interesting or of use. It does *not* strive to compete with other frameworks that provide similar functionality, and comparing WebPlus to other frameworks is like comparing an abacus with an electronic calculator.

WebPlus is also only works with Windows because I don't have any Apple or Linux stuff, sorry.

If you create something using WebPlus, please let me know, I'd love to see what you do with it. Maybe you would also consider [buying me a coffee](https://www.buymeacoffee.com/antixdevelu) :coffee:

Oh, and feel free to open an issue if you need any assistance or just have a question.

# Getting Started

## Requirements

All you really need is a text/code editor such as Notepad, [Visual Studio Code](https://code.visualstudio.com/), [NotePad++](https://notepad-plus-plus.org/), or [Sublime Text](https://www.sublimetext.com/). If you want to build your own binary though, you will need [Visual Studio](https://visualstudio.microsoft.com/vs/).

## Your first WebPlus app

Lets make an app called *MyCoolApp*.

&emsp;:one: Copy the ***"dist"*** folder somewhere handy and rename it as ***"MyCoolApp"***.

&emsp;:two: Rename the WebPlus.exe file to ***"MyCoolApp.exe"***.

&emsp;:three: Run the ***"MyCoolApp.exe"*** file and an empty grey window will magically appear.

&emsp;:four: Edit ***"app.html"***, ***"style.css"***, and ***"app.js"*** inside the ***"app"*** folder. Saving them will update the app view.

&emsp;:five: Rinse and repeat :four: until your app is done.

&emsp;:six: Set `HotReload` to `false` in ***"app/app.json"*** to disable hot reloading.

&emsp;:seven: Delete the ***"app/MyCoolApp.exe.WebView"*** folder (a temprorary folder not required for distribution).

&emsp;:eight: Perform extra tasks such as minifying or obfuscating your code, etc.

&emsp;:nine: Replace ***"app/app.ico"*** with your own icon, and then you're ready to commence distribution.

That's it, you're done! Time for a beverage :coffee: :tea: :sake: :baby_bottle: :beer: :beers: :cocktail: :tropical_drink: :wine_glass:

:warning: the *"webplus.js"* file in the *"app"* folder contains the WebPlus engine code and you don't need to modify it.

# Under the Hood

When a WebPlus application launches, it..

1. Creates a [Windows Form](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/?view=netframeworkdesktop-4.8) containing a [WebView2](https://learn.microsoft.com/en-us/microsoft-edge/webview2/) control.
2. Loads persistent options from ***"app/app.json"***.
3. Modifies the windows form according to the options.
4. Loads ***"app/app.html"*** into the WebView2 control.

When ***"app/app.html"*** has fully loaded the `window.onload` event in *"app/app.js"* is fired, which starts the app running.

So, for all intents and purposes WebPlus is a just a Windows Form encapsulating a WebView2 control that fills its client area, with a [host object coclass](https://learn.microsoft.com/en-us/microsoft-edge/webview2/how-to/hostobject?tabs=win32) glued on to provide some extra functionality, and hot reloading powered by a [FileSystemWatcher](https://learn.microsoft.com/en-us/dotnet/api/system.io.filesystemwatcher?vview=netframework-4.8.1).

The default icon provided inside the ***"app"*** folder contains images with sizes of 16x16, 24x24, 32x32, 48x48, and 256x256, which are considered [the bare minimum any icon should contain](https://learn.microsoft.com/en-us/windows/apps/design/style/iconography/app-icon-construction).

:love_letter: I was delighted at how easy it was to actually get a WebView2 set up and working inside Visual Studio.. finally Microsoft made something that didn't have me cursing loudly and tearing at what little hair I have left :thumbsup:

# API Reference

## Options

WebPlus stores its persistent state in a the *"app/app.json"* file which has the following structure..

```
{
  HotReload: {Boolean},
  SaveOnExit {Boolean},       // Set to disable persistent state.
  Title: {String}             // Window title.
  X: {Number},                // Window position
  Y: {Number},
  Width: {Number},            // Window dimensions.
  Height: {Number},
  FullScreen: {Boolean},      // The remaining properties are not yet implemented.
  Frameless: {Boolean},
  Minimized: {Boolean},
  MinimizeToTray: {Boolean},
}
```

:point_right: Remember to set `HotReload` to `false` before distributing your app.

## Variables

WebPlus provides a number of global variables that you might find useful...

### `wp`

The WebPlus object that encapsulates all methods that can be called on the host.

### `WP_HOST`

The cached host (`window.chrome.webview.hostObjects.sync.hostedObject`).

### `WP_PATH`

The applications home directory.

### `WP_FULLSCREEN`

True if the application window is currently in full-screen mode.

### `WP_FRAMELESS`

True if the application window has no frame.

### `WP_MINIMIZE_TO_TRAY`

True when app window will minimize to the system tray.

### `WP_HOTRELOAD_ENABLED`

True if the application is currently in hot-reload mode.

### `WP_TEXTFILE_FILTER`

A pre generated filetype filter for TEXT files.

### `WP_JSONFILE_FILTER`

A pre generated filetype filter for JSON files.

### `WP_PNGFILE_FILTER`

A pre generated filetype filter for PNG files.

## Objects

Some WebPlus methods return objects, and others may require you to supply an object. These objects are...

### FileDetails

```
{
  name: {String},       // Name of file, including extension.
  extension: {String},  // Forced to lowercase.
  type: {String},       // "FILE", "DIRECTORY", or "UNKNOWN".
  size: {Number},       // Size in bytes.
  path: {String},       // Directory where file is stored.
  fullPath: {String},   // Fully qualified file path.
}
```

:warning: A `type` of `"UNKNOWN"` will be present when `saveFileDialog` was called and the user entered the name of a file that doesn't exist *"yet"*.

### DialogOptions

```
{
  filter: {String},       // A File Dialog Filter.
  multiSelect: {Boolean}, // Set to true to enable multiple file selection.
  title: {String}         // Dialog title.
}
```

:point_right: WebPlus includes some handy [file dialog filters](https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.filedialog.filter?view=netframework-4.8.1) for common filetypes.. `WP_TEXTFILE_FILTER` for for TEXT files, `WP_JSONFILE_FILTER` for JSON files, and `WP_PNGFILE_FILTER` for PNG files.

## Events

You can subscribe to the "windowresize" event in your app to receive notifications when your apps window resizes.

```
window.addEventListener('windowresize', (e) => {
    console.log(`windowresize: ${e.detail}`);
});
```

The events `detail`` property will contain a string describing what type of resize event just occurred, and it will be one of the following:

&emsp;:small_orange_diamond: "windowEnteredFullScreen"

&emsp;:small_orange_diamond: "windowLeftFullScreen"

&emsp;:small_orange_diamond: "windowRestored"

&emsp;:small_orange_diamond: "windowMinimized"

&emsp;:small_orange_diamond: "windowMaximized"

## Messages

:hammer: Code for asynchronous messaging between the app and host is included in the various source code files (and this readme) but has been commented out because making use of this code means editing the C# source and recompiling the binaries. This is not how I intend for WebPlus to work and not something I personally require. Hoswever I have left the code in situ incase anyone else wants that functionality and can be bothered messing about with it.

## Methods

All callable host methods are encapsulated inside the wp object and you call them the same way you would a class, so if you wanted to set the windows title to *"WebPlus Rocks"* you would use the following code:

```
wp.setWindowTitle("WebPlus Rocks");
```

## `exit()`

Exit the application.


<!-- 
## `messageHost(message)`

Send the given message to the host. The message will be sent as a JSON string.

:small_blue_diamond: parameter {String} message



## `setMessageHandler(handler)`

Set the callback that will receive responses from the host to the given callback function.

:small_blue_diamond: parameter (Function) handler -->



## `enableHotReload(state)`

Enable or disable hot reloading according to the given state.

:small_blue_diamond: parameter {Boolean} state



## `getPath()`

Get the applications folder.

:small_orange_diamond: returns {String}



## `getLastError()`

Get the last error encountered by the host. Useful for determining why some method or another failed.

:small_orange_diamond: returns {String}



## `setWindowLocation(x, y)`

Set the host window location to the given coordinates.

:small_blue_diamond: parameter {Number} x

:small_blue_diamond: parameter {Number} y



## `setWindowSize(width, height)`

Set the host window size to the given dimensions.

:small_blue_diamond: parameter {Number} width

:small_blue_diamond: parameter {Number} height



## `setWindowTitle(title)`

Set the host window title to the given title.

:small_blue_diamond: parameter {String} title



## `setWindowIcon(path)`

Set the host window icon to the given path pointing to a .PNG or .ICO file.

:small_blue_diamond: parameter {String} path



## `minimizeToTray(state)`

Set the host window to minify to the system tray instead of the task bar according to the given state.

:small_blue_diamond: parameter {Boolean} state



## `setFullScreen(state)`

Enter or leave fullscreen mode according to the given state.

:small_blue_diamond: parameter {Boolean} state



## `setFrameless(state)`

Remove or add window frame according to the given state.

:small_blue_diamond: parameter {Boolean} state



## `fileInfo(path)`

Get the [`FileDetails`](#filedetails) for the file or directory at the given path.

:small_blue_diamond: parameter {String} path

:small_orange_diamond: returns {FileDetails}



## `deleteFile(path)`

Delete the file with the given path.

:small_blue_diamond: parameter {String} path



## `renameFile(path, name)`

Rename the file with the given path to the given name.

:small_blue_diamond: parameter {String} path



## `dirInfo(path)`

Get an array of [`FileDetails`](#filedetails) for all files and directories in the given path.

:small_blue_diamond: parameter {String} path

:small_orange_diamond: returns ([FileDetails])



## `createDirectory(path)`

Create a directory with the given path.

:small_blue_diamond: parameter {String} path



## `deleteDirectory(path)`

Recursively delete the directory with the given path.

:small_blue_diamond: parameter {String} path



## `renameDirectory(path, name)`

Rename the directory with the given path to the given name.

:small_blue_diamond: parameter {String} path



## `openFileDialog(options = {})`

Using the given options, display an open file dialog where a file can be selected, and return its `FileDetails` if it wasn't cancelled.

:small_blue_diamond: parameter {DialogOptions} [`options`](#dialogoptions)

:small_orange_diamond: returns {FileDetails}



## `saveFileDialog(options = {})`

Using the given options, display a save file dialog where a file can be selected, and return its `FileDetails` if it wasn't cancelled.

:small_blue_diamond: parameter {DialogOptions} [`options`](#dialogoptions)

:small_orange_diamond: returns {FileDetails}



## `openFolderDialog()`

Display a dialog where a folder can be selected, and return its `FileDetails` if it wasn't cancelled.

:small_orange_diamond: returns {FileDetails}



## `loadTextFile(path)`

Load the text file with the given path.

:small_blue_diamond: parameter {String} path

:small_orange_diamond: returns {String}



## `brosweForAndLoadTextFile(options = {})`

Browse for a text file using a `OpenFileDialog`, and if not cancelled, load the selected text file.

:small_blue_diamond: parameter {DialogOptions} [`options`](#dialogoptions)

:small_orange_diamond: returns {String}



## `saveTextFile(text, path) => WP_HOST.saveTextFile(text, path),`

Save the given text to the file at the given path.

:small_blue_diamond: parameter {String} text

:small_blue_diamond: parameter {String} path



## `browseForAndSaveTextFile(text, options = {})`

Browse for a text file using a `SaveFileDialog`, and if not cancelled, save the given text to the selected file.

:small_blue_diamond: parameter {String} text

:small_blue_diamond: parameter {DialogOptions} [`options`](#dialogoptions)



## `savePNG(canvas, path)`

Save the given canvas as a PNG image at the given path.

:small_blue_diamond: parameter (HTMLCanvasElement) canvas

:small_blue_diamond: parameter {String} path
