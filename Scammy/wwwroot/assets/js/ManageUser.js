$(document).ready(function () {

    // Keep current tab after refresh
    var currentTab = localStorage.getItem("activeUserTab") || "#admin";
    $('.nav-link[href="' + currentTab + '"]').tab('show');

    $('.nav-link').on('shown.bs.tab', function (e) {
        var target = $(e.target).attr("href");
        localStorage.setItem("activeUserTab", target);
    });

    // Tab-specific search
    $('.search-input').on('keyup', function () {
        var value = $(this).val().toLowerCase();
        var activeTab = $('.tab-pane.active');
        activeTab.find('tbody tr').filter(function () {
            $(this).toggle(
                $(this).find('td:eq(0)').text().toLowerCase().indexOf(value) > -1 ||
                $(this).find('td:eq(1)').text().toLowerCase().indexOf(value) > -1
            );
        });
    });

    // Toggle user active/inactive
    window.toggleUser = function (userId, activate, btn) {
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
                    statusSpan.text('Active').removeClass('inactive').addClass('active');
                    $(btn).text('Deactivate').removeClass('activate').addClass('deactivate');
                    $(btn).attr('onclick', 'toggleUser(' + userId + ', false, this)');
                } else {
                    statusSpan.text('Inactive').removeClass('active').addClass('inactive');
                    $(btn).text('Activate').removeClass('deactivate').addClass('activate');
                    $(btn).attr('onclick', 'toggleUser(' + userId + ', true, this)');
                }
            },
            error: function (err) {
                alert("Action failed!");
                console.error(err);
            }
        });
    };

    // Header scroll/reset
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
