$(function () {
    CKEDITOR.replace('categoryEditor', {
        removeDialogTabs: 'image:advanced;image:Link;info:preview;info:advanced;',
        removePlugins: 'uploadimage',
        toolbarGroups: [
            { name: 'clipboard', groups: ['clipboard', 'undo'] },
		    { name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
		    { name: 'links', groups: ['links'] },
		    { name: 'insert', groups: ['insert'] },
		    { name: 'forms', groups: ['forms'] },
		    { name: 'tools', groups: ['tools'] },
		    { name: 'document', groups: ['mode', 'document', 'doctools'] },
		    '/',
		    { name: 'others', groups: ['others'] },
		    { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
		    { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
		    { name: 'styles', groups: ['styles'] },
		    { name: 'colors', groups: ['colors'] },
		    { name: 'about', groups: ['about'] }
        ],
        removeButtons: 'CodeSnippet,Subscript,Superscript,PasteFromWord,Scayt,Anchor,SpecialChar,About'
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