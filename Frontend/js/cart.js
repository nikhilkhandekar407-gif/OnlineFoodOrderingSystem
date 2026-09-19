/* =========================================================
   cart.js — cart is stored in localStorage as an array of:
   { foodItemId, name, price, imageUrl, quantity }
   ========================================================= */

const CART_KEY = "foodCart";

function getCart() {
  const raw = localStorage.getItem(CART_KEY);
  return raw ? JSON.parse(raw) : [];
}

function saveCart(cart) {
  localStorage.setItem(CART_KEY, JSON.stringify(cart));
  updateCartBadge();
}

function addToCart(item) {
  const cart = getCart();
  const existing = cart.find((c) => c.foodItemId === item.id);

  if (existing) {
    existing.quantity += 1;
  } else {
    cart.push({
      foodItemId: item.id,
      name: item.name,
      price: item.price,
      imageUrl: item.imageUrl,
      quantity: 1,
    });
  }

  saveCart(cart);
  showToast(`${item.name} added to cart`);
}

function updateQuantity(foodItemId, newQty) {
  let cart = getCart();
  if (newQty <= 0) {
    cart = cart.filter((c) => c.foodItemId !== foodItemId);
  } else {
    const item = cart.find((c) => c.foodItemId === foodItemId);
    if (item) item.quantity = newQty;
  }
  saveCart(cart);
  if (typeof renderCartPage === "function") renderCartPage();
}

function removeFromCart(foodItemId) {
  updateQuantity(foodItemId, 0);
}

function clearCart() {
  localStorage.removeItem(CART_KEY);
  updateCartBadge();
}

function getCartTotal() {
  return getCart().reduce((sum, c) => sum + c.price * c.quantity, 0);
}

function getCartCount() {
  return getCart().reduce((sum, c) => sum + c.quantity, 0);
}

function updateCartBadge() {
  const badge = document.getElementById("cartBadge");
  if (!badge) return;
  const count = getCartCount();
  badge.textContent = count;
  badge.classList.toggle("d-none", count === 0);
}

// Simple bootstrap toast for "added to cart" feedback
function showToast(message) {
  let container = document.getElementById("toastContainer");
  if (!container) {
    container = document.createElement("div");
    container.id = "toastContainer";
    container.className = "toast-container position-fixed bottom-0 end-0 p-3";
    container.style.zIndex = 1080;
    document.body.appendChild(container);
  }

  const toastEl = document.createElement("div");
  toastEl.className = "toast align-items-center text-bg-success border-0";
  toastEl.setAttribute("role", "alert");
  toastEl.innerHTML = `
    <div class="d-flex">
      <div class="toast-body">${message}</div>
      <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
    </div>`;
  container.appendChild(toastEl);

  const toast = new bootstrap.Toast(toastEl, { delay: 1800 });
  toast.show();
  toastEl.addEventListener("hidden.bs.toast", () => toastEl.remove());
}
