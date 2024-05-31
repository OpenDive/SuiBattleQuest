import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App.js';
import { SuiClientProvider, createNetworkConfig } from "@mysten/dapp-kit";
import { getFullnodeUrl } from "@mysten/sui.js/client";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import OAuthCallback from './OAuthCallback';

const { networkConfig } = createNetworkConfig({
  devnet: { url: getFullnodeUrl("devnet") },
});

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <BrowserRouter>
    <SuiClientProvider networks={networkConfig} network="devnet">
        <Routes>
          <Route path="/" element={<App />}></Route>
          <Route path="/oauth/callback" element={<OAuthCallback />} />
        </Routes>
    </SuiClientProvider>
  </BrowserRouter>
);