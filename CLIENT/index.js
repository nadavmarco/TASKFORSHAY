document.addEventListener("DOMContentLoaded", function () {
  if (document.getElementById("moviesContainer")) initHomePage();
  if (document.getElementById("wishlistContainer")) initWishlistPage();
  if (document.getElementById("castTableBody")) loadCastsAndRender();
});

// ---------------------- Home Page ----------------------
function initHomePage() {
  getAllMoviesFromServer().then(function (moviesFromDb) {
    renderMoviesList(moviesFromDb, "moviesContainer", true);
  });

  document.getElementById("filterByRatingBtn").addEventListener("click", function () {
    const minRating = parseFloat(document.getElementById("ratingInput").value);
    if (isNaN(minRating)) return alert("Please enter a numeric rating");

    getMoviesByRatingFromServer(minRating).then(function (filtered) {
      renderMoviesList(filtered, "moviesContainer", true);
    });
  });

  document.getElementById("filterByDurationBtn").addEventListener("click", function () {
    const maxDuration = parseInt(document.getElementById("durationInput").value);
    if (isNaN(maxDuration)) return alert("Please enter movie duration");

    getMoviesByDurationFromServer(maxDuration).then(function (filtered) {
      renderMoviesList(filtered, "moviesContainer", true);
    });
  });
}

// ---------------------- Wish List Page ----------------------
function initWishlistPage() {
  getAllMoviesFromServer().then(function (wishlist) {
    renderMoviesList(wishlist, "wishlistContainer", false);
  });
}