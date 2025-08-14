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

    //Submit Form
    const form = document.getElementById("articleForm");
    const saveBtn = document.getElementById("saveDraftBtn");
    const submitBtn = document.getElementById("submitArticleBtn");

    function validateForm() {
        const title = document.getElementById("title").value.trim();
        const excerpt = document.getElementById("excerpt").value.trim();
        const content = document.getElementById("content").value.trim();
        const category = document.getElementById("category").value;

        if (!title || !excerpt || !content || !category) {
            alert("Please fill in all required fields.");
            return false;
        }
        return true;
    }

    saveBtn.addEventListener("click", function () {
        if (!validateForm()) return;
        document.getElementById("submitAction").value = "saveDraft";
        form.submit(); // normal form submit
    });

    submitBtn.addEventListener("click", function () {
        if (!validateForm()) return;
        document.getElementById("submitAction").value = "submitArticle";
        form.submit(); // normal form submit
    });
    
});






