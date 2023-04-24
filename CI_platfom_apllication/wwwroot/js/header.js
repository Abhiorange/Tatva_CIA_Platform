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
