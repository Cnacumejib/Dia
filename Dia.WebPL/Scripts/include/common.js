var btnSendExam = document.getElementById("btnSendExam");
var goReportBtn = document.getElementById("goReportBtn");
var goExamBtn = document.getElementById("goExamBtn");
var selectUser = document.getElementById("selectUser");
var selectSection = document.getElementById("selectSection");
var sectionId = selectSection.value;

$(".newvalue").attr('pattern', '[+|-]?\\d*([.|,]?\\d\\d*)?');

var newValues = document.getElementsByClassName("newvalue");

var hiddenFrom = $("#filledFrom");
var hiddenTo =$("#filledTo");

if (hiddenFrom.val() == "") {
    $(".newvalue").bind('input', function () {
        hiddenFrom.val(new Date());
        $("#filledFromInfo").html("Начало замеров: " + hiddenFrom.val());
        $(".newvalue").unbind("input");
    });
      
}

function checkReady() {
    var isReady = true;
    var diff = 0;
    for (i = 0; i < newValues.length; i++) {
        if (typeof newValues[i].value ==="undefined" || isNaN(newValues[i].value.replace(",", ".")) || newValues[i].value == "") {
            isReady = false;
            newValues[i].focus();
            break;
        }
        else
        {
            newValues[i].value = newValues[i].value.replace(",", ".");
        }
    }

    if (isReady && hiddenTo.val()=="") {
        hiddenTo.val(new Date());
        $("#filledToInfo").html("Окончание замеров: " + hiddenTo.val());
    }

    return isReady;
}

function changeSection() {
    if (hiddenFrom.value != "") {
        var answer = confirm("При смене отделения все внесённые данные будут утеряны.\nВы уверены, что хотите сменить отделение");
        if (answer) {
            window.location = "Index.cshtml/?sectionid=" + selectSection.value + "&userId=" + selectUser.value;
        }
        else { 
            selectSection.value=sectionId;
            return 0;
        }
    }
    else {
        window.location = "Index.cshtml/?sectionid=" + selectSection.value+"&userId="+selectUser.value;
    }
}

function sendExam() {
    if (selectUser.value == "0") {
        alert("Укажите имя ответственного.")
        return 0;
    }

    if (checkReady()) {
        document.forms[0].submit();
    }
    else {
        alert("Проверьте внесённые данные.\nУстраните пробелы и ошибки");
        return 0;
    }
}

function goReport() {
    window.location = "/Report.cshtml";
}

function goExam() {
    window.location = "Index.cshtml";
}

