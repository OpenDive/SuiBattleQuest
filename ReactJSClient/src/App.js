import './App.css';
import GoogleLogo from "./assets/google.svg";
import { Ed25519Keypair } from "@mysten/sui.js/keypairs/ed25519";
import {
  CLIENT_ID,
  FULLNODE_URL,
  KEY_PAIR_SESSION_STORAGE_KEY,
  MAX_EPOCH_LOCAL_STORAGE_KEY,
  RANDOMNESS_SESSION_STORAGE_KEY,
  REDIRECT_URI,
  STEPS_LABELS_TRANS_KEY,
  SUI_DEVNET_FAUCET,
  SUI_PROVER_DEV_ENDPOINT,
  USER_SALT_LOCAL_STORAGE_KEY,
} from "./constant";
import {
  genAddressSeed,
  generateNonce,
  generateRandomness,
  getExtendedEphemeralPublicKey,
  getZkLoginSignature,
  jwtToAddress,
} from "@mysten/zklogin";
import { SuiClient } from "@mysten/sui.js/client";
import { useState } from "react";
import React from 'react';
import styled from "styled-components";

const theme = {
  blue: {
    default: "#3f51b5",
    hover: "#283593",
  },
  pink: {
    default: "#e91e63",
    hover: "#ad1457",
  },
};

function App() {
  const [nonce, setNonce] = useState("");
  const [ephemeralKeyPair, setEphemeralKeyPair] = useState();
  const [maxEpoch, setMaxEpoch] = useState(0);
  const [randomness, setRandomness] = useState("");
  const [currentEpoch, setCurrentEpoch] = useState("");

  const suiClient = new SuiClient({ url: FULLNODE_URL });

  const Button = styled.button`
  background-color: ${(props) => theme[props.theme].default};
  color: white;
  padding: 5px 15px;
  border-radius: 5px;
  outline: 0;
  border: 0; 
  text-transform: uppercase;
  margin: 10px 0px;
  cursor: pointer;
  box-shadow: 0px 2px 2px lightgray;
  transition: ease background-color 250ms;
  &:hover {
    background-color: ${(props) => theme[props.theme].hover};
  }
  &:disabled {
    cursor: default;
    opacity: 0.7;
  }
`;

  return (
    <div className="App">
      <button
                variant="contained"
                size="small"
                onClick={() => {
                  const randomness = generateRandomness();
                  window.sessionStorage.setItem(
                    RANDOMNESS_SESSION_STORAGE_KEY,
                    randomness
                  );
                  setRandomness(randomness);
                }}
              >
                {"Get randomness"}
              </button>
      <button
                variant="contained"
                size="small"
                onClick={async () => {
                  const { epoch } = await suiClient.getLatestSuiSystemState();

                  setCurrentEpoch(epoch);
                  window.localStorage.setItem(
                    MAX_EPOCH_LOCAL_STORAGE_KEY,
                    String(Number(epoch) + 10)
                  );
                  setMaxEpoch(Number(epoch) + 10);
                }}
              >
                {"Get Epoch"}
              </button>
      <button
                variant="contained"
                disabled={Boolean(ephemeralKeyPair)}
                onClick={() => {
                  const ephemeralKeyPair = Ed25519Keypair.generate();
                  window.sessionStorage.setItem(
                    KEY_PAIR_SESSION_STORAGE_KEY,
                    ephemeralKeyPair.export().privateKey
                  );
                  setEphemeralKeyPair(ephemeralKeyPair);
                }}
              >
                Create random ephemeral KeyPair{" "}
              </button>
      <button
                  variant="contained"
                  onClick={() => {
                    if (!ephemeralKeyPair) {
                      return;
                    }
                    const nonce = generateNonce(
                      ephemeralKeyPair.getPublicKey(),
                      maxEpoch,
                      randomness
                    );
                    setNonce(nonce);
                  }}
                >
                  Generate Nonce
                </button>
      <button
                sx={{
                  mt: "24px",
                }}
                variant="contained"
                onClick={() => {
                  const params = new URLSearchParams({
                    client_id: CLIENT_ID,
                    redirect_uri: REDIRECT_URI,
                    response_type: "id_token",
                    scope: "openid",
                    nonce: nonce,
                  });
                  const loginURL = `https://accounts.google.com/o/oauth2/v2/auth?${params}`;
                  window.location.replace(loginURL);
                }}
              >
                <img
                  src={GoogleLogo}
                  width="16px"
                  style={{
                    marginRight: "8px",
                  }}
                  alt="Google"
                />{" "}
                Sign In With Google
              </button>
    </div>
  );
}

export default App;
