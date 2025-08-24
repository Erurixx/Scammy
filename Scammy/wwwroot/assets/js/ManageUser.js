$(document).ready(function () {

    // Keep current tab after refresh
    var currentTab = localStorage.getItem("activeUserTab") || "#admin";
    $('.nav-link[href="' + currentTab + '"]').tab('show');

    $('.nav-link').on('shown.bs.tab', function (e) {
        var target = $(e.target).attr("href");
        localStorage.setItem("activeUserTab", target);

        // change tab clear
        $('#searchInput').val('');
        var activeTab = $(target);
        activeTab.find('tbody tr').not('.empty-state').show();
        activeTab.find('.empty-state').remove();
    });

    // Tab-specific search
    $('.search-input').on('keyup', function () {
        var value = $(this).val().toLowerCase();
        var activeTab = $('.tab-pane.active');
        var rows = activeTab.find('tbody tr').not('.empty-state'); 
        var visibleCount = 0;

        rows.each(function () {
            var match = $(this).find('td:eq(0)').text().toLowerCase().indexOf(value) > -1 ||
                $(this).find('td:eq(1)').text().toLowerCase().indexOf(value) > -1;
            $(this).toggle(match);
            if (match) visibleCount++;
        });

        var emptyState = activeTab.find('.empty-state');
        if (visibleCount === 0 && emptyState.length === 0) {
            activeTab.find('tbody').append(
                `<tr class="empty-state"><td colspan="6" class="text-center text-muted fst-italic">No users found</td></tr>`
            );
        } else if (visibleCount > 0 && emptyState.length > 0) {
            emptyState.remove();
        }
    });

    // Reset 
    $('#resetFilters').on('click', function () {
        $('#searchInput').val('');
        var activeTab = $('.tab-pane.active');
        var rows = activeTab.find('tbody tr').not('.empty-state');
        rows.show();
        activeTab.find('.empty-state').remove();
    });

    // Toggle user active/inactive with confirmation popup
    window.toggleUser = function (userId, activate, btn) {
        const actionText = activate ? "Activate" : "Deactivate";

        Swal.fire({
            title: `${actionText} User?`,
            text: `Are you sure you want to ${actionText.toLowerCase()} this user?`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: actionText,
            cancelButtonText: 'Cancel',
            reverseButtons: true,
            focusCancel: true
        }).then((result) => {
            if (result.isConfirmed) {
                let url = activate ? '/Admin/ActivateUser' : '/Admin/DeactivateUser';

                $.ajax({
                    url: url,
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({ id: userId }),
                    success: function (res) {
                        var row = $(btn).closest('tr');
                        var statusSpan = row.find('.status');

                        if (activate) {
                            statusSpan.text('Active').removeClass('deactivate').addClass('active');
                            $(btn).text('Deactivate').removeClass('activate').addClass('deactivate');
                            $(btn).attr('onclick', 'toggleUser(' + userId + ', false, this)');
                        } else {
                            statusSpan.text('Deactivate').removeClass('active').addClass('deactivate');
                            $(btn).text('Activate').removeClass('deactivate').addClass('activate');
                            $(btn).attr('onclick', 'toggleUser(' + userId + ', true, this)');
                        }

                        Swal.fire({
                            icon: 'success',
                            title: 'Success',
                            text: `User has been ${activate ? 'activated' : 'deactivated'} successfully!`,
                            timer: 2000,
                            showConfirmButton: false
                        });
                    },
                    error: function (err) {
                        Swal.fire('Error', 'Action failed! Please try again.', 'error');
                        console.error(err);
                    }
                });
            }
        });
    };

    // Header reset
    let lastScrollTop = 0;
    let headerFixed = false;

    function resetHeader() {
        $('.header-area, .background-header').removeAttr('style');
        $('.header-area .main-nav').removeAttr('style');
        $('.header-area .main-nav .nav').removeAttr('style');
        $('.header-area .main-nav .nav li').removeAttr('style');
        $('.dropdown-menu, .submenu ul, .dropdown, .submenu').removeAttr('style');
    }

    $(window).scroll(function () {
        let scrollTop = $(this).scrollTop();
        if (scrollTop < 200 && !headerFixed) {
            resetHeader();
        } else if (scrollTop > 200) {
            headerFixed = false;
        }
        lastScrollTop = scrollTop;
    });

    resetHeader();
    $(document).on('click', function () {
        if (!headerFixed) setTimeout(resetHeader, 100);
    });

    setInterval(function () {
        if ($(window).scrollTop() < 200) resetHeader();
    }, 5000);
});
