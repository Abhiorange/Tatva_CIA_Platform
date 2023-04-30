function saveData() {
    // Get input values
    const missionid = document.getElementById("mission").value;
    const title = document.getElementById("title").value;
    var textarea = tinymce.get("storytext").getContent();
    var description = textarea.substring(3, textarea.length - 4);
    const date = document.getElementById("date").value;
    const video = document.getElementById("videoURL").value;
    const imagePaths = [];
    const images = document.getElementById('image-preview');
    const image_tag = images.getElementsByTagName("img");

    for (let i = 0; i < image_tag.length; i++) {
        const image = image_tag[i].getAttribute("src");
        imagePaths.push(image);
    }
    if (isValidated()) {
        $.ajax({
            url: "/StoryListing/storydatabse",
            type: "POST",
            data: {
                missionid: missionid,
                title: title,
                description: description,
                status: 'DRAFT',
                images: imagePaths,
                videos: video,
                date: date
            },
            success: function (response) {

                window.location = response.redirectUrl;

            },
            Error: function () {
                alert('error in skill');
            }
        })
    }
   
}

function editData() {
    // Get input values
    const missionid = document.getElementById("mission").value;
    const title = document.getElementById("title").value;
    var textarea = tinymce.get("storytext").getContent();
    var description = textarea.substring(3, textarea.length - 4);
    const date = document.getElementById("date").value;
    const video = document.getElementById("videoURL").value;


    const imagePaths = [];
    const images = document.getElementById('image-preview');
    const image_tag = images.getElementsByTagName("img");
    console.log(image_tag);

    var currentTime = new Date();
    var currentTimeString = currentTime.toISOString();

    for (let i = 0; i < image_tag.length; i++) {
        const image = image_tag[i].getAttribute("src");
        imagePaths.push(image);
    }
    console.log(imagePaths);
    if (isValidated()) {
        $.ajax({
            url: "/StoryListing/editdatabase",
            type: "POST",
            data: {
                missionid: missionid,
                title: title,
                description: description,
                status: 'PENDING',
                images: imagePaths,
                videos: video,
                date: date,
             
            },
            success: function (response) {

                window.location = response.redirectUrl;
                toaster.success("Story is edited successfully");
            },
            Error: function () {
                alert('error in skill');
            }
        })
    }
   
}
getmissions();
function getmissions() {
    const missionid = document.getElementById("mission").value;
    console.log("after mission id", missionid);
    
        $.ajax({
            url: '/StoryListing/getmissions',
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
function handleMissionChange() {
    const missionid = document.getElementById("mission").value;
     $.ajax({
         url: '/StoryListing/loadaddstory',
         type: "GET",
         data: {
             missionid: missionid,
         },
         success: function (result) {

             console.log(result);
             window.location = result.redirectUrl;
           
            }
     })
}
function submitData(storyid) {
    $.ajax({
        url: '/StoryListing/submit',
        type: "POST",
        data: {
            storyId: storyid,
        },
        success: function (result) {
            window.location = result.redirectUrl;
            alert("submit is succesfull");
        }
    })
}

const dropZone = document.getElementById("drop-zone");
const fileInput = document.getElementById("file-input");
const imagePreview = document.getElementById("image-preview");
const uploadedFiles = new Set();

if (listData != null) {
    var files = new Array();
    for (var img = 0; img < listData.length; img++) {
        var byteCharacters = atob(listData[img].toString().split(',')[1]);
        var byteNumbers = new Array(byteCharacters.length);
        for (var i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        var byteArray = new Uint8Array(byteNumbers);
        var blob = new Blob([byteArray], { type: "image/jpeg" });
        var file = new File([blob], "image" + i + ".jpg", { type: "image/jpeg" });
        files.push(file);
    }
    handleFiles(files);
}
dropZone.addEventListener("click", () => {
    fileInput.click();
});

dropZone.addEventListener("dragover", (event) => {
    event.preventDefault();
    dropZone.classList.add("dragover");
});

dropZone.addEventListener("dragleave", () => {
    dropZone.classList.remove("dragover");
});

dropZone.addEventListener("drop", (event) => {
    event.preventDefault();
    dropZone.classList.remove("dragover");
    const files = event.dataTransfer.files;
    handleFiles(files);
});

fileInput.addEventListener("change", () => {
    const files = fileInput.files;
    handleFiles(files);
});
function handleFiles(files) {
    for (let i = 0; i < files.length; i++) {
        const file = files[i];
        if (!file.type.startsWith("image/") && !file.type.startsWith("video/")) continue;
        if (uploadedFiles.has(file.name)) {
            alert(`File "${file.name}" has already been uploaded.`);
            continue;
        }
        uploadedFiles.add(file.name);
        const image = document.createElement("img");
        image.classList.add("image-preview");
        const imageContainer = document.createElement("div");
        imageContainer.classList.add("image-container");
        const removeImage = document.createElement("div");
        removeImage.innerHTML = "&#10006;";
        removeImage.classList.add("remove-image");
        removeImage.addEventListener("click", () => {
            uploadedFiles.delete(file.name);
            imageContainer.remove();
        });
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => {
            image.src = reader.result;
            imageContainer.appendChild(image);
            imageContainer.appendChild(removeImage);
            imagePreview.appendChild(imageContainer);
        };
    }
}
tinymce.init({
    selector: '#storytext',
    plugins: 'link image code',
    toolbar: 'undo redo | bold italic | fontsizeselect | alignleft aligncenter alignright alignjustify | superscript subscript ',
    height: 300
});
function isValidated() {
    
    console.log("hello");
    const missionId = $("#mission").val();
    const title = $("#title").val();
    const date = $("#date").val();
    const video = $("#videoURL").val();
    var textarea = tinymce.get("storytext").getContent();
    const imagePaths = [];
    validate = true;
    if (missionId == "") {
        validate = false;
        $('#missionValidate').removeClass('d-none');
    } else {
        $('#missionValidate').addClass('d-none');
    }
    if (title == "") {
        validate = false;
        $('#titleValidate').removeClass('d-none');
    } else {
        $('#titleValidate').addClass('d-none');
    }
    if (date == "") {
        validate = false;
        $('#dateValidate').removeClass('d-none');
    } else {
        $('#dateValidate').addClass('d-none');
    }
    if (video == "") {
        validate = false;
        $('#videoValidate').removeClass('d-none');
    } else {
        $('#videoValidate').addClass('d-none');
    }
    if (textarea == "") {
        validate = false;
        $('#textValidate').removeClass('d-none');
    } else {
        $('#textValidate').addClass('d-none');
    }
    if (imagePaths.length > 20) {
        validate = false;
        $('#photoValidate').removeClass('d-none');
    } else {
        $('#photoValidate').addClass('d-none');
    }
    return validate;
}
$('#mission').keyup(function () {
    isValidated();
});
$('#title').keyup(function () {
    isValidated();
});
$('#date').keyup(function () {
    isValidated();
});
$('#videoURL').keyup(function () {
    isValidated();
});
$('#storytext').keyup(function () {
    isValidated();
});

