var btnNext = document.getElementById("nextBtn");
var btns = document.getElementsByTagName("li");
var oldValues = document.getElementsByClassName("oldValue");
var newValues = document.getElementsByClassName("newValue");
//var consumptions = document.getElementsByClassName("consumption");
var inpElem = newValues[0];
//var homesite = document.getElementById("homesite").value;
var homesite ="file:///C:/Program%20Files/Eleks/Suo/htdocs/index.html";
var timeout_id = setTimeout('goHome();', 60000);//возврат в меню через полминуты
//inpElem.focus();
inpElem.className = "active newValue";
  //      var inpElem = newValues[i];

for (i = 0; i < newValues.length; i++) {
    newValues[i].addEventListener("focus", function () {
        this.value = "";
        btnNext.className = "btn inactive";
        inpElem.className = "newValue";
        inpElem = this;
        inpElem.className = "active newValue";
    });

    /*
    newValues[i].addEventListener("blur", function () {
    });
    newValues[i].addEventListener("click", function () {
        inpElem = this;
    });*/
}

for (i = 0; i < btns.length; i++) {
    btns[i].addEventListener("click", function () {
        inpValue = inpElem.value;
        btnValue = this.innerHTML;
        if (btnValue == "←") {
            if (inpValue.length > 1) {
                inpElem.value = inpValue.substr(0, inpValue.length - 1);
            }
            else
            {
                inpElem.value = "";
            }
        }
        else if (btnValue.length > 1) {
            inpElem.value = "";
        }
        else if (btnValue == ","){
            //кнопка запятой
            if (inpValue.indexOf(",") == -1 && inpValue.length > 0 && inpValue.length < 7) {
                //еще нет в слове запятой и длина меньше
                inpElem.value += ",";
            }
        }
        else if (inpValue.length == "") {
            //цифровая кнопка в пустое поле
            inpElem.value = btnValue;
        }
        else {
            //цифровая кнопка в добавление
            if (inpValue.indexOf(",") == -1) {
                //число еще без запятой
                if (inpValue.length <6 ) {
                    inpElem.value = (inpValue + btnValue);
                }
            }
            else {
                // число уже c запятой
                if (inpValue.length < 10 && (inpValue.length-inpValue.indexOf(",")<4)) {
                    inpElem.value = (inpValue + btnValue); //.replace(/(\d{3})/g, '$1 ')
                }
            }
        }

        checkReady();
    });
}

function checkReady() {
    clearTimeout(timeout_id);
    timeout_id = setTimeout('goHome();', 60000);//отсрочка возврата в меню на полминуты

    var isReady = true;
    var diff = 0;
    for (i = 0; i < newValues.length; i++) {
        if (isNaN(newValues[i].value.replace(",", ".")) || newValues[i].value == "") {
            isReady = false;
            newValues[i].value = "";
            consumptions[i].innerHTML="&nbsp;";
        }
        else {
            diff = newValues[i].value.replace(",", ".") - oldValues[i].innerHTML.replace(",", ".");
            if (diff < 0 ) {
                isReady = false;
            }
            if ( diff > 2500) {
                isReady = false;
                newValues[i].className = "tooBig newValue";
            }            
   //         consumptions[i].innerHTML=diff.toString().replace(".",",");
        }
    }

    if (isReady) {
        btnNext.className = "btn";
    }
    else {
        btnNext.className = "btn inactive";
    }
    return isReady;
}


function goAhead() {
    if (checkReady()) {
        clearTimeout(timeout_id);//убрать
        document.forms[0].submit();
    }
    else{
        return 0;
    }
}

function goAstern() {
//alert("astern");
    window.history.go(-2);
}

function goHome() {
window.history.go(1-window.history.length);
/*
    if (homesite == "") {
//alert("back");
        window.history.go(-2);
    } else {
//alert("home="+homesite);
        window.location.reload(homesite);
    }
*/
}

