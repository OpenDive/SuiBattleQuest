import React, { useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { jwtDecode } from "jwt-decode";
import {
    computeZkLoginAddress
  } from "@mysten/zklogin";
import { JWT_ADDRESS_STORAGE_KEY } from './constant';

function OAuthCallback() {
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    const hash = location.hash;
    if (hash) {
      const params = new URLSearchParams(hash.substring(1)); // Remove the leading '#'
      const idToken = params.get('id_token');
      const jwt = jwtDecode(idToken);
      const login_address = computeZkLoginAddress({
        userSalt: "",
        claimName: "sub",
        claimValue: jwt.sub,
        aud: jwt.aud,
        iss: jwt.iss
      });
      window.sessionStorage.setItem(
        JWT_ADDRESS_STORAGE_KEY,
        login_address
      );
      navigate('/game');
    }
  }, [location, navigate]);

  return (
    <div>
      <h2>OAuth Callback</h2>
      <p>Processing authentication...</p>
    </div>
  );
}

export default OAuthCallback;