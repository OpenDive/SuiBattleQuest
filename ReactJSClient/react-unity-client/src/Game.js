import React, { Fragment } from "react";
import { Unity, useUnityContext } from "react-unity-webgl";

function Game() {
    const unityContext = useUnityContext({
        loaderUrl: '/build/SuiBattleQuest.loader.js',
        dataUrl: '/build/SuiBattleQuest.data',
        frameworkUrl: '/build/SuiBattleQuest.framework.js',
        codeUrl: '/build/SuiBattleQuest.wasm'
    });

    return (
        <div>
            <Unity unityContext={unityContext} style={{ width: 1280, height: 720 }} />
        </div>
    );
}

export default Game;