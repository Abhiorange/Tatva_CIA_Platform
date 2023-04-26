
getmissionsbytime();
getmissionsbygoal();
function getmissionsbytime() {
    alert('success');
    const missionid = document.getElementById("mission").value;
    $.ajax({
        url: '/TimeSheet/getmissionsbytime',
        success: function (result) {
            if (result.length === 0) {
                $('#mission').html('<option>No countries selected</option>');
            } else {
                $.each(result, function (i, data) {
                    if (missionid != data.missionId) {
                        $('#mission').append('<option value="' + data.missionId + '" id="' + data.missionId + '">' + data.title + '</option>');
                    }

                })
            }
        }
    })
}

function getmissionsbygoal() {
    alert('success1');

      const missionid = document.getElementById("mission").value;
  
    $.ajax({
        url: '/TimeSheet/getmissionsbygoal',
        success: function (result) {
            if (result.length === 0) {
                $('#mission_id').html('<option>No mission is selected</option>');
            } else {
                $.each(result, function (i, data) {
                     if (missionid != data.missionId) {
                    $('#mission_id').append('<option value="' + data.missionId + '" id="' + data.missionId + '">' + data.title + '</option>');
                    }

                })
            }
        }
    })
}
$(document).ready(function () {
    $('#staticBackdrop').on('show.bs.modal', function (event) {
        alert("modal called");
        var button = $(event.relatedTarget); // Button that triggered the modal
        var timesheetId = button.data('timesheetid'); // Extract missionid from data attribute
        var missionTitle = button.data('missiontitle');
        var notes = button.data('notes');
        var modal = $(this);
        modal.find('#goalmission').val(missionTitle);
        modal.find('#message').val(notes);
        modal.find('#timesheet').val(timesheetId);
    });
});
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
function DeleteMission(timesheetId) {
    alert("delete misson called");
    $.ajax({
        url: '/TimeSheet/deletedatabase',
        type: 'GET',
        data: {
            timesheetid: timesheetId
        },
        success: function (result) {
            $('.table').html($(result).find('.table').html());
            

        }
    });
}

function showModal1(id) {
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
            DeleteMission1(id);
            Swal.fire(
                'Deleted!',
                'Your file has been deleted.',
                'success'
            )
        }
    })
}
function DeleteMission1(timesheetId) {
    alert("delete misson called");
    $.ajax({
        url: '/TimeSheet/deletedatabasegoal',
        type: 'GET',
        data: {
            timesheetid: timesheetId
        },
        success: function (result) {
            $('.table1').html($(result).find('.table1').html());


        }
    });
}

