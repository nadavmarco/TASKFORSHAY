let currentUser = JSON.parse(localStorage.getItem("user") || "null");

if (!currentUser) {
  mustLoginMsg.style.display = "block";
  addMovieSection.style.display = "none";
} else {
  mustLoginMsg.style.display = "none";
  addMovieSection.style.display = "block";
}

movieForm.addEventListener("submit", function (e) {
  e.preventDefault();
  movieErrors.innerHTML = "";

  const movie = {
    title: title.value.trim(),
    rating: parseFloat(rating.value),
    income: parseFloat(income.value),
    releaseYear: parseInt(releaseYear.value),
    duration: parseInt(duration.value),
    language: language.value.trim(),
    description: description.value.trim(),
    genre: genre.value.trim(),
    photoUrl: photoUrl.value.trim()
  };

  const errors = validateMovie(movie);
  if (errors.length) {
    movieErrors.innerHTML =
      "<ul>" + errors.map(x => `<li>${x}</li>`).join("") + "</ul>";
    return;
  }

  fetch(`${API_BASE}/Movies`, {
    method:"POST",
    headers: {"Content-Type":"application/json"},
    body: JSON.stringify(movie)
  })
    .then(function (res) {
      if (!res.ok) {
        return res.text().then(function (msg) {
          throw new Error(msg);
        });
      }
      alert("Movie added successfully!");
      movieForm.reset();
    })
    .catch(function (err) {
      movieErrors.innerHTML = err.message;
    });
});

function validateMovie(m) {
  const errors = [];
  if (!m.title) errors.push("Title required");
  if (isNaN(m.rating) || m.rating < 0 || m.rating > 10) errors.push("Rating 0-10 required");
  if (isNaN(m.income) || m.income < 0) errors.push("Income positive required");
  if (isNaN(m.releaseYear)) errors.push("Release year required");
  if (isNaN(m.duration) || m.duration <= 0) errors.push("Duration positive required");
  if (!m.language) errors.push("Language required");
  if (!m.description) errors.push("Description required");
  if (!m.genre) errors.push("Genre required");
  if (!m.photoUrl) errors.push("Photo URL required");
  return errors;
}