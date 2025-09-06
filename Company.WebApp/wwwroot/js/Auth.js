// File: Company.WebApp/wwwroot/js/Auth.js
function toggleAuthPanel(showSignUp) {
    const container = document.getElementById('container');
    if (container) {
        if (showSignUp) {
            container.classList.add("right-panel-active");
        } else {
            container.classList.remove("right-panel-active");
        }
    }
}
