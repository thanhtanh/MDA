
    $(document).ready(function () {

        CKEDITOR.config.forcePasteAsPlainText = true;
        CKEDITOR.config.toolbar_SimpleCode = [
            ['Cut', 'Copy', 'Paste', 'PasteText'],
            ['Undo', 'Redo'],
            ['Bold', 'Italic', 'Underline']
        ];
        $('.getimagefromelfinder').on('click', function () {
            //get id
            var id = $(this).parent().find('input')[0].id;
            // set id to controller
            window.open('/Admin/File/GetImageFromElfinder?elementId='+ id+'', 'GetImageFromElfinder', 'height=' + (window.screen.height - 100));
        });
    });
