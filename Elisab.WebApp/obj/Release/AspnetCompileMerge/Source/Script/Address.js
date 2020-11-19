var routeURL = location.protocol + '//' + location.host;

$(document).ready(function () {

  $("#imgUpload").change(function (e) {
    imageUpload(this.files, 'imgUpload', 'backgroundImg', 'deleteImg');
  });
});

function onFashionShowChange() {
  clear();
  var response = getRequest(routeURL + '/api/AdminApi/GetAddressByFashionShowId?Id=' + $("#fashionShowId").val());
  if (response.status == 1) {
    $("#HeaderText").val(response.dataenum.HeaderText);
    $("#addressId").val(response.dataenum.Id);
    $('#backgroundImg').attr("src", '/UploadImg/' + response.dataenum.ImageName);
    $("#deleteImg").show();
  }
}

function onSubmit() {
  if (validation()) {
    var requestData = new FormData();
    var img = document.getElementById('imgUpload');
    if (img.files.length > 0) {
      requestData.append(img.files[0].name, img.files[0]);
    }
    var addressId = 0;
    if ($("#addressId").val() != "") {
      addressId = $("#addressId").val();
    }
    requestData.append('addressId', addressId);
    requestData.append('fashionShowId', $('#fashionShowId').val());
    requestData.append('headerText', $.trim($('#HeaderText').val()));

    var response = postRequest(routeURL + '/api/AdminApi/SaveAddress', requestData);
    if (response.status == 1) {
      clear();
      $('#fashionShowId').prop('selectedIndex', 0);
      $('.selectpicker').selectpicker('refresh');
      msg_success(response.message);
    }
    else {
      msg_failure(result.message);
    }
  }
  else {
    msg_failure("Please fill all require fields")
  }
}

function validation() {
  var isValid = true;

  if (jQuery.trim($("#HeaderText").val()) == "") {
    isValid = false;
  }

  if ($('#fashionShowId').val() == null) {
    isValid = false;
  }

  var imgSrc = $('#backgroundImg').attr("src");
  if (imgSrc == "") {
    isValid = false;
  }
  return isValid;
}

function removeImg() {
  $("#imgUpload").val('');
  $('#backgroundImg').attr("src", '');
  $("#deleteImg").hide();
}

function clear() {
  $("#HeaderText").val("");
  $("#addressId").val("");
  $('#backgroundImg').attr("src", "");
  $("#deleteImg").hide();
  $("#imgUpload").val('');
}
