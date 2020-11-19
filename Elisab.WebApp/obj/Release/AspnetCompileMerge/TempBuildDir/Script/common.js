function imageUpload(files, uploadElement, imageElement, deleteElement) {
  isValid = true;
  if (files.length > 0) {
    var file = files[0];
    var fileExtension = file.name.split('.').pop().toLowerCase();
    var validExtensions = ['png', 'jpg', 'jpeg'];

    if (file.size / 1024 > 10240) {
      isValid = false;
      msg_failure("Image size should be under 10MB");
    }
    else if ($.inArray(fileExtension, validExtensions) == -1) {
      isValid = false;
      msg_failure("Invalid file type");
    }

    if (isValid == false) {
      $("#" + uploadElement).val('');
      $('#' + imageElement).attr("src", '');
      $("#" + deleteElement).hide();
    }
    else {
      if (imageElement != undefined) {
        var reader = new FileReader();
        var imgtag = document.getElementById(imageElement);
        imgtag.title = file.name;
        reader.onload = function (event) {
          imgtag.src = event.target.result;
        };
        reader.readAsDataURL(file);
        $("#" + deleteElement).show();
      }
    }
  }
  else {
    $("#" + uploadElement).val('');
    $('#' + imageElement).attr("src", '');
    $("#" + deleteElement).hide();
    isValid = false;
  }

  return isValid;
}

function getRequest(url) {
  var response = $.ajax({
    url: url,
    type: 'GET',
    async: false,
  });

  if (response.responseJSON.status == -3) {
    msg_failure(data.message);
    setTimeout(function () {
      window.location = routeURL + '/Home/Login';
    }, 1000);
  }
  else {
    return response.responseJSON;
  }
}

// POST with upload file
function postRequest(url, data) {
  var response = $.ajax({
    url: url,
    type: 'POST',
    data: data,
    dataType: 'json',
    contentType: false,
    processData: false,
    async: false,
    error: function (xhr) {
      msg_failure(result.message);
    },
  });

  if (response.responseJSON.status == -3) {
    msg_failure(data.message);
    setTimeout(function () {
      window.location = routeURL + '/Home/Login';
    }, 1000);
  }
  else {
    return response.responseJSON;
  }
}

function formPostRequest(url, data) {
  var response = $.ajax({
    url: url,
    type: 'POST',
    data: data,
    dataType: 'json',
    async: false,
    error: function (xhr) {
      msg_failure(result.message);
    },
  });

  if (response.responseJSON.status == -3) {
    msg_failure(data.message);
    setTimeout(function () {
      window.location = routeURL + '/Home/Login';
    }, 1000);
  }
  else {
    return response.responseJSON;
  }
}
