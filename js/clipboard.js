window.clipboardPaste = async function () {
    try {
        const text = await navigator.clipboard.readText();
        return text;
    } catch (error) {
        console.error('JS-CB01 Failed to paste clipboard contents: ', error);
        return '';
    }
}

window.clipboardCopy = async function (text) {
    try {
        navigator.clipboard.writeText(text)
            .then(() => {
                console.log('Text copied to clipboard');
            })
            .catch(err => {
                console.error('Failed to copy text: ', err);
            });
        return true;
    } catch (error) {
        console.error('JS-CB02 Failed to copy to clipboard: ', error);
        return false;
    }
}