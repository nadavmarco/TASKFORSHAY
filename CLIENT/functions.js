// functions.js

// אם ה-CLIENT וה-API באותו פרויקט, מספיק /api
const API_BASE = "/api";

// ---------------------- Movies / Wish List ----------------------

// רינדור רשימת סרטים לקונטיינר
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
    img.onerror = () => {
      img.style.display = "none";
    };

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
      btn.addEventListener("click", async () => {
        await addMovieToWishList(movie);
      });
      card.appendChild(btn);
    }

    container.appendChild(card);
  });
}

// שולח סרט שנבחר לשרת – POST /api/Movies
async function addMovieToWishList(movie) {
  try {
    const response = await fetch(`${API_BASE}/Movies`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(movie),
    });

    if (response.ok) {
      alert("Movie added to Wish List");
    } else if (response.status === 409) {
      const msg = await response.text();
      alert(msg || "Movie already exists in Wish List");
    } else {
      const msg = await response.text();
      alert("Error adding movie: " + msg);
    }
  } catch (err) {
    console.error(err);
    alert("Network error while adding movie");
  }
}

// GET /api/Movies/byRating/{minRating}
async function getMoviesByRatingFromServer(minRating) {
  try {
    const response = await fetch(`${API_BASE}/Movies/byRating/${minRating}`);
    if (response.ok) {
      return await response.json();
    } else if (response.status === 404) {
      alert("No movies found for this rating");
      return [];
    } else {
      const msg = await response.text();
      alert("Error getting movies by rating: " + msg);
      return [];
    }
  } catch (err) {
    console.error(err);
    alert("Network error while filtering by rating");
    return [];
  }
}

// GET /api/Movies/duration?maxDuration=XX
async function getMoviesByDurationFromServer(maxDuration) {
  try {
    const response = await fetch(
      `${API_BASE}/Movies/duration?maxDuration=${maxDuration}`
    );
    if (response.ok) {
      return await response.json();
    } else if (response.status === 404) {
      alert("No movies found for this duration");
      return [];
    } else {
      const msg = await response.text();
      alert("Error getting movies by duration: " + msg);
      return [];
    }
  } catch (err) {
    console.error(err);
    alert("Network error while filtering by duration");
    return [];
  }
}

// GET /api/Movies – רשימת ה-Wish List השמורה בצד שרת
async function getWishListFromServer() {
  try {
    const response = await fetch(`${API_BASE}/Movies`);
    if (response.ok) {
      return await response.json();
    } else {
      const msg = await response.text();
      alert("Error getting wish list: " + msg);
      return [];
    }
  } catch (err) {
    console.error(err);
    alert("Network error while getting wish list");
    return [];
  }
}

// ---------------------- Cast ----------------------

// מביא את רשימת השחקנים מהשרת ומרנדר לטבלה
async function loadCastsAndRender() {
  try {
    const response = await fetch(`${API_BASE}/Casts`);
    if (response.ok) {
      const casts = await response.json();
      renderCastTable(casts);
    } else {
      const msg = await response.text();
      alert("Error getting casts: " + msg);
    }
  } catch (err) {
    console.error(err);
    alert("Network error while getting casts");
  }
}

function renderCastTable(casts) {
  const tbody = document.getElementById("castTableBody");
  if (!tbody) return;

  tbody.innerHTML = "";

  casts.forEach((cast) => {
    const tr = document.createElement("tr");

    const tdId = document.createElement("td");
    tdId.textContent = cast.id;

    const tdName = document.createElement("td");
    tdName.textContent = cast.name;

    const tdRole = document.createElement("td");
    tdRole.textContent = cast.role;

    const tdDob = document.createElement("td");
    // תאריך יפה
    const date = cast.dateOfBirth ? new Date(cast.dateOfBirth) : null;
    tdDob.textContent = date ? date.toISOString().substring(0, 10) : "";

    const tdCountry = document.createElement("td");
    tdCountry.textContent = cast.country;

    tr.appendChild(tdId);
    tr.appendChild(tdName);
    tr.appendChild(tdRole);
    tr.appendChild(tdDob);
    tr.appendChild(tdCountry);

    tbody.appendChild(tr);
  });
}

// קורא את הנתונים מהטופס
function getCastFromForm() {
  return {
    id: parseInt(document.getElementById("castId").value),
    name: document.getElementById("castName").value.trim(),
    role: document.getElementById("castRole").value.trim(),
    dateOfBirth: document.getElementById("castDob").value,
    country: document.getElementById("castCountry").value.trim(),
  };
}

// ולידציות לטופס Cast (Form validations)
function validateCast(cast) {
  const errors = [];

  if (isNaN(cast.id) || cast.id <= 0) {
    errors.push("Id must be a positive number");
  }

  if (!cast.name || cast.name.length < 2) {
    errors.push("Name must be at least 2 characters");
  }

  if (!cast.role || cast.role.length < 2) {
    errors.push("Role must be at least 2 characters");
  }

  if (!cast.dateOfBirth) {
    errors.push("Date of birth is required");
  }

  if (!cast.country || cast.country.length < 2) {
    errors.push("Country must be at least 2 characters");
  }

  return errors;
}

function clearCastErrors() {
  const div = document.getElementById("castErrors");
  if (div) {
    div.textContent = "";
  }
}

function showCastErrors(errors) {
  const div = document.getElementById("castErrors");
  if (!div) return;

  div.innerHTML = "";
  const ul = document.createElement("ul");
  errors.forEach((err) => {
    const li = document.createElement("li");
    li.textContent = err;
    ul.appendChild(li);
  });
  div.appendChild(ul);
}

// שליחת Cast חדש לשרת – POST /api/Casts
async function submitCastToServer(cast) {
  try {
    const response = await fetch(`${API_BASE}/Casts`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(cast),
    });

    if (response.ok) {
      alert("Cast added successfully");
      return true;
    } else if (response.status === 409) {
      const msg = await response.text();
      alert(msg || "Cast with same Id already exists");
      return false;
    } else {
      const msg = await response.text();
      alert("Error adding cast: " + msg);
      return false;
    }
  } catch (err) {
    console.error(err);
    alert("Network error while adding cast");
    return false;
  }
}