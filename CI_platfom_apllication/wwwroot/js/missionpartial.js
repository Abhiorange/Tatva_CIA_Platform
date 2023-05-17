$('.nav-link').each(function () {
    $(this).parent().removeClass('bg-light');
    $(this).css('color', 'white');
});
$('.nav-link.mission').parent().addClass('bg-light');
$('.nav-link.mission ').css('color', 'orange');
function showModal(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "This Mission will be de-activated",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            DeleteMission(id);
            Swal.fire(
                'Deleted!',
                'Your file has been deleted.',
                'success'
            )
        }
    })
}
function DeleteMission(missionId) {
    
    $.ajax({
        url: '/admin/DeleteMission',
        type: 'GET',
        data: {
            missionid: missionId
        },
        success: function (result) {
            $('#loadPartialView').html($(result).find('#loadPartialView').html());
            toastr.success("mission is deleted ");
          
        }
    });
}




$(document).on('click', '.mis li', function (e) {
    e.preventDefault();
    $('.mis li').each(function () {
        $(this).removeClass('misactive');
    })
    $(this).addClass('misactive');
    filtermissions();
});
function filtermissions() {
   
    var pageIndex = $('.mis .misactive a').attr('id');
    var keyword = $('#searchmission').val();
    $.ajax({
        url: "/Admin/Mission",
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

    $('#searchmission').keyup(function () {
       
        $('.pagination .misactive').removeClass('misactive');
        filterSearch();

    });
});

function filterSearch() {
    var keyword = $('#searchmission').val();

    $.ajax({
        url: "/Admin/Mission",
        type: "POST",
        data: {
            SearchInputdata: keyword,

        },
        success: function (response) {
        
           
            $('.table').empty().html($(response).find('.table').html());
            $('.page').empty().html($(response).find('.page').html());

        }
    })
}

  