import React from "react";
import { Unity } from "react-unity-webgl";
import { useUnityContext } from "react-unity-webgl";

function Game() {
    const { unityProvider } = useUnityContext({
        loaderUrl: 'build/SuiBattleQuest.loader.js',
        dataUrl: 'build/SuiBattleQuest.data',
        frameworkUrl: 'build/SuiBattleQuest.framework.js',
        codeUrl: 'build/SuiBattleQuest.wasm'
    });

    return <Unity unityProvider={unityProvider} />;
}

export default Game; 