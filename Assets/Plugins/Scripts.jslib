mergeInto(LibraryManager.library, {

  AddMap: function (cohortId, mapJSON) {
    var protocol = window.location.protocol;
    var host = window.location.hostname;

    var formattedCohortId = UTF8ToString(cohortId);

    var url = protocol + "//" + host + ":3000/maps/add/" + formattedCohortId;

    console.log(mapJSON);    

    var xhr = new XMLHttpRequest();
    xhr.open("POST", url, true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.send(UTF8ToString(mapJSON));
  },  

  UpdateMap: function (mapId, mapJSON) {
    var protocol = window.location.protocol;
    var host = window.location.hostname;
    var formattedMapId = UTF8ToString(mapId);
    var url = protocol + "//" + host + ":3000/maps/" + formattedMapId + "/edit";

    console.log(mapId);    
    console.log(mapJSON);    
    console.log(formattedMapId);    
    console.log(UTF8ToString(mapJSON));    

    var xhr = new XMLHttpRequest();
    xhr.open("PUT", url, true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.send(UTF8ToString(mapJSON));
  },   

});