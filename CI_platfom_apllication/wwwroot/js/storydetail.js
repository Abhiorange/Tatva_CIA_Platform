
         $('.slider-for').slick({
   slidesToShow: 1,
   slidesToScroll: 1,
   arrows: false,
   fade: true,
   asNavFor: '.slider-nav'
 });
 $('.slider-nav').slick({
   slidesToShow: 3,
   slidesToScroll: 1,
   asNavFor: '.slider-for',
   dots: true,
   focusOnSelect: true,
   autoplay : true
 });

 $('a[data-slide]').click(function(e) {
   e.preventDefault();
   var slideno = $(this).data('slide');
   $('.slider-nav').slick('slickGoTo', slideno - 1);
 });

var model = $('#model');

function senduser(sid) {
    var selecteduserid = [];
    $('.modal-body input[type="checkbox"]:checked').each(function () {
        selecteduserid.push($(this).attr("id"));
    });
   
    $.ajax({
        type: "POST",
        url: '/StoryListing/usersthrouid',
        data: {
            ids: selecteduserid,
            storyid: sid,
            from_user: model.attr('data-userid'),
        },
        beforeSend: function () {
            swal.fire({
                html: '<h5>Loading...</h5><div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div>',
                showConfirmButton: false,
                onRender: function () {
                    $('.swal2-content').prepend(sweet_loader);
                }
            });
        },
        traditional: true,
        success: function (response) {
            Swal.fire({
                icon: 'success',
                title: 'success',
                text: 'Invite link sent',
            })
            alert('sucess fully send mail...');
        }
    });
}
var storyId = 0;
function btnShowUsers(sid) {
    storyId = sid;


    $('.modal-body').empty();
    $.ajax({
        url: '/StoryListing/getusers',
        success: function (result) {
            // iterate through the result and append each user to the list code=bhilykvfemjbcceg
            $.each(result, function (i, data) {
                $('.modal-body').append('<div class="form-check ms-3"><input class="form-check-input checkbox" type="checkbox" value="' + data.firstName + " " + data.lastName + '" id=' + data.userId + '><label class="form-check-label" for=' + data.userId + '>' + data.firstName + " " + data.lastName + '</label></div>')
            });

        }
    });
}

function sendmail() {
    senduser(storyId);
}
