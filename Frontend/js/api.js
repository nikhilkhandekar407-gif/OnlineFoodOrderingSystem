/* =========================================================
   api.js — central place for talking to the ASP.NET Core API
   Change API_BASE_URL if your backend runs on a different port.
   ========================================================= */

const API_BASE_URL = "https://onlinefoodorderingsystem-3onp.onrender.com/api";

/**
 * Generic fetch wrapper.
 * Automatically attaches the JWT token (if the user is logged in)
 * and parses JSON responses / throws readable errors.
 */
async function apiRequest(endpoint, method = "GET", body = null, requiresAuth = false) {
  const headers = { "Content-Type": "application/json" };

  if (requiresAuth) {
    const token = localStorage.getItem("authToken");
    if (!token) {
      window.location.href = "login.html";
      throw new Error("Not authenticated");
    }
    headers["Authorization"] = `Bearer ${token}`;
  }

  const options = { method, headers };
  if (body) options.body = JSON.stringify(body);

  const response = await fetch(`${API_BASE_URL}${endpoint}`, options);

  // No content
  if (response.status === 204) return null;

  let data = null;
  try {
    data = await response.json();
  } catch {
    data = null;
  }

  if (!response.ok) {
    const message = (data && data.message) || `Request failed (${response.status})`;
    throw new Error(message);
  }

  return data;
}

const Api = {
  // Categories
  getCategories: () => apiRequest("/categories"),

  // Food items
  getFoodItems: (categoryId = null, search = null) => {
    const params = new URLSearchParams();
    if (categoryId) params.append("categoryId", categoryId);
    if (search) params.append("search", search);
    const qs = params.toString();
    return apiRequest(`/fooditems${qs ? "?" + qs : ""}`);
  },
  getFoodItem: (id) => apiRequest(`/fooditems/${id}`),

  // Auth
  register: (payload) => apiRequest("/auth/register", "POST", payload),
  login: (payload) => apiRequest("/auth/login", "POST", payload),

  // Orders (require auth)
  createOrder: (payload) => apiRequest("/orders", "POST", payload, true),
  getMyOrders: () => apiRequest("/orders/my", "GET", null, true),
  getOrder: (id) => apiRequest(`/orders/${id}`, "GET", null, true),
};
