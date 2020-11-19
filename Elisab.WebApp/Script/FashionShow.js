var routeURL = location.protocol + '//' + location.host;

function GetAll() {
  var response = getRequest(routeURL + '/api/AdminApi/GetAllFashionShow');
  if (response.status == 1) {
    $("#fashionShowList").html(response.dataenum);
  }
  else {
    $("#fashionShowList").html('');
    msg_failure(response.message);
  }
}

function Add() {

  if (validation() == false) {
    msg_failure("Please fill all require fileds");
    return false;
  }

  var Id = $("#Id").val();
  var IsActive = false;

  if ($("#IsActive").prop('checked') == true) {
    IsActive = true;
  }

  var requestData = {
    Id: Id,
    Name: $.trim($('#Name').val()),
    ShowDate: $.trim($('#ShowDate').val()),
    ShowTime: $.trim($('#ShowTime').val()),
    IsActive: IsActive
  };

  if (Id > 0) {
    var url = routeURL + '/api/AdminApi/UpdateFashionShow';
  }
  else {
    url = routeURL + '/api/AdminApi/AddFashionShow';
  }

  var response = formPostRequest(url, requestData);
  if (response.status == 1) {
    msg_success(response.message);
    setTimeout(function () {
      window.location = routeURL + '/FashionShow/List';
    }, 1000);
  }
  else {
    msg_failure(response.message)
  }
}

function Delete(Id) {
  var resonse = getRequest(routeURL + '/api/AdminApi/DeleteFashionShow?Id=' + Id);
  if (resonse.status == 1) {
    msg_success(resonse.message);
    GetAll();
  }
  else {
    msg_failure(resonse.message);
  }
}

function validation() {
  var isValid = true;

  if ($.trim($('#Name').val()) == "") {
    isValid = false;
  }

  if ($.trim($('#ShowDate').val()) == "") {
    isValid = false;
  }

  if ($.trim($('#ShowTime').val()) == "") {
    isValid = false;
  }

  return isValid;
}

function onActiveChange(Id) {
  var IsActive = false;

  if ($("#" + Id).prop("checked") == true) {
    IsActive = true;
  }
  else if ($("#" + Id).prop("checked") == false) {
    IsActive = false;
  }

  var response = getRequest(routeURL + '/api/AdminApi/UpdateFashionShowActive?Id=' + Id + "&IsActive=" + IsActive);
  if (response.status == 1) {
    msg_success(response.message);
    GetAll();
  }
  else {
    msg_failure(response.message);
  }
}

function goBack() {
  window.history.back();
}
