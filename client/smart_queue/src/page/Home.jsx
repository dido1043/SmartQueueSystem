import React, { useEffect } from "react";
import Navigation from "../components/navigation/Navigation";

function Home() {
  useEffect(() => {
    const params = new URLSearchParams(window.location.search);

    const accessToken = params.get("accessToken");
    const expiration = params.get("expiration");
    const refreshToken = params.get("refreshToken");
    const userRole = params.get("userRole");

    if (!accessToken) {
      return;
    }

    localStorage.setItem("accessToken", accessToken);

    if (expiration) {
      localStorage.setItem("expiration", expiration);
    }

    if (refreshToken) {
      localStorage.setItem("refreshToken", refreshToken);
    }

    if (userRole) {
      localStorage.setItem("userRole", userRole);
    }

    window.history.replaceState({}, document.title, window.location.pathname);
  }, []);

  return (
    <div>
       <Navigation />
      <h1>Welcome to the Smart Queue System</h1>
    </div>
  );
}

export default Home;
