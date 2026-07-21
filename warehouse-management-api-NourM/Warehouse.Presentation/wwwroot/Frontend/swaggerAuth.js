function authorizeSwagger() {
    const token = sessionStorage.getItem("firebaseToken");

    if (!token) {
        //so nobody enters the url and goes to swagger directly
        window.location.href = "/Frontend/";
        return;
    }

    if (!window.ui) {
        setTimeout(authorizeSwagger, 200);
        return;
    }

    window.ui.preauthorizeApiKey("Bearer", token);
}

authorizeSwagger();