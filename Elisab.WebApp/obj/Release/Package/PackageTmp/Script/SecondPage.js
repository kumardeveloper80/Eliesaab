var routeURL = location.protocol + '//' + location.host;
var count = 0;

function AddContent() {
  if (count <= 4) {
    count++;
    showContent(count);
  }
  if (count >= 1) {
    $("#removeContent").show();
  }
  if (count == 4) {
    $("#addContent").hide();
    $("#removeContent").show();
  }
}

function RemoveContent() {
  if (count >= 1) {
    hideContent(count);
    count--;
  }
  if (count >= 1) {
    $("#addContent").show();
    $("#removeContent").show();
  }
  if (count < 1) {
    $("#removeContent").hide();
  }
}

function showContent(count) {
  $("#div_" + count).show();
}

function hideContent(count) {
  $('#backGroundImage' + (count + 1)).attr("src", '');
  $('#imageUpload' + (count + 1)).val('');
  CKEDITOR.instances["HtmlContent" + (count + 1)].setData('');
  $("#div_" + count).hide();
}

function onFashionShowChange() {

  for (var i = 0; i <= 4; i++) {
    $("#div_" + i).hide();
  }

  clear();
  var response = getRequest(routeURL + '/api/AdminApi/GetSecondPageByFashionShowId?Id=' + $("#fashionShowId").val());
    if (response.status == 1) {
    count = 0;
      $("#secondPageId").val(response.dataenum.Id);
        $('#IsActive').prop('checked', response.dataenum.IsActive);

        if (response.dataenum.IsActive) {
            $("#IsShowAtLastDiv").removeClass("d-none")
            if (response.dataenum.IsShowAtLast) {
                document.getElementById("last").checked = true;
            } else {
                document.getElementById("second").checked = true;
            }
        } else {
            $("#IsShowAtLastDiv").addClass("d-none")
        } 

    for (var i = 0; i <= 4; i++) {
      if (response.dataenum["HtmlContent" + (i + 1)] != null) {
        count = i;
        $("#div_" + i).show();
        CKEDITOR.instances["HtmlContent" + (i + 1)].setData(response.dataenum["HtmlContent" + (i + 1)]);
        $('#backGroundImage' + (i + 1)).attr("src", '/UploadImg/' + response.dataenum["Image" + (i + 1)]);
      }
    }
    AddContent();
    RemoveContent();
  }
  else if (response.status == 0) {
    clear();
    $("#div_0").show();
    CKEDITOR.instances["HtmlContent1"].setData('');
  }
  else {
    msg_failure(response.message);
  }
}

function onUpolad(id) {
  var files = $("#imageUpload" + id)[0].files;
  imageUpload(files, 'imageUpload' + id, 'backGroundImage' + id);
}

function onSubmit() {
    var selectedRadiobtn = $("input[type='radio'][name='IsShowAtLast']:checked").val();
    if (validation()) {
       
    var requestData = new FormData();
    var secondPageId = 0;
    if ($("#secondPageId").val() != "") {
      secondPageId = $("#secondPageId").val();
    }
    requestData.append('secondPageId', secondPageId);
      requestData.append('fashionShowId', $('#fashionShowId').val());
      var IsActive = $('#IsActive').is(":checked")
        requestData.append("isActive", IsActive);
        requestData.append("isShowAtLast", selectedRadiobtn === "1" ? true : false);

    for (var i = 0; i <= 4; i++) {
      if ($("#div_" + i).is(":visible")) {
        var img = document.getElementById('imageUpload' + (i + 1))
        if (img.files.length > 0) {
          requestData.append("div_" + i, img.files[0]);
        }
        var text = CKEDITOR.instances["HtmlContent" + (i + 1)].getData();
        requestData.append("HtmlContent" + (i + 1), text);
      }
    }

    var response = postRequest(routeURL + '/api/AdminApi/SaveSecondPage', requestData)
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
  var isValid = [true, true, true, true, true];
  var isValidfShowId = true;

  if ($('#fashionShowId').val() == null) {
    isValidfShowId = false;
  }


  for (var i = 0; i <= 4; i++) {
    if ($("#div_" + i).is(":visible")) {
      var text = CKEDITOR.instances["HtmlContent" + (i + 1)].getData();
      if (text == "") {
        isValid[i] = false;
      }

      var imgSrc = $('#backGroundImage' + (i + 1)).attr("src");
      if (imgSrc == "") {
        isValid[i] = false;
      }
    }
  }

  if (isValidfShowId && isValid.filter(x => x == false).length == 0) {
    return true;
  }
  else {
    return false;
  }
}

function clear() {
    count = 0;
    $("#addContent").show();
    $("#removeContent").hide();
    $('#IsActive').prop('checked', true);
    $("#IsShowAtLastDiv").removeClass("d-none")
    document.getElementById("second").checked = true;
  $("#secondPageId").val('');
  for (var i = 0; i <= 4; i++) {
    if ($("#div_" + i).is(":visible")) {
      CKEDITOR.instances["HtmlContent" + (i + 1)].setData('');
      $('#backGroundImage' + (i + 1)).attr("src", "");
      $("#imageUpload" + (i + 1)).val('')

      if (i > 0) {
        $("#div_" + i).hide();
      }
    }
  }
}

function onIsActiveChange() {
    var IsActive = $('#IsActive').is(":checked")
    if (IsActive) {
        $("#IsShowAtLastDiv").removeClass("d-none")
    } else {
        $("#IsShowAtLastDiv").addClass("d-none")
    }
}
