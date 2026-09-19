/* =========================================================
   orders.js — orders.html (order history) and order-success.html
   ========================================================= */

document.addEventListener("DOMContentLoaded", async () => {
  updateCartBadge();
  refreshAuthNav();

  if (document.getElementById("ordersList")) {
    if (!isLoggedIn()) {
      window.location.href = "login.html?redirect=orders.html";
      return;
    }
    await loadMyOrders();
  }

  if (document.getElementById("successOrderId")) {
    const params = new URLSearchParams(window.location.search);
    document.getElementById("successOrderId").textContent = params.get("orderId") || "";
  }
});

async function loadMyOrders() {
  const container = document.getElementById("ordersList");
  try {
    const orders = await Api.getMyOrders();

    if (orders.length === 0) {
      container.innerHTML = `<p class="text-muted">You haven't placed any orders yet.</p>`;
      return;
    }

    container.innerHTML = orders.map((o) => `
      <div class="card mb-3">
        <div class="card-header d-flex justify-content-between align-items-center">
          <span>Order #${o.id} — ${new Date(o.orderDate).toLocaleString()}</span>
          <span class="badge bg-${statusColor(o.status)}">${o.status}</span>
        </div>
        <div class="card-body">
          <ul class="list-group list-group-flush mb-2">
            ${o.items.map((i) => `
              <li class="list-group-item d-flex justify-content-between">
                <span>${i.foodItemName} × ${i.quantity}</span>
                <span>₹${i.lineTotal.toFixed(2)}</span>
              </li>`).join("")}
          </ul>
          <div class="d-flex justify-content-between fw-bold">
            <span>Total</span><span>₹${o.totalAmount.toFixed(2)}</span>
          </div>
          <p class="text-muted small mt-2 mb-0">Delivering to: ${o.deliveryAddress}</p>
        </div>
      </div>
    `).join("");
  } catch (err) {
    container.innerHTML = `<div class="alert alert-danger">${err.message}</div>`;
  }
}

function statusColor(status) {
  switch (status) {
    case "Pending": return "secondary";
    case "Confirmed": return "info";
    case "Preparing": return "warning";
    case "OutForDelivery": return "primary";
    case "Delivered": return "success";
    case "Cancelled": return "danger";
    default: return "secondary";
  }
}
