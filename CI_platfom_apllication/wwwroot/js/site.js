// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('#contact-link').click(() => {
    let userid = $('#hiddeninput').val();
    $.ajax({
        url: '/home/Addcontact',
        type: "POST",
        data: {
            Userid: userid,
        },
        success: function (result) {
            $('#modal-contact').html(result);
            $('#staticBackdrop3').modal('show');
            alert("submit is succesfull");
        }
    })
})
function contactsave() {
    var subject = $('#subject').val();
    var message = $('#message').val();

    $('#contact-form').valid()

    if (!$('#contact-form').valid()) return;

    $.ajax({
        url: '/home/editcontact',
        type: "POST",
        data: {                                                      
            subject: subject,
            message: message
        },
        success: function (result) {
            console.log(result);
            alert('issue saved corrcetly');
            toastr.success('Problem is sent!');
        }
    })
}