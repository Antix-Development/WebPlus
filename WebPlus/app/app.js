/**
 * <ApplicationName>
 * <TagLine>
 * @copyright <Author>
 * @license <License>
 * @namespace <Namespace>
*/

'use_strict';

let debugOutput;
let hotReloadLabel;
let fullScreenLabel;
let framelessLabel;

window.onload = () => {

    wp.enableHotReload(true);

    debugOutput = document.getElementById('debug_output');
    hotReloadLabel = document.getElementById('wp_hotreload_enabled_label');
    fullScreenLabel = document.getElementById('wp_fullscreen_label');
    framelessLabel = document.getElementById('wp_frameless_label');

    document.getElementById('wp_path_label').value = WP_PATH;

    hotReloadLabel.value = (WP_HOTRELOAD_ENABLED) ? 'Enabled' : 'Disabled';

    fullScreenLabel.value = (WP_FULLSCREEN) ? 'full screen' : 'windowed';

    framelessLabel.value = (WP_FRAMELESS) ? 'frameless' : 'framed';

    wp.setMessageHandler((response) => {
        console.log(response);
    });

//    wp.renameFile("d:\\haha.txt", "poopoo.txt");
//    wp.deleteDirectory("d:\\deleteme");

    console.log(wp.fileInfo("d:\\fucktard.txt"));
    console.log(wp.getLastError());

    //wp.setWindowIcon("d:\\icon.png")

}

function toggleHotReload() {
    wp.enableHotReload(!WP_HOTRELOAD_ENABLED);
    hotReloadLabel.value = (WP_HOTRELOAD_ENABLED) ? 'Enabled' : 'Disabled';
}

function toggleFullScreen() {
    wp.setFullScreen(!WP_FULLSCREEN);
    fullScreenLabel.value = (WP_FULLSCREEN) ? 'full screen' : 'windowed';
}

function toggleFrameless() {
    wp.setFrameless(!WP_FRAMELESS);
    framelessLabel.value = (WP_FRAMELESS) ? 'frameless' : 'framed';
}

function testOpenFileDialog() {
    const fd = wp.openFileDialog();
    if (fd) debugOutput.value += `\n{\n  name:${fd.name}\n  type:${fd.type}\n  extension:${fd.extension}\n  size:${fd.size.toLocaleString() }\n  path:${fd.path}\n  fullPath:${fd.fullPath}\n}`;
}

function testOpenFolderDialog() {
    const fd = wp.openFolderDialog();
    if (fd) debugOutput.value += `\n{\n  name:${fd.name}\n  type:${fd.type}\n  extension:${fd.extension}\n  size:${fd.size.toLocaleString() }\n  path:${fd.path}\n  fullPath:${fd.fullPath}\n}`;
}

function testOpenFileDialogMultiple() {
    const arr = wp.openFileDialog({ multiSelect: true });
    if (arr) arr.forEach((fd) => debugOutput.value += `\n{\n  name:${fd.name}\n  type:${fd.type}\n  extension:${fd.extension}\n  size:${fd.size.toLocaleString()}\n  path:${fd.path}\n  fullPath:${fd.fullPath}\n}`);
}

function testSetWindowTitle() {
    wp.setWindowTitle(document.getElementById('window_name').value);
}

function testBrowseForAndLoadTextFile() {
    const text = wp.brosweForAndLoadTextFile();
    debugOutput.value = `${text}\n`;
}

function testBrowseForAndSaveTextFile() {
    const text = wp.browseForAndSaveTextFile(debugOutput.value);
}

function testDirInfo() {
    const di = wp.dirInfo("C:");
    console.log(di);
}

function testHostedObject() {
    const txt = wp.loadTextFile('pooh');
    console.log(txt);
}


const options = {
    location: {
        x: 0,
        y: 0
    },
    size: {
        width: 800,
        height: 600
    },

    fullScreen: false,
    frameless: false,
    hotReload: true,

}