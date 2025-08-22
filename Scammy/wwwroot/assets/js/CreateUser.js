$(document).ready(function () {
    $('#createUserForm').submit(function (e) {
        e.preventDefault();

        var myModal = new bootstrap.Modal(document.getElementById('statusModal'));
        $('#modalIcon').html('<i class="bi bi-arrow-repeat spinner-border text-primary"></i>');
        $('#modalMessage').text('Processing...');
        myModal.show();

        var formData = $(this).serialize();

        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: formData,
            success: function (response) {
                $('#modalIcon').html('<i class="bi bi-check-circle-fill text-success"></i>');
                $('#modalMessage').text(response.message);
                $('#createUserForm')[0].reset();
            },
            error: function (xhr) {
                $('#modalIcon').html('<i class="bi bi-x-circle-fill text-danger"></i>');
                $('#modalMessage').text(xhr.responseText || "Error creating user!");
            }
        });
    });
});
