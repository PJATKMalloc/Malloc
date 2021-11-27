﻿var map;
var layerGroup;

window.leafletJsFunctions =
{
    initialize: function (dotnetHelper) {
        map = L.map('map').setView([54.372158, 18.638306], 13);

        /* Trzeba zamienić na własny tileset serwer */
        L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=pk.eyJ1IjoibWFwYm94IiwiYSI6ImNpejY4NXVycTA2emYycXBndHRqcmZ3N3gifQ.rJcFIG214AriISLbB6B5aw', {
            maxZoom: 18,
            attribution: '',
            id: 'mapbox/streets-v11',
            tileSize: 512,
            zoomOffset: -1
        }).addTo(map);

        layerGroup = L.layerGroup().addTo(map);


        map.on('click', function onMapClick(e) {
            return dotnetHelper.invokeMethodAsync('AddPoint', e.latlng.lat, e.latlng.lng);
        });
    },

    setRoute: function (data) {
        layerGroup.clearLayers();
        L.geoJSON(JSON.parse(data)).addTo(layerGroup);
    }
};