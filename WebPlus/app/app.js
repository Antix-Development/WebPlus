/**
 * <ApplicationName>
 * <TagLine>
 * @copyright <Author>
 * @license <License>
 * @namespace <Namespace>
*/

'use_strict';

let debugOutput;

window.onload = () => {

    //wp.enableHotReload(true);

    debugOutput = document.getElementById('debug_output');


    document.getElementById('wp_path_label').value = WP_PATH;




//    wp.renameFile("d:\\haha.txt", "poopoo.txt");
//    wp.deleteDirectory("d:\\deleteme");

    console.log(wp.fileInfo("d:\\fucktard.txt"));
    console.log(wp.getLastError());

    wp.setWindowIcon("d:\\icon.png")



    window.addEventListener('windowresize', (e) => {
        console.log(`windowresize: ${e.detail}`);
    });

    const canvas = document.createElement('canvas');
    canvas.width = 512;
    canvas.height = 256;
    const ctx = canvas.getContext('2d');
    ctx.fillStyle = '#f80';
    ctx.fillRect(0, 0, 512, 256);

    //wp.savePNG(canvas, "d:\\fucktard.png");
}

function toggleHotReload() {
    wp.enableHotReload(!WP_HOTRELOAD_ENABLED);
    document.getElementById('toggle_hot_reload_button').innerHTML = (WP_HOTRELOAD_ENABLED) ? 'disable hot reload' : 'enable hot reload';

}

function toggleFullScreen() {
    wp.setFullScreen(!WP_FULLSCREEN);
    document.getElementById('toggle_fullscreen_button').innerHTML = (WP_FULLSCREEN) ? 'leave fullscreen' : 'enter fullscreen';
}

function toggleFrameless() {
    wp.setFrameless(!WP_FRAMELESS);
    document.getElementById('toggle_frameless_button').innerHTML = (WP_FRAMELESS) ? 'Go framed' : 'go frameless';
}

function toggleMinimizeToTray() {
    wp.minimizeToTray(!WP_MINIMIZE_TO_TRAY);
    document.getElementById('toggle_minimize_to_tray_button').innerHTML = (WP_MINIMIZE_TO_TRAY) ? 'minimize to bar' : 'minimize to tray';
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