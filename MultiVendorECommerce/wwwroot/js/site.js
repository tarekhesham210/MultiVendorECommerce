// File size validation
$.validator.addMethod("filesize", function (value, element, maxSize) {
    if (element.files.length === 0)
        return true;

    return element.files[0].size <= maxSize;
});

// File extension validation
$.validator.addMethod("extension", function (value, element, allowedExtensions) {
    if (!value)
        return true;

    var extension = value.split('.').pop().toLowerCase();
    var allowed = allowedExtensions.toLowerCase().split(',');

    return allowed.includes(extension);
});

// Register unobtrusive adapters
$.validator.unobtrusive.adapters.add("filesize", ["filesize"], function (options) {
    options.rules["filesize"] = options.params.filesize;
    options.messages["filesize"] = options.message;
});

$.validator.unobtrusive.adapters.add("extension", ["extension"], function (options) {
    options.rules["extension"] = options.params.extension;
    options.messages["extension"] = options.message;
});




// image preview function
function previewImage(input) {
    if (input.files && input.files[0]) {
        const reader = new FileReader();
        reader.onload = e => document.getElementById('profileImagePreview').src = e.target.result;
        reader.readAsDataURL(input.files[0]);
    }
}