////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: NotificationPulling.js ClientEndProcess
//FileType: Javascript Source file
//Author : Asaduzzaman
//Created On : 02-18-2020
//Copy Rights : Evision Soft. Ltd.
////////////////////////////////////////////////////////////////////////////////////////////////////////


var myId, myPic, myName;
var $chatBox = $("#chatboxWrapper");
var $chatList = $("#chat_list");
var $myFriendList = $("#myFriendList");
var $chatmessageCount = $("#chatmessageCount");
var $chatNotificationList = $("#chatNotificationList");
$(document).ready(function () {
    myId = $('#user-me').val();
    myName = $('#user-me-name').val();
    myPic = $('#user-me-pic').val();
    console.info("start sync for " + myId);
    bellCount();

  

    /*var source = new EventSource('/api/notificationpush?id=' + userid);
    source.onmessage = function (event) {
       // console.info(event);
        $('#verify_notification').text(parseInt($('#verify_notification').text()) + 1);
        var element, name, arr;
        element = document.getElementById("verify_notification");
        arr = element.className.split(" ");
        name = "show";
        //console.info(arr);
       // console.info($('#verify_notification').text());
        if (parseInt($('#verify_notification').text()) > 0) {
            if (arr.indexOf(name) == -1) {
                element.classList.remove("hide");
                element.className += " " + name;
            }
        }
        var myJSON = JSON.parse(event.data)
        if (myJSON.Id != null) {
            //console.info(myJSON);
            openVerifyPage(myJSON.Id);
        }
        
    };*/
    var source = new EventSource('/api/notificationpush?id=' + myId);
    source.onmessage = function (event) {
        //console.info(event);
        
        var mySyncData = JSON.parse(event.data);
        //console.info(mySyncData);
        if (mySyncData != null) {
            //console.info(mySyncData);
            syncOnlineUser(mySyncData.OnlineUsers);
            if (mySyncData.ChatMessage != null) {
                chatReciver(mySyncData.ChatMessage);
            }
        }
   
    };



});

function bellHide() {
    var element, name, arr;
    element = document.getElementById("verify_notification");
    name = "show";
    arr = element.className.split(" ");
    if (arr.indexOf(name) == 1) {
        $('#verify_notification').text(0);
        element.classList.remove(name);
        element.className += " hide";
    } 
}

function bellShow(count) {
    var element, name, arr;
    element = document.getElementById("verify_notification");
    name = "show";
    arr = element.className.split(" ");
    if (arr.indexOf(name) == -1) {
        $('#verify_notification').text(count);
        element.classList.remove("hide");
        element.className += " "+name;
    }
}

function syncOnlineUser(users) {
    $('#myFriendList li').each(function () {
        //console.log($(this).attr('id'));
        var id = $(this).attr('id');
        //console.log($.inArray(id, users));
        $(this).removeClass('status-online status-offline');
        if ($.inArray(id, users) == -1) {
            $(this).addClass('status-offline');
        } else {
            $(this).addClass('status-online');
        }
    })
}

function openChatBox(reciver, name, pic) {
    $html = $('html'),
    $html.trigger('click.close-right-sidebar');
    var id = $chatBox.data('reciver'); // exist-userid
    //console.log(id);
    if (reciver !== id) {
        $chatList.empty();
        $chatBox.find('figure img').attr("src", pic);
        $chatBox.find('.panel-title').html(name);
        //console.log('in');
        if ($chatBox.hasClass('hide')) {
            $chatBox.removeClass('hide');
            $chatBox.addClass('show'); 
        }
        $chatBox.data('reciver', reciver);
        getChatMessage(reciver);
    }
    $("#chatText").focus();
}

function chatReciver(data) {
    var id = $chatBox.data('reciver'); // exist-userid
    //console.log(data.ReceiverId);
    //console.log(id);
    if ($chatBox.hasClass('hide')) {
        openChatBox(data.SenderId, data.SenderName, data.SenderPic);
        chatNotificationBellCount();
    } else {
        if (data.SenderId == id) {
            //console.log('chatting');
            if ($chatBox.hasClass('show')) {
                chatRowReciver(data.Date, data.Text);
                scrollDown();
            }
        } else {
            chatNotificationBellCount();
            //addChatNotificationRow(data.SenderId, data.SenderName, data.SenderPic, data.Text);
        }
    }
    
}

function chatNotificationBellCount() {
    $chatmessageCount.text(parseInt($chatmessageCount.text()) + 1);
    document.title = 'ESLERP (' + $chatmessageCount.text() + ')';
    chatBellShow();
}

function addChatNotificationRow(id, name, pic, text, isread) {
    var read = isread ? ' ' : 'isRead';
    var boldtext = isread ? ' ' : 'boldtext';
    var row = `<li class="` + read + `" id=` + id + ` onclick="openChatBox(this.id,this.dataset.name,this.dataset.pic)" data-name=` + name + ` data-pic=` + pic + `><a href="#" class="clearfix"><figure class="image profile-picture"><img src=` + pic + ` alt="" class="img-circle"/></figure><span class="title">` + name + `</span><span class="message ` + boldtext + `">` + text + `</span></a></li>`;
    $chatNotificationList.append(row);
}

function chatBellShow() {
    var name = "show";
    //console.info(arr);
    // console.info($('#verify_notification').text());
    if (parseInt($chatmessageCount.text()) > 0) {
        if (!$chatmessageCount.hasClass(name)) {
            $chatmessageCount.removeClass("hide");
            $chatmessageCount.addClass(name);
        }
    }
}
function chatBellHide() {
    var name = "show";
    $chatmessageCount.text('0');
    document.title = 'ESLERP';
    if ($chatmessageCount.hasClass(name)) {
        $chatmessageCount.removeClass(name);
        $chatmessageCount.addClass('hide');
    }
}



$(function () {
    $("#chat_toggole").click(function () {
        //chatBellHide();
        loadChatNotification();
    });
    $("#notify_toggole").click(function () {
        bellHide();
        //loadData();
    });
    $('#sendChat').click(function () {
        sendMessage(); 
    });
    
    $('#chatText').on('keypress', function (e) {
        if (!e.shiftKey && e.which === 13) {
            sendMessage(); 
        }
    });

});

function getChatMessage(reciverId) {
    //console.log(reciverId);
    //console.log(myId);
    $.ajax({
        type: "GET",
        url: "/Admin/Notification/GetMessage?senderId=" + reciverId+"&reciverId="+myId,
        success: function (res) {
            //console.log(res);
            
            if (res.result) {
                if (res.count > 0) {
                    $chatmessageCount.text(res.count);
                    document.title = 'ESLERP (' + res.chat + ')';
                    chatBellShow();
                } else {
                    chatBellHide();
                }
                $.each(res.data, function (index, value) {
                    //console.info(value);
                    if (value.senderId == myId)
                        chatRowSender(value.date, value.text);
                    else
                        chatRowReciver(value.date, value.text);
                });
                scrollDown();
            }
        }

    });
}

function sendMessage() {
    var text = $("#chatText").val();
    var reciverId = $chatBox.data('reciver');
    //console.log($chatBox.find('.profile-picture img').attr('src'));
    //console.log($chatBox.find('.panel-title').text());
    //var reciverName = $chatBox.find('.panel-title').text();
   // var reciverPic = $chatBox.find('.profile-picture img').attr('src');
   // console.log(text);
    if (text.trim().length > 0) {
        var chat = {
            SenderId: myId,
            SenderName: myName,
            SenderPic: myPic,
            ReceiverId: reciverId,
            ReceiverName: "",
            Text: text,
            IsRead: false,
            Date: new Date()
        };
        //console.log(chat);
        $.ajax({
            type: "POST",
            url: "/Admin/Notification/SendMessage",
            data: chat,
            //dataType: 'json',
            //contentType: "application/json",
            success: function (res) {
                //console.log(res);
                if (res.result) {
                    $("#chatText").val('');
                    chatRowSender(res.data.date,res.data.text);
                    scrollDown();
                }
            }

        });
    }
}

function chatRowSender(date,text) {
    $chatList.append(`<li class="clear right"><div class="date">` + (moment(date).format("DD MMM YY (hh:mm A)")) + `</div><div class="message">` + text+`</div></li>`)
}
function chatRowReciver(date,text) {
    $chatList.append(`<li class="clear left"><div class="date">` + (moment(date).format("DD MMM YY (hh:mm A)")) + `</div><div class="message">` + text + `</div></li>`)
}
function scrollDown() {
    //console.log(document.getElementById('chat_list').scrollTop);
    //console.log(document.getElementById('chat_list').scrollHeight);
    document.getElementById('chat_list').scrollTop = document.getElementById('chat_list').scrollHeight;
}

function loadChatNotification() {
    const myNode = document.getElementById("chatNotificationList");
    myNode.innerHTML = '';
    $.ajax({
        type: "GET",
        url: "/Admin/Notification/GetUnReadChat",
        async: false,
        success: function (response) {
            //console.log(response.data);
            $.each(response.data, function (index, value) {
                addChatNotificationRow(value.senderId, value.senderName, value.senderPic, value.text, value.isRead);
            })
        },
        
    });
}



function bellCount() {
    $.ajax({
        type: "POST",
        url: "/Admin/Notification/Count",

        success: function (response) {
            //alert("Hello: " + response);
            //console.info(response);
            if (response.chat > 0) {
                $chatmessageCount.text(response.chat);
                document.title = 'ESLERP (' + response.chat + ')';
                chatBellShow();
            } else {
                chatBellHide();
            }
            /*if (response.notify > 0) {
                bellShow(response.notify);
            } else {
                bellHide();
            }*/

        },
        
    });
};


(function ($) {

    $(function () {
        $('.chatbox')
            .on('click', '.panel-actions a.fa-times', function (e) {
                e.preventDefault();

                var $panel,
                    $row;

                $panel = $(this).closest('.chatbox');

                if (!!($panel.parent('div').attr('class') || '').match(/col-(xs|sm|md|lg)/g) && $panel.siblings().length === 0) {
                    $row = $panel.closest('.row');
                    //$panel.parent('div').remove();
                    $panel.data('reciver', '');
                    $panel.removeClass('show');
                    $panel.addClass('hide');
                    if ($row.children().length === 0) {
                        $row.remove();
                    }
                } else {
                    $panel.data('reciver', '');
                    $panel.removeClass('show');
                    $panel.addClass('hide');
                }
            });
    });

})(jQuery);

