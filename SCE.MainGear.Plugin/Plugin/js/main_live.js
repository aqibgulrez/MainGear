//Constraints and Pre-Reqs:
//jQuery Should be present
//Parent container should be responsive


////////////////////////////////////////////////////////////////// GLOBAL VARIABLES ////////////////////////////////////////////////////////////////// >>
var WIDGET_UTILITY_LOADED = false;
var WIDGET_ROOT_URL = "http://custom.maingear.com/widget/";
var IMAGE_ROOT_URL = "http://custom.maingear.com/admin/files/sections";
var JSON_URL = "http://custom.maingear.com/api/";
var WIDGET_CONTAINER_ID = "";
var PRODUCT_ID;
// var FILE_VERSION = 0.6;
var WIDGET_REQUEST_MODE = "get"; //Can be set to AJAX for fully trusted cross-domain access allowed
////////////////////////////////////////////////////////////////// GLOBAL VARIABLES ////////////////////////////////////////////////////////////////// <<

////////////////////////////////////////////////////////////////// UTILITY METHODS ////////////////////////////////////////////////////////////////// >>


//Add the widget CSS to the calling document
function maingear_addWidgetCss() {
  var cssId = 'maingearWidgetCss';

  //Check if its already added or not. Do not add, if its already added.
  if (!document.getElementById(cssId)) {
    var head = document.getElementsByTagName('head')[0];
    var link = document.createElement('link');
    link.id = cssId;
    link.rel = 'stylesheet';
    link.type = 'text/css';
    link.href = (WIDGET_ROOT_URL + 'css/widget.css').replace("http://", "//").replace("https://", "//");
    link.media = 'all';
    head.appendChild(link);
  }
  cssId = 'maingearWidgetBootstrap';

  //Check if its already added or not. Do not add, if its already added.
  if (!document.getElementById(cssId)) {
    var head = document.getElementsByTagName('head')[0];
    var link = document.createElement('link');
    link.id = cssId;
    link.rel = 'stylesheet';
    link.type = 'text/css';
    link.href = (WIDGET_ROOT_URL + 'css/bootstrap.css').replace("http://", "//").replace("https://", "//");
    link.media = 'all';
    head.appendChild(link);
  }
  cssId = 'maingearWidgetRadioButtons';

  //Check if its already added or not. Do not add, if its already added.
  if (!document.getElementById(cssId)) {
    var head = document.getElementsByTagName('head')[0];
    var link = document.createElement('link');
    link.id = cssId;
    link.rel = 'stylesheet';
    link.type = 'text/css';
    link.href = (WIDGET_ROOT_URL + 'css/radiobuttons.css').replace("http://", "//").replace("https://", "//");
    link.media = 'all';
    head.appendChild(link);
  }
  cssId = 'maingearWidgetFontAwesome';
  //Check if its already added or not. Do not add, if its already added.
  if (!document.getElementById(cssId)) {
    var head = document.getElementsByTagName('head')[0];
    var link = document.createElement('link');
    link.id = cssId;
    link.rel = 'stylesheet';
    link.type = 'text/css';
    link.href = (WIDGET_ROOT_URL + 'css/font-awesome.min.css').replace("http://", "//").replace("https://", "//");
    link.media = 'all';
    head.appendChild(link);
  }
  cssId = 'maingearWidgetAnimate';
  //Check if its already added or not. Do not add, if its already added.
  if (!document.getElementById(cssId)) {
    var head = document.getElementsByTagName('head')[0];
    var link = document.createElement('link');
    link.id = cssId;
    link.rel = 'stylesheet';
    link.type = 'text/css';
    link.href = (WIDGET_ROOT_URL + 'css/animate.css').replace("http://", "//").replace("https://", "//");
    link.media = 'all';
    head.appendChild(link);
  }
 

  cssId = 'maingearWidgeJqueryUI';
  //Check if its already added or not. Do not add, if its already added.
  if (!document.getElementById(cssId)) {
    var head = document.getElementsByTagName('head')[0];
    var link = document.createElement('link');
    link.id = cssId;
    link.rel = 'stylesheet';
    link.type = 'text/css';
    link.href = (WIDGET_ROOT_URL + 'css/jquery-ui.css').replace("http://", "//").replace("https://", "//");
    link.media = 'all';
    head.appendChild(link);
  }
}

//Add a JS file to the head
function maingear_addJSFile(fileRef, filePath, doNotInclude) {

  //Do not load if notInclude flag is passed.
  if (!doNotInclude) {
    if (!document.getElementById(fileRef)) {
      var head = document.getElementsByTagName('head')[0];
      var scriptTag = document.createElement('script');
      scriptTag.id = fileRef;
      scriptTag.type = 'text/javascript';
      scriptTag.src = (WIDGET_ROOT_URL + filePath).replace("http://", "//").replace("https://", "//");
      head.appendChild(scriptTag);
    }
  }
}
////////////////////////////////////////////////////////////////// UTILITY METHODS ////////////////////////////////////////////////////////////////// <<

//Wrapper to add all related JS files to the document
function maingear_addRelatedJSFiles() {
  //Only load if its not already loaded by the parent page
  maingear_addJSFile("widget_jqueryui", "js/jquery-ui.js", (typeof(jQuery.ui) != 'undefined'));
  maingear_addJSFile("widget_bootstrap", "js/bootstrap.js", (typeof $().modal == 'function'));
  maingear_addJSFile("widget_jTemplate", "js/jtemplates.js");
  maingear_addJSFile("widget_script", "js/script.js");
    maingear_addJSFile("widget_utility", "js/utility.js");
  maingear_addJSFile("widget_notify", "js/bootstrap-notify.min.js");

}

//Validate that all pre-reqs for widget are present - we can add further validations here
//Validation Rules:
// 1 - jQuery should be present (for now any version would work, we can add version check later)
// 2 - The widget container id should be valid DOM element
function maingear_validateWidget(containerId) {
  WIDGET_CONTAINER_ID = containerId;
  message = "";
  if (typeof(jQuery) == "undefined") {
    message = "jQuery is not included in the parent page. Please include jQuery and try again.";
  } else if ($(containerId).length == 0) {
    message = "Invalid widget container \"" + containerId + "\".";
  }
  if (message != "") {
    document.write("<center><span style='color: red;'>" + message + "</span></center>");
    console.log(message);
  } else {
    console.log("Validation successful.");
    maingear_addWidgetCss();
    maingear_addRelatedJSFiles();
  }
  return (message == "");
}


//Initialize the widget
function maingear_initWidget(containerId, productId) {
  PRODUCT_ID = productId;
  WIDGET_CONTAINER_ID = containerId;
  if (maingear_validateWidget(containerId)) {
    $container = $(containerId);
    $container.addClass("widget_root");
    $container.append("<div id='widget_root'></div>"); //Add root element
    $containerRoot = $container.find("#widget_root").addClass("container"); //Find the widget root and add class for styling etc.
    $containerRoot.append("<center><div class=\"loader\"'>Loading...</div></center>");
    $containerRoot.append("<div id=\"widget_container\"></div>");
    //Wait for utility JS to be loaded - that contains definition for: maingear_LoadProducts
    maingearLoadInteval = setInterval(function() {
      if (WIDGET_UTILITY_LOADED) {
        clearInterval(maingearLoadInteval);
        maingear_LoadProducts();
      }
    }, 500);
  }

}