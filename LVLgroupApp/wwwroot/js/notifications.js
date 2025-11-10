
$(document).ready(function () {

    /*----------- Notifications ----------------*/


    // indica se alguma notification foi removida
    var notificationDeleted = false;

    // popover's content is hidden
    // refresh notifications from server if notification deleted
    $('#notificationPopover').on('hidden.bs.popover', function () {
        getNotifications();
        notificationDeleted = false;
    });

    // popover's content is shown (add event listeners to content)
    $('#notificationPopover').on('shown.bs.popover', function () {

        $('.stretched-link').click(function (e) {

            console.log("card clicked");

            e.preventDefault();
            e.stopPropagation();
            // find clicked card
            let $card = $(this).parent()
            let header = $card.children(":first").attr("class", "card-header text-white bg-success p-1");

            let id = e.target.getAttribute("data-sendedid");
            let isRead = e.target.getAttribute("data-isread");
            // 0 se false, 1 se true
            if (isRead == '0') {
                decrementReadNotification()
                e.target.setAttribute("data-isread", 1);
                $card.attr("class", "card card-notification border-success mb-3");
            }

            // set isRead=true no server side
            readNotification(id);
            return false;
        });

        $('.btnclose').click(function (e) {

            console.log("btnclose clicked");

            e.preventDefault();
            e.stopPropagation();
            // find clicked row
            let notificationRow = $(this).parents('.notification-row')[0];
            let sendedId = notificationRow.querySelector('.stretched-link').getAttribute("data-sendedid");

            // delete notificationSended no client side
            deleteNotification(sendedId, notificationRow);

            notificationDeleted = true;
            let isRead = notificationRow.querySelector('.stretched-link').getAttribute("data-isread");
            // 0 se false, 1 se true
            if (isRead == '0') decrementReadNotification();

            $(notificationRow).css({
                display: 'block',
                overflow: 'hidden'
            }).animate({
                height: 0
            },
                700,
                function () {
                $(this).remove();
            });
            return false;
        });
    });

    function truncate(str, n, useWordBoundary) {
        if (str == null) return "";
        if (str.length <= n) { return str; }
        const subString = str.slice(0, n - 1); // the original check
        return (useWordBoundary
            ? subString.slice(0, subString.lastIndexOf(" "))
            : subString) + "&hellip;";
    };

    function decrementReadNotification() {
        let span = document.getElementById("notificationCount");
        if (span == null) return;
        let counter = span.innerHTML;
        let value = parseInt(counter, 10) - 1;
        if (value > 0) {
            document.getElementById("notificationCount").innerHTML = value;
        }
        else {
            // remover contador
            span.parentNode.removeChild(span);
        }
    };

    function readNotification(sendedId) {

        $('[data-toggle="popover"]').popover('hide');
        // marcar notificação como lida e retornar view de visualização
        jQueryModalGet('/notification/mynotification/OnGetView?id=' + sendedId, localizedStrings.notification, null, 'modal-lg')
    };

    function deleteNotification(sendedId, notificationRow) {

        console.log('Delete notification = ' + sendedId);
        let _url = '/Notification/MyNotification/DeleteNotification?id=' + sendedId;

        $.ajax(
            {
                type: "POST",
                url: _url,
                processData: false,
                contentType: false,
                dataType: 'json',
                success: function (data) {
                    let isRead = notificationRow.querySelector('.stretched-link').getAttribute("data-isread");
                    // 0 se false, 1 se true
                    if (isRead == '0') decrementReadNotification();

                    //$(notificationRow).fadeOut("slow", function () {
                    //    $(notificationRow).remove();
                    //});

                    $(notificationRow).addClass("closeSlide");
                    setTimeout(function () {
                        $(notificationRow).fadeOut("slow", function () {
                            $(notificationRow).remove();
                        });
                    }, 100);

                    notificationDeleted = true;
                    console.log("Notification deleted (true) = " + sendedId);
                },
                error: function (erro) {
                    console.log("Notification deleted - Error occurs: " + erro);
                }
            }
        );

    };

    function getSenderNotification(objNotification) {

        //let str = JSON.stringify(objNotification, null, 4); // (Optional) beautiful indented output.
        //console.log("Notification: " + str); // Logs output to dev tools console.

        let src = "";
        if (objNotification.fromUser.profilePicture != null && objNotification.fromUser.profilePicture.length > 0) {
            src = "data: image/*;base64," + objNotification.fromUser.profilePicture;
        }
        else {
            src = "/images/default-user.png";
        }

        let email = objNotification.fromUser.email ?? "";
        let name = objNotification.fromUser.name ?? "";
        let roleName = objNotification.fromUser.roleName ?? "";
        let local = objNotification.fromUser.local ?? "";

        let row = document.createElement("div");
        row.setAttribute("class", "row");

        let col = document.createElement("div");
        col.setAttribute("class", "col-sm-3");
        let img = document.createElement("img");
        img.setAttribute("class", "rounded-circle img-fluid m-0");
        img.setAttribute("src", src);
        col.appendChild(img);

        // col2 = email + nome + role
        let col2 = document.createElement("div");
        col2.setAttribute("class", "col-sm-8 m-0 p-1");
        let p = document.createElement("p");
        p.setAttribute("class", "email m-0 p-0");
        p.innerHTML = truncate(email, 30, false);
        let p2 = document.createElement("p");
        p2.setAttribute("class", "person m-0");
        p2.innerHTML = name;
        let p3 = document.createElement("p");
        p3.setAttribute("class", "designation m-0");
        p3.innerHTML = roleName + " " + local;
        col2.appendChild(p);
        col2.appendChild(p2);
        col2.appendChild(p3);

        // col3 = close button
        let col3 = document.createElement("div");
        col3.setAttribute("class", "col-sm-1 my-0 p-0");
        let closeBtn = document.createElement('button');
        //closeBtn.setAttribute("class", "btn btnclose p-0");
        if (objNotification.notificationSended.isRead) {
            closeBtn.setAttribute("class", "btn btnclose btnclose-success p-0");
        }
        else {
            closeBtn.setAttribute("class", "btn btnclose btnclose-warning p-0");
        };
        let closeIcon = document.createElement("i");
        closeIcon.setAttribute("class", "fas fa-times-circle fa-lg");
        closeBtn.appendChild(closeIcon);
        col3.appendChild(closeBtn);

        row.appendChild(col);
        row.appendChild(col2);
        row.appendChild(col3);
        return row;
    };

    function getNotificationHeader(objNotification) {

        let cardHeader = document.createElement("div");
        if (objNotification.notificationSended.isRead) {
            cardHeader.setAttribute("class", "card-header text-white bg-success p-1");
        }
        else {
            cardHeader.setAttribute("class", "card-header text-black bg-warning p-1");
        };

        cardHeader.appendChild(getSenderNotification(objNotification));
        return cardHeader;
    };

    function getNotificationBody(objNotification) {

        let cardBody = document.createElement("div");
        cardBody.setAttribute("class", "card-body");

        let title = document.createElement("h7");
        title.setAttribute("class", "card-title");
        title.innerHTML = truncate(objNotification.subject, 30, true);
        cardBody.appendChild(title);

        let hr = document.createElement('hr');
        cardBody.appendChild(hr);

        let text = document.createElement("p");
        text.setAttribute("class", "card-text mt-2");
        text.style.fontSize = "x-small";
        text.innerHTML = truncate(objNotification.text, 100, true);
        //text.innerHTML = objNotification.text;
        cardBody.appendChild(text);

        return cardBody;
    };

    function getNotificationFooter(objNotification) {

        let cardFooter = document.createElement("div");
        cardFooter.setAttribute("class", "card-footer");

        if (objNotification.notificationSended.isRead) {
            cardFooter.setAttribute("class", "card-footer text-white border-0");
        }
        else {
            cardFooter.setAttribute("class", "card-footer border-0");
        };

        let row = document.createElement("div");
        row.setAttribute("class", "row");
        let col1 = document.createElement("div");
        col1.setAttribute("class", "col-sm-6");
        let col2 = document.createElement("div");
        col2.setAttribute("class", "col-sm-6");

        let smallLeft = document.createElement("small");
        smallLeft.setAttribute("class", "text-muted float-start");
        var myDate = new Date(objNotification.date);
        smallLeft.innerHTML = myDate.toLocaleDateString();

        let smallRight = document.createElement("small");
        smallRight.setAttribute("class", "text-muted float-end");
        smallRight.innerHTML = myDate.toLocaleTimeString();

        col1.appendChild(smallLeft);
        col2.appendChild(smallRight);
        row.appendChild(col1);
        row.appendChild(col2);
        cardFooter.appendChild(row);

        return cardFooter;
    };

    function appendNotification(container, objNotification) {

        let row = document.createElement("div");
        row.setAttribute("class", "row notification-row");
        let col = document.createElement("div");
        col.setAttribute("class", "col-sm-12");
        let card = document.createElement("div");
        if (objNotification.notificationSended.isRead) {
            card.setAttribute("class", "card card-notification border-success mb-3");
        }
        else {
            card.setAttribute("class", "card card-notification border-warning mb-3");
        };

        card.appendChild(getNotificationHeader(objNotification));
        card.appendChild(getNotificationBody(objNotification));
        card.appendChild(getNotificationFooter(objNotification));

        let anchor = document.createElement("a");
        anchor.setAttribute("class", "stretched-link");
        anchor.setAttribute("href", "#");
        anchor.setAttribute("data-sendedid", objNotification.notificationSended.id);
        // 0 se false, 1 se true
        if (objNotification.notificationSended.isRead) {
            anchor.setAttribute("data-isread", 1);
        }
        else {
            anchor.setAttribute("data-isread", 0);
        }
        card.appendChild(anchor);

        console.log("sended id = " + objNotification.notificationSended.id);

        col.appendChild(card);
        row.appendChild(col);
        container.appendChild(row);
    };

    function removePrevNotifications() {
        $(".notification-count").remove();
        $(".notification-count-bell").remove();
        if ($('#notification-content').length) {
            $('#notification-content').empty();
        }
    };

    function getNotifications() {

        console.log("getNotifications() starting...");

        let _url = "/Notification/MyNotification/GetMyNotifications";

        $.ajax(
            {
                type: "POST",
                url: _url,
                processData: false,
                contentType: false,
                dataType: 'json',
                success: function (data) {

                    //let str = JSON.stringify(data, null, 4); // (Optional) beautiful indented output.
                    //console.log("GetNotifications: " + str); // Logs output to dev tools console.

                    // preparar title
                    let titleText = $('#notification-title-template').html();

                    // preparar container
                    let container = document.getElementById("notification-content");
                    let rootContainer = document.getElementById("notification-rootContent");
                    if ($('#notification-content').length) {
                        $('#notification-content').empty();
                    }
                    else {
                        container = document.createElement("div");
                        container.setAttribute("id", "notification-content");
                        container.setAttribute("class", "container-fluid");
                        rootContainer.appendChild(container);
                    }

                    // criar lista de notificações
                    let isNotRead = 0;
                    for (let i = 0; i < data.count; i++) {
                        appendNotification(container, data.notifications[i]);
                        if (!data.notifications[i].notificationSended.isRead) isNotRead += 1;
                    }

                    const oldPopover = bootstrap.Popover.getInstance('#notificationPopover')
                    if (oldPopover) oldPopover.dispose();

                    // criar popover de notificações
                    var popoverElement = document.getElementById('notificationPopover');
                    var popoverInstance = new bootstrap.Popover(popoverElement, {
                        html: true,
                        title: titleText,
                        content: document.getElementById('notification-content'),
                        container: 'body',
                        placement: 'bottom'
                    });

                    // remover contador anterior
                    $(".notification-count").remove();
                    $(".notification-count-bell").remove();

                    let anchor = document.getElementById("notificationPopover");
                    let bellIcon = document.createElement("i");
                    bellIcon.setAttribute("class", "far fa-bell fa-lg notification-count-bell");
                    anchor.appendChild(bellIcon);

                    // contador de notificações por ler
                    if (isNotRead > 0) {
                        // adicionar contador de notificações
                        let span = document.createElement("span");
                        span.setAttribute("id", "notificationCount");
                        span.setAttribute("class", "badge rounded-pill bg-warning notification-count");
                        span.textContent = isNotRead;
                        anchor.appendChild(span);
                    }

                    console.log("end of getMyNotifications isNotRead = " + isNotRead);
                },
                error: function (erro) {
                    console.log("Error occurs: " + erro);
                }
            }
        );

    };


    if ($('#hfUserId').length) {
        console.log("userId = " + $('#hfUserId').val());

        let connection = new signalR.HubConnectionBuilder()
            .withUrl("/notificationHub")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        connection.start().then(function () {
            console.log('connected to hub');
        }).catch(function (err) {
            return console.error(err.toString());
        });

        connection.on("OnConnected", function () {
            OnConnected();
        });

        function OnConnected() {
            let userId = $('#hfUserId').val();
            connection.invoke("SaveUserConnection", userId).catch(function (err) {
                return console.error(err.toString());
            })
        };

        connection.on("ReceivedNotification", function () {
            console.log("ReceivedNotification: ready to getNotifications()");
            getNotifications();
        });

        getNotifications();
    }
    else {
        console.log("userId = unAuthenticated");
    }
 
});