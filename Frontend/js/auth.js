/* =========================================================
   auth.js — login / registration / session helpers
   ========================================================= */

function saveSession(authResponse) {
  localStorage.setItem("authToken", authResponse.token);
  localStorage.setItem("customerId", authResponse.customerId);
  localStorage.setItem("customerName", authResponse.fullName);
}

function clearSession() {
  localStorage.removeItem("authToken");
  localStorage.removeItem("customerId");
  localStorage.removeItem("customerName");
}

function isLoggedIn() {
  return !!localStorage.getItem("authToken");
}

function getCustomerName() {
  return localStorage.getItem("customerName");
}

function logout() {
  clearSession();
  window.location.href = "index.html";
}

// Handles the login form on login.html
async function handleLoginForm(event) {
  event.preventDefault();
  const errorBox = document.getElementById("authError");
  errorBox.classList.add("d-none");

  const email = document.getElementById("email").value.trim();
  const password = document.getElementById("password").value;

  try {
    const result = await Api.login({ email, password });
    saveSession(result);
    window.location.href = "index.html";
  } catch (err) {
    errorBox.textContent = err.message;
    errorBox.classList.remove("d-none");
  }
}

// Handles the registration form on register.html
async function handleRegisterForm(event) {
  event.preventDefault();
  const errorBox = document.getElementById("authError");
  errorBox.classList.add("d-none");

  const fullName = document.getElementById("fullName").value.trim();
  const email = document.getElementById("email").value.trim();
  const password = document.getElementById("password").value;
  const phone = document.getElementById("phone").value.trim();
  const address = document.getElementById("address").value.trim();

  try {
    const result = await Api.register({ fullName, email, password, phone, address });
    saveSession(result);
    window.location.href = "index.html";
  } catch (err) {
    errorBox.textContent = err.message;
    errorBox.classList.remove("d-none");
  }
}

// Updates the navbar to reflect logged in / logged out state.
// Called on every page from app.js's DOMContentLoaded handler.
function refreshAuthNav() {
  const authLink = document.getElementById("authNavLink");
  if (!authLink) return;

  if (isLoggedIn()) {
    authLink.innerHTML = `<i class="bi bi-person-circle"></i> ${getCustomerName()} (Logout)`;
    authLink.href = "#";
    authLink.onclick = (e) => { e.preventDefault(); logout(); };
  } else {
    authLink.innerHTML = `<i class="bi bi-person"></i> Login`;
    authLink.href = "login.html";
    authLink.onclick = null;
  }
}
