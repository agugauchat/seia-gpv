$(function () {
    CKEDITOR.replace('postEditor', {
        codeSnippet_theme: 'monokai_sublime',
        extraPlugins: 'uploadimage',
        uploadUrl: '/Post/UploadImage',
        filebrowserImageBrowseUrl: '/Post/UploadImagePartial',
        filebrowserImageUploadUrl: '/Post/UploadImage',
        removeDialogTabs: 'image:advanced;image:Link;info:preview;info:advanced;'
    });

    CKEDITOR.on('dialogDefinition', function (ev) {
        var dialogName = ev.data.name;
        var dialogDefinition = ev.data.definition;
        ev.data.definition.resizable = CKEDITOR.DIALOG_RESIZE_NONE;

        if (dialogName === 'link') {
            var infoTabLink = dialogDefinition.getContents('info');
            infoTabLink.remove('protocol');
            dialogDefinition.removeContents('target');
            dialogDefinition.removeContents('advanced');
        }

        if (dialogName === 'image') {
            dialogDefinition.removeContents('Link');
            dialogDefinition.removeContents('advanced');
            var infoTab = dialogDefinition.getContents('info');
            infoTab.remove('basic');
        }
    });
});

function UpdateValue(value) {
    // this gets called from the popup window and updates the field with a new value 
    var urlObject = $(".cke_dialog_ui_input_text")[1];
    urlObject.value = value;

}