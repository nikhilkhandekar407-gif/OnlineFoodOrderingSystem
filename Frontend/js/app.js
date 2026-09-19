/* =========================================================
   app.js — home page (index.html): categories + menu grid
   ========================================================= */

let allFoodItems = [];
let activeCategoryId = null;

document.addEventListener("DOMContentLoaded", async () => {
  updateCartBadge();
  refreshAuthNav();

  // Only run menu-loading logic if this page has the menu containers
  if (document.getElementById("categoryList") || document.getElementById("foodGrid")) {
    await loadCategories();
    await loadFoodItems();
  }

  const searchInput = document.getElementById("searchInput");
  if (searchInput) {
    searchInput.addEventListener("input", debounce(onSearch, 300));
  }
});

async function loadCategories() {
  const container = document.getElementById("categoryList");
  if (!container) return;

  try {
    const categories = await Api.getCategories();

    let html = `
      <button class="btn category-pill active" data-id="" onclick="selectCategory(null, this)">
        All
      </button>`;

    categories.forEach((c) => {
      html += `
        <button class="btn category-pill" data-id="${c.id}" onclick="selectCategory(${c.id}, this)">
          ${c.name}
        </button>`;
    });

    container.innerHTML = html;
  } catch (err) {
    container.innerHTML = `<div class="text-danger">Could not load categories: ${err.message}</div>`;
  }
}

function selectCategory(categoryId, btn) {
  activeCategoryId = categoryId;
  document.querySelectorAll(".category-pill").forEach((el) => el.classList.remove("active"));
  btn.classList.add("active");
  loadFoodItems();
}

function onSearch(e) {
  loadFoodItems(e.target.value.trim());
}

async function loadFoodItems(search = "") {
  const grid = document.getElementById("foodGrid");
  if (!grid) return;

  grid.innerHTML = `<div class="text-center py-5 w-100"><div class="spinner-border text-danger"></div></div>`;

  try {
    allFoodItems = await Api.getFoodItems(activeCategoryId, search || null);
    renderFoodGrid(allFoodItems);
  } catch (err) {
    grid.innerHTML = `<div class="alert alert-danger">Could not load menu: ${err.message}. Is the API running at ${API_BASE_URL}?</div>`;
  }
}

function renderFoodGrid(items) {
  const grid = document.getElementById("foodGrid");

  if (items.length === 0) {
    grid.innerHTML = `<p class="text-muted text-center w-100 py-5">No dishes found.</p>`;
    return;
  }

  grid.innerHTML = items.map((item) => `
    <div class="col-6 col-md-4 col-lg-3 mb-4">
      <div class="card food-card h-100">
        <div class="food-img-wrap">
          <img src="${item.imageUrl || 'images/placeholder.jpg'}"
               onerror="this.onerror=null;this.src='images/placeholder.jpg'"
               class="card-img-top" alt="${item.name}">
          <span class="veg-badge ${item.isVeg ? 'veg' : 'nonveg'}"></span>
        </div>
        <div class="card-body d-flex flex-column">
          <h6 class="card-title mb-1">${item.name}</h6>
          <p class="card-text text-muted small flex-grow-1">${item.description || ""}</p>
          <div class="d-flex justify-content-between align-items-center">
            <span class="fw-bold">₹${item.price.toFixed(2)}</span>
            <button class="btn btn-sm btn-danger" ${!item.isAvailable ? "disabled" : ""}
                    onclick='addToCart(${JSON.stringify(item)})'>
              ${item.isAvailable ? "Add +" : "Sold out"}
            </button>
          </div>
        </div>
      </div>
    </div>
  `).join("");
}

function debounce(fn, delay) {
  let timer;
  return (...args) => {
    clearTimeout(timer);
    timer = setTimeout(() => fn(...args), delay);
  };
}
