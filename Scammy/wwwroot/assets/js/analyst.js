// Handle dropdown functionality for the analyst dashboard
document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".dropdown-toggle").forEach(function (toggle) {
        toggle.addEventListener("click", function (e) {
            e.preventDefault();
            const menu = this.nextElementSibling;
            menu.style.display = (menu.style.display === "block") ? "none" : "block";
        });
    });

    // Close dropdown if clicked outside
    document.addEventListener("click", function (e) {
        if (!e.target.closest(".dropdown")) {
            document.querySelectorAll(".dropdown-menu").forEach(function (menu) {
                menu.style.display = "none";
            });
        }
    });
});
