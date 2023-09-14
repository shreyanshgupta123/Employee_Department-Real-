// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    // Select All Checkbox
    $('#Select_All').on('click', function () {
        let checkboxes = document.getElementsByTagName('input');
        let val = null;
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox') {
                if (val == null) {
                    val = checkboxes[i].checked;
                }
                else {
                    checkboxes[i].checked = val;
                }
            }
        }
    });
    // Delete Records
    $('#btnDelete').on('click', function () {
        let val = [];
        $(':checkbox:checked').each(function (i) {
            val[i] = $(this).val();
        });

        // Send an AJAX request to delete records
        $.ajax({
            url: '/Employee/DeleteSingleOrMultiple',
            type: 'POST',
            data: { 'ids': val },
            traditional: true,
            success: function (response) {
                if (response == 'success') {
                    location.reload();
                }
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
});
$('#txtSearch').keyup(function () {
    var typeValue = $(this).val();
    $('tbody tr').each(function () {
        if ($(this).text().search(new RegExp(typeValue, "i")) < 0) {
            $(this).fadeOut();
        } else {
            $(this).show();
        }
    });
});
