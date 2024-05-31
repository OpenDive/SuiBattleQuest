 
 
 var WebGLTool = {
     IsMobileJS: function()
     {
         return (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent));
     }
 };
 
 mergeInto(LibraryManager.library, WebGLTool);