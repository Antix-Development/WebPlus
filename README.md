
![alt text](images/logo.svg)

WebPlus is a lightweight portable desktop application development framework that lets you turn your web applications into native desktop applications for the Microsoft Windows Operating System.

<div style="text-align: center">

# [xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx](#xxxxxxxxxxxxxxxxxxxxxx)

# [Getting Started](#getting-stared)

# [API Reference](#api-reference)


[dfghdfghf](# introduction)


</div>

# Introduction

## What is WebPlus

Transform web applications into Windows native desktop applications.


**^^wordy or not wordy??**

## Why WebPlus?

Well why not.




# Getting Started

## Requirements

All you really need is a text editor but if you want to build your own binary you will need [Visual Studio](https://visualstudio.microsoft.com/vs/).






<hr>

<br>

<br>

<br>

<br>

<br>

<br>

# API Reference

## Variables

WebPlus contains a number of global variables that you can access:

### wp

The WebPlus object that encapsulates all methods that can be called on the host.

### WP_HOST

The cached host (`window.chrome.webview.hostObjects.sync.hostedObject`).

### WP_PATH

The applications home directory.

### WP_MESSAGE_HANDLER

A callback function that will be called when messages arrive from the host.

### WP_FULLSCREEN

True if the application window is currently in full-screen mode.

### WP_FRAMELESS

True if the application window has no frame.

### WP_HOTRELOAD_ENABLED

True if the application is currently in hot-reload mode.

### WP_TEXTFILE_FILTER

A pre generated filetype filter for TEXT files.

### WP_JSONFILE_FILTER

A pre generated filetype filter for JSON files.

### WP_PNGFILE_FILTER

A pre generated filetype filter for PNG files.




## Structures

Some WebPlus methods return custom structures:

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




## Methods

To call a WebPlus method use `wp.` followed by `methodname`.

For example if you wanted to set the windows title to "WebPlus Rocks" you would use the following code:

```
wp.setWindowTitle("WebPlus Rocks");
```

If 
WebPlus encapsulates the following methods:

#### exit()

&emsp; Exit the application.

<hr>

#### messageHost(message)

&emsp; Send the given message to the host. The message will be sent as a JSON string.

&emsp; @param (String) message

<hr>

#### setMessageHandler(handler)

&emsp; Set the callback that will receive responses from the host to the given callback function.

&emsp; @param (Function) handler

<hr>

#### enableHotReload(state)

&emsp; Enable or disable hot reloading according to the given state.

&emsp; @param (Boolean) state

<hr>

#### getPath()

&emsp; Get the applications folder.

&emsp; @returns (String)

<hr>

#### getLastError()

&emsp; Get the last error. Useful for determining why some method or another failed.

&emsp; @returns (String)

<hr>

#### setWindowTitle(title)

&emsp; Set the host window title to the given title.

&emsp; @param (String) title

<hr>

#### setWindowIcon(path)

&emsp; Set the host window icon to the given path pointing to a .PNG or .ICO file.

&emsp; @param (String) path

<hr>

#### setFullScreen(state)

&emsp; Enter or leave fullscreen mode according to the given state.

&emsp; @param (Boolean) state

<hr>

#### fileInfo(path)

&emsp; Get the `FileDetails` for the file or directory at the given path.

&emsp; @param (String) path

&emsp; @returns (FileDetails)

<hr>

#### deleteFile(path)

&emsp; Delete the file with the given path.

&emsp; @param (String) path

<hr>

#### renameFile(path, name)

&emsp; Rename the file with the given path to the given name.

&emsp; @param (String) path

<hr>

#### dirInfo(path)

&emsp; Get an array of 'FileDetails' for all files and directories in the given path.

&emsp; @param (String) path

&emsp; @returns ([FileDetails])

<hr>

#### createDirectory(path)

&emsp; Create a directory with the given path.

&emsp; @param ( String ) path

<hr>

#### deleteDirectory(path)

&emsp; Recursively delete the directory with the given path.

&emsp; @param ( String ) path

<hr>

#### openFileDialog(options = ())d) ? JSON.parse(fd) : null;

&emsp; Using the given options, display a dialog where a file can be selected, and return its `FileDetails` if it wasn't cancelled.

&emsp; @param (Object) options

&emsp; @returns (FileDetails)

<hr>

#### openFolderDialog()

&emsp; Display a dialog where a folder can be selected, and return its `FileDetails` if it wasn't cancelled.

&emsp; @returns (FileDetails)

<hr>

#### loadTextFile(path)

&emsp; Load the text file with the given path.

&emsp; @param (String) path

&emsp; @returns (String)

<hr>

#### brosweForAndLoadTextFile(options = ())

&emsp; Browse for a text file using a `OpenFileDialog`, and if not cancelled, load the selected text file.

&emsp; @param (Object) options

&emsp; @returns (String)

<hr>

#### saveTextFile(text, path) => WP_HOST.saveTextFile(text, path),

&emsp; Save the given text to the file at the given path.

&emsp; @param (String) text

&emsp; @param (String) path

<hr>

#### browseForAndSaveTextFile(text, options = ())

&emsp; Browse for a text file using a `SaveFileDialog`, and if not cancelled, save the given text to the selected file.

&emsp; @param (String) text

&emsp; @param (String) filter
