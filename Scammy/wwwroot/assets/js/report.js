// Add this function to handle TempData messages
function checkForMessages() {
    // Check for success message
    const successMsg = document.querySelector('meta[name="success-message"]');
    if (successMsg) {
        showToast(successMsg.getAttribute('content'), 'success');
    }

    // Check for error message
    const errorMsg = document.querySelector('meta[name="error-message"]');
    if (errorMsg) {
        showToast(errorMsg.getAttribute('content'), 'error');
    }
}

// Update the DOMContentLoaded event
document.addEventListener('DOMContentLoaded', function () {
    // Check for messages first
    checkForMessages();

    // Then handle form validation
    const form = document.getElementById('contact');
    if (form) {
        form.addEventListener('submit', function (e) {
            let name = document.querySelector('input[name="Name"]').value.trim();
            let jobTitle = document.querySelector('input[name="JobTitle"]').value.trim();
            let email = document.querySelector('input[name="Email"]').value.trim();
            let description = document.querySelector('textarea[name="Description"]').value.trim();

            if (!name || !jobTitle || !email || !description) {
                e.preventDefault();
                showToast('Please fill in all required fields', 'error');
                return false;
            }
        });
    }
});