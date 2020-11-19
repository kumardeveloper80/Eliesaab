var routeURL = location.protocol + '//' + location.host;

$(document).ready(function () {
  $("#headerLogoUpload").change(function (e) {
    imageUpload(this.files, 'headerLogoUpload', 'headerLogo');
  });

  $("#mainBgImgUpload").change(function (e) {
    imageUpload(this.files, 'mainBgImgUpload', 'mainBgImg');
  });

  $("#innerImgUpload").change(function (e) {
    imageUpload(this.files, 'innerImgUpload', 'innerImg');
  });

  $("#footerBgImgUpload").change(function (e) {
    imageUpload(this.files, 'footerBgImgUpload', 'footerBgImg');
  });
});

function onFashionShowChange() {
  clear();
  var response = getRequest(routeURL + '/api/AdminApi/GetCountDownPage?Id=' + $("#fashionShowId").val());
  if (response.dataenum.Id > 0) {
    CKEDITOR.instances.MainContent.setData(response.dataenum.MainContent);
    $("#countDownPageId").val(response.dataenum.Id);

    $('#headerLogo').attr("src", '/UploadImg/' + response.dataenum.HeaderLogo);
    $('#mainBgImg').attr("src", '/UploadImg/' + response.dataenum.MainBgImg);
    $('#innerImg').attr("src", '/UploadImg/' + response.dataenum.MainInnerImg);
    $('#footerBgImg').attr("src", '/UploadImg/' + response.dataenum.FooterBgImg);
  }
  $("#evenDateSpan").html(response.dataenum.ShowDate);
  $("#evenDateDiv").removeClass('hide');
}

function onSubmit() {
  if (validation()) {
    var requestData = new FormData();
    var headerLogo = document.getElementById('headerLogoUpload');
    var mainBgImg = document.getElementById('mainBgImgUpload');
    var innerImg = document.getElementById('innerImgUpload');
    var footerBgImg = document.getElementById('footerBgImgUpload');

    if (headerLogo.files.length > 0) {
      requestData.append("headerLogo", headerLogo.files[0]);
    }
    if (mainBgImg.files.length > 0) {
      requestData.append("mainBgImg", mainBgImg.files[0]);
    }
    if (innerImg.files.length > 0) {
      requestData.append("innerImg", innerImg.files[0]);
    }
    if (footerBgImg.files.length > 0) {
      requestData.append("footerBgImg", footerBgImg.files[0]);
    }
    var countDownPageId = 0;

    if ($("#countDownPageId").val() != "") {
      countDownPageId = $("#countDownPageId").val();
    }
    requestData.append('countDownPageId', countDownPageId);
    requestData.append('fashionShowId', $('#fashionShowId').val());

    var mainContent = CKEDITOR.instances["MainContent"].getData();
    requestData.append("mainContent", mainContent);

    var response = postRequest(routeURL + '/api/AdminApi/SaveCountDownPage', requestData);
    if (response.status == 1) {
      clear();
      CKEDITOR.instances.MainContent.setData('');
      $('#fashionShowId').prop('selectedIndex', 0);
      $('.selectpicker').selectpicker('refresh');
      $('#evenDateDiv').addClass('hide');
      $('#evenDateSpan').html('');
      msg_success(response.message);
    }
    else {
      msg_failure(response.message);
    }
  }
  else {
    msg_failure("Please fill all require fields");
  }
}

function validation() {
  var isValid = true;

  if ($('#fashionShowId').val() == null) {
    isValid = false;
  }

  if ($('#headerLogo').attr("src") == "") {
    isValid = false;
  }

  if ($('#mainBgImg').attr("src") == "") {
    isValid = false;
  }

  if ($('#innerImg').attr("src") == "") {
    isValid = false;
  }

  if ($('#footerBgImg').attr("src") == "") {
    isValid = false;
  }

  var mainContent = CKEDITOR.instances["MainContent"].getData();
  if (mainContent == "") {
    isValid = false;
  }

  return isValid;
}

function clear() {
  $("#HeaderText").val("");
  $("#countDownPageId").val("");

  $('#headerLogo').attr("src", "");
  $('#mainBgImg').attr("src", "");
  $('#innerImg').attr("src", "");
  $('#footerBgImg').attr("src", "");

  $("#headerLogoUpload").val('');
  $("#mainBgImgUpload").val('');
  $("#innerImgUpload").val('');
  $("#footerBgImgUpload").val('');
}
