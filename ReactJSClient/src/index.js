import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App.js';
import { SuiClientProvider, createNetworkConfig } from "@mysten/dapp-kit";
import { getFullnodeUrl } from "@mysten/sui.js/client";
import { BrowserRouter, Route, Routes } from "react-router-dom";

const { networkConfig } = createNetworkConfig({
  devnet: { url: getFullnodeUrl("devnet") },
});

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <BrowserRouter>
    <SuiClientProvider networks={networkConfig} network="devnet">
      <React.StrictMode>
        <Routes>
          <Route path="/" element={<App />}></Route>
        </Routes>
      </React.StrictMode>
    </SuiClientProvider>
  </BrowserRouter>
);
