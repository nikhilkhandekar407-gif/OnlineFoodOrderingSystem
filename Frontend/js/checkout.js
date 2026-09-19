/* =========================================================
   checkout.js — cart.html + checkout.html
   ========================================================= */

document.addEventListener("DOMContentLoaded", () => {
  updateCartBadge();
  refreshAuthNav();

  if (document.getElementById("cartItemsBody")) renderCartPage();
  if (document.getElementById("checkoutForm")) initCheckoutPage();
});

// ---------- cart.html ----------
function renderCartPage() {
  const body = document.getElementById("cartItemsBody");
  const cart = getCart();
  const emptyMsg = document.getElementById("emptyCartMsg");
  const summaryBox = document.getElementById("cartSummaryBox");

  if (cart.length === 0) {
    body.innerHTML = "";
    emptyMsg.classList.remove("d-none");
    summaryBox.classList.add("d-none");
    return;
  }

  emptyMsg.classList.add("d-none");
  summaryBox.classList.remove("d-none");

  body.innerHTML = cart.map((item) => `
    <tr>
      <td class="d-flex align-items-center gap-2">
        <img src="${item.imageUrl || 'images/placeholder.jpg'}" onerror="this.onerror=null;this.src='images/placeholder.jpg'"
             class="cart-thumb" alt="${item.name}">
        <span>${item.name}</span>
      </td>
      <td>₹${item.price.toFixed(2)}</td>
      <td>
        <div class="input-group input-group-sm" style="width:110px">
          <button class="btn btn-outline-secondary" onclick="updateQuantity(${item.foodItemId}, ${item.quantity - 1})">-</button>
          <input type="text" class="form-control text-center" value="${item.quantity}" readonly>
          <button class="btn btn-outline-secondary" onclick="updateQuantity(${item.foodItemId}, ${item.quantity + 1})">+</button>
        </div>
      </td>
      <td>₹${(item.price * item.quantity).toFixed(2)}</td>
      <td><button class="btn btn-sm btn-link text-danger" onclick="removeFromCart(${item.foodItemId})">
        <i class="bi bi-trash"></i> Remove</button></td>
    </tr>
  `).join("");

  document.getElementById("cartTotal").textContent = `₹${getCartTotal().toFixed(2)}`;
}

// ---------- checkout.html ----------
function initCheckoutPage() {
  if (!isLoggedIn()) {
    window.location.href = "login.html?redirect=checkout.html";
    return;
  }

  const cart = getCart();
  if (cart.length === 0) {
    window.location.href = "cart.html";
    return;
  }

  const summaryList = document.getElementById("orderSummaryList");
  summaryList.innerHTML = cart.map((item) => `
    <li class="list-group-item d-flex justify-content-between">
      <span>${item.name} × ${item.quantity}</span>
      <span>₹${(item.price * item.quantity).toFixed(2)}</span>
    </li>
  `).join("");

  const total = getCartTotal();
  const deliveryFee = total > 500 ? 0 : 40;
  document.getElementById("subtotalAmount").textContent = `₹${total.toFixed(2)}`;
  document.getElementById("deliveryFeeAmount").textContent = deliveryFee === 0 ? "FREE" : `₹${deliveryFee.toFixed(2)}`;
  document.getElementById("grandTotalAmount").textContent = `₹${(total + deliveryFee).toFixed(2)}`;

  document.getElementById("checkoutForm").addEventListener("submit", (e) => placeOrder(e, deliveryFee));
}

async function placeOrder(event, deliveryFee) {
  event.preventDefault();
  const errorBox = document.getElementById("checkoutError");
  errorBox.classList.add("d-none");

  const address = document.getElementById("deliveryAddress").value.trim();
  const phone = document.getElementById("contactPhone").value.trim();
  const paymentMethod = document.getElementById("paymentMethod").value;

  const cart = getCart();
  const payload = {
    deliveryAddress: address,
    contactPhone: phone,
    paymentMethod,
    items: cart.map((c) => ({ foodItemId: c.foodItemId, quantity: c.quantity })),
  };

  const submitBtn = event.target.querySelector("button[type=submit]");
  submitBtn.disabled = true;
  submitBtn.textContent = "Placing order...";

  try {
    const order = await Api.createOrder(payload);
    clearCart();
    window.location.href = `order-success.html?orderId=${order.id}`;
  } catch (err) {
    errorBox.textContent = err.message;
    errorBox.classList.remove("d-none");
    submitBtn.disabled = false;
    submitBtn.textContent = "Place Order";
  }
}
