﻿
getmissionsbytime();
getmissionsbygoal();
function getmissionsbytime() {
 
    const missionid = document.getElementById("mission").value;
    $.ajax({
        url: '/TimeSheet/getmissionsbytime',
        success: function (result) {
            $('#mission').html('<option value="">Select Misson</option>');
            if (result.length === 0) {
                $('#mission').html('<option>No MISSION is found</option>');
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
    

    const missionid = document.getElementById("mission_id").value;
    console.log("this mission id", missionid);
    $.ajax({
        url: '/TimeSheet/getmissionsbygoal',
        success: function (result) {
            $('#mission_id').html('<option value="">Select Misson</option>');
            if (result.length === 0) {
                $('#mission_id').html('<option>No mission is found</option>');
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
            
            var button = $(event.relatedTarget); // Button that triggered the modal
            var timesheetId = button.data('timesheetid'); // Extract missionid from data attribute
            var missionTitle = button.data('missiontitle');
            var notes = button.data('notes');
        var action = button.data('action');
        var goalvalue = button.data('goalvalue');
        console.log("this is goal value", goalvalue);
        var totalgoalachieve = button.data('totalachieved');
            console.log(action);
            var modal = $(this);
            modal.find('#goalmission').val(missionTitle);
        modal.find('#action1').val(action);
            modal.find('#message1').val(notes);
        modal.find('#timesheet').val(timesheetId);
        modal.find('#goalvalue1').val(goalvalue);
        modal.find('#totalgoalachieve').val(totalgoalachieve);
    });
});
$('#edittimeform').submit(function (event) {
    
    console.log("hour value", $('#edithour').val());
    event.preventDefault();
    if ($('#edithour').val() > 24) {
        $('#edithourspan').text("Hour value should be less than 24");
    }
    if ($('#editminute').val() > 60) {
        $('#editminutespan').text("minute value should be less than 60");
    }
    else if ($('#edittimeform').valid()) {
        $('#edittimeform')[0].submit();
    }
})
$('#goalform').submit(function (event) {
        $('#messageval').addClass('d-none');
    $('#actionval').addClass('d-none');
    $('#validaction').addClass('d-none');
    event.preventDefault();
    var validgoalvalue = $('#goalvalue1').val() - $('#totalgoalachieve').val();
    console.log("goal valuw", $('#goalvalue1').val());
    console.log("goal achieve", $('#totalgoalachieve').val());
    console.log("valid goal value", validgoalvalue);

            if ($('#action1').val() == '') {
        console.log("for action", $('#action1').val());
        $('#actionval').removeClass('d-none');
    }
   else if ($('#message1').val() == '') {
        console.log("for message", $('#message1').val());
        $('#messageval').removeClass('d-none');
            }
           else if (validgoalvalue == 0)
            {
                $('#validaction').removeClass('d-none').text('Your Goal is achived you cant edit');
            }
   else if ($('#action1').val() >= validgoalvalue) { 
                $('#validaction').removeClass('d-none').text('The value should be less than ' + validgoalvalue);
    }
   else{
        $('#actionval').addClass('d-none');
                $('#messageval').addClass('d-none');
                $('#validaction').addClass('d-none');

        $('#goalform')[0].submit();
    }
});

$('#addgoalform').submit(function (event) {
  
    $('#addvalidspan').addClass('d-none');
    event.preventDefault();
    var goalvalue = $('#goalvalue2').val();
    console.log("goal for this mission value", goalvalue);
    console.log("written action", $('#addaction').val());

    if (parseInt($('#addaction').val()) > parseInt(goalvalue)) {
        $('#addvalidspan').removeClass('d-none').text('This value should be less than ' + goalvalue);
    }
    else if ($('#addgoalform').valid()){
        $('#addvalidspan').addClass('d-none');
        $('#addgoalform')[0].submit();
    }
});
function Takegoalvalue() {
    const missionid = document.getElementById("mission_id").value;

    $.ajax({
        url: '/TimeSheet/Getgoalvalueformission',
        data: {
            missionid: missionid
        },
        success: function (result) {
            console.log("goalvalue", result);
            $('#goalvalue2').val(result);
        }
    })
}
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

//AddTime modal
$('#exampleModal1').on('hidden.bs.modal', function (e) {
    $('#mission').val('');
    $('#date').val('');
    $('#hour').val(0);
    $('#minute').val(0);
    $('#message').val('');
    $('#addmissiontime').text('');
    $('#adddatetime').text('');
    $('#addhourtime').text('');
    $('#addminutetime').text('');
    $('#addnotetime').text('');
});
//AddGoal modal
$('#exampleModal2').on('hidden.bs.modal', function (e) {
    $('#mission_id').val('');
    $('#addaction').val('');
    $('#date').val(0);
    $('#minute').val(0);
    $('#message').val('');
    $('#addmissiongoal').text('');
    $('#addvalidspan').text('');
    $('#adddategoal').text('');
    $('#addmissionnote').text('');
});

