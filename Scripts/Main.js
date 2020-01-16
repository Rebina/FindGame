$(document).ready(function () {
    $('.timer').text("2:00");
    countdown();
    //$('#submit').html('Completed!!!<hr /> Total score: ' + totalLength);
    //$("#submit").modal({
    //    fadeDuration: 1000,
    //    fadeDelay: 0.50,
    //    showClose: false
    //});
    //setTimeout(function () {
    //    $.modal.close();
    //}, 3000);
    //alert(totalLength);
});

var totalLength = 0;
function countdown() {
    var interval;
    clearInterval(interval);
    interval = setInterval(function () {
        var timer = $('.timer').html();
        timer = timer.split(':');
        var minutes = timer[0];
        var seconds = timer[1];
        seconds -= 1;
        if (minutes < 0) return;
        else if (seconds < 0 && minutes !== 0) {
            minutes -= 1;
            seconds = 59;
        }
        else if (seconds < 10 && length.seconds !== 2) seconds = '0' + seconds;

        $('.timer').html(minutes + ':' + seconds);

        if (minutes === '0' && seconds === '00') {
            clearInterval(interval);
            $('#submit').html('Completed!!!<hr /> Total score: ' + totalLength);
            $('#InvalidWord').html('');
            $("#submit").modal({
                fadeDuration: 1000,
                fadeDelay: 0.50,
                showClose: false
            });
            setTimeout(function () {
                $.modal.close();
            }, 4000);
            $('#Records').append('<tr>'
                + '<td style="font-weight:600">' + 'Total' + '</td>'
                + '<td style="font-weight:600">' + totalLength + '</td>'
                + '</tr>'
            );
            $('#RandomCharacter').prop('readonly', true);
        }
    }, 1000);
}

function CheckWords(model) {
    model.RandomCharacter = $('#RandomCharacter').val();
    var duplicate = false;
    $('#Records tr td').each(function () {
        if ($(this).text().toUpperCase().indexOf($('#RandomCharacter').val().toUpperCase()) >= 0) {
            duplicate = true;
            $('#InvalidWord').html('Already found!!!');
        }
    });
    if (duplicate === false) {
        $.ajax({
            url: "/Home/CheckWord/",
            type: 'POST',
            data: { model },
            datatype: 'json',
            cache: false,
            async: true,
            success: function (data) {
                $('#InvalidWord').html('');
                if (data === 'True') {
                    $('#Records').append('<tr>'
                        + '<td>' + $('#RandomCharacter').val().toUpperCase() + '</td>'
                        + '<td>' + $('#RandomCharacter').val().length + '</td>'
                        + '</tr>'
                    );
                    totalLength = totalLength + $('#RandomCharacter').val().length;
                }
                else {
                    $('#InvalidWord').html('Invalid word!!!');
                }
                $('#RandomCharacter').val('');
            },
            error: function (xhr, status, error) {
                var verr = xhr.status + "\r\n" + status + "\r\n" + error;
            }
        });
    }
}


function GetPossibleWords(model) {
    $('#PossibleWords').val('');
    model.RandomCharacter = $('#RandomCharacter').val();
    $.ajax({
        url: "/Home/GetPossibleWords/",
        type: 'POST',
        data: { model },
        datatype: 'json',
        cache: false,
        async: true,
        success: function (data) {
            if (data.possibleWords.length > 0) {
                $.each(data.possibleWords, function (index, value) {
                    $('#PossibleWords').append(', ' + value);
                });
            }
        },
        error: function (xhr, status, error) {
            var verr = xhr.status + "\r\n" + status + "\r\n" + error;
        }
    });
}

