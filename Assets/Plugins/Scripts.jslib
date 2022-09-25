mergeInto(LibraryManager.library, {

  SaveTest: function () {
    window.alert("Save test");
  }, 

  ExportMapJSONTest: function (mapJSON) {
    console.log(UTF8ToString(mapJSON));
     window.alert(UTF8ToString(mapJSON));
     window.alert(UTF8ToString(mapJSON).length);
  }, 

  ExportMapJSON: function (mapJSON) {

    var protocol = window.location.protocol;
    var host = window.location.hostname;
    var url = protocol + "//" + host + ":3000/maps/add";

    console.log(mapJSON);    

    var xhr = new XMLHttpRequest();
    xhr.open("POST", url, true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.send(UTF8ToString(mapJSON));
  },   

});