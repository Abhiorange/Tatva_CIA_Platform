$('.nav-link').each(function () {
    $(this).parent().removeClass('bg-light');
    $(this).css('color', 'white');
});
$('.nav-link.story').parent().addClass('bg-light');
$('.nav-link.story ').css('color', 'orange');



$(document).on('click', '.story li', function (e) {
    e.preventDefault();
    $('.story li').each(function () {
        $(this).removeClass('stactive');
    })
    $(this).addClass('stactive');
    filterskills();
});
function filterskills() {

    var pageIndex = $('.story .stactive a').attr('id');
    var keyword = $('#storysearch').val();
    $.ajax({
        url: "/Admin/Story",
        type: "POST",
        data: {
            SearchInputdata: keyword,
            pageindex: pageIndex
        },
        success: function (response) {
          
            $('.table').html($(response).find('.table').html());
            $('.pagination').html($(response).find('.pagination').html());


        }
    })
}
$(document).ready(function () {

    $('#storysearch').keyup(function () {
        
        $('.pagination .stactive').removeClass('stactive');
        filterSearch();

    });
});

function filterSearch() {
    var keyword = $('#storysearch').val();

    $.ajax({
        url: "/Admin/Story",
        type: "POST",
        data: {
            SearchInputdata: keyword,

        },
        success: function (response) {
            alert('called');

            $('.table').empty().html($(response).find('.table').html());
            $('.page').empty().html($(response).find('.page').html());

        }
    })
}
function Approvestory(storyid) {
    Swal.fire({
        title: 'Are you sure?',
        text: "This Story will be approved",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            approvestory(storyid);
            Swal.fire(
                'Deleted!',
                'Your file has been deleted.',
                'success'
            )
        }
    })
}
function approvestory(storyId) {
    $.ajax({
        url: "/Admin/ApproveStory",
        type: "POST",
        data: {
            storyid: storyId,
        },
        success: function (response) {
            $('#loadPartialView').html($(response).find('#loadPartialView').html());
        }
    })
}
function Declinestory(storyid) {
    Swal.fire({
        title: 'Are you sure?',
        text: "This Story will be Declined",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            declinestory(storyid);
            Swal.fire(
                'Deleted!',
                'Your file has been deleted.',
                'success'
            )
        }
    })
}
function declinestory(storyId) {
    $.ajax({
        url: "/Admin/DeclineStory",
        type: "POST",
        data: {
            storyid: storyId,
        },
        success: function (response) {
            $('#loadPartialView').html($(response).find('#loadPartialView').html());
        }
    })
}
function Deletestory(storyid) {
    Swal.fire({
        title: 'Are you sure?',
        text: "This Story will be Deleted",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            deletestory(storyid);
            Swal.fire(
                'Deleted!',
                'Your file has been deleted.',
                'success'
            )
        }
    })
}
function deletestory(storyId) {
    $.ajax({
        url: "/Admin/DeleteStory",
        type: "POST",
        data: {
            storyid: storyId,
        },
        success: function (response) {
            $('#loadPartialView').html($(response).find('#loadPartialView').html());
        }
    })
}