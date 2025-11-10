

$(document).ready(function () {

    $('.form-image').click(function () { $('#customFile').trigger('click'); });

    setTimeout(function () {
        $('body').addClass('loaded');
    }, 200);

    jQueryModalGet = (url, title, event, sizeClass) => {
        try {
            $.ajax({
                type: 'GET',
                url: url,
                contentType: false,
                processData: false,
                success: function (res) {
                    if (res.html == "") return;
                    $('#form-modal .modal-dialog').removeClass("modal-xl", "modal-lg", "modal-md", "modal-sm");
                    $('#form-modal .modal-dialog').addClass(sizeClass);
                    $('#form-modal .modal-body').html(res.html);
                    $('#form-modal .modal-title').html(title);
                    $('#form-modal').modal({ backdrop: 'static', keyboard: false })  
                    $('#form-modal').modal('show');

                    //var form = $('#form-modal .modal-body').find('form');
                    //form.removeData("validator");
                    //form.removeData("unobtrusiveValidation");
                    //$.validator.unobtrusive.parse(form);

                    console.log(res);
                },
                error: function (err) {
                    console.log(err)
                }
            })
            //to prevent default form submit event
            event.stopImmediatePropagation();
            return false;
        } catch (ex) {
            console.log(ex)
        }
    }

    jQueryModalPost = (form, event, returnToVoid) => {

        var isvalid = $(form).valid();
        if (!isvalid) {
            event.preventDefault();
            return false;
        }

        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    if (res.isValid) {
                        if (!returnToVoid) {
                            $('#viewAll').html(res.html)
                        };
                        $('#form-modal').modal('hide');
                    }
                },
                error: function (err) {
                    console.log(err)
                }
            })
            //prevent default form submit event
            event.preventDefault();
            return false;
        } catch (ex) {
            console.log(ex)
        }
    }

    jQueryModalDelete = (form, event, strConfirmation) => {
        if (confirm(strConfirmation)) {
            try {
                $.ajax({
                    type: 'POST',
                    url: form.action,
                    data: new FormData(form),
                    contentType: false,
                    processData: false,
                    success: function (res) {
                        if (res.isValid) {
                            $('#viewAll').html(res.html)
                        }
                    },
                    error: function (err) {
                        console.log(err)
                    }
                })
                //to prevent default form submit event
                event.preventDefault();
                return false;
            } catch (ex) {
                console.log(ex)
            }
        }

        //prevent default form submit event
        return false;
    }

    jQueryChildModalGet = (url, title, event, sizeClass) => {
        try {
            $.ajax({
                type: 'GET',
                url: url,
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#form-childmodal .modal-dialog').removeClass("modal-xl", "modal-lg", "modal-md", "modal-sm");
                    $('#form-childmodal .modal-dialog').addClass(sizeClass);
                    $('#form-childmodal .modal-body').html(res.html);
                    $('#form-childmodal .modal-title').html(title);
                    $('#form-childmodal').modal({ backdrop: 'static', keyboard: false })
                    $('#form-childmodal').modal('show');
                    console.log(res);
                },
                error: function (err) {
                    console.log(err)
                }
            })
            //to prevent default form submit event
            event.preventDefault();
            return false;
        } catch (ex) {
            console.log(ex)
        }
    }

    jQueryChildModalPost = (form, event) => {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    if (res.status == 'success') {
                        //$('#viewAll').html(res.html)
                        $('#form-childmodal').modal('hide');
                    }
                },
                error: function (err) {
                    console.log(err)
                }
            })
            //to prevent default form submit event
            event.preventDefault();
            return false;
        } catch (ex) {
            console.log(ex)
        }
    }

    $.validator.setDefaults({
        ignore: ''
    });

    $('.sidebar-collapse').each(function () {
        if (localStorage.getItem("sidebar_coll_" + this.id) === "true") {
            $(this).collapse("show");
            $(this).prev().find(".right-icon i.fas").toggleClass("rotate");
            console.log('show ' + this.id);
        }
        else {
            $(this).collapse("hide");
            //$(this).prev().find(".right-icon i.fas").toggleClass("rotate");
            console.log('hide ' + this.id);
        }
    });


});