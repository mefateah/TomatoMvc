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
})(jQuery);