
![alt text](images/logo.svg)

WebPlus adds super powers to web applications and strives to be easy to setup, easy to use, produce small distributions, and require no compilation.

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

&emsp;:zap: Base distribution size of 794KB.

## What's New?

v1.0.0 (7 Nov 2023)

&emsp;:small_blue_diamond: Initial release.

## Why WebPlus?

I really enjoy creating applications using HTML, CSS, and JavaScript.

These *"web apps"* are fantastic but they have no access to the local file system, because of security. This issue can be solved by using currently available frameworks, so I tried a bunch of them, [ElectronJS](https://www.electronjs.org/), [NWjs](https://nwjs.io/), [CEFSharp](https://cefsharp.github.io/), [Tauri](https://tauri.app/), and [Neutralino](https://neutralino.js.org/).

Because some of them created gigantic distributions (90Mb+) and others had unresolvable issues, I just decided to write my own.. WebPlus.

WebPlus was created for my own personal use and is publicly availabe in case anyone finds it interesting or of use. It does *not* strive to compete with any established more advanced frameworks that provide similar functionality. In fact, comparing WebPlus to other mature frameworks is probably a bit like comparing an abacus with an electronic calculator.

WebPlus is just for Windows because I don't have any Apple or Linux stuff, sorry.

# Getting Started

## Requirements

All you really need is a text editor but if you want to build your own binary you will need [Visual Studio](https://visualstudio.microsoft.com/vs/).

## Your first WebPlus app

Lets make an app called *MyCoolApp*.

&emsp;:one: Copy the *"dist"* folder somewhere handy and rename it as *"MyCoolApp"*.

&emsp;:two: Rename the WebPlus.exe file to *"MyCoolApp.exe"*.

&emsp;:three: Run the "MyCoolApp.exe" file and an empty grey window will magically appear.

&emsp;:four: Edit *"app.html"*, *"style.css"*, and *"app.js"* inside the "app" folder. Saving them will show the changes the app window.

&emsp;:five: Rinse and repeat :four: until your app is done.

&emsp;:six: Remove the line in *"app.js"* that reads `wp.enableHotReload(true);` to disable hot reloading.

&emsp;:seven: Delete the ".WebView" folder. This is a temprorary cache folder that isn't required for distribution.

&emsp;:eight: Perform extra tasks such as minifying or obfuscating your code.

&emsp;:nine: Replace the *"icon.ico"* with your own icon, and then you're ready to commence distribution.

:warning: the *"webplus.js"* file in the *"app"* folder contains the WebPlus engine code and you don't need to modify it.

# Under the Hood

For all intents and purposes WebPlus is a just a [WebView2](https://learn.microsoft.com/en-us/microsoft-edge/webview2/) control that fills the entire client area of a [Windows Form](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/?view=netframeworkdesktop-4.8), with a [host object coclass](https://learn.microsoft.com/en-us/microsoft-edge/webview2/how-to/hostobject?tabs=win32) glued on to provide some extra functionality.

Hot reloading uses a [FileSystemWatcher](https://learn.microsoft.com/en-us/dotnet/api/system.io.filesystemwatcher?view=net-7.0)

The default icon provided inside the *"app"* folder contains with sizes of 16x16, 24x24, 32x32, 48x48, and 256x256, which are considered [the bare minimum any icon should contain](https://learn.microsoft.com/en-us/windows/apps/design/style/iconography/app-icon-construction).


*NOTE:* I was delighted at how easy it was to actually set it all up and get it going inside Visual Studio.. finally Microsoft made something that didn't have me cursing loudly and tearing at what little hair I have left :)




<br>



# API Reference

## Variables

WebPlus provides a number of global variables that you might find useful...

### `wp`

The WebPlus object that encapsulates all methods that can be called on the host.

### `WP_HOST`

The cached host (`window.chrome.webview.hostObjects.sync.hostedObject`).

### `WP_PATH`

The applications home directory.

### `WP_MESSAGE_HANDLER`

A callback function that will be called when messages arrive from the host.

### `WP_FULLSCREEN`

True if the application window is currently in full-screen mode.

### `WP_FRAMELESS`

True if the application window has no frame.

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
  type: {String},       // "FILE" or "DIRECTORY".
  size: {Number},       // Size in bytes.
  path: {String},       // Directory where file is stored.
  fullPath: {String},   // Fully qualified file path.
}
```

### DialogOptions

```
{
  filter: {String},
  multiSelect: {Boolean},
  title: {String}
}
```

## Events

You can subscribe to the "windowresize" event in your app to receive notifications when your apps window resizes.

```
window.addEventListener('windowresize', (e) => {
    console.log(`windowresize: ${e.detail}`);
});
```

The events `detail`` property will contain a string describing what type of resize event just occurred, and it will be one of the following:

- "windowEnteredFullScreen"
- "windowLeftFullScreen"
- "windowRestored"
- "windowMinimized"
- "windowMaximized"

## Messages

Code for asynchronous messaging between the app and host is included in the various source code files but has been commented out because making use of this code means editing the C# source and recompiling the binaries. This is not how I intend for WebPlus to work and not something I personally require. Hoswever I have left the code in situ incase anyone else wants that functionality and can be bothered messing about with it.

## Methods

All callable host methods are encapsulated inside the wp object and you call them the same way you would a class, so if you wanted to set the windows title to "WebPlus Rocks" you would use the following code:

```
wp.setWindowTitle("WebPlus Rocks");
```

## `exit()`

Exit the application.



## `messageHost(message)`

Send the given message to the host. The message will be sent as a JSON string.

&emsp; @param (String) message



## `setMessageHandler(handler)`

Set the callback that will receive responses from the host to the given callback function.

&emsp; @param (Function) handler



## `enableHotReload(state)`

Enable or disable hot reloading according to the given state.

&emsp; @param (Boolean) state



## `getPath()`

Get the applications folder.

&emsp; @returns (String)



## `getLastError()`

Get the last error encountered by the host. Useful for determining why some method or another failed.

&emsp; @returns (String)



## `setWindowLocation(x, y)`

Set the host window location to the given coordinates.

&emsp; @param (Number) x

&emsp; @param (Number) y



## `setWindowSize(width, height)`

Set the host window size to the given dimensions.

&emsp; @param (Number) width

&emsp; @param (Number) height



## `setWindowTitle(title)`

Set the host window title to the given title.

&emsp; @param (String) title



## `setWindowIcon(path)`

Set the host window icon to the given path pointing to a .PNG or .ICO file.

&emsp; @param (String) path



## `minimizeToTray(state)`

Set the host window to minify to the system tray instead of the task bar according to the given state.

&emsp; @param (Boolean) state



## `setFullScreen(state)`

Enter or leave fullscreen mode according to the given state.

&emsp; @param (Boolean) state



## `setFrameless(state)`

Remove or add window frame according to the given state.

&emsp; @param (Boolean) state



## `fileInfo(path)`

Get the [`FileDetails`](#filedetails) for the file or directory at the given path.

&emsp; @param (String) path

&emsp; @returns (FileDetails)



## `deleteFile(path)`

Delete the file with the given path.

&emsp; @param (String) path



## `renameFile(path, name)`

Rename the file with the given path to the given name.

&emsp; @param (String) path



## `dirInfo(path)`

Get an array of [`FileDetails`](#filedetails) for all files and directories in the given path.

&emsp; @param (String) path

&emsp; @returns ([FileDetails])



## `createDirectory(path)`

Create a directory with the given path.

&emsp; @param ( String ) path



## `deleteDirectory(path)`

Recursively delete the directory with the given path.

&emsp; @param ( String ) path




## `renameDirectory(path, name)`

Rename the directory with the given path to the given name.

&emsp; @param (String) path



## `openFileDialog(options = {})`

Using the given options, display a dialog where a file can be selected, and return its `FileDetails` if it wasn't cancelled.

&emsp; @param (DialogOptions) [`options`](#dialogoptions)

&emsp; @returns (FileDetails)



## `openFolderDialog()`

Display a dialog where a folder can be selected, and return its `FileDetails` if it wasn't cancelled.

&emsp; @returns (FileDetails)



## `loadTextFile(path)`

Load the text file with the given path.

&emsp; @param (String) path

&emsp; @returns (String)



## `brosweForAndLoadTextFile(options = {})`

Browse for a text file using a `OpenFileDialog`, and if not cancelled, load the selected text file.

&emsp; @param (DialogOptions) [`options`](#dialogoptions)

&emsp; @returns (String)



## `saveTextFile(text, path) => WP_HOST.saveTextFile(text, path),`

Save the given text to the file at the given path.

&emsp; @param (String) text

&emsp; @param (String) path



## `browseForAndSaveTextFile(text, options = {})`

Browse for a text file using a `SaveFileDialog`, and if not cancelled, save the given text to the selected file.

&emsp; @param (String) text

&emsp; @param (DialogOptions) [`options`](#dialogoptions)



## `savePNG(canvas, path)`

Save the given canvas as a PNG image at the given path.

&emsp; @param (HTMLCanvasElement) canvas
&emsp; @param (String) path
