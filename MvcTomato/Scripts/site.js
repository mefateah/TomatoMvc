// Write your Javascript code.
(function ($) {
    var datepicker = new Pikaday(
        {
            field: $('#datepicker')[0],
            firstDay: 1,
            onSelect: date => {
                // Momento.js can be used instead
                var month = date.getMonth() + 1
                var day = date.getDate()
                var userFormattedDate = [
                    day < 10 ? '0' + day : day,
                    month < 10 ? '0' + month : month,
                    date.getFullYear()
                ].join('/')
                var serverFormattedDate = [
                    date.getFullYear(),
                    month < 10 ? '0' + month : month,
                    day < 10 ? '0' + day : day
                ].join('-')
                $('#datepicker')[0].value = userFormattedDate
                $('#Date')[0].value = serverFormattedDate
            }
        });
    datepicker.setDate($('#Date').data('date'))

    // bootbox confirm dialog
    $(document).on('click', '#deleteLink', function (e) {
        var id = $(this).data('id'); 
        var link = $(this).data('url');
        
        bootbox.confirm({
            message: "Do you want to delete this record?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result) {
                    window.location.href = link + '/' + id; 
                }
            }
        });
    });

    // Month selector
    // TODO: make it work with AJAX, 'cause on reloading the selector looks like crazy, makes double loading
    // TODO: remove hardcoded path (Home/History)
    $('#monthSelector').change(function () {
        window.location.href = '/Home/History?year=' + $('#yearSelector select').val() + '&month=' + $('#monthSelector select').val(); 
    })

    // Year selector
    $('#yearSelector').change(function () {
        window.location.href = '/Home/History?year=' + $('#yearSelector select').val() + '&month=' + $('#monthSelector select').val(); 
    })
})(jQuery);