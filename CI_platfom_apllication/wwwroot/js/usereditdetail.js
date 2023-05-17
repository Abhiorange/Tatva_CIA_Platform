
$(document).ready(function () {
    $('#countryselected').on('change', function () {
       
        ddlCity = $('#cityselected');
        $.ajax({
            url: '/home/City',
            type: 'POST',
            dataType: 'json',
            data: { id: $(this).val() },
            success: function (d) {
                ddlCity.empty();
                $.each(d, function (i, data) {
                    ddlCity.append('<option value="' + data.cityId + '" id="' + data.cityId + '">' + data.name + '</option>');
                });
            },
            error: function () {
                alert('Error');
            }
        });
    });
});


$('.available-skills div').click(function () {

    $(this).hasClass("bg-secondary") ? $(this).removeClass("bg-secondary") : $(this).addClass("bg-secondary");
})

function selectSkill() {
    $('.selected-skills').empty();
    $('.available-skills div').each(function () {

        if ($(this).hasClass("bg-secondary")) {
            var div = $(this).clone();
            div.removeClass('bg-secondary');
            $('.selected-skills').append(div);
        }
    })
}
$(document).on('click', '.selected-skills div', function () {
    $(this).hasClass("bg-secondary") ? $(this).removeClass("bg-secondary") : $(this).addClass("bg-secondary");
});



function deselectSkill() {
    $('.selected-skills div').each(function () {
        if ($(this).hasClass('bg-secondary')) {
            var id = $(this).attr('id');
            $('.available-skills').find('#' + id).removeClass('bg-secondary');
            $(this).remove();
        }
    })
}




function addskill() {
    console.log("done")
    $('.skill-selected').empty();

    $('.selected-skills div').each(function () {
        var id = $(this).attr('id');
        $('.skill-selected').append($(this).clone());

    })

}

function addtodatabase() {
  
    var skillids = [];
    $('.skill-selected div').each(function () {
        skillids.push($(this).attr('id'));
    });
    console.log($('#contactform').html());
    
  
        if ($('#contactform').valid()) {
            $.ajax({
                url: '/home/AddSkills',
                type: "POST",
                data: {
                    skillids: skillids,
                },
                success: function (result) {
                    console.log("profile updated successfully");
                }
            })
        }
    
}
function handleSelectedFile(file) {

    var formData = new FormData();
    formData.append("Image", file);
    $.ajax({
        type: 'POST',
        url: '/home/AddImage',
        data: formData,
        processData: false,
        contentType: false,
        success: function (result) {
         
            window.location = result.redirectUrl;

        }
    });

}
var img1 = $('#model-img');

var img = document.getElementById("profile-photo");
img.src = img1.attr('data-avtar');

$('.img-wrapper').mouseover(function () {
    $('#boot-icon').removeClass("d-none");
})
$('.img-wrapper').mouseout(function () {
    $('#boot-icon').addClass("d-none");
})



document.getElementById("profileimg").onclick = function () {
    
    document.getElementById("file-input").click();
}
document.getElementById("boot-icon").onclick = function () {
 
    document.getElementById("file-input").click();
}
document.getElementById("file-input").onchange = function () {
  
    handleSelectedFile(this.files[0]);
}

$('#staticBackdrop1').on('hidden.bs.modal', function (e) {
    $('#oldpass').val('');
    $('#newpass').val('');
    $('#confirm').val('');
    $('#oldpassspan').text('');
    $('#newspassspan').text('');
    $('#confirmpassspan').text('');
});