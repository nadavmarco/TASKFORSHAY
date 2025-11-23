const AUTH_API = `${API_BASE}/Users`;

// Register
document.getElementById("registerForm").addEventListener("submit", function (e) {
  e.preventDefault();
  registerErrors.innerHTML = "";

  const userName = regUserName.value.trim();
  const email = regEmail.value.trim();
  const password = regPassword.value;

  const errors = [];
  if (userName.length < 2) errors.push("User name must be at least 2 chars");
  if (!email.includes("@")) errors.push("Email not valid");
  if (password.length < 4) errors.push("Password must be at least 4 chars");

  if (errors.length) {
    registerErrors.innerHTML =
      "<ul>" + errors.map(x => `<li>${x}</li>`).join("") + "</ul>";
    return;
  }

  fetch(`${AUTH_API}/register`, {
    method: "POST",
    headers: {"Content-Type":"application/json"},
    body: JSON.stringify({ userName, email, password })
  })
    .then(function (res) {
      if (!res.ok) {
        return res.text().then(function (msg) {
          throw new Error(msg);
        });
      }
      alert("Registered successfully! Now login.");
      registerForm.reset();
    })
    .catch(function (err) {
      registerErrors.innerHTML = err.message;
    });
});

// Login
document.getElementById("loginForm").addEventListener("submit", function (e) {
  e.preventDefault();
  loginErrors.innerHTML = "";

  const email = loginEmail.value.trim();
  const password = loginPassword.value;

  fetch(`${AUTH_API}/login`, {
    method: "POST",
    headers: {"Content-Type":"application/json"},
    body: JSON.stringify({ email, password })
  })
    .then(function (res) {
      if (res.status === 401) throw new Error("Invalid credentials");
      if (!res.ok) {
        return res.text().then(function (msg) {
          throw new Error(msg);
        });
      }
      return res.json();
    })
    .then(function (user) {
      localStorage.setItem("user", JSON.stringify(user));
      alert(`Welcome ${user.userName}!`);
      window.location.href = "index.html";
    })
    .catch(function (err) {
      loginErrors.innerHTML = err.message;
    });
});