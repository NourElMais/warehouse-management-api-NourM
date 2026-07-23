const firebaseApiKey = "AIzaSyBQGw-EBUoRqOcopnXRTNTw6FztAO-4soo";
const loginUrl = `https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=${firebaseApiKey}`;

const loginForm = document.getElementById("loginForm");
const errorMessage = document.getElementById("errorMessage");

loginForm.addEventListener("submit", async function (event) {
    event.preventDefault();

    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;

    errorMessage.textContent = "";

    try {
        const response = await fetch(loginUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                email: email,
                password: password,
                returnSecureToken: true
            })
        });

        const data = await response.json();

        if (!response.ok) {
            errorMessage.textContent = "Invalid email or password.";
            return;
        }

        sessionStorage.setItem("firebaseToken", data.idToken);

        window.location.href = "/swagger";
    } catch (error) {
        errorMessage.textContent = "Something went wrong. Please try again.";
    }
});