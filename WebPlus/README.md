
# WebPlus

### Web Apps go Native





## heading



## methods

intro to methods goes here

### exit()

Exit the application.

<hr>

### messageHost(message)

Send the given message to the host.

@param (String) message

<hr>

### setMessageHandler(handler)

Set the callback that will receive responses from the host to the given callback function.

@param (Function) handler

<hr>

### enableHotReload(state)

Enable or disable hot reloading according to the given state.

@param (Boolean) state

<hr>

### getPath()

Get the applications folder.

@returns (String)

<hr>

### getLastError()

Get the last error.

@returns (String)

<hr>

### setWindowTitle(title)

Set the host window title to the given title.

@param (String) title

<hr>

### setWindowIcon(path)

Set the host window icon to the given path pointing to a .PNG or .ICO file.

@param (String) path

<hr>

### setFullScreen(state)

Enter or leave fullscreen mode according to the given state.

@param (Boolean) state

<hr>

### fileInfo(path)

Get the `FileDetails` for the file or directory at the given path.

@param (String) path

@returns (FileDetails)

<hr>

### deleteFile(path)

Delete the file with the given path.

@param (String) path

<hr>

### renameFile(path, name)

Rename the file with the given path to the given name.

@param (String) path

<hr>

### dirInfo(path)

Get an array of 'FileDetails' for all files and directories in the given path.

@param (String) path

@returns ([FileDetails])

<hr>

### createDirectory(path)

Create a directory with the given path.

@param ( String ) path

<hr>

### deleteDirectory(path)

Recursively delete the directory with the given path.

@param ( String ) path

<hr>

### openFileDialog(options = ())d) ? JSON.parse(fd) : null;

Using the given options, display a dialog where a file can be selected, and return its `FileDetails` if it wasn't cancelled.

@param (Object) options

@returns (FileDetails)

<hr>

### openFolderDialog()

Display a dialog where a folder can be selected, and return its `FileDetails` if it wasn't cancelled.

@returns (FileDetails)

<hr>

### loadTextFile(path)

Load the text file with the given path.

@param (String) path

@returns (String)

<hr>

### brosweForAndLoadTextFile(options = ())

Browse for a text file using a `OpenFileDialog`, and if not cancelled, load the selected text file.

@param (Object) options

@returns (String)

<hr>

### saveTextFile(text, path) => WP_HOST.saveTextFile(text, path),

Save the given text to the file at the given path.

@param (String) text

@param (String) path

<hr>

### browseForAndSaveTextFile(text, options = ())

Browse for a text file using a `SaveFileDialog`, and if not cancelled, save the given text to the selected file.

@param (String) text

@param (String) filter
