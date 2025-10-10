/*
 * Translated default messages for the jQuery validation plugin.
 * Locale: EN (English)
 */

jQuery.extend(jQuery.validator.messages, {
    required: "This field is required.",
    remote: "Please fix this field.",
    email: "Please enter a valid email address.",
    url: "Please enter a valid URL.",
    date: "Please enter a valid date.",
    dateISO: "Please enter a valid date (ISO).",
    number: "Please enter a valid number.",
    digits: "Please enter only digits.",
    creditcard: "Please enter a valid credit card number.",
    equalTo: "Please enter the same value again.",
    oneDigit: "Password must contain at least one digit.",
    oneLowerCase: "Password must contain at least one lowercase.",
    oneUpperCase: "Password must contain at least one uppercase.",
    oneSpecialChar: "Password must contain at least one special character.",
    uppercaseonly: "Please enter uppercase letters only.",
    loja: "Please select a store.",
    maiorque: $.validator.format("Please enter a value greater than {0}."),
    colorcode: "Please enter a color code. Should start with #, followed by letters from a-f, A-F and/or digits 0-9.",
    telefone: "Please enter a phone number in the format: +xxx xxx xxx xxx.",
    accept: "Please enter a value with a valid extension.",
    maxlength: jQuery.validator.format("Please enter no more than {0} characters."),
    minlength: jQuery.validator.format("Please enter at least {0} characters."),
    rangelength: jQuery.validator.format("Please enter a value between {0} and {1} characters long."),
    range: jQuery.validator.format("Please enter a value between {0} and {1}."),
    max: jQuery.validator.format("Please enter a value less than or equal to {0}."),
    min: jQuery.validator.format("Please enter a value greater than or equal to {0}.")
});

