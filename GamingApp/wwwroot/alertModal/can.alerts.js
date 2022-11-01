var alerts;

$(document).ready(function () {

    alerts = {
        BasariliAlert: function (title, msg, url , onYes, settings) {
            debugger;
            alerts.MesajGoster(title, msg, 'success', settings);
            $(".yes").click(function () {
                alerts.CallFunction(onYes);
            });
            $(".kClose").click(function () {
                alerts.CallFunction(onYes);
            });
            if (url != null) {
                $('#errorModalX').on('hidden.bs.modal', function () {
                    window.location.href = url;
                });
            }
        },
        OnayAlert: function (title, msg, onYes, onNo, settings) {
            alerts.MesajGoster(title, msg, 'onayIcerik', settings);
            $(".yes").click(function () {
                alerts.CallFunction(onYes);
            });
            $(".no").click(function () {
                alerts.CallFunction(onNo);
            });
            $(".kClose").click(function () {
                alerts.CallFunction(onNo);
            });
        },
        HataAlert: function (title, msg, onNo) {
            debugger;
            alerts.MesajGoster(title, msg, 'hataIcerik');
            /*
            //  ----- Manuel Verilen Hataların Loglanması için//
            var arg = {
                message: title,
                url: 'Custom Error Message',
                line: 0,
                column: 0
            };
            var xhr = new XMLHttpRequest();
            xhr.open('POST', '/Error/RaiseServerSideException');

            xhr.setRequestHeader('Content-Type', 'application/json');
            xhr.send(JSON.stringify(arg));
            // ----- End Manuel Log
            */
            $(".no").click(function () {
                alerts.CallFunction(onNo);
            });
            $(".kClose").click(function () {
                alerts.CallFunction(onNo);
            });
        },
        AlertVer: function (title, msg, onNo) {
            debugger;
            alerts.MesajGoster(title, msg, onNo);
        },
        BilgiAlert: function (title, msg, onInfo) {
            alerts.MesajGoster(title, msg, 'bilgiIcerik');
            $(".info").click(function () {
                alerts.CallFunction(onInfo);
            });
            $(".kClose").click(function () {
                alerts.CallFunction(onInfo);
            });
        },
        UyariAlert: function (title, msg, onInfo) {
            alerts.MesajGoster(title, msg, 'uyariMesajiIcerik');
            $(".kClose,.warning").click(function () {
                alerts.CallFunction(onInfo);
            });
        },
        SilAlert: function (title, msg, onYes, onNo, onCancel) {
            alerts.MesajGoster(title, msg, 'silIcerik');
            $(".yes").click(function () {
                alerts.CallFunction(onYes);
            });
            $(".no").click(function () {
                alerts.CallFunction(onNo);
            });
            $(".cancel").click(function () {
                alerts.CallFunction(onCancel);
            });
            $(".kClose").click(function () {
                alerts.CallFunction(onCancel);
            });
        },
        OnayVazgecAlert: function (title, msg, onYes, onNo, onCancel) {
            alerts.MesajGoster(title, msg, 'onayVazgecIcerik');
            $(".yes").click(function () {
                alerts.CallFunction(onYes);
            });
            $(".no").click(function () {
                alerts.CallFunction(onNo);
            });
            $(".cancel").click(function () {
                alerts.CallFunction(onCancel);
            });
            $(".kClose").click(function () {
                alerts.CallFunction(onCancel);
            });
        },
        BasariliInfo: function (msg, autoclose, owner) {
            alerts.BilgiGoster(msg, "success", autoclose, owner);
        },
        BilgiInfo: function (msg, autoclose, owner) {
            alerts.BilgiGoster(msg, "info", autoclose, owner);
        },
        HataInfo: function (msg, autoclose, owner) {
            alerts.BilgiGoster(msg, "error", autoclose, owner);
        },
        UyariInfo: function (msg, autoclose, owner) {
            alerts.BilgiGoster(msg, "warning", autoclose, owner);
        },
        BilgiGoster: function (message, messageType, autoclose, owner) {
            if (owner != undefined) {
                this.AltBilgiGoster(message, messageType, autoclose, owner);
                return;
            }
            var infoWindow = $("<div class='infoWindow'><div class='infoOverlay'></div><div class='" + messageType + " infoBox'><span>" + message + "</span><span class='close'>x</span></div></div>");
            //eskisini temizleyelim
            //$(".infoWindow").remove();
            //kendisinden üstte varsa altına açsın
            var topCoord = 0;
            $(".infoWindow").each(function () {
                topCoord += $(this).height() + 1;
            });
            $("body").prepend(infoWindow);
            if (messageType == "success") {
                $(infoWindow).css({ "top": 0,"right":"100px", "left":"100px","width":"calc(100% - 215px)" });
            } else {
                $(infoWindow).css({ "top": topCoord, "right": "100px", "left": "100px", "width": "calc(100% - 215px)" });
            }
            $(infoWindow).find("span.close").on("click", function () { alerts.CloseInfo(infoWindow); });
            //mesaj kutusunu göster
            $(infoWindow).css({ "display": "block", "opacity": "0" }).show().animate({ opacity: 1 }, 1000, "easeInOutBack");
            if (autoclose) {
                $(infoWindow).find("span.close").hide();
                if (messageType == "success") {
                    setTimeout(function () { alerts.CloseInfo(infoWindow); }, 1000);
                } else {
                    setTimeout(function () { alerts.CloseInfo(infoWindow); }, 3000);
                }
            }
        },
        AltBilgiGoster: function (message, messageType, autoclose, owner) {
            var infoWindow = $("<div class='infoWindow'><div class='infoOverlay'></div><div class='" + messageType + " infoBox'><span>" + message + "</span><span class='close'>x</span></div></div>");
            //eskisini temizleyelim
            //$(".infoWindow").remove();
            //açık olan pencereleri kapat
            this.CloseAllInfo();
            //owner'a ekleyelim.
            owner.prepend(infoWindow);

            $(infoWindow).css({ "position": "absolute", "bottom": 0 });
            $(infoWindow).find("span.close").on("click", function () { alerts.CloseInfo(infoWindow); });
            $(infoWindow).attr("data-align", "bottom");

            //mesaj kutusunu göster
            $(infoWindow).show();
            if (autoclose) {
                $(infoWindow).find("span.close").hide();
                setTimeout(function () { alerts.CloseInfo(infoWindow); }, 2000);
            }
        },
        MesajGoster: function (title, msg, messageType) {
            alerts.CloseAlert();
            var alertHtml = "<div id='errorModalX' class='modal fade' data-easein='swing' style='z-index:9999;'> <div class='modal-dialog modal-confirm'><div class='modal-content' style='background-color: floralwhite;border: 2px black solid;'><div class='modal-header'>";
            if (messageType == "success") {
                alertHtml += "<div class='icon-box' style='background:#47c9a2'> <i class='material-icons'>&#xE876;</i></div>";
            }
            else if (messageType == "onayIcerik")
            {
                alertHtml += "<div class='icon-box' style='background:#47c9a2'> <i class='material-icons'>&#xE887;</i></div>";

            }
            else if (messageType == "bilgiIcerik") {
                alertHtml += "<div class='icon-box' style='background:#47c9a2'> <i class='material-icons'>&#xE887;</i></div>";

            }
            else {
                alertHtml += "<div class='icon-box' style='background:#e85e6c'><i class='material-icons'>&#xE5CD;</i></div>";
            }
            alertHtml += "<h4 class='modal-title w-100'>" + title + "</h4></div><div class='modal-body'><p style='text-align: center;'>" + msg + "</p></div> <div class='modal-footer'>";
            if (messageType == "success") {
                alertHtml += "<button class='btn btn-success btn-block yes' style='background-color:#47c9a2!important' data-dismiss='modal'>Tamam</button>";
            }
            else if (messageType == "onayIcerik") {
                alertHtml += "<button class='btn btn-success btn-block yes' style='background-color:#47c9a2!important' data-dismiss='modal'>Evet</button>";
                alertHtml += "<button class='btn btn-danger btn-block no' style='background-color:#e85e6c!important;margin: 0 !important;' data-dismiss='modal'>Hayır</button>";

            }
            else if (messageType == "bilgiIcerik") {
                alertHtml += "<button class='btn btn-danger btn-block no' style='background-color:#e85e6c!important;margin: 0 !important;' data-dismiss='modal'>Kapat</button>";

            }
            else {
                alertHtml += "<button class='btn btn-danger btn-block' style='background-color:#e85e6c!important' data-dismiss='modal'>Tamam</button>";
            }
            alertHtml += "</div></div></div>";

            //eskisini temizleyelim
            // alerts.CloseAlert();

            //yenisini ekleyelim
            $("body").prepend(alertHtml);

            //overlay gösterelim
            $("#errorModalX").modal('show');
            alerts.center();

        },
        center: function () {
            //mesaj kutusunu göster
            var top = ($(window).height() - $("#alertsContent .alert .alrt").height()) / 2;
            var left = ($(window).width() - $("#alertsContent .alert .alrt").width()) / 2;

            $(".alert").css({ "display": "block", "opacity": "0" }).show().animate({ opacity: 1 }, 300, "easeInOutBack");
            /*$("#alertBack .alrt").css("top", Math.max(0, (($(window).height() - $("#alertBack .alrt").outerHeight()) / 2) +
                                                $(window).scrollTop()) + "px");
            $("#alertBack .alrt").css("left", Math.max(0, (($(window).width() - $("#alertBack .alrt").outerWidth()) / 2) +
                                                        $(window).scrollLeft()) + "px");*/
            $("#alertBack .alrt").css("top", Math.max(0, (($(window).height() - $("#alertBack .alrt").outerHeight()) / 2)) + "px");
            $("#alertBack .alrt").css("left", Math.max(0, (($(window).width() - $("#alertBack .alrt").outerWidth()) / 2)) + "px");
            //$("#alertsContent .alrt").css({ "margin-top": -top});
        },
        CloseAlert: function () {
            $("#alertBack").remove();
            $("#alertsContent").remove();
        },
        CloseInfo: function (info) {
            info.animate({ opacity: 0 }, 500, function () {
                //kapatılanın üst koordinatını al.
                var thisTop = $(this).position().top;
                //kapatılanı kaldır
                $(this).remove();

                if ($(this).attr("data-align") != "bottom") {
                    //ekranda başka infowindow var mı?
                    $(".infoWindow").each(function () {
                        //kapatılandan aşağıda infowindow var mı?
                        if ($(this).position().top >= thisTop) {
                            //yükekliği + 1 kadar yukarı kaydır
                            var newTop = $(this).position().top - ($(this).height() + 1);
                            $(this).animate({ "top": newTop });
                        }
                    });
                }
            });
        },
        CloseAllInfo: function () {
            $(".infoWindow").remove();
        },
        CallFunction: function (callbackEvent) {
            alerts.CloseAlert();

            if (typeof callbackEvent === "function")
                callbackEvent();

        },
        GetCustomButtonArrHtml: function (buttons) {
            if (!buttons)
                return "";

            var retHtml = $("<div class='alertButtons'></div>");
            $.each(buttons, function (i, e) {
                var id = Math.round(+new Date() / 1000);
                var btn = $("<span class='" + e.cssClass + "' id='" + id + "'>" + e.text + "</span>");
                if (e.click) {
                    $(document).on("click", "#" + id, e.click);
                }
                retHtml.append(btn);
            });
           
            //return retHtml.html();
            return retHtml.prop("outerHTML");
        }
    };

    alerts.center();
});

