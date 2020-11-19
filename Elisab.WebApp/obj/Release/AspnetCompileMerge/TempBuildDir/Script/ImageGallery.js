
var routeURL = location.protocol + '//' + location.host;

$(document).ready(function () {

  $("#imgUpload").change(function (e) {
    imageUpload(this.files, 'imgUpload');
    var fashionId = $('#fashionShowId').val();
    if (fashionId != null && fashionId != 0) {
      if (imageUpload(this.files, 'imgUpload')) {
        var file = this.files[0];
        var requestData = new FormData();
        var imageId = 0;
        if ($("#imageId").val() != "") {
          imageId = $("#imageId").val();
        }
        requestData.append('imageId', imageId);
        requestData.append('fashionShowId', fashionId);
        requestData.append('file', file);

        var response = postRequest(routeURL + '/api/AdminApi/SaveImageGallery', requestData);
        if (response.status == 1) {
          msg_success(response.message);
          var src = '/GalleryImg/' + response.dataenum.ThumbImage;
          var html = "<div style='width:200px; height:200px; float:left; margin:5px 20px 25px 5px; position:relative;'><img style='width:200px; height:200px; margin-top:10px;' src=" + src + " ><div id='deleteImg' class='deleteImage' style='' onclick='RemovegalleryImg()'></div></div>";
          $("#imgDiv").append(html);
        }
        else {
          msg_failure(response.message);
        }
      }
    }
    else {
      msg_failure('Please select fashiow show from list.');
    }
  });
});

function BindImageGallery() {
  $("#imgUpload").val('');
  var response = getRequest(routeURL + '/api/AdminApi/GetImageGalleryByFashionShowId?Id=' + $("#fashionShowId").val());
  if (response.status == 1) {
    $("#imgDiv").html("");
    var html = ""
    for (var i = 0; i < response.dataenum.length; i++) {
      var obj = response.dataenum[i];
      var src = '/GalleryImg/' + obj.ThumbImage;
      html += "<div style='width:200px; height:200px; float:left; margin:5px 20px 25px 5px; position:relative;'><img style='width:200px; height:200px; margin-top:10px;' src=" + src + " ><div id='deleteImg' class='deleteImage' style='' onclick='RemovegalleryImg(" + obj.Id + ")'></div></div>";
    }
    $("#imgDiv").append(html);
  }
  else {
    msg_failure(response.message);
  }
}

function validation() {
  var isValid = true;

  if ($('#fashionShowId').val() == null) {
    isValid = false;
  }

  var imgSrc = $('#Img').attr("src");
  if (imgSrc == "") {
    isValid = false;
  }
  return isValid;
}

function RemovegalleryImg(imageId) {
  var ans = confirm("Are you sure you want to delete the image?");
  if (ans) {
    var response = getRequest(routeURL + '/api/AdminApi/DeleteImage?Id=' + imageId);
    if (response.status == 1) {
      msg_success(response.message);
      BindImageGallery();
    }
    else {
      msg_failure(response.message);
    }
  }
}
