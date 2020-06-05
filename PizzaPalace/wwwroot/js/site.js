// Write your JavaScript code.

$(function () {
    $('.multiple').select2();
    $('#orders').DataTable(); 
});

/* Dialog pop up*/
showInPopup = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#form-modal .modal-body").html(res);
            $("#form-modal .modal-title").html(title);
            $("#form-modal").modal("show");
        }
    })
}

jQueryAjaxPost = form => {
    try {
        $.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                    $.notify('Submitted successfully', { globalPosition: 'top center', className: 'success' })
                    getReceipt(res.html);
                }
                else {
                    $('#form-modal .modal-body').html(res.html);
                }
            },
            error: function (err) {
                console.log(err);
            }
        })
    } catch (ex) {
        console.log(ex);
    }

    //prevent default form submit event
    return false;
}

getReceipt = (res) => {
    setTimeout(() => {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html('Receipt');
            $('#form-modal').modal('show');
    }, 2000);
}

/* Delete record via ajax request*/
jQueryAjaxDelete = form => {
    if (confirm("Are you sure you want to delete this record?")) {
        try {
            $.ajax({
                type: "POST",
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-all').html(res.html);
                    $.notify('Deleted successfully', { globalPosition: 'top center', className: 'success' })
                },
                error: function (err) {
                    console.log(err);
                }
            })
        } catch (ex) {
            console.log(ex);
        }
    }


    //prevent default form submit event
    return false;
}