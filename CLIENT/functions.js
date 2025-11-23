const API_BASE = "http://localhost:5011/api";

// ---------------------- Movies / Wish List ----------------------

// רינדור רשימת סרטים
function renderMoviesList(moviesArray, containerId, showAddButton) {
  const container = document.getElementById(containerId);
  if (!container) return;

  container.innerHTML = "";

  moviesArray.forEach((movie) => {
    const card = document.createElement("div");
    card.className = "movie-card";

    const title = document.createElement("h3");
    title.textContent = `${movie.title} (${movie.releaseYear})`;

    const img = document.createElement("img");
    img.src = movie.photoUrl || "";
    img.alt = movie.title;
    img.onerror = () => (img.style.display = "none");

    const details = document.createElement("p");
    details.textContent = `Rating: ${movie.rating} | Duration: ${movie.duration} min | Genre: ${movie.genre}`;

    const desc = document.createElement("p");
    desc.textContent = movie.description || "";

    card.appendChild(title);
    card.appendChild(img);
    card.appendChild(details);
    card.appendChild(desc);

    if (showAddButton) {
      const btn = document.createElement("button");
      btn.textContent = "Add to Wish List";
      btn.addEventListener("click", function () {
        addMovieToWishList(movie);
      });
      card.appendChild(btn);
    }

    container.appendChild(card);
  });
}

// GET /api/Movies – כל הסרטים מהשרת
function getAllMoviesFromServer() {
  return fetch(`${API_BASE}/Movies`)
    .then(function (response) {
      if (!response.ok) {
        return response.text().then(function (msg) {
          throw new Error("Error getting movies: " + msg);
        });
      }
      return response.json();
    });
}

// POST /api/Movies – הוספת סרט (Wish List)
function addMovieToWishList(movie) {
  fetch(`${API_BASE}/Movies`, {
    method: "POST",
    headers: {"Content-Type": "application/json"},
    body: JSON.stringify(movie),
  })
    .then(function (response) {
      if (response.ok) {
        alert("Movie added to Wish List");
      } else {
        return response.text().then(function (msg) {
          alert("Error adding movie: " + msg);
        });
      }
    })
    .catch(function (err) {
      console.error(err);
      alert("Network error while adding movie");
    });
}

// GET /api/Movies/byRating/{minRating}
function getMoviesByRatingFromServer(minRating) {
  return fetch(`${API_BASE}/Movies/byRating/${minRating}`)
    .then(function (response) {
      if (response.ok) return response.json();
      if (response.status === 404) return [];
      return response.text().then(function (msg) {
        throw new Error(msg);
      });
    })
    .catch(function (err) {
      alert("Error filtering by rating: " + err.message);
      return [];
    });
}

// GET /api/Movies/duration?maxDuration=XX
function getMoviesByDurationFromServer(maxDuration) {
  return fetch(`${API_BASE}/Movies/duration?maxDuration=${maxDuration}`)
    .then(function (response) {
      if (response.ok) return response.json();
      if (response.status === 404) return [];
      return response.text().then(function (msg) {
        throw new Error(msg);
      });
    })
    .catch(function (err) {
      alert("Error filtering by duration: " + err.message);
      return [];
    });
}

// ---------------------- Cast ----------------------

function loadCastsAndRender() {
  fetch(`${API_BASE}/Casts`)
    .then(function (response) {
      if (!response.ok) {
        return response.text().then(function (msg) {
          throw new Error(msg);
        });
      }
      return response.json();
    })
    .then(function (casts) {
      renderCastTable(casts);
    })
    .catch(function (err) {
      console.error(err);
      alert("Error getting casts: " + err.message);
    });
}

function renderCastTable(casts) {
  const tbody = document.getElementById("castTableBody");
  if (!tbody) return;

  tbody.innerHTML = "";

  casts.forEach(function (cast) {
    const tr = document.createElement("tr");
    tr.innerHTML = `
      <td>${cast.id}</td>
      <td>${cast.name}</td>
      <td>${cast.role || ""}</td>
      <td>${cast.dateOfBirth ? cast.dateOfBirth.substring(0,10) : ""}</td>
      <td>${cast.country || ""}</td>
      <td>${cast.photoUrl || ""}</td>
    `;
    tbody.appendChild(tr);
  });
}

// ---------------------- Auth UI (all pages) ----------------------
document.addEventListener("DOMContentLoaded", setupAuthUI);

function setupAuthUI() {
  const user = JSON.parse(localStorage.getItem("user") || "null");

  const userBox = document.getElementById("userBox");
  const loginLink = document.getElementById("loginLink");
  const logoutBtn = document.getElementById("logoutBtn");

  if (user) {
    if (userBox) userBox.textContent = `מחובר: ${user.userName}`;

    if (loginLink) {
      loginLink.textContent = "Logout";
      loginLink.href = "#";
      loginLink.onclick = function (e) {
        e.preventDefault();
        localStorage.removeItem("user");
        alert("Logged out");
        window.location.href = "index.html";
      };
    }

    if (logoutBtn) {
      logoutBtn.style.display = "inline-block";
      logoutBtn.onclick = function () {
        localStorage.removeItem("user");
        alert("Logged out");
        window.location.href = "index.html";
      };
    }
  } else {
    if (userBox) userBox.textContent = "";
    if (loginLink) {
      loginLink.textContent = "Login";
      loginLink.href = "login.html";
      loginLink.onclick = null;
    }
    if (logoutBtn) logoutBtn.style.display = "none";
  }
}