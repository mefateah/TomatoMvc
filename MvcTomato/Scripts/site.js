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
                    window.location.href = $('#deleteLink').data('url') + '/' + $('#deleteLink').data('id'); 
                }
            }
        });
    });
})(jQuery);