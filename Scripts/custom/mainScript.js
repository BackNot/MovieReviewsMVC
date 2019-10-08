$(document).ready(function () {
    $('.privacy-div').hide();
    $('#thanks-result').hide(); // hide this div that is in index
    // ajax start
    $('.moviesOption').click(function (event) {
        event.preventDefault();
        var url = $(this).attr('data-url');

        var searchString = $('.searchString').val();
        searchString = searchString.replace(/ /g, '%20');
        $('#content').load(url + "?searchString=" + searchString);
        // ajax end
    });
    // ajax search movie in admin panel
    $('.ajaxMovie').click(function (event) {
        event.preventDefault();
        var url = $(this).attr('data-url');

        var searchString = $('.ajaxMovieSearchString').val();
        $('#ajax-render-zone').load(url + "?searchString=" + searchString);
        // ajax end
    });
    $('.ajaxUser').click(function (event) {
        event.preventDefault();
        var url = $(this).attr('data-url');

        var searchString = $('.ajaxUserSearchString').val();
        $('#ajax-render-zone').load(url + "?searchString=" + searchString);
        // ajax end
    });

    // when user click on header img show him index
    $('.head').click(function (event) {
        window.location.href = '/Home/Index'
        console.log('click')
    });

    $('#thanks-button').click(function () {
        $(this).fadeOut(1000);
        $('#thanks-result').fadeIn(2000);
    });
    // Theme dropdown button
    $('.dropbtn').click(function () {
        $("#myDropdown").toggleClass("show");

    });
    $('.btn-change-pw').click(function () {
        window.location.href = "/Auth/Change";
    })
    $('.privacy').click(function () {   
        $('.privacy-div').show();
    })

   
   
});

