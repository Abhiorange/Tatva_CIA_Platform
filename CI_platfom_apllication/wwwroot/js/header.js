var checkboxes = document.querySelectorAll(".checkbox");


let filtersSection = document.querySelector(".filters-section");

var listArray = [];

var filterList = document.querySelector(".filter-list");

var len = listArray.length;

for (var checkbox of checkboxes) {
    checkbox.addEventListener("click", function () {
        if (this.checked == true) {
            addElement(this, this.value);
        }
        else {

            removeElement(this.value);
            console.log("unchecked");
        }
    })
}


function addElement(current, value) {
    let filtersSection = document.querySelector(".filters-section");

    let createdTag = document.createElement('span');
    createdTag.classList.add('filter-list');
    createdTag.classList.add('ps-3');
    createdTag.classList.add('pe-1');
    createdTag.classList.add('me-2');
    createdTag.innerHTML = value;

    createdTag.setAttribute('id', value);
    let crossButton = document.createElement('button');
    crossButton.classList.add("filter-close-button");
    let cross = '&times;'



    crossButton.addEventListener('click', function () {
        let elementToBeRemoved = document.getElementById(value);

        console.log(elementToBeRemoved);
        console.log(current);
        elementToBeRemoved.remove();

        current.checked = false;




    })

    crossButton.innerHTML = cross;


    // let crossButton = '&times;'

    createdTag.appendChild(crossButton);
    filtersSection.appendChild(createdTag);

}

function removeElement(value) {

    let filtersSection = document.querySelector(".filters-section");

    let elementToBeRemoved = document.getElementById(value);
    filtersSection.removeChild(elementToBeRemoved);

}

var buttons = document.getElementById("buttons");
var dropdown = document.getElementById("dropdown");

function showButtons() {
  dropdown.style.display = "none";
  buttons.style.display = "block";
  sessionStorage.setItem("buttonsDisplayed", "true");
}

// Check if the buttons should be displayed on page load
var shouldDisplayButtons = sessionStorage.getItem("buttonsDisplayed");
if (shouldDisplayButtons) {
  dropdown.style.display = "none";
  buttons.style.display = "block";
}


$(document).ready(function () {
    $('#search-input').on('input', function () {
        var searchQuery = $(this).val().toLowerCase();
        var displayedCardsCount = 0;
        $('.remo').each(function () {
            var cardText = $(this).text().toLowerCase();
            if (cardText.indexOf(searchQuery) !== -1 || searchQuery.length === 0) {
                $(this).removeClass("d-none");
                displayedCardsCount++;
            } else {
                $(this).addClass("d-none");
            }
        });
        $('#explore-missions-count').text(displayedCardsCount);
    });
});

function messagenable(event) {
    event.stopPropagation();
    $('.noti').addClass('d-none');
    $('.check').removeClass('d-none');
    $('.titleswithcheck').empty();
    $.ajax({
        type: "GET",
        url: "/home/GetTitles",
        success: function (result) {
            $.each(result.titles, function (i, data) {
                console.log("result", data.title);
                if ($.inArray(data.notificationId, result.ids) !== -1) {
                    $('.titleswithcheck').append('<div class="form-check ms-3"><input class="form-check-input checkbox title" checked type="checkbox" value="' + data.notificationId + '" id=' + data.notificationId + '><label class="form-check-label" for=' + data.notificationId + ' >' + data.title + '</label></div>')
                }
                else {
                    $('.titleswithcheck').append('<div class="form-check ms-3"><input class="form-check-input checkbox title" type="checkbox" value="' + data.notificationId + '" id=' + data.notificationId + '><label class="form-check-label" for=' + data.notificationId + '>' + data.title + '</label></div>')
                }
            })
        }
    });
}
function cancel(event) {
    event.stopPropagation();
    $('.noti').removeClass('d-none');
    $('.check').addClass('d-none');
}

function selecttitles() {
    titles = [];
    $('.title:checkbox:checked').each(function () {
        titles.push($(this).attr("id"));
    })
    console.log('titles', titles);
    $.ajax({
        type: "POST",
        url: "/home/SetStatus",
        data: {
            titles: titles
        },
        success: function (result) {
            getusernotification();
            toastr.success("Now for selected titles Notification will be shown!!");
        }
    });
}
    getusernotification();

    function getusernotification() {

        alert('called');
        $('.usernoti').empty();
        $.ajax({
            type: "GET",
            url: "/home/GetNotification",
            success: function (result) {
                var dictionary = result;
                var count = Object.keys(dictionary).length;
                $('.circletext').text(count);
                $.each(result, function (i, data) {

                    switch (data.item2) {
                        case 5:
                            console.log("success");
                            var img = '<img src="/Assets/add.png" class="imgsize mt-1 mx-2">';
                            if (data.item6 == 1) {
                                icon = '<i class="bi bi-circle-fill text-warning col-2 text-end me-2" id="message-' + data.item5 + '"></i>';
                            }
                            else {
                                icon = '<i class="bi bi-check-circle-fill col-2 text-end me-2" id="message-' + data.item5 + '"></i>';
                            }
                            datestring = '<span>' + data.item3 + '</span>'
                            $('.usernoti').append('<div class="form-check p-0 d-flex align-items-start border-bottom bg-light" onclick="notify(\'' + data.item4 + '\', ' + data.item5 + ')" >' + img + '<div class="d-flex flex-column">' + datestring + '<span class="form-check-label mx-1" for=' + data.item2 + '>' + data.item1 + '</span></div> ' + icon + '</div>');
                            break;
                        case 1:
                            console.log("success recomended");
                            var img = '<img src="' + data.item7 + '" class="imgsize mt-1 mx-2">';
                            if (data.item6 == 1) {
                                icon = '<i class="bi bi-circle-fill text-warning col-2 text-end me-2" id="message-' + data.item5 + '"></i>';
                            }
                            else {
                                icon = '<i class="bi bi-check-circle-fill col-2 text-end me-2 text-warning" id="message-' + data.item5 + '"></i>';
                            }
                            datestring = '<span>' + data.item3 + '</span>'
                            $('.usernoti').append('<div class="form-check p-0 d-flex align-items-start border-bottom bg-light" onclick="notify(\'' + data.item4 + '\', ' + data.item5 + ')" >' + img + '<div class="d-flex flex-column">' + datestring + '<span class="form-check-label mx-1" for=' + data.item2 + '>' + data.item1 + '</span></div> ' + icon + '</div>');
                            break;
                        case 4:
                            console.log("success");
                            var img = '<img src="/Assets/right.png" class="imgsize mx-2 mt-1 col-2">';
                            if (data.item6 == 1) {
                                icon = '<i class="bi bi-circle-fill text-warning col-2 text-end me-2" id="message-' + data.item5 + '"></i>';
                            }
                            else {
                                icon = '<i class="bi bi-check-circle-fill text-warning col-2 text-end me-2" id="message-' + data.item5 + '"></i>';
                            }
                            datestring = '<span>' + data.item3 + '</span>'
                            $('.usernoti').append('<div class="form-check p-0 d-flex align-items-start border-bottom bg-light"onclick="notify(\'' + data.item4 + '\', ' + data.item5 + ')">' + img + '<div class="d-flex flex-column">' + datestring + '<span class="form-check-label mx-1" for=' + data.item2 + '>' + data.item1 + '</span></div>' + icon + '</div>');
                            break;
                        case 8:
                            console.log("success");
                            var img = '<img src="/Assets/right.png" class="imgsize mx-2 mt-1 col-2">';
                            if (data.item6 == 1) {
                                icon = '<i class="bi bi-circle-fill text-warning col-2 text-end me-2" id="message-' + data.item5 + '"></i>';
                            }
                            else {
                                icon = '<i class="bi bi-check-circle-fill text-warning col-2 text-end me-2" id="message-' + data.item5 + '"></i>';
                            }
                            datestring = '<span>' + data.item3 + '</span>'
                            $('.usernoti').append('<div class="form-check p-0 d-flex align-items-start border-bottom bg-light" onclick="notify(\'' + data.item4 + '\', ' + data.item5 + ')">' + img + '<div class="d-flex flex-column">' + datestring + '<span class="form-check-label mx-1" for=' + data.item2 + '>' + data.item1 + '</span></div>' + icon + '</div>');
                            break;
                        default:
                            console.log("Value is not 1, 2, or 3.");
                    }
                })
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("Error: " + errorThrown);
            }
        });
    }
    function notify(url, id) {
        console.log("url", url);
        alert('notify called');
        var icon = $('#message-' + id);
        icon.removeClass('bi-circle-fill');
        icon.addClass('bi-check-circle-fill');

        $.ajax({
            type: "POST",
            url: "/home/ChangeStatus",
            data: {
                messageid: id
            },
            success: function (result) {
                alert('success hit');
                if (url != null) {
                    window.location.href = url;
                }
            }
        });
    }
    function clearseen() {
        $.ajax({
            type: "POST",
            url: "/home/ClearAll",
            success: function (result) {
                getusernotification();
                toastr.success("All notifications seen!!");
            }
        });
    }
