/*$(document).ready(function () {
   
    getCountry();
    
    $('#country').on('change', function () {
        var countryid = $('#country').val();
        $('#city').empty();
        console.log("countryid", countryid);
        $.ajax({
            url: '/home/City',
            data: {
                id: countryid,
            },
            traditional: true,
            success: function (result) {
                console.log('1')
                if (result.length === 0) {
                    $('#country').html('<option>No countries selected</option>');
                }
                else {
                    $('#city').prepend('<option value="" selected>Select City</option>');
                    $.each(result, function (i, data) {             
                        $('#city').append('<option value="' + data.cityId + '" id="' + data.cityId + '">' + data.name + '</option>');
                    })
                }
            }
        })

    });
});*/
$(document).ready(function () {
    $('#countryselected').on('change', function () {
        alert('inside');
        ddlCity = $('#cityselected');
        $.ajax({
            url: '/home/City',
            type: 'POST',
            dataType: 'json',
            data: { id: $(this).val() },
            success: function (d) {
                console.log(d);
                ddlCity.empty();
                $('#cityselected').prepend('<option value="" selected>Select City</option>');

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

/*function getCountry()   
{
    var id = $('#country option:selected').val();
    console.log(id);
    alert('success');
    $.ajax({
        url: '/home/Country',
        type:'POST',
        success: function(result) {
            console.log(result);
            if (result.length === 0)
            {
                $('#country').html('<option>No countries selected</option>');
            }
            else
            {
                $.each(result, function (i, data) {
                    if (data.countryId != id) {
                        $('#country').append('<option value="' + data.countryId + '" id="' + data.countryId + '">' + data.name + '</option>');
                    }
                   
                })
            }
        }
    })
}
*/

$('.available-skills div').click(function () {
  
    $(this).hasClass("bg-secondary") ? $(this).removeClass("bg-secondary") : $(this).addClass("bg-secondary");
})

function selectSkill() {
    $('.selected-skills').empty();
    $('.available-skills div').each(function () {
        
        if ($(this).hasClass("bg-secondary")) {
            //$(this).removeClass('bg-secondary');
            var div = $(this).clone();
            div.removeClass('bg-secondary');       
            $('.selected-skills').append(div);
        }
    })
}
$(document).on('click', '.selected-skills div', function () {
    $(this).hasClass("bg-secondary") ? $(this).removeClass("bg-secondary") : $(this).addClass("bg-secondary");
});


/*$('.selected-skills div').click(function () {
;
   $(this).hasClass("bg-secondary") ? $(this).removeClass("bg-secondary") : $(this).addClass("bg-secondary");
})*/

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
   
    console.log("add skills");
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
  
    /*console.log("file object",file)
        $.ajax({    url: '/home/AddSkills',
            type: 'POST',
            url: '/home/AddImage',
            data: {
                Image: file,
            },
            traditional: true,
            success: function (result) {
                alert('sucess');
            }
        })*/
    var formData = new FormData();
    formData.append("Image", file);
    $.ajax({
        type: 'POST',
        url: '/home/AddImage',
        data: formData,
        processData: false,
        contentType: false,
        success: function (result) {
            alert('success');
            window.location = result.redirectUrl;

        }
    });
    // Read the file as a data URL
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
    alert("sucess");
    document.getElementById("file-input").click();
}
document.getElementById("boot-icon").onclick = function () {
    alert("sucess");
    document.getElementById("file-input").click();
}
document.getElementById("file-input").onchange = function () {
    alert('inside file');
    handleSelectedFile(this.files[0]);
}
