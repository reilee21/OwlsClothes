

$("#accountForm").validate({
    rules: {
        FullName: {
            required: true
        },

        UserName: {
            required: true
        },
        Email: {
            required: true,
            email: true
        },
        Password: {
            required: true,
            minlength: 6
        }
    },
    messages: {
        FullName: {
            required: ""
        },

        UserName: {
            required: ""
        },
        Email: {
            required: "",
            email: "Email không hợp lệ."
        },
        Password: {
            required: "",
            minlength: "Mật khẩu phải ít nhất 6 ký tự."
        },

    },
    errorClass: "invalid-feedback",
    errorElement: "div",
    highlight: function (element) {
        $(element).addClass("is-invalid");
    },
    unhighlight: function (element) {
        $(element).removeClass("is-invalid");
    }
});


$("#accountForm").on("submit", function (event) {
    event.preventDefault();
    if ($(this).valid()) {
        var formdata = $(this).serialize();
        $.ajax({
            url: url,
            type: 'post',
            data: formdata,
            success: function (rs) {
                $('#accountForm').modal('hide');
                location.reload();
            },
            error: function (xhr, status, error) {
                console.log(error);
            }
        })
    }
});
