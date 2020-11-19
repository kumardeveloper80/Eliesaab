var routeURL = location.protocol + '//' + location.host;
function onLogin() {
  $("#loginDiv").removeClass("hide");
  $("#forgotPwdDiv").addClass("hide");
  reset();
}

function onForgotPassword() {
  $("#loginDiv").addClass("hide");
  $("#forgotPwdDiv").removeClass("hide");
  reset();
}

function login() {
  if (validation()) {
    var login = {
      Username: $.trim($('#txtUserName').val()),
      Password: $.trim($('#txtPassword').val())
    };
    var response = formPostRequest(routeURL + '/api/AdminApi/Authentication', login);
    if (response.status == 1) {
      window.location = routeURL + '/Home/Index';
    }
    else {
      msg_failure(response.message);
    }
  }
}

function forgotPassword() {
  if (isEmailValid()) {
    var login = {
      Email: $.trim($('#txtEmail').val())
    };

    var response = formPostRequest(routeURL + '/api/AdminApi/ForgotPassword', login)
    if (response.status == 1) {
      msg_success(response.message);
    }
    else {
      msg_failure(response.message);
    }
  }
}

function validation() {
  var isValid = true;

  if (jQuery.trim($("#txtUserName").val()) == "") {
    isValid = false;
    $("#txtUserName").addClass("error");
  }
  else {
    $("#txtUserName").removeClass("error");
  }

  if (jQuery.trim($("#txtPassword").val()) == "") {
    isValid = false;
    $("#txtPassword").addClass("error");
  }
  else {
    $("#txtPassword").removeClass("error");
  }
  return isValid;
}

function isEmailValid() {
  var isValid = true;
  var emailRegx = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
  if ($.trim($('#txtEmail').val()) == "") {
    isValid = false;
    $("#txtEmail").addClass("error");
  }
  else if (!emailRegx.test($.trim($('#txtEmail').val()))) {
    isValid = false;
    $("#txtEmail").addClass("error");
  }
  else {
    $("#txtEmail").removeClass("error");
  }

  return isValid;
}

function reset() {
  $("#txtUserName").val("");
  $("#txtPassword").val("");
  $("#txtEmail").val("");
  $("#txtUserName").removeClass("error");
  $("#txtPassword").removeClass("error");
  $("#txtEmail").removeClass("error");
}

function onKeyPressForm(e) {
  if (e.keyCode === 13) {
    login();
  }
}

