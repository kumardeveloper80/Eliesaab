var routeURL = location.protocol + '//' + location.host;
var fashionShowId = 0;
var sectionId = 0;
var sectionMediaId = 0;
var multiImages = [];
var oldImages = [];
var isEdit = false;

var ImagesType = "Image";
var ImageTxtType = "Image And Text";
var VideoType = "Video";
var TextType = "Text";

$(document).ready(function () {

  $("#btnHideModal").click(function () {
    $("#sectionModel").modal('hide');
  });

  $("#imgTxtUpload").change(function (e) {
    imageUpload(this.files, 'imgTxtUpload', 'sectionImg');
  });

  $("#txtImgUpload").change(function (e) {
    imageUpload(this.files, 'txtImgUpload', 'sectionTxtImg');
  });

  $("#videoUpload").change(function (e) {
    var file;
    var isValid = true;
    if ((file = this.files[0])) {

      if (file.size / 1024 > 40960) {
        isValid = false;
        msg_failure("Video size should be under 40MB");
        $("#videoUpload").val('');
        $('#sectionVideo').attr("src", '');
        return false;
      }

      var fileExtension = file.name.split('.').pop().toLowerCase();
      var validExtensions = ['avi', 'flv', 'mp4', '3gp', 'wmv'];
      if ($.inArray(fileExtension, validExtensions) == -1) {
        isValid = false;
        $("#videoUpload").val('');
        $('#sectionVideo').attr("src", '');
        msg_failure("Invalid file type");
        return false;
      }
      var video = $('#divVideo #sectionVideo')[0];
      video.src = URL.createObjectURL(this.files[0]);
      video.load();
      video.play();
    }
  });

  $("#imgsUpload").change(function (e) {
    $("#multiImgDiv").html('');
    $("#multiImgDiv").hide();
    $("#image-listDiv").show();
    multiImages = [];
    var file = this.files.length;
    if (file > 0) {
      var validExtensions = ['png', 'jpg', 'jpeg'];
      for (i = 0; i < file; i++) {
        var fileExtension = this.files[i].name.split('.').pop().toLowerCase();
        if ($.inArray(fileExtension, validExtensions) == -1) {
          $("#imgsUpload").val('');
          $("#multiImgDiv").html('');
          $("#multiImgDiv").hide();

          msg_failure("Invalid file type");
          multiImages = [];
          break;
        }
        else {
          multiImages.push(this.files[i]);
          var reader = new FileReader();
          reader.onload = function (event) {
            $($.parseHTML('<img class="h-150 mr-10 mb-10" width="150">')).attr('src', event.target.result).appendTo("#multiImgDiv");
          }
          reader.readAsDataURL(this.files[i]);
        }
        $("#multiImgDiv").show();
      }
    }

  });

  $("#posterUpload").change(function (e) {
    imageUpload(this.files, 'posterUpload', 'posterImg');
  });
});

function fnCloseModel() {
  clear();
  $("#sectionModel").modal('hide');
}

function onFashionShowChange() {
  fashionShowId = $("#fashionShowId").val();

  var response = getRequest(routeURL + '/api/AdminApi/GetSectionList?Id=' + fashionShowId);
  if (response.status == 1) {
    $("#sectionListDiv").html(response.dataenum);
  }
  else if (response.status == 0) {
  }
  else {
    msg_failure(response.message);
  }
}

function onAddSection() {

  if ($("#fashionShowId").val() != null) {
    $(".modal-title").html("Add Section");
    fashionShowId = $("#fashionShowId").val();
    $("#sectionModel").modal('show');

    $("#div_video").show();
    $("#div_Txt").hide();
    $("#div_image").hide();
    $("#div_imgTxt").hide();
    $("#mediaType").val('Video');

    var video = $('#divVideo #sectionVideo')[0];
    video.src = '';

    $('.selectpicker').selectpicker('refresh');
    sectionId = 0;
    sectionMediaId = 0;
    isEdit = false;
  }
  else {
    msg_failure("Please select fashion show name");
  }

}

function onMediaTypeChange() {
  if ($("#mediaType").val() == VideoType) {
    $("#div_video").show();
    $("#div_image").hide();
    $("#div_imgTxt").hide();
    $("#div_Txt").hide();
  }
  else if ($("#mediaType").val() == ImageTxtType) {
    $("#div_imgTxt").show();
    $("#div_video").hide();
    $("#div_image").hide();
    $("#div_Txt").hide();
  }
  else if ($("#mediaType").val() == ImagesType) {
    $("#div_image").show();
    $("#div_video").hide();
    $("#div_imgTxt").hide();
    $("#div_Txt").hide();
  }
  else if ($("#mediaType").val() == TextType) {
    $("#div_image").hide();
    $("#div_video").hide();
    $("#div_imgTxt").hide();
    $("#div_Txt").show();
  }
}

function onDelete(Id) {
  var response = getRequest(routeURL + '/api/AdminApi/DeleteSection?Id=' + Id);
  if (response.status == 1) {
    msg_success(response.message);
    onFashionShowChange();
  }
  else {
    msg_failure(response.message)
  }
}

function onEdit(Id) {
  fashionShowId = $("#fashionShowId").val();
  $(".modal-title").html("Edit Section");
  oldImages = [];

  var response = getRequest(routeURL + '/api/AdminApi/GetSectionById?Id=' + Id);
  if (response.status == 1) {
    isEdit = true;
    $("#sectionModel").modal('show');
    $("#mediaType").val(response.dataenum.MediaType);
    $('.selectpicker').selectpicker('refresh');
    onMediaTypeChange();
    sectionId = response.dataenum.Id;
    sectionMediaId = response.dataenum.sectionMedia[0].Id;

    if (response.dataenum.MediaType == ImageTxtType) {
      $('#Description').val(response.dataenum.Description);
      $('#sectionImg').attr("src", '/UploadImg/' + response.dataenum.sectionMedia[0].MediaName);
    }
    else if (response.dataenum.MediaType == TextType) {
      CKEDITOR.instances["multiText"].setData(response.dataenum.Description);
      $('#sectionTxtImg').attr("src", '/UploadImg/' + response.dataenum.sectionMedia[0].MediaName);
    }
    else if (response.dataenum.MediaType == VideoType) {
      var video = $('#divVideo #sectionVideo')[0];
      video.src = '/UploadImg/' + response.dataenum.sectionMedia[0].MediaName;
      video.load();
      video.play();

      $('#posterImg').attr("src", '/UploadImg/' + response.dataenum.sectionMedia[0].PosterImageName);
    }
    else if (response.dataenum.MediaType == ImagesType) {
      for (var i = 0; i < response.dataenum.sectionMedia.length; i++) {

        oldImages.push(response.dataenum.sectionMedia[i].MediaName);
        htmlString = "<div id=old_img" + i + " class='fl-left'>";
        htmlString += "<img class='h-150 mr-10 mb-10' width='150' src=/UploadImg/" + response.dataenum.sectionMedia[i].MediaName + ">"
        htmlString += '<div id="deleteImg" class="deleteImage" onclick="removeOldImg(' + i + ')"></div>';
        htmlString += "</div>"
        $("#oldImgDiv").append(htmlString);
      }
      $("#image-listDiv").show();
    }
  }
  else {
    msg_failure(response.message);
  }

}

function onSubmit() {
  if (validation()) {
    var requestData = new FormData();
    if ($("#sectionId").val() != "") {
      sectionId = $("#sectionId").val();
    }
    requestData.append('fashionShowId', fashionShowId);
    requestData.append('sectionId', sectionId);
    requestData.append('sectionMediaId', sectionMediaId);
    requestData.append('mediaType', $('#mediaType').val());

    if ($("#mediaType").val() == VideoType) {
      var video = document.getElementById('videoUpload');
      if (video.files.length > 0) {
        requestData.append("video", video.files[0]);
        requestData.append("isVideoChange", true);
      }
      else {
        requestData.append("isVideoChange", false);
      }

      var img = document.getElementById('posterUpload');
      if (img.files.length > 0) {
        requestData.append("posterImg", img.files[0]);
        requestData.append("isPosterImgChange", true);
      }
      else {
        requestData.append("isPosterImgChange", false);
      }
    }
    else if ($("#mediaType").val() == ImageTxtType) {
      requestData.append('description', $.trim($('#Description').val()));
      var img = document.getElementById('imgTxtUpload');
      if (img.files.length > 0) {
        requestData.append("singleImage", img.files[0]);
        requestData.append("isSingleImgChange", true);
      } else {
        requestData.append("isSingleImgChange", false);
      }
    }
    else if ($("#mediaType").val() == TextType) {
      requestData.append('description', CKEDITOR.instances["multiText"].getData());
      var img = document.getElementById('txtImgUpload');
      if (img.files.length > 0) {
        requestData.append("singleImage", img.files[0]);
        requestData.append("isSingleImgChange", true);
      }
      else {
        requestData.append("isSingleImgChange", false);
      }
    }
    else if ($("#mediaType").val() == ImagesType) {
      var imgs = document.getElementById('imgsUpload');
      if (multiImages.length > 0) {
        for (var i = 0; i < multiImages.length; i++) {
          requestData.append("multiImg", multiImages[i]);
        }
      }
      requestData.append('oldImages', oldImages);

    }

    var response = postRequest(routeURL + '/api/AdminApi/SaveSection', requestData);
    if (response.status == 1) {
      $("#sectionModel").modal('hide');
      onFashionShowChange();
      msg_success(response.message);
      clear();
    }
    else {
      msg_failure(response.message);
    }
  }
  else {
    msg_failure("Please fill all require fileds");
  }
}

function validation() {
  var isValid = true;
  if ($("#mediaType").val() == VideoType) {
    var videoSrc = $('#sectionVideo').attr('src');
    if (videoSrc == "" || videoSrc == undefined) {
      isValid = false;
    }
    var imgSrc = $('#posterImg').attr("src");
    if (imgSrc == "") {
      isValid = false;
    }
  }
  else if ($("#mediaType").val() == TextType) {
    var text = CKEDITOR.instances["multiText"].getData();
    if (text == "") {
      isValid = false;
    }

    var imgSrc = $('#sectionTxtImg').attr("src");
    if (imgSrc == "") {
      isValid = false;
    }
  }
  else if ($("#mediaType").val() == ImageTxtType) {
    if (jQuery.trim($("#Description").val()) == "") {
      isValid = false;
    }

    var imgSrc = $('#sectionImg').attr("src");
    if (imgSrc == "") {
      isValid = false;
    }
  }
  else if ($("#mediaType").val() == ImagesType) {
    if (isEdit == false) {
      if (multiImages.length == 0) {
        isValid = false;
      }
    }
    else {
      var oldCount = oldImages.filter(x => x == null).length;
      if (multiImages.length == 0 && oldCount == oldImages.length) {
        isValid = false;
      }
    }
  }
  return isValid;
}

function clear() {
  sectionId = 0;
  sectionMediaId = 0;
  multiImages = [];
  isEdit = false;
  $('#sectionVideo').attr('src', '');
  $('#sectionImg').attr('src', '');
  $('#sectionTxtImg').attr('src', '');
  $('#posterImg').attr('src', '');

  $("#Description").val('');
  CKEDITOR.instances["multiText"].setData('');

  $("#multiImgDiv").html('');
  $("#multiImgDiv").hide();

  $("#videoUpload").val('');
  $("#imgTxtUpload").val('');
  $("#imgsUpload").val('');
  $("#txtImgUpload").val('');

  $("#oldImgDiv").html('');
  $("#image-listDiv").hide();
}

function removeOldImg(index) {
  $("#old_img" + index).remove();
  if (index > -1) {
    oldImages[index] = null;
  }
}

function updateSequence(data) {
  var SequenceList = new Object();
  SequenceList.sequences = data;
  var response = formPostRequest(routeURL + '/api/AdminApi/UpdateSectionSequence', SequenceList);
  if (response.status == 1) {
    onFashionShowChange();
  }
  else {
    msg_failure(response.message);
  }
}

