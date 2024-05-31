mergeInto(LibraryManager.library, {

  ZkLogin: function (objectName, callback) {

  },

  GetSuiFrens: function (objectName, callback) {

  },

  ConnectToWallet: function (objectName, callback) {
    var parsedObjectName    = Pointer_stringify(objectName);
    var parsedCallback      = Pointer_stringify(callback);
    
    console.log("parsedObjectName: " + parsedObjectName + ", parsedCallback" + parsedCallback);
    // ReactUnityWebGL.ConnectToWallet(parsedObjectName, parsedCallback);
    window.dispatchReactUnityEvent(
      "ConnectToWallet",
      parsedCallback,
      parsedObjectName
    );
  },

  ApproveEntryFee: function(objectName, callback, stakeAmount) {
    var parsedObjectName    = Pointer_stringify(objectName);
    var parsedCallback      = Pointer_stringify(callback);
    var parsedStakeAmount   = Pointer_stringify(stakeAmount);

    console.log("parsedObjectName: " + parsedObjectName + ", parsedCallback" + parsedCallback);
    window.dispatchReactUnityEvent(
      "ApproveEntryFee",
      parsedCallback,
      parsedObjectName,
      parsedStakeAmount
    );
  },

  StakeEntryFee: function(objectName, callback, stakeAmount) {
    var parsedObjectName    = Pointer_stringify(objectName);
    var parsedCallback      = Pointer_stringify(callback);
    var parsedStakeAmount   = Pointer_stringify(stakeAmount);

    console.log("parsedObjectName: " + parsedObjectName + ", parsedCallback" + parsedCallback);
    window.dispatchReactUnityEvent(
      "StakeEntryFee",
      parsedCallback,
      parsedObjectName,
      parsedStakeAmount
    );
  }
  
});