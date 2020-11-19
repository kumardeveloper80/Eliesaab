function msg_success(msg) {
  $.notify({
    message: msg
  },
    {
      type: 'success',
      placement: {
        align: "center"
      }
    });
}

function msg_failure(msg) {
  $.notify({
    message: msg
  },
    {
      type: 'danger',
      placement: {
        align: "center"
      }
    });
}
