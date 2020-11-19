var routeURL = location.protocol + '//' + location.host;

$(document).ready(function () {

  $("#imgUpload1").change(function (e) {
    imageUpload(this.files, 'imgUpload1', 'backgroundImg');
  });

  $("#imgUpload2").change(function (e) {
    imageUpload(this.files, 'imgUpload2', 'imgInner');
  });

  $("#imgUpload3").change(function (e) {
    imageUpload(this.files, 'imgUpload3', 'imgLogo');
  });
});

function onFashionShowChange() {
  clear();
    var response = getRequest(routeURL + '/api/AdminApi/GetMainPageByFashionShowId?Id=' + $("#fashionShowId").val());
    if (response.status == 1) {
      debugger
        $("#ContentText").val(response.dataenum.ContentText);
        $("#mainPageId").val(response.dataenum.Id);
        $('#backgroundImg').attr("src", '/UploadImg/' + response.dataenum.BackgroundImage);
        $('#imgInner').attr("src", '/UploadImg/' + response.dataenum.InnerImage);
        $('#imgLogo').attr("src", '/UploadImg/' + response.dataenum.LogoImage);
        $('#IsActive').prop('checked', response.dataenum.IsActive);
  }
}

function onSubmit() {

  if (validation()) {
    var requestData = new FormData();
    var imgback = document.getElementById('imgUpload1');
    var imginner = document.getElementById('imgUpload2');
    var imglogo = document.getElementById('imgUpload3');

    if (imgback.files.length > 0) {
      requestData.append("backImg", imgback.files[0]);
    }
    if (imginner.files.length > 0) {
      requestData.append("innerImg", imginner.files[0]);
    }
    if (imglogo.files.length > 0) {
      requestData.append("logoImg", imglogo.files[0]);
    }
    var mainPageId = 0;

    if ($("#mainPageId").val() != "") {
      mainPageId = $("#mainPageId").val();
    }
    requestData.append('mainPageId', mainPageId);
    requestData.append('fashionShowId', $('#fashionShowId').val());
      requestData.append('contentText', $.trim($('#ContentText').val()));
      var IsActive = $('#IsActive').is(":checked")
      requestData.append("isActive", IsActive);

    var response = postRequest(routeURL + '/api/AdminApi/SaveMainPage', requestData);
    if (response.status == 1) {
      clear();
      $('#fashionShowId').prop('selectedIndex', 0);
      $('.selectpicker').selectpicker('refresh');
      msg_success(response.message);
    }
    else {
      msg_failure(response.message);
    }
  }
  else {
    msg_failure("Please fill all require fields")
  }
}

function validation() {
  var isValid = true;

  if (jQuery.trim($("#ContentText").val()) == "") {
    isValid = false;
  }

  if ($('#fashionShowId').val() == null) {
    isValid = false;
  }

  var imgSrc = $('#backgroundImg').attr("src");
  if (imgSrc == "") {
    isValid = false;
  }

  var imgInner = $('#innerImg').attr("src");
  if (imgInner == "") {
    isValid = false;
  }

  var imgLogo = $('#logoImg').attr("src");
  if (imgLogo == "") {
    isValid = false;
  }

  return isValid;
}

function clear() {
  $("#ContentText").val("");
  $("#mainPageId").val("");

  $('#backgroundImg').attr("src", "");
  $('#imgInner').attr("src", "");
  $('#imgLogo').attr("src", "");

  $("#imgUpload1").val('');
  $("#imgUpload2").val('');
    $("#imgUpload3").val('');
    $('#IsActive').prop('checked', true);
}
