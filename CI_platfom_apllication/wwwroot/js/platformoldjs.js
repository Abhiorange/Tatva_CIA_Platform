
























function showList(e) {

    var $gridCont = $('.grid-container');
    var $new_item = $('.new-items');
    var $apply = $('.apply_btn');

    var $only_list = $('.only_list');
    var $only_grid = $('.only_grid');
    var $time_pill = $('.timepill');
    var $star_text = $('.star_text');
    var $tree_star = $('.tree_star');
    var $resize_img = $('.card-img-top');
    e.preventDefault();
    $gridCont.addClass('list-view');
    $resize_img.removeClass('resize_img_grid');
    $resize_img.addClass('resize_img_list');

/*    $gridCont.hasClass('list-view') ? $gridCont.removeClass('list-view') : $gridCont.addClass('list-view');
*/    $gridCont.hasClass('list-view') ? $new_item.removeClass('d-none') : $new_item.addClass('d-block');
    $gridCont.hasClass('list-view') ? $apply.addClass('d-none') : $apply.removeClass('d-block');
    // $gridCont.hasClass('list-view') ? $badge.removeClass('d-none') : $badge.addClass('d-block');
    $only_list.hasClass('list-view') ? $only_list.addClass('d-none') : $only_list.removeClass('d-none');
    $only_grid.hasClass('list-view') ? $only_grid.removeClass('.only_grid') : $only_grid.addClass('d-none');
    $time_pill.hasClass('list-view') ? $time_pill.removeClass('d-none') : $time_pill.addClass('d-none');
    $star_text.hasClass('list-view') ? $star_text.addClass('d-none') : $star_text.removeClass('d-none');
    $tree_star.hasClass('list-view') ? $tree_star.removeClass('d-none') : $tree_star.addClass('d-none');
    // $only_list.addClass('d-flex');
    // $only_grid.addClass('d-none');
}
function gridList(e) {

    var $gridCont = $('.grid-container');
    var $new_item = $('.new-items');
    var $apply = $('.apply_btn');
    var $resize_img = $('.card-img-top');
    // var $badge=$('.extra_badge');
    var $only_list = $('.only_list');
    var $only_grid = $('.only_grid');
    var $time_pill = $('.timepill');
    var $star_text = $('.star_text');
    var $tree_star = $('.tree_star');
    // e.preventDefault();
    $gridCont.removeClass('list-view');
    $new_item.addClass('d-none');
    $apply.removeClass('d-none');
    $resize_img.removeClass('resize_img_list');
    $resize_img.addClass('resize_img_grid');
    // $badge.addClass('d-none');
    $only_grid.removeClass('d-none');
    $only_list.addClass('d-none');
    $time_pill.removeClass('d-none');
    $star_text.addClass('d-none');
    $tree_star.removeClass('d-none');
    //  $tree_star.addClass('.tree_star');
    // $time_pill.addClass('.timepill');
}

$(document).on('click', '.btn-grid', gridList);
$(document).on('click', '.btn-list', showList);

var city = [];
$(document).ready(function () {
    var selectedCountryIds = [];
    debugger;
    getCountry();
    getThemes();
    getSkills();
    $('#city').attr('disabled', true);


    $('#country').on('change', 'input[type="checkbox"]', function () {
        var id = $(this).val();
        var selectedCityIds = [];
        var countryName = $(this).next('label').text();

        if ($(this).prop('checked') && !selectedCountryIds.includes(id)) {
            selectedCountryIds.push(id);
        } else if (!$(this).prop('checked') && selectedCountryIds.includes(id)) {
            selectedCountryIds = selectedCountryIds.filter(function (value) {
                return value !== id;
            });
        }
        $('.city:checkbox:checked').each(function () {
            var cityname = $(this).next('label').text()
            var id = parseInt($(this).attr("id"));
            $('.filters-section').append('<span class="filter-list ps-3 pe-3 me-2">' + cityname + '<i class="bi bi-x filter-close-button" onclick="cross_city(' + id + ')"></i></span>');
            if (city.indexOf(id) === -1) { // check if id is not already present in the array
                city.push(id);
            }
        });
        console.log("city list", city);
        $('#city').empty();
        $.ajax({
            type: "GET",
            url: '/Mission/City',
            data: {
                ids: selectedCountryIds
            },
            traditional: true,
            success: function (result) {
                if (result.length === 0) {
                    $('#city').html('<option>No cities found</option>');
                } else {
                    $.each(result, function (i, data) {
                        if (city.includes(data.cityId)) {
                            $('#city').append('<div class="form-check ms-3"><input class="form-check-input checkbox city" type="checkbox" checked value="' + data.cityId + '" id="' + data.cityId + '"><label class="form-check-label" for="' + data.cityId + '">' + data.name + '</label></div>');
                        }
                        else {
                            $('#city').append('<div class="form-check ms-3"><input class="form-check-input checkbox city" type="checkbox" value="' + data.cityId + '" id="' + data.cityId + '"><label class="form-check-label" for="' + data.cityId + '">' + data.name + '</label></div>');
                        }
                    });
                }
            }
        })
        $('.pagination .active').removeClass('active');
        filterMission()
    })
    $('#city').change(function () {
        $('.checkbox.city:not(:checked)').each(function () { // loop through all unchecked checkboxes with the 'city' class
            var index = city.indexOf($(this).val()); // get the index of the unchecked checkbox's ID in the city array
            if (index !== -1) { // if the unchecked checkbox's ID is found in the city array
                city.splice(index, 1); // remove the ID from the city array
            }
        });
        $('.pagination .active').removeClass('active');
        filterMission()
    });
    $('#theme').change(function () {
        $('.pagination .active').removeClass('active');
        filterMission()
    });
    $('#skill').change(function () {
        $('.pagination .active').removeClass('active');
        filterMission()
    });
    $('#sort').change(function () {

        filterMission()
    });
    $('#search-input').keyup(function () {
        $('.pagination .active').removeClass('active');
        filterMission()
    });
    totalmissions();
});

function filterMission() {

    var country = [];
    $('.filters-section').empty()
    $('.country:checkbox:checked').each(function () {

        var countryName = $(this).next('label').text();
        var id = $(this).val();
        country.push($(this).attr("id"));
        $('.filters-section').append('<span class="filter-list ps-3 pe-3 me-2">' + countryName + '<i class="bi bi-x filter-close-button" onclick="cross_country(' + id + ')"></i></span>')
    })

    $('.city:checkbox:checked').each(function () {
        var cityname = $(this).next('label').text()
        var id = parseInt($(this).attr("id"));
        console.log("here is the cityname list", cityname);
        if (city.indexOf(id) === -1) {
            console.log("it checks for duplicate");// check if id is not already present in the array
            city.push(id);
        }
        $('.filters-section').append('<span class="filter-list ps-3 pe-3 me-2">' + cityname + '<i class="bi bi-x filter-close-button" onclick="cross_city(' + id + ')"></i></span>');
    });
    /* $('.city:checkbox:checked').each(function () {                                    
         var cityname = $(this).next('label').text();
         var id = $(this).val();
         city.push($(this).attr("id"));
       
 
         $('.filters-section').append('<span class="filter-list ps-3 pe-3 me-2">' + cityname + '<i class="bi bi-x filter-close-button" onclick="cross_city(' + id + ')"></i></span>')
     })*/

    var theme = [];
    $('.theme:checkbox:checked').each(function () {
        var id = $(this).attr("id");
        theme.push($(this).attr("id"));
        $('.filters-section').append('<span class="filter-list ps-3 pe-3 me-2">' + $(this).val() + '<i class="bi bi-x filter-close-button" onclick="cross_theme(' + id + ')"></i></span>')
    })

    var skill = [];
    $('.skill:checkbox:checked').each(function () {
        var id = $(this).attr("id");
        skill.push($(this).attr("id"));
        $('.filters-section').append('<span class="filter-list ps-3 pe-3 me-2">' + $(this).val() + '<i class="bi bi-x filter-close-button" onclick="cross_skill(' + id + ')"></i></span>')
    })

    if ($('.filters-section .filter-list').length > 0) {

        $('.filters-section').append('<span class="filter-list ps-3 pe-3 me-2 p-2 clear-all" onclick="clearall()">Clear All</span>');
    }
    var sortId = $('#sort').val();
    var keyword = $('#search-input').val();
    var pageIndex = $('.pagination .active a').attr('id');
    var $gridCont = $('.grid-container');
    var list = $gridCont.hasClass('list-view') ? true : false;

    $.ajax({
        url: "Mission/platformLanding",
        type: "POST",
        data: {
            countryids: country,
            cityids: city,
            themeids: theme,
            skillids: skill,
            id: sortId,
            SearchInputdata: keyword,
            pageindex: pageIndex
        },
        success: function (response) {

            $('#result').html($(response).find('#result').html());
            $('#totalrecord').html($(response).find('#totalrecord').html());
            if (list) {
                $('.btn-list').click();
            }
            $('.page').html($(response).find('.page').html());

        },
        Error: function () {
            alert('error in skill');
        }
    })

}

function cross_country(id) {
    $('.country:checkbox:checked').each(function () {
        if ($(this).val() == id) {
            $(this).prop('checked', false);
        }
    });

    filterMission();
}
function cross_city(id) {

    $('.city:checkbox:checked').each(function () {
        if ($(this).val() == id) {
            $(this).prop('checked', false);
        }
    });
    var index = city.indexOf(id);
    if (index > -1) {
        city.splice(index, 1);
    }
    filterMission();
}
function cross_theme(id) {
    $('.theme:checkbox:checked').each(function () {
        if ($(this).attr("id") == id) {
            $(this).prop('checked', false);
        }
    });
    filterMission();
}
function cross_skill(id) {
    $('.skill:checkbox:checked').each(function () {
        if ($(this).attr("id") == id) {
            $(this).prop('checked', false);
        }
    });
    filterMission();
}
function clearall() {
    $('.country:checkbox:checked').each(function () {
        $(this).prop('checked', false);
    });
    $('.skill:checkbox:checked').each(function () {
        $(this).prop('checked', false);
    });
    $('.theme:checkbox:checked').each(function () {
        $(this).prop('checked', false);
    });
    $('.city:checkbox:checked').each(function () {
        $(this).prop('checked', false);
    });
    $('.filters-section').empty();
    emptyArray(city);
    filterMission();
}
function emptyArray(city) {
    city.splice(0, city.length);
};


function totalmissions() {
    var totalmission = $('#totalmission').val();
    $('#explore-missions-count').text(totalmission);
}


function getCountry() {
    $.ajax({
        url: '/Mission/Country',
        success: function (result) {
            if (result.length === 0) {
                $('#country').html('<option>No countries selected</option>');
            } else {
                $.each(result, function (i, data) {
                    $('#country').append('<div class="form-check ms-3"><input class="form-check-input checkbox country" type="checkbox" value="' + data.countryId + '" id="' + data.countryId + '"><label class="form-check-label" for="' + data.countryId + '">' + data.name + '</label></div>');
                })
            }
        }
    })
}
function getThemes() {
    $.ajax({
        url: '/Mission/Theme',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#theme').append('<div class="form-check ms-3"><input class="form-check-input checkbox theme" type="checkbox" value="' + data.title + '" id="' + data.missionThemeId + '"><label class="form-check-label" for="' + data.missionThemeId + '">' + data.title + '</label></div>');
            })
        }
    })
}

function getSkills() {
    $.ajax({
        url: '/Mission/Skill',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#skill').append('<div class="form-check ms-3"><input class="form-check-input checkbox skill" type="checkbox" value="' + data.skillName + '" id=' + data.skillId + '><label class="form-check-label" for=' + data.skillId + '>' + data.skillName + '</label></div>')
            })
        }
    })
}



var model = $('#model')
var userid = null;

$(document).on('click', '.pagination li', function (e) {
    e.preventDefault();
    $('.pagination li').each(function () {
        $(this).removeClass('active');
    })
    $(this).addClass('active');
    filterMission();
});

function senduser(mid) {
    var selecteduserid = [];
    $('.modal-body input[type="checkbox"]:checked').each(function () {
        selecteduserid.push($(this).attr("id"));
    });
    if (selecteduserid.length === 0) {

        Swal.fire({
            icon: 'info',
            title: 'Oops...',
            text: 'First select user',

        })
    }
    else {
        $('#model-content').addClass('d-none');
        $('#loader').removeClass('d-none');
        $.ajax({
            type: "POST",
            url: '/Mission/usersthrouid',
            data: {
                ids: selecteduserid,
                missionid: userid,
                from_id: model.attr("data-userid")
            },
            traditional: true,
            success: function (response) {
                $('#model-content').removeClass('d-none');
                $('#loader').addClass('d-none');
                alert('sucesscefully mail sent!!');
                toastr.success('Mail is sent!');
            }
        });

        selecteduserid.length = 0;
    }

}
$(document).on('click', '#sendmail', function () {
    senduser(userid);
});
function btnshowUsers(mid) {
    userid = mid;
    $('.modal-body').empty();
    $.ajax({
        url: '/Mission/getusers',
        success: function (result) {
            $.each(result, function (i, data) {
                $('.modal-body').append('<div class="form-check ms-3"><input class="form-check-input checkbox" type="checkbox" value="' + data.firstName + " " + data.lastName + '" id=' + data.userId + '><label class="form-check-label" for=' + data.userId + '>' + data.firstName + " " + data.lastName + '</label></div>')
            });

        }
    });


}

function addtofavorite(mid) {

    var favouriteImage = $('#' + mid).find('.favourite-image');
    if (favouriteImage.attr('src') === '/Assets/heart1.png') {
        favouriteImage.attr('src', '/Assets/fill-heart.png');
    } else {
        favouriteImage.attr('src', '/Assets/heart1.png');
    } ``
    $.ajax({
        type: 'POST',
        url: '/Mission/favroite_mission',
        data: {
            missionid: mid,
            userid: model.attr("data-userid"),
            actionmethod: "platformlanding"
        },
        success: function (response) {
            $(`#favstar-${mid}`).html($(response).find(`#favstar-${mid}`).html());
        }

    });
    filterMission();
}
function showModal() {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: 'Please Login!',
    })
}


