
$(document).ready(function () {
    $('#search-input').keyup(function () {
        $('.pagination .active').removeClass('active');
        //   $('.pagination .active').find('#1').parent().addClass('active');
        filterSearch()
    });

});
$(document).on('click', '.pagination li', function (e) {
    e.preventDefault();
    $('.pagination li').each(function () {
        $(this).removeClass('active');
    })
    $(this).addClass('active');
    console.log($(this).children().attr("id"))
    filterSearch();
});
function filterSearch() {
    var keyword = $('#search-input').val();
    var pageIndex = $('.pagination .active a').attr('id');
    $.ajax({
        url: "StoryListing/storylisting",
        type: "POST",
        data: {          
            SearchInputdata: keyword,
            pageindex: pageIndex
        },
        success: function (response) {

            $('#storyresult').html($(response).find('#storyresult').html());
           /* if (list) {
                $('.btn-list').click();
            }*/
            $('.page').html($(response).find('.page').html());

        }
    })
    }