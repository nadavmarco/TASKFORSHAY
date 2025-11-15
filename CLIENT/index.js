// index.js

document.addEventListener("DOMContentLoaded", () => {
  // עמוד הבית (סרטים + סינון)
  if (document.getElementById("moviesContainer")) {
    initHomePage();
  }

  // עמוד ה-Wish List
  if (document.getElementById("wishlistContainer")) {
    initWishlistPage();
  }

  // עמוד ה-Cast
  if (document.getElementById("castForm")) {
    initCastPage();
  }
});

// ---------------------- Home Page (movies) ----------------------

function initHomePage() {
  // רינדור כל הסרטים מתוך movie.js (קובץ המרצה)
  if (typeof movies !== "undefined" && Array.isArray(movies)) {
    renderMoviesList(movies, "moviesContainer", true);
  }

  const ratingBtn = document.getElementById("filterByRatingBtn");
  const durationBtn = document.getElementById("filterByDurationBtn");

  ratingBtn.addEventListener("click", async () => {
    const ratingInput = document.getElementById("ratingInput");
    const minRating = parseFloat(ratingInput.value);

    if (isNaN(minRating)) {
      alert("Please enter a numeric rating");
      return;
    }

    const filtered = await getMoviesByRatingFromServer(minRating);
    if (filtered) {
      renderMoviesList(filtered, "moviesContainer", true);
    }
  });

  durationBtn.addEventListener("click", async () => {
    const durationInput = document.getElementById("durationInput");
    const maxDuration = parseInt(durationInput.value);

    if (isNaN(maxDuration)) {
      alert("Please enter movie duration (minutes)");
      return;
    }

    const filtered = await getMoviesByDurationFromServer(maxDuration);
    if (filtered) {
      renderMoviesList(filtered, "moviesContainer", true);
    }
  });
}

// ---------------------- Wish List Page ----------------------

async function initWishlistPage() {
  const wishlist = await getWishListFromServer();
  if (wishlist) {
    renderMoviesList(wishlist, "wishlistContainer", false);
  }
}

// ---------------------- Cast Page ----------------------

async function initCastPage() {
  await loadCastsAndRender();

  const form = document.getElementById("castForm");

  form.addEventListener("submit", async (event) => {
    event.preventDefault();
    clearCastErrors();

    const cast = getCastFromForm();
    const errors = validateCast(cast);

    if (errors.length > 0) {
      showCastErrors(errors);
      return;
    }

    const success = await submitCastToServer(cast);
    if (success) {
      form.reset();
      await loadCastsAndRender();
    }
  });
}