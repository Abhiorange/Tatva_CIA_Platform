$('.nav-link').each(function () {
    $(this).parent().removeClass('bg-light');
    $(this).css('color', 'white');
});
$('.nav-link.user1').parent().addClass('bg-light');
$('.nav-link.user1 ').css('color', 'orange');

$(document).on('click', '.use li', function (e) {
    e.preventDefault();
    $('.use li').each(function () {
        $(this).removeClass('usactive');
    })
    $(this).addClass('usactive');
    console.log($(this).children().attr("id"))
    filteruser();
});
function filteruser() {
   
    var pageIndex = $('.use .usactive a').attr('id');
    var keyword = $('#search').val();
    $.ajax({
        url: "/Admin/User",
        type: "POST",
        data: {
            SearchInputdata: keyword,
            pageindex: pageIndex
        },
        success: function (response) {
            $('.table').html($(response).find('.table').html());
            $('.page').html($(response).find('.page').html());
        }
    })
}
$(document).ready(function () {
   
    $('#search').keyup(function () {
      
        $('.pagination .usactive').removeClass('usactive');
        filterSearch();
       
    });
});
function filterSearch() {
    var keyword = $('#search').val();
   
    $.ajax({
        url: "/Admin/User",
        type: "POST",
        data: {
            SearchInputdata: keyword,

        },
        success: function (response) {
          
            console.log(response);
            console.log("the id element", $(response).find("#nouser").html());
            $('.table').empty().html($(response).find('.table').html());
            $('.page').empty().html($(response).find('.page').html());
           
        }
    })
}

function showModal(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "This User will be de-activated",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            deleteUser(id);
            Swal.fire(
                'Deleted!',
                'Your file has been deleted.',
                'success'
            )
        }
    })
}
function deleteUser(userId) {
  
    $.ajax({
        url: '/admin/DeleteUser',
        type: 'GET',
        data: {
            userid: userId
        },
        success: function (result) {
            $('#loadPartialView').html($(result).find('#loadPartialView').html());
            toastr.success("User is deleted ");

        }
    });
}